using ClassLibrary.Mvc.Localization;
using ClassLibrary.Mvc.Services.AppSettings;
using gam.cdcavell.dev.DataLayer.Sudoku;
using gam.cdcavell.dev.Filters;
using gam.cdcavell.dev.Models.AppSettings;
using gam.cdcavell.dev.Services;
using gam.cdcavell.dev.Services.Sudoku;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Serilog;
using System.Globalization;
using System.Net;
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
	/// | Christopher D. Cavell | 1.0.5.0 | 05/12/2023 | Game Development - Sudoku |~ 
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

            // cache authority discovery and add to DI
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IDiscoveryCache>(options =>
            {
                var factory = options.GetRequiredService<IHttpClientFactory>();
                return new DiscoveryCache(_appSettings.Authentication.IdP.Authority, () => factory.CreateClient());
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, cookieOptions => {
                    cookieOptions.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = (int)(HttpStatusCode.Unauthorized);
                        return Task.CompletedTask;
                    };
                    cookieOptions.Events.OnSigningOut = context =>
                    {
                        context.CookieOptions.Expires = DateTime.Now.AddDays(-1);
                        return Task.CompletedTask;
                    };
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                    options.Authority = _appSettings.Authentication.IdP.Authority;
                    options.RequireHttpsMetadata = true;

                    options.ClientId = _appSettings.Authentication.IdP.ClientId;
                    options.ClientSecret = _appSettings.Authentication.IdP.ClientSecret;
                    options.ResponseType = OidcConstants.ResponseTypes.Code;
                    options.ResponseMode = OidcConstants.ResponseModes.FormPost;
                    options.UsePkce = true;

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.Scope.Add("IdentityServerApi");

                    // This aligns the life of the cookie with the life of the token.
                    // Note this is not the actual expiration of the cookie as seen by the browser.
                    // It is an internal value stored in "expires_at".
                    options.UseTokenLifetime = false;
                    options.SaveTokens = true;

                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Events = new OpenIdConnectEvents
                    {
                        OnTicketReceived = context =>
                        {
                            Log.Debug("{@logMessageHeader} - OnTicketReceived({@context})",
                                context.HttpContext.Request.LogMessageHeader(),
                                nameof(context)
                            );

                            if (context.Principal?.Claims != null)
                                foreach (var claim in context.Principal.Claims)
                                    Log.Debug("{@logMessageHeader} - [User Name]: {@userName} [Claim Type]: {@claimType} [Claim Value]: {@claimValue}",
                                        context.HttpContext.Request.LogMessageHeader(),
                                        context.Principal.Identity?.Name,
                                        claim.Type,
                                        claim.Value
                                    );

                            string bearerToken = context.Properties?.Items
                                .Where(x => x.Key == ".Token.access_token")
                                .Select(x => x.Value)
                                .FirstOrDefault() ?? string.Empty;

                            List<Claim> claims = new()
                            {
                                new Claim("BearerToken", bearerToken)
                            };
                            ClaimsIdentity claimsIdentity = new(claims);

                            context.Principal?.AddIdentity(claimsIdentity);

                            return Task.FromResult(context.Result);
                        },
                        OnTokenValidated = context =>
                        {
                            Log.Debug("{@logMessageHeader} - OnTokenValidated({@context})",
                                context.HttpContext.Request.LogMessageHeader(),
                                nameof(context)
                            );

                            return Task.FromResult(context.Result);
                        },
                        OnAccessDenied = context =>
                        {
                            Log.Warning("{@logMessageHeader} - OnAccessDenied({@context}) [AccessDeniedPath]: {@path}",
                                context.HttpContext.Request.LogMessageHeader(),
                                nameof(context),
                                context.AccessDeniedPath.Value
                            );

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.HandleResponse();

                            return Task.FromResult(context.Result);
                        },
                        OnAuthenticationFailed = context =>
                        {
                            Log.Error(context.Exception, "{@logMessageHeader} - OnAuthenticationFailed({@context})",
                                context.HttpContext.Request.LogMessageHeader(),
                                nameof(context)
                            );

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.HandleResponse();

                            return Task.FromResult(context.Result);
                        }
                    };
                });

            // Register Sudoku Interfaces
			builder.Services.AddScoped<IPuzzleRepository, XmlFilePuzzleRepository>();
			builder.Services.AddScoped<IPuzzleLoader, XDocPuzzleLoader>();
			builder.Services.AddScoped<IPuzzleSaver, XDocPuzzleSaver>();
			builder.Services.AddScoped<IPuzzleService, PuzzleService>();

			return builder.Build();
        }

        /// <summary>
        /// Method used to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">WebApplication</param>
        /// <returns>WebApplication</returns>
        /// <method>ConfigurePipeline(this WebApplication app)</method>
        public static WebApplication ConfigurePipeline(this WebApplication app)
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
                    context.Response.Redirect($"/Home/Error/{statusCode}?RequestPath={requestPath}");
                    await context.Response.StartAsync().ConfigureAwait(false);
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

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapControllers();

			return app;
        }
	}
}
