using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace ClassLibrary.Mvc.Authorization
{
    /// <summary>
    /// Custom  authorization filter
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/26/2022 | User Role Claims Development |~ 
    /// </revision>
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly List<string> _permissions;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="logger">ILogger&lt;CustomAuthorizeFilter&gt;</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        /// <param name="permissions">List&lt;string&gt;</param>
        /// <method>CustomAuthorizeFilter(ILogger&lt;CustomAuthorizeFilter&gt; logger, IHttpContextAccessor httpContextAccessor, List&lt;string&gt; permissions)</method>
        public CustomAuthorizeFilter(ILogger<CustomAuthorizeFilter> logger, IHttpContextAccessor httpContextAccessor,  List<string> permissions)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _permissions = permissions;
        }

        /// <summary>
        /// Validation method
        /// </summary>
        /// <param name="context">AuthorizationFilterContext</param>
        /// <method>OnAuthorization(AuthorizationFilterContext context)</method>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            HttpRequest request = context.HttpContext.Request;
            ClaimsPrincipal user = context.HttpContext.User;

            if (_permissions.Any())
            {
                if (user != null)
                    if (user.Identity != null)
                        foreach (string item in _permissions)
                            if (user.HasRoleValue(item))
                            {
                                _logger.LogDebug("{@requestMessage} {@userMessage} - Authorized [Role]: {@item}", request.LogMessageHeader(), user?.LogMessageHeader(), item);
                                return;
                            }
            }

            context.Result = new UnauthorizedResult();
            _logger.LogWarning("{@requestMessage} {@userMessage} - Unauthorized, no valid role", request.LogMessageHeader(), user?.LogMessageHeader());
        }
    }
}
