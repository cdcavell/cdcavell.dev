using IdentityModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace System.Security.Claims
{
    /// <summary>
    /// Extension methods for existing ClaimsPrincipal types.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/31/2022 | User Role Claims Development |~ 
    /// </revision>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Method to return standard log message header
        /// </summary>
        /// <param name="claimsPrincipal">this ClaimsPrincipal</param>
        /// <returns>string</returns>
        /// <method>LogMessageHeader(this ClaimsPrincipal claimsPrincipal)</method>
        public static string LogMessageHeader(this ClaimsPrincipal claimsPrincipal)
        {
            List<Claim> claims = claimsPrincipal.Claims.ToList();

            string clientId = claims
                .Where(x => x.Type.Equals("client_id", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value?.ToString())
                .FirstOrDefault() ?? string.Empty;

            string name = claims
               .Where(x => x.Type.Equals(JwtClaimTypes.Name))
               .Select(x => x.Value?.ToString())
               .FirstOrDefault() ?? string.Empty;

            string email = claims
               .Where(x => x.Type.Equals(JwtClaimTypes.Email))
               .Select(x => x.Value?.ToString())
               .FirstOrDefault() ?? string.Empty;

            string logMessageHeader = $"";
            if (!string.IsNullOrEmpty(clientId))
                logMessageHeader += $" [Client Id]: {clientId}";
            if (!string.IsNullOrEmpty(name))
                logMessageHeader += $" [User Name]: {name}";
            if (!string.IsNullOrEmpty(email))
                logMessageHeader += $" [User Email]: {email}";

            return logMessageHeader;
        }

        /// <summary>
        /// Method to return if User.Claims has given role value
        /// </summary>
        /// <param name="claimsPrincipal">this ClaimsPrincipal</param>
        /// <param name="roleValues">string</param>
        /// <returns>bool</returns>
        /// <method>HasRoleValue(this ClaimsPrincipal claimsPrincipal, string roleValue)</method>
        public static bool HasRoleValue(this ClaimsPrincipal claimsPrincipal, string roleValues)
        {
            if (string.IsNullOrEmpty(roleValues.Clean())) return false;

            var checkRoleValues = roleValues.Clean().ToLower().Split(',');
            foreach (string checkRoleValue in checkRoleValues)
                if (claimsPrincipal.Claims
                    .Where(x => x.Type.Equals(JwtClaimTypes.Role))
                    .Where(x => x.Value.Equals(checkRoleValue, StringComparison.OrdinalIgnoreCase))
                    .Any()
                ) return true;

            return false;
        }

        /// <summary>
        /// Method to return if User.Claims has given role value
        /// </summary>
        /// <param name="claimsPrincipal">this ClaimsPrincipal</param>
        /// <returns>string</returns>
        /// <method>GetEmail(this ClaimsPrincipal claimsPrincipal)</method>
        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            string email = claimsPrincipal.Claims
                .Where(x => 
                       x.Type == ClaimTypes.Email
                    || x.Type == JwtClaimTypes.Email
                )
                .Select(x => x.Value)
                .FirstOrDefault() ?? string.Empty;

            return email.Trim();
        }
    }
}
