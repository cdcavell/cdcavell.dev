using ClassLibrary.Mvc.Localization;
using ClassLibrary.Mvc.Services.AppSettings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace dis.cdcavell.dev.Controllers
{
    /// <summary>
    /// Home controller class
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.5.0 | 05/11/2023 | Game Development - Sudoku |~ 
    /// | Christopher D. Cavell | 1.0.4.0 | 02/25/2023 | User Role Claims Development |~ 
    /// | Christopher D. Cavell | 1.0.3.0 | 11/12/2022 | User Registration Development |~ 
    /// | Christopher D. Cavell | 1.0.2.0 | 10/01/2022 | Duende IdentityServer Development |~ 
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
        [AllowAnonymous]
        [HttpGet("{controller}/SignIn")]
        public IActionResult SignIn()
        {
            string clientUri = (_appSettings.Clients
                .Where(x => x.ClientId.Equals("usr.cdcavell.dev"))
                .Select(x => x.ClientUri)
                .FirstOrDefault() ?? string.Empty)
                .Trim('/').Trim('\\');

            if (string.IsNullOrEmpty(clientUri))
                return NotFound();

            return Redirect(clientUri + "/Home/SignIn");
        }

        /// <summary>
        /// SignedOff
        /// </summary>
        /// <returns>Task&lt;IActionResult&gt;</returns>
        /// <method>SignedOff()</method>
        [AllowAnonymous]
        [HttpGet("{controller}/SignedOff")]
        public async Task<IActionResult> SignedOff()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await HttpContext.SignOutAsync();
            }

            foreach (var cookie in Request.Cookies)
                Response.Cookies.Delete(cookie.Key);

            string clientUri = (_appSettings.Clients
                .Where(x => x.ClientId.Equals("cdcavell.dev"))
                .Select(x => x.ClientUri)
                .FirstOrDefault() ?? string.Empty)
                .Trim('/').Trim('\\');

            if (string.IsNullOrEmpty(clientUri))
                return NotFound();

            return Redirect(clientUri + "/Home/SignedOff");
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
            string clientUri = (_appSettings.Clients
                .Where(x => x.ClientId.Equals("cdcavell.dev"))
                .Select(x => x.ClientUri)
                .FirstOrDefault() ?? string.Empty)
                .Trim('/').Trim('\\');

            if (string.IsNullOrEmpty(clientUri))
                return NotFound();

            return Redirect(clientUri + "/Home/Index");
        }

        /// <summary>
        /// Public License method
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpGet("{controller}/License")]
        public IActionResult License()
        {
            string clientUri = (_appSettings.Clients
                .Where(x => x.ClientId.Equals("cdcavell.dev"))
                .Select(x => x.ClientUri)
                .FirstOrDefault() ?? string.Empty)
                .Trim('/').Trim('\\');

            if (string.IsNullOrEmpty(clientUri))
                return NotFound();

            return Redirect(clientUri + "/Home/License");
        }

        /// <summary>
        /// Public Culture method
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpGet("{controller}/Culture")]
        public IActionResult Culture()
        {
            string clientUri = (_appSettings.Clients
                .Where(x => x.ClientId.Equals("cdcavell.dev"))
                .Select(x => x.ClientUri)
                .FirstOrDefault() ?? string.Empty)
                .Trim('/').Trim('\\');

            if (string.IsNullOrEmpty(clientUri))
                return NotFound();

            return Redirect(clientUri + "/Home/Culture");
        }

        /// <summary>
        /// Public About method
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpGet("{controller}/About")]
        public IActionResult About()
        {
            string clientUri = (_appSettings.Clients
                .Where(x => x.ClientId.Equals("cdcavell.dev"))
                .Select(x => x.ClientUri)
                .FirstOrDefault() ?? string.Empty)
                .Trim('/').Trim('\\');

            if (string.IsNullOrEmpty(clientUri))
                return NotFound();

            return Redirect(clientUri + "/Home/About");
        }

        /// <summary>
        /// Public Games method
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpGet("{controller}/Games")]
        public IActionResult Games()
        {
            string clientUri = (_appSettings.Clients
                .Where(x => x.ClientId.Equals("gam.cdcavell.dev"))
                .Select(x => x.ClientUri)
                .FirstOrDefault() ?? string.Empty)
                .Trim('/').Trim('\\');

            if (string.IsNullOrEmpty(clientUri))
                return NotFound();

            return Redirect(clientUri + "/Home/Index");
        }
    }
}
