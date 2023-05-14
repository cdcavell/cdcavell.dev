using ClassLibrary.Data;
using ClassLibrary.Data.Models;
using ClassLibrary.Mvc.Authorization;
using ClassLibrary.Mvc.Localization;
using ClassLibrary.Mvc.Services.AppSettings;
using dis.cdcavell.dev.Models.Apis.Role;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace dis.cdcavell.dev.Apis
{
    /// <class>RoleController</class>
    /// <summary>
    /// Custom IdentityServer API Endpoints
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 02/21/2023 | User Role Claims Development |~ 
    /// </revision>
    public class RoleController : ApiBaseController<RoleController>
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="logger">ILogger&lt;RoleController&gt;</param>
        /// <param name="webHostEnvironment">IWebHostEnvironment</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        /// <param name="appSettingsService">IAppSettingsService</param>
        /// <param name="localizer">IStringLocalizer&lt;RoleController&gt;</param>
        /// <param name="sharedLocalizer">IStringLocalizer&lt;SharedResource&gt;</param>
        /// <param name="roleManager">RoleManager&lt;IdentityRole&gt;</param>
        /// <param name="userManager">UserManager&lt;ApplicationUser&gt;</param>
        /// <method>
        /// ApplicationBaseController(
        ///     ILogger&lt;RoleController&gt; logger, 
        ///     IWebHostEnvironment webHostEnvironment, 
        ///     IHttpContextAccessor httpContextAccessor,
        ///     IAppSettingsService appSettingsService,
        ///     IStringLocalizer&lt;RoleController&gt; localizer,
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
        public RoleController(
            ILogger<RoleController> logger,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IAppSettingsService appSettingsService,
            IStringLocalizer<RoleController> localizer,
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
        /// Returns Role Listing
        /// </summary>
        /// <returns>Task&lt;IActionResult&gt;</returns>
        /// <method>Listing()</method>
        [HttpGet()]
        [CustomAuthorize("SysAdmin.View, UserAdmin.View")]
        public async Task<IActionResult> Listing()
        {
            try
            {
                List<RoleClaimsModel> roleClaims = new();

                List<IdentityRole> roles = _roleManager.Roles.ToList();
                foreach(IdentityRole role in roles)
                {
                    roleClaims.Add(new RoleClaimsModel()
                    {
                        Id = role.Id,
                        Name = role.Name,
                        NormalizedName = role.NormalizedName,
                        ConcurrencyStamp = role.ConcurrencyStamp,
                        Claims = (await _roleManager.GetClaimsAsync(role)).ToList()
                    });
                }

                return Ok(roleClaims);
            }
            catch (Exception exception)
            {
                return LogException(exception);
            }
        }
    }
}
