using ClassLibrary.Mvc.Controllers;
using ClassLibrary.Mvc.Http;
using ClassLibrary.Mvc.Localization;
using ClassLibrary.Mvc.Services.AppSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System.Net;
using usr.cdcavell.dev.Filters;
using usr.cdcavell.dev.Models;
using usr.cdcavell.dev.Models.AppSettings;

namespace usr.cdcavell.dev.Controllers
{
    /// <class>ApplicationBaseController</class>
    /// <summary>
    /// Base controller class for application
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/30/2022 | User Role Claims Development |~ 
    /// </revision>
    [ServiceFilter(typeof(SecurityHeadersAttribute))]
    public abstract partial class ApplicationBaseController<T> : WebBaseController<ApplicationBaseController<T>> where T : ApplicationBaseController<T>
    {
        /// <value>AppSettings</value>
        protected readonly AppSettings _appSettings;

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
        /// ApplicationBaseController(
        ///     ILogger&lt;T&gt; logger, 
        ///     IWebHostEnvironment webHostEnvironment, 
        ///     IHttpContextAccessor httpContextAccessor,
        ///     IAppSettingsService appSettingsService,
        ///     IStringLocalizer&lt;T&gt; localizer,
        ///     IStringLocalizer&lt;SharedResource&gt; sharedLocalizer
        /// ) : base(logger, webHostEnvironment, httpContextAccessor, localizer, sharedLocalizer)
        /// </method>
        protected ApplicationBaseController(
            ILogger<T> logger,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IAppSettingsService appSettingsService,
            IStringLocalizer<T> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer
        ) : base(logger, webHostEnvironment, httpContextAccessor, localizer, sharedLocalizer)
        {
            _appSettings = appSettingsService.ToObject<AppSettings>();
        }

        /// <summary>
        /// Called after the action method has been invoked
        /// </summary>
        /// <param name="context">ActionExecutedContext</param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.reCAPTCHA_SiteKey = _appSettings.Authentication.reCAPTCHA.SiteKey;
            base.OnActionExecuted(context);
        }

        /// <summary>
        /// Validate returned captcha token
        /// &lt;br /&gt;&lt;br /&gt;
        /// https://www.google.com/recaptcha/about/
        /// </summary>
        /// <param name="model">CaptchaInputModel</param>
        /// <returns>Task&lt;IActionResult&gt;</returns>
        /// <method>ValidateCaptchaToken(CaptchaInputModel model)</method>
        [AllowAnonymous]
        [HttpPost("{controller}/ValidateCaptchaToken")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidateCaptchaToken([Bind(CaptchaInputModel.BindProperties)] CaptchaInputModel model)
        {
            if (ModelState.IsValid)
            {
                string request = "siteverify";
                request += "?secret=" + _appSettings.Authentication.reCAPTCHA.SecretKey;
                request += "&response=" + model.CaptchaToken.Clean();
                request += "&remoteip=" + _remoteIPAddress?.MapToIPv4() ?? string.Empty;

                ApiClient client = new("https://www.google.com/recaptcha/api/");
                HttpStatusCode statusCode = await client.SendRawRequest(HttpMethod.Post, request);
                if (client.IsResponseSuccess)
                {
                    CaptchaResponse response = client.GetResponseObject<CaptchaResponse>() ?? new CaptchaResponse();
                    if (response.success)
                        if (response.action.Equals("submit", StringComparison.OrdinalIgnoreCase))
                            if (response.score > 0.6)
                                return Ok(client.GetResponseString());
                }

                return BadRequest(client.GetResponseString());
            }

            return InvalidModelState();
        }
    }
}
