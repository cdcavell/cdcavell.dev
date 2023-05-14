using ClassLibrary.Data.Models;
using ClassLibrary.Mvc.Localization;
using ClassLibrary.Mvc.Services.AppSettings;
using dis.cdcavell.dev.Models.Apis.User;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace dis.cdcavell.dev.Apis
{
    /// <class>UserController</class>
    /// <summary>
    /// Custom IdentityServer API Endpoints
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 02/21/2023 | User Role Claims Development |~ 
    /// </revision>
    public class UserController : ApiBaseController<UserController>
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="logger">ILogger&lt;UserController&gt;</param>
        /// <param name="webHostEnvironment">IWebHostEnvironment</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        /// <param name="appSettingsService">IAppSettingsService</param>
        /// <param name="localizer">IStringLocalizer&lt;UserController&gt;</param>
        /// <param name="sharedLocalizer">IStringLocalizer&lt;SharedResource&gt;</param>
        /// <param name="roleManager">RoleManager&lt;IdentityRole&gt;</param>
        /// <param name="userManager">UserManager&lt;ApplicationUser&gt;</param>
        /// <method>
        /// ApplicationBaseController(
        ///     ILogger&lt;UserController&gt; logger, 
        ///     IWebHostEnvironment webHostEnvironment, 
        ///     IHttpContextAccessor httpContextAccessor,
        ///     IAppSettingsService appSettingsService,
        ///     IStringLocalizer&lt;UserController&gt; localizer,
        ///     IStringLocalizer&lt;SharedResource&gt; sharedLocalizer,
        ///     RoleManager&lt;IdentityRole&gt; roleManager,
        ///     UserManager&lt;ApplicationUser&gt; userManager
        /// ) : base(
        ///    logger,
        ///    webHostEnvironment,
        ///    httpContextAccessor,
        ///    appSettingsService,
        ///    localizer,
        ///    sharedLocalizer,
        ///    roleManager,
        ///    userManager
        /// )
        /// </method>
        public UserController(
            ILogger<UserController> logger,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IAppSettingsService appSettingsService,
            IStringLocalizer<UserController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager
        ) : base(
            logger,
            webHostEnvironment,
            httpContextAccessor,
            appSettingsService,
            localizer,
            sharedLocalizer,
            roleManager,
            userManager
        )
        {
        }

        /// <summary>
        /// Returns User Listing
        /// </summary>
        /// <returns>Task&lt;IActionResult&gt;</returns>
        /// <method>Listing()</method>
        [HttpGet()]
        public async Task<IActionResult> Listing()
        {
            try
            {
                string? subjectId = User.Claims
                    .FirstOrDefault(x => x.Type == JwtClaimTypes.Subject)?.Value.Clean();
                if (string.IsNullOrEmpty(subjectId))
                {
                    _logger.LogWarning("{logMessage} - SubjectId null or empty", _logMessage);
                    return Unauthorized();
                }

                ApplicationUser user = await _userManager.FindByIdAsync(subjectId);
                if (user == null)
                {
                    _logger.LogWarning("{logMessage} [SubjectId]: {subjectId} - ApplicationUser null", _logMessage, subjectId);
                    return Unauthorized();
                }

                List<string> roles = (await _userManager.GetClaimsAsync(user) ?? new List<Claim>())
                    .Where(x => x.Type == JwtClaimTypes.Role)
                    .Select(x => x.Value.Clean())
                    .ToList();

                UserRolesModel model = new()
                {
                    SubjectId = user.SubjectId,
                    Status = user.Status,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Roles = roles
                };

                return Ok(model);
            }
            catch (Exception exception)
            {
                return LogException(exception);
            }
        }
    }
}
