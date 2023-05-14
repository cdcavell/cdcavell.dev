using ClassLibrary.Data;
using ClassLibrary.Data.Models;
using ClassLibrary.Mvc.Localization;
using ClassLibrary.Mvc.Services.AppSettings;
using ClassLibrary.Mvc.Services.Email;
using dis.cdcavell.dev.Filters;
using dis.cdcavell.dev.Models.AppSettings;
using dis.cdcavell.dev.Services;
using dis.cdcavell.dev.Validators;
using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// The WebApplicationExtensions internal static class configures services and the application's request pipeline&lt;br /&gt;&lt;br /&gt;
    /// _Services_ are components that are used by the app. For example, a logging component is a service. Code to configure (_or register_) services are added to the ```ConfigureServices``` method.&lt;br /&gt;&lt;br /&gt;
    /// The request handling pipeline is composed as a series of _middleware_ components. For example, a middleware might handle requests for static files or redirect HTTP requests to HTTPS. Each middleware performs asynchronous operations on an ```HttpContext``` and then either invokes the next middleware in the pipeline or terminates the request. Code to configure the request handling pipeline is added to the ```ConfigurePipeline``` method.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 02/21/2023 | User Role Claims Development |~ 
    /// | Christopher D. Cavell | 1.0.3.3 | 12/14/2022 | Migrate to universeodon.com |~ 
    /// | Christopher D. Cavell | 1.0.3.0 | 12/11/2022 | User Registration Development |~ 
    /// | Christopher D. Cavell | 1.0.2.0 | 10/02/2022 | Duende IdentityServer Development |~ 
    /// </revision>
    internal static class WebApplicationExtensions
    {
        private static AppSettings? _appSettings;

        /// <summary>
        /// Method used to add services to the container.
        /// </summary>
        /// <param name="builder">WebApplicationBuilder</param>
        /// <returns>WebApplication</returns>
        /// <method>ConfigureServices(this WebApplicationBuilder builder)</method>
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((ctx, lc) => lc
                .ReadFrom.Configuration(ctx.Configuration));

            _appSettings = new(builder.Configuration);
            builder.Services.AddAppSettingsService(options =>
            {
                options.AppSettings = _appSettings;
            });

            builder.Services.AddDataProtection().UseCryptographicAlgorithms(
                new AuthenticatedEncryptorConfiguration
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddMvc();
            builder.Services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options => {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                });

            builder.Services.AddSingleton<SharedResource>();
            builder.Services.AddScoped<SecurityHeadersAttribute>();

            // Enable localization
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Enable HSTS and HTTPS Redirect
            if (_appSettings.IsProduction)
            {
                builder.Services.AddHsts(options =>
                {
                    options.Preload = true;
                    options.IncludeSubDomains = true;
                    options.MaxAge = TimeSpan.FromDays(730);
                });

                builder.Services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });
            }

            builder.Services.AddEmailService(options =>
            {
                options.Host = _appSettings.EmailService.Host;
                options.Port = _appSettings.EmailService.Port;
                options.EnableSsl = _appSettings.EmailService.EnableSsl;
                options.Credentials = new NetworkCredential(
                    _appSettings.EmailService.UserId,
                    _appSettings.EmailService.Password);
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    _appSettings.ConnectionStrings.EntityFrameworkConnection,
                    sql =>
                    {
                        sql.MigrationsAssembly("dis.cdcavell.dev");
                        sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    }
                ));

            // cache authority discovery and add to DI
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton((Func<IServiceProvider, IDiscoveryCache>)(options =>
            {
                var factory = options.GetRequiredService<IHttpClientFactory>();
                return new DiscoveryCache(_appSettings.Authentication.IdP.Authority, () => factory.CreateClient());
            }));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;

                options.Discovery.CustomEntries.Add("api_role_listing", "~/Api/Role/Listing");
                options.Discovery.CustomEntries.Add("api_user_listing", "~/Api/User/Listing");
            })
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore<ApplicationDbContext>()
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore<ApplicationDbContext>(options =>
                {
                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 3600; // interval in seconds (default is 3600)
                })
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<CustomProfileService>()
                .AddCustomTokenRequestValidator<CustomTokenRequestValidator>();

            // log PII in development only in order to be in compliance with GDPR
            if (_appSettings.IsDevelopment)
                IdentityModelEventSource.ShowPII = true;
            else
                IdentityModelEventSource.ShowPII = false;

            builder.Services.AddAuthentication()
                .AddMicrosoftAccount("Microsoft", "Microsoft", microsoftOptions =>
                {
                    microsoftOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    microsoftOptions.ClientId = _appSettings.Authentication.Microsoft.ClientId;
                    microsoftOptions.ClientSecret = _appSettings.Authentication.Microsoft.ClientSecret;

                    microsoftOptions.SaveTokens = true;

                    microsoftOptions.Events = new OAuthEvents
                    {
                        OnRemoteFailure = context =>
                        {
                            Log.Error(context.Failure, "{@logMessageHeader} - Microsoft.OnRemoteFailure [Failure Message]: {@Message}", context.Request.LogMessageHeader(), context.Failure?.Message);

                            var response = context.Response;
                            response.StatusCode = (int)HttpStatusCode.FailedDependency;
                            context.HandleResponse();

                            return Task.FromResult(response);
                        },
                        OnAccessDenied = context =>
                        {
                            Log.Warning("{@logMessageHeader} - Microsoft.OnAccessDenied [Access Denied Path]: {@Path}", context.Request.LogMessageHeader(), context.AccessDeniedPath);

                            var response = context.Response;
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.HandleResponse();

                            return Task.FromResult(response);
                        },
                        OnCreatingTicket = async context =>
                        {
                            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();

                            var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                            Log.Information("OnCreatingTicket: {@ResponseContent", await response.Content.ReadAsStringAsync());
                        }
                    };
                })
                .AddGoogle("Google", "Google", googleOptions =>
                {
                    googleOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    googleOptions.ClientId = _appSettings.Authentication.Google.ClientId;
                    googleOptions.ClientSecret = _appSettings.Authentication.Google.ClientSecret;

                    googleOptions.SaveTokens = true;

                    googleOptions.Events = new OAuthEvents
                    {
                        OnRemoteFailure = context =>
                        {
                            Log.Error(context.Failure, "{@logMessageHeader} - Google.OnRemoteFailure [Failure Message]: {@Message}", context.Request.LogMessageHeader(), context.Failure?.Message);

                            var response = context.Response;
                            response.StatusCode = (int)HttpStatusCode.FailedDependency;
                            context.HandleResponse();

                            return Task.FromResult(response);
                        },
                        OnAccessDenied = context =>
                        {
                            Log.Warning("{@logMessageHeader} - Google.OnAccessDenied [Access Denied Path]: {@Path}", context.Request.LogMessageHeader(), context.AccessDeniedPath);

                            var response = context.Response;
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.HandleResponse();

                            return Task.FromResult(response);
                        },
                        OnCreatingTicket = async context =>
                        {
                            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();

                            var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                            Log.Information("OnCreatingTicket: {@ResponseContent", await response.Content.ReadAsStringAsync());
                        }
                    };
                })
                .AddGitHub("GitHub", "GitHub", githubOptions =>
                {
                    githubOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    githubOptions.ClientId = _appSettings.Authentication.GitHub.ClientId;
                    githubOptions.ClientSecret = _appSettings.Authentication.GitHub.ClientSecret;

                    githubOptions.SaveTokens = true;

                    githubOptions.Events = new OAuthEvents
                    {
                        OnRemoteFailure = context =>
                        {
                            Log.Error(context.Failure, "{@logMessageHeader} - GitHub.OnRemoteFailure [Failure Message]: {@Message}", context.Request.LogMessageHeader(), context.Failure?.Message);

                            var response = context.Response;
                            response.StatusCode = (int)HttpStatusCode.FailedDependency;
                            context.HandleResponse();

                            return Task.FromResult(response);
                        },
                        OnAccessDenied = context =>
                        {
                            Log.Warning("{@logMessageHeader} - GitHub.OnAccessDenied [Access Denied Path]: {@Path}", context.Request.LogMessageHeader(), context.AccessDeniedPath);

                            var response = context.Response;
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.HandleResponse();

                            return Task.FromResult(response);
                        },
                        OnCreatingTicket = async context =>
                        {
                            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();

                            var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                            Log.Information("OnCreatingTicket: {@ResponseContent", await response.Content.ReadAsStringAsync());
                        }
                    };
                });
                //.AddOAuth("Universeodon", "Universeodon", universeodonOptions =>
                // {
                //     universeodonOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                //     universeodonOptions.ClientId = _appSettings.Authentication.Universeodon.ClientId;
                //     universeodonOptions.ClientSecret = _appSettings.Authentication.Universeodon.ClientSecret;
                //     universeodonOptions.CallbackPath = new PathString("/signin-universeodon");

                //     universeodonOptions.AuthorizationEndpoint = _appSettings.Authentication.Universeodon.AuthorizationEndpoint;
                //     universeodonOptions.TokenEndpoint = _appSettings.Authentication.Universeodon.TokenEndpoint;
                //     universeodonOptions.UserInformationEndpoint = _appSettings.Authentication.Universeodon.UserInformationEndpoint;

                //     universeodonOptions.SaveTokens = true;
                //     universeodonOptions.Scope.Add("read:accounts");

                //     universeodonOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                //     universeodonOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                //     universeodonOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

                //     universeodonOptions.Events = new OAuthEvents
                //     {
                //         OnRemoteFailure = context =>
                //         {
                //             Log.Error(context.Failure, "{@logMessageHeader} - Universeodon.OnRemoteFailure [Failure Message]: {@Message}", context.Request.LogMessageHeader(), context.Failure?.Message);

                //             var response = context.Response;
                //             response.StatusCode = (int)HttpStatusCode.FailedDependency;
                //             context.HandleResponse();

                //             return Task.FromResult(response);
                //         },
                //         OnAccessDenied = context =>
                //         {
                //             Log.Warning("{@logMessageHeader} - Universeodon.OnAccessDenied [Access Denied Path]: {@Path}", context.Request.LogMessageHeader(), context.AccessDeniedPath);

                //             var response = context.Response;
                //             response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //             context.HandleResponse();

                //             return Task.FromResult(response);
                //         },
                //         OnCreatingTicket = async context =>
                //         {
                //             var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                //             request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //             request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                //             var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                //             response.EnsureSuccessStatusCode();

                //             var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                //             Log.Information("OnCreatingTicket: {@ResponseContent", await response.Content.ReadAsStringAsync());

                //             var identifier = user.Value<string>("id")?.Clean();
                //             if (!string.IsNullOrEmpty(identifier))
                //             {
                //                 context.Identity?.AddClaim(new Claim(
                //                     ClaimTypes.NameIdentifier, identifier,
                //                     ClaimValueTypes.String, context.Options.ClaimsIssuer));
                //             }

                //             var userName = user.Value<string>("display_name")?.Clean();
                //             if (!string.IsNullOrEmpty(userName))
                //             {
                //                 context.Identity?.AddClaim(new Claim(
                //                     ClaimTypes.Name, userName,
                //                     ClaimValueTypes.String, context.Options.ClaimsIssuer));
                //             }
                //         }
                //     };
                // });

            builder.Services.AddLocalApiAuthentication();

            builder.Services.AddTransient<IEventSink, CustomEventService>();

            return builder.Build();
        }

        /// <summary>
        /// Method used to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">WebApplication</param>
        /// <returns>Task&lt;WebApplication&gt;</returns>
        /// <method>ConfigurePipeline(this WebApplication app)</method>
        public static async Task<WebApplication> ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseExceptionHandler("/Home/Error/500");
            app.UseStatusCodePages(subApp =>
            {
                subApp.Run(async context =>
                {
                    int statusCode = context.Response.StatusCode;
                    string requestPath = (context.Request.Path.Value ?? string.Empty).Clean();

                    if (!requestPath.StartsWith("/connect/", StringComparison.OrdinalIgnoreCase)
                     && !requestPath.StartsWith("/localapi/", StringComparison.OrdinalIgnoreCase)
                     && !requestPath.StartsWith("/.well-known/", StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        context.Response.Redirect($"/Home/Error/{statusCode}?RequestPath={requestPath}");
                        await context.Response.StartAsync().ConfigureAwait(false);
                    }
                });
            });

            // Add HSTS and HTTPS Redirect to pipeline
            if (_appSettings != null && _appSettings.IsProduction)
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            // Localization support
            List<CultureInfo> supportedCultures = CultureHelper.GetSupportedCultures()
                .Select(culture => new CultureInfo(culture))
                .ToList();

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(CultureHelper.GetDefaultCulture()),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            // Perform Migrations
            foreach (string message in await ApplicationDbContext.Migrate(app))
                Log.Information(message);

            // Initialize database
            foreach (KeyValuePair<LogLevel, string> message in await ApplicationDbContext.InitializeDatabase(app))
                switch(message.Key)
                {
                    case LogLevel.Information:
                        Log.Information(message.Value);
                        break;
                    case LogLevel.Warning:
                        Log.Warning(message.Value);
                        break;
                }

            // Backup Database
            ApplicationDbBackup.Run(app);

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            return app;
        }
    }
}
