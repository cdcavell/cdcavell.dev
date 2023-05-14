using ClassLibrary.Mvc.Localization;
using ClassLibrary.Mvc.Services.AppSettings;
using dis.cdcavell.dev.Models.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Net;

namespace dis.cdcavell.dev.Controllers
{
    /// <summary>
    /// Diagnostics controller class
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 01/21/2023 | User Role Claims Development |~ 
    /// | Christopher D. Cavell | 1.0.2.0 | 09/24/2022 | Duende IdentityServer Development |~ 
    /// </revision>
    public class DiagnosticsController : ApplicationBaseController<DiagnosticsController>
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
        /// public DiagnosticsController(
        ///     ILogger&lt;DiagnosticsController&gt; logger,
        ///     IWebHostEnvironment webHostEnvironment,
        ///     IHttpContextAccessor httpContextAccessor,
        ///     IAppSettingsService appSettingsService,
        ///     IStringLocalizer&lt;DiagnosticsController&gt; localizer,
        ///     IStringLocalizer&lt;SharedResource&gt; sharedLocalizer
        /// ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService, localizer, sharedLocalizer)
        /// </method>
        public DiagnosticsController(
            ILogger<DiagnosticsController> logger,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IAppSettingsService appSettingsService,
            IStringLocalizer<DiagnosticsController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer
        ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService, localizer, sharedLocalizer)
        {
        }

        /// <summary>
        /// Index action method
        /// </summary>
        /// <returns>Task&lt;IActionResult&gt;</returns>
        [HttpGet("Diagnostics")]
        [HttpGet("Diagnostics/Index")]
        [HttpGet("Diagnostics/Index/{id?}")]
        public async Task<IActionResult> Index()
        {
            IPAddress? localIpAddress = HttpContext.Connection.LocalIpAddress;
            if (localIpAddress == null)
                return NotFound();
            else
            {
                var localAddresses = new string[] { "127.0.0.1", "::1", localIpAddress.ToString() };
                if (!localAddresses.Contains(_remoteIPAddress?.ToString() ?? string.Empty))
                {
                    return NotFound();
                }

                var model = new DiagnosticsViewModel(await HttpContext.AuthenticateAsync());
                return View(model);
            }
        }
    }
}

