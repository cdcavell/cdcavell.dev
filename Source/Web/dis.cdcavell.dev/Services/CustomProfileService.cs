using ClassLibrary.Data;
using ClassLibrary.Data.Models;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;

namespace dis.cdcavell.dev.Services
{
    /// <summary>
    /// Class to retrieve user claims from a data source
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 02/20/2023 | User Role Claims Development |~ 
    /// | Christopher D. Cavell | 1.0.2.0 | 10/02/2022 | Duende IdentityServer Development |~ 
    /// </revision>
    public class CustomProfileService : IProfileService
    {
        private readonly ILogger<CustomProfileService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Constructor method.
        /// </summary>
        /// <param name="logger">ILogger&lt;CustomProfileService&gt;</param>
        /// <param name="httpContextAccessor">HttpContextAccessor</param>
        /// <param name="userClaimsPrincipalFactory">IUserClaimsPrincipalFactory&lt;ApplicationUser&gt;</param>
        /// <param name="userManager">UserManager&lt;ApplicationUser&gt;</param>
        /// <param name="dbContext">ApplicationDbContext</param>
        /// <method>
        /// CustomProfileService(
        ///     ILogger&lt;CustomProfileService&gt; logger,
        ///     IHttpContextAccessor httpContextAccessor,
        ///     IUserClaimsPrincipalFactory&lt;ApplicationUser&gt; userClaimsPrincipalFactory,
        ///     UserManager&lt;ApplicationUser&gt; userManager,
        ///     ApplicationDbContext dbContext
        /// )
        /// </method>
        public CustomProfileService(
            ILogger<CustomProfileService> logger,
            IHttpContextAccessor httpContextAccessor,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext
        )
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        /// <summary>
        /// This method adds claims that should go into the token to context. IssuedClaims
        /// </summary>
        /// <param name="context">ProfileDataRequestContext</param>
        /// <returns>Task</returns>
        /// <method>GetProfileDataAsync(ProfileDataRequestContext context)</method>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
            Client? client = context.Client;
            string logMessage = $"{_httpContextAccessor.HttpContext?.Request.LogMessageHeader()} - CustomProfileService.GetProfileDataAsync(ProfileDataRequestContext {nameof(context)}) [User Name]: {user?.Identity?.Name} [Claims]: {user?.Claims} [Client ID]: {client.ClientId}";

            string subjectId = context.Subject.GetSubjectId();
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(subjectId);

            context.IssuedClaims.AddRange(await GetUserClaims(applicationUser));
            foreach (Claim claim in context.IssuedClaims)
                _logger.LogDebug("{@logMessage} [User ID]: {@UserID} [Claim Type]: {@ClaimType} [Claim Value]: {@ClaimValue}", logMessage, applicationUser.UserName, claim.Type, claim.Value);
        }

        /// <summary>
        /// Returns if ApplicationUser is active or not
        /// </summary>
        /// <param name="context">ProfileDataRequestContext</param>
        /// <returns>Task</returns>
        /// <method>IsActiveAsync(IsActiveContext context)</method>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            string subjectId = context.Subject.GetSubjectId();
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(subjectId);
            context.IsActive = applicationUser != null;
        }

        private async Task<List<Claim>> GetUserClaims(ApplicationUser applicationUser)
        {
            List<Claim> userClaims = new();
            userClaims.Add(new Claim(JwtClaimTypes.Name, applicationUser.DisplayName));
            userClaims.Add(new Claim(JwtClaimTypes.Email, applicationUser.Email));

            List<Claim> applicationUserClaims = (await _userManager.GetClaimsAsync(applicationUser))
                .Where(x => x.Type == JwtClaimTypes.Role)
                .ToList();

            foreach (Claim claim in applicationUserClaims)
                userClaims.Add(claim);

            return userClaims;
        }
    }
}
