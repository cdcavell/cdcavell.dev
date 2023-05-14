using ClassLibrary.Data;
using ClassLibrary.Mvc.Services.AppSettings;
using dis.cdcavell.dev.Models.AppSettings;
using dis.cdcavell.dev.Services;
using Duende.IdentityServer.Validation;

namespace dis.cdcavell.dev.Validators
{
    /// <summary>
    /// Custom validation of token request
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.2.0 | 09/24/2022 | Duende IdentityServer Development |~ 
    /// </revision>
    public class CustomTokenRequestValidator : ICustomTokenRequestValidator
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CustomTokenRequestValidator> _logger;
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Constructor method.
        /// </summary>
        /// <param name="logger">ILogger&lt;CustomTokenRequestValidator&gt;</param>
        /// <param name="httpContextAccessor">HttpContextAccessor</param>
        /// <param name="dbContext">ApplicationDbContext</param>
        /// <param name="appSettingsService">IAppSetingsService</param>
        /// <method>
        /// CustomTokenRequestValidator(
        ///     ILogger&lt;CustomTokenRequestValidator&gt; logger,
        ///     IHttpContextAccessor httpContextAccessor,
        ///     ApplicationDbContext dbContext,
        ///     IAppSetingsService appSettingService
        /// )
        /// </method>
        public CustomTokenRequestValidator(
            ILogger<CustomTokenRequestValidator> logger,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext dbContext,
            IAppSettingsService appSettingsService
        )
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _appSettings = appSettingsService.ToObject<AppSettings>();
        }

        /// <summary>
        /// Validation of token request
        /// </summary>
        /// <param name="context">CustomTokenRequestValidationContext</param>
        /// <returns>Task</returns>
        /// <method>ValidateAsync(CustomTokenRequestValidationContext context)</method>
        public Task ValidateAsync(CustomTokenRequestValidationContext context)
        {
            //TODO: Validation of token request logic

            return Task.FromResult(context.Result);
        }
    }
}
