using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Mvc.Authorization
{
    /// <summary>
    /// Class to set absolute authentication cookie lifetime. After a given period 
    /// user won't be able to extend it anymore and will be kicked from the app.
    /// &lt;br/&gt;&lt;br/&gt;
    /// https://brokul.dev/authentication-cookie-lifetime-and-sliding-expiration
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/28/2022 | User Role Claims Development |~ 
    /// </revision>
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private const string TicketIssuedTicks = nameof(TicketIssuedTicks);

        /// <summary>
        /// Signing In Method
        /// </summary>
        /// <param name="context">CookieSigningInContext</param>
        /// <method>SigningIn(CookieSigningInContext context)</method>
        public override async Task SigningIn(CookieSigningInContext context)
        {
            context.Properties.SetString(
                TicketIssuedTicks,
                DateTimeOffset.UtcNow.Ticks.ToString());

            await base.SigningIn(context);
        }

        /// <summary>
        /// Validate Principal Method
        /// </summary>
        /// <param name="context">CookieValidatePrincipalContext</param>
        /// <returns>WebApplication</returns>
        /// <method>ValidatePrincipal(CookieValidatePrincipalContext context)</method>
        public override async Task ValidatePrincipal(
            CookieValidatePrincipalContext context)
        {
            var ticketIssuedTicksValue = context
                .Properties.GetString(TicketIssuedTicks);

            if (ticketIssuedTicksValue is null ||
                !long.TryParse(ticketIssuedTicksValue, out var ticketIssuedTicks))
            {
                await RejectPrincipalAsync(context);
                return;
            }

            var ticketIssuedUtc =
                new DateTimeOffset(ticketIssuedTicks, TimeSpan.FromHours(0));

            if (DateTimeOffset.UtcNow - ticketIssuedUtc > TimeSpan.FromMinutes(15))
            {
                await RejectPrincipalAsync(context);
                return;
            }

            await base.ValidatePrincipal(context);
        }

        private static async Task RejectPrincipalAsync(
            CookieValidatePrincipalContext context)
        {
            context.RejectPrincipal();
            await context.HttpContext.SignOutAsync();
        }
    }
}
