using ClassLibrary.Data;
using ClassLibrary.Data.Models;
using ClassLibrary.Mvc.Localization;
using ClassLibrary.Mvc.Services.AppSettings;
using dis.cdcavell.dev.Models.AppSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using static Duende.IdentityServer.IdentityServerConstants;

namespace dis.cdcavell.dev.Apis
{
    /// <class>ApiBaseController</class>
    /// <summary>
    /// Base controller class for application apis
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/26/2022 | User Role Claims Development |~ 
    /// </revision>
    [ApiController]
    [Route("/Api/{controller}/{action}")]
    [Authorize(LocalApi.PolicyName)]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public abstract partial class ApiBaseController<T> : Controller where T : ControllerBase
    {
        /// <value>ILogger</value>
        protected readonly ILogger _logger;
        /// <value>IWebHostEnvironment</value>
        protected readonly IWebHostEnvironment _webHostEnvironment;
        /// <value>IWebHostEnvironment</value>
        protected readonly IHttpContextAccessor _httpContextAccessor;
        /// <value>IStringLocalizer</value>
        protected readonly IStringLocalizer<T> _localizer;
        /// <value>IStringLocalizer&lt;SharedResource&gt;</value>
        protected readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        /// <value>AppSettings</value>
        protected readonly AppSettings _appSettings;
        /// <value>RoleManager&lt;IdentityRole&gt;</value>
        protected readonly RoleManager<IdentityRole> _roleManager;
        /// <value>UserManager&lt;ApplicationUser&gt;</value>
        protected readonly UserManager<ApplicationUser> _userManager;

        /// <value>string</value>
        protected string _logMessage = string.Empty;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="logger">ILogger&lt;T&gt;</param>
        /// <param name="webHostEnvironment">IWebHostEnvironment</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        /// <param name="appSettingsService">IAppSettingsService</param>
        /// <param name="localizer">IStringLocalizer&lt;T&gt;</param>
        /// <param name="sharedLocalizer">IStringLocalizer&lt;SharedResource&gt;</param>
        /// <param name="roleManager">RoleManager&lt;IdentityRole&gt;</param>
        /// <param name="userManager">UserManager&lt;ApplicationUser&gt;</param>
        /// <method>
        /// ApiBaseController(
        ///     ILogger&lt;T&gt; logger, 
        ///     IWebHostEnvironment webHostEnvironment, 
        ///     IHttpContextAccessor httpContextAccessor,
        ///     IAppSettingsService appSettingsService,
        ///     IStringLocalizer&lt;T&gt; localizer,
        ///     IStringLocalizer&lt;SharedResource&gt; sharedLocalizer,
        ///     RoleManager&lt;IdentityRole&gt; roleManager,
        ///     UserManager&lt;ApplicationUser&gt; userManager
        /// )
        /// </method>
        public ApiBaseController(
            ILogger<T> logger,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IAppSettingsService appSettingsService,
            IStringLocalizer<T> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager
        ) : base()
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
            _appSettings = appSettingsService.ToObject<AppSettings>();
            _roleManager = roleManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Called before the action method is invoked
        /// </summary>
        /// <param name="context">ActionExecutingContext</param>
        /// <method>OnActionExecuting(ActionExecutingContext context)</method>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logMessage = $"{_httpContextAccessor.HttpContext?.Request.LogMessageHeader()}";
            _logMessage += $" {User.LogMessageHeader()}";

            _logger.LogTrace("{@logMessage}", _logMessage);
        }

        /// <summary>
        /// Called to log and return exception
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <method>LogException(Exception exception)</method>
        public IActionResult LogException(Exception exception)
        {
            _logger.LogError(exception, "{@logMessage} - Exception", _logMessage);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
