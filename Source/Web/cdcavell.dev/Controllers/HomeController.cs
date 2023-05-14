using cdcavell.dev.Models.Home;
using ClassLibrary.Mvc.Localization;
using ClassLibrary.Mvc.Services.AppSettings;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

namespace cdcavell.dev.Controllers
{
    /// <summary>
    /// Home controller class
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.5.0 | 05/12/2023 | Game Development - Sudoku |~ 
    /// | Christopher D. Cavell | 1.0.4.0 | 02/25/2023 | User Role Claims Development |~ 
    /// | Christopher D. Cavell | 1.0.3.0 | 12/12/2022 | User Registration Development |~ 
    /// | Christopher D. Cavell | 1.0.0.0 | 08/22/2022 | Initial Development |~ 
    /// </revision>
    public class HomeController : ApplicationBaseController<HomeController>
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="webHostEnvironment">IWebHostEnvironment</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        /// <param name="appSettingsService">IAppSettingsService</param>
        /// <param name="localizer">IStringLocalizer&lt;T&gt;</param>
        /// <param name="sharedLocalizer">IStringLocalizer&lt;SharedResource&gt;</param>
        /// <method>
        /// public HomeController(
        ///     ILogger&lt;HomeController&gt; logger,
        ///     IWebHostEnvironment webHostEnvironment,
        ///     IHttpContextAccessor httpContextAccessor,
        ///     IAppSettingsService appSettingsService,
        ///     IStringLocalizer&lt;HomeController&gt; localizer,
        ///     IStringLocalizer&lt;SharedResource&gt; sharedLocalizer
        /// ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService, localizer, sharedLocalizer)
        /// </method>
        public HomeController(
            ILogger<HomeController> logger,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IAppSettingsService appSettingsService,
            IStringLocalizer<HomeController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer
        ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService, localizer, sharedLocalizer)
        {
        }

        /// <summary>
        /// SignIn method
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <method>SignIn()</method>
        [HttpGet("{controller}/SignIn")]
        public IActionResult SignIn()
        {
            string authorityUri = _appSettings.Authentication.IdP.Authority
                .Trim('/').Trim('\\');

            if (string.IsNullOrEmpty(authorityUri))
                return NotFound();

            return Redirect(authorityUri + "/Home/SignIn");
        }

        /// <summary>
        /// SignOff method
        /// </summary>
        /// <returns>Task&lt;IActionResult&gt;</returns>
        /// <method>SignOff()</method>
        [AllowAnonymous]
        [HttpGet("{controller}/SignOff")]
        public async Task<IActionResult> SignOff()
        {
            // Instruct search engines not to index. https://developers.google.com/search/docs/crawling-indexing/block-indexing?visit_id=638042798023618216-3766845920&rd=1
            if (!HttpContext.Response.Headers.ContainsKey("X-Robots-Tag"))
            {
                HttpContext.Response.Headers.Add("X-Robots-Tag", "noindex");
            }

            string state = Request.Query
                .Where(x => x.Key == "state")
                .Select(x => x.Value)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(state))
            {
				if (HttpContext.RequestServices
					.GetService(typeof(IDiscoveryCache)) is DiscoveryCache discoveryCache)
				{
					DiscoveryDocumentResponse discovery = await discoveryCache.GetAsync();
					if (!discovery.IsError)
					{
						string idToken = (await HttpContext.GetTokenAsync("id_token")) ?? string.Empty;
						Dictionary<string, string?> parameters = new()
						{
							{ "id_token_hint", idToken },
							{ "post_logout_redirect_uri", $"{Request.Scheme}://{Request.Host}/Home/SignOff" },
							{ "state", "signOff" }
						};

						string endSessionEndpoint = QueryHelpers.AddQueryString(discovery.EndSessionEndpoint, parameters);
						return Redirect(endSessionEndpoint);
					}
					else
						_logger.LogWarning("{@logMessageHeader} [Error]: {@error}", _logMessageHeader, discovery.Error);
				}
			}

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            foreach (var cookie in Request.Cookies)
                Response.Cookies.Delete(cookie.Key);

            string authorityUri = _appSettings.Authentication.IdP.Authority
                .Trim('/').Trim('\\');

            if (string.IsNullOrEmpty(authorityUri))
                return NotFound();

            return Redirect(authorityUri + "/Home/SignedOff");
        }

        /// <summary>
        /// Front Channel SLO Logout method
        /// &lt;br /&gt;&lt;br /&gt;
        /// https://andersonnjen.com/2019/03/22/identityserver4-global-logout/
        /// </summary>
        /// <returns>Task&lt;IActionResult&gt;</returns>
        /// <method>FrontChannelLogout(string sid)</method>
        [AllowAnonymous]
        [HttpGet("{controller}/FrontChannelLogout")]
        public async Task<IActionResult> FrontChannelLogout(string sid)
        {
            // Instruct search engines not to index. https://developers.google.com/search/docs/crawling-indexing/block-indexing?visit_id=638042798023618216-3766845920&rd=1
            if (!HttpContext.Response.Headers.ContainsKey("X-Robots-Tag"))
            {
                HttpContext.Response.Headers.Add("X-Robots-Tag", "noindex");
            }

            if (User.Identity?.IsAuthenticated ?? false)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            }

            foreach (var cookie in Request.Cookies)
                Response.Cookies.Delete(cookie.Key);

            return NoContent();
        }

        /// <summary>
        /// Public SignedOff method
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpGet("{controller}/SignedOff")]
        public IActionResult SignedOff()
        {
            // Instruct search engines not to index. https://developers.google.com/search/docs/crawling-indexing/block-indexing?visit_id=638042798023618216-3766845920&rd=1
            if (!HttpContext.Response.Headers.ContainsKey("X-Robots-Tag"))
            {
                HttpContext.Response.Headers.Add("X-Robots-Tag", "noindex");
            }

            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("SignOff", "Home");
            }

            var vm = new SignedOffModel();
            string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            vm.RequestId = requestId;

            return View(vm);
        }

        /// <summary>
        /// Public Index method
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpGet("/")]
        [HttpGet("{controller}/")]
        [HttpGet("{controller}/Index")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Public License method
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpGet("{controller}/License")]
        public IActionResult License()
        {
            return View();
        }

        /// <summary>
        /// Public Culture method
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpGet("{controller}/Culture")]
        public IActionResult Culture()
        {
            return View();
        }

        /// <summary>
        /// Public About method
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpGet("{controller}/About")]
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Public Games method
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <method>Games()</method>
        [AllowAnonymous]
        [HttpGet("{controller}/Games")]
        public IActionResult Games()
        {
            string authorityUri = _appSettings.Authentication.IdP.Authority
                .Trim('/').Trim('\\');

            if (string.IsNullOrEmpty(authorityUri))
                return NotFound();

            return Redirect(authorityUri + "/Home/Games");
        }
    }
}
