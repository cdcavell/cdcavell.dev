using ClassLibrary.Mvc.Http;
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
using System.Net;

namespace gam.cdcavell.dev.Controllers
{
	/// <summary>
	/// MVC Sudoku 4 by AmosLong&lt;br /Gt;&lt;br /gt;
	/// &lt;hr /&gt; https://www.codeproject.com/Articles/1119451/MVC-Sudoku &lt;hr /&gt;
	/// </summary>
	/// <revision>
	/// __Revisions:__~~
	/// | Contributor | Build | Revison Date | Description |~
	/// |-------------|-------|--------------|-------------|~
	/// | Christopher D. Cavell | 1.0.5.0 | 05/12/2023 | Game Development - Sudoku |~ 
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
        /// Public Index method
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <method>Index()</method>
        [AllowAnonymous]
        [HttpGet("/")]
        [HttpGet("{controller}/")]
        [HttpGet("{controller}/Index")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// SignIn method
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <method>SignIn()</method>
        [HttpGet("{controller}/SignIn")]
        public IActionResult SignIn()
        {
            return RedirectToAction("AccountInformation", "Home");
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
        /// <method>SignedOff()</method>
        [AllowAnonymous]
        [HttpGet("{controller}/SignedOff")]
        public IActionResult SignedOff()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("SignOff", "Home");
            }

            string authorityUri = _appSettings.Authentication.IdP.Authority
                .Trim('/').Trim('\\');

            if (string.IsNullOrEmpty(authorityUri))
                return NotFound();

            return Redirect(authorityUri + "/Home/SignedOff");
        }

        /// <summary>
        /// Public License method
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <method>License()</method>
        [AllowAnonymous]
        [HttpGet("{controller}/License")]
        public IActionResult License()
        {
            string authorityUri = _appSettings.Authentication.IdP.Authority
                .Trim('/').Trim('\\');

            if (string.IsNullOrEmpty(authorityUri))
                return NotFound();

            return Redirect(authorityUri + "/Home/License");
        }

        /// <summary>
        /// Public Culture method
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <method>Culture()</method>
        [AllowAnonymous]
        [HttpGet("{controller}/Culture")]
        public IActionResult Culture()
        {
            string authorityUri = _appSettings.Authentication.IdP.Authority
                .Trim('/').Trim('\\');

            if (string.IsNullOrEmpty(authorityUri))
                return NotFound();

            return Redirect(authorityUri + "/Home/Culture");
        }

        /// <summary>
        /// Public About method
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <method>About()</method>
        [AllowAnonymous]
        [HttpGet("{controller}/About")]
        public IActionResult About()
        {
            string authorityUri = _appSettings.Authentication.IdP.Authority
                .Trim('/').Trim('\\');

            if (string.IsNullOrEmpty(authorityUri))
                return NotFound();

            return Redirect(authorityUri + "/Home/About");
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
