using dis.cdcavell.dev.Models;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace dis.cdcavell.dev.Extensions
{
    /// <summary>
    /// Extension methods used within IdentityServer classes.
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.2.0 | 09/08/2022 | Duende IdentityServer Development |~ 
    /// </revision>
    public static class IdentityServerExtensions
    {
        /// <summary>
        /// Checks if the redirect URI is for a native client.
        /// </summary>
        /// <param name="context">AuthorizationRequest</param>
        /// <returns>bool</returns>
        public static bool IsNativeClient(this AuthorizationRequest context)
        {
            return !context.RedirectUri.StartsWith("https", StringComparison.Ordinal)
               && !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);
        }

        /// <summary>
        /// Checks if the redirect URI is for a native client.
        /// </summary>
        /// <param name="controller">Controller</param>
        /// <param name="viewName">string</param>
        /// <param name="redirectUri">string</param>
        /// <returns>bool</returns>
        public static IActionResult LoadingPage(this Controller controller, string viewName, string redirectUri)
        {
            controller.HttpContext.Response.StatusCode = 200;
            controller.HttpContext.Response.Headers["Location"] = "";

            return controller.View(viewName, new RedirectViewModel { RedirectUrl = redirectUri });
        }
    }
}
