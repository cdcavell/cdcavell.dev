namespace dis.cdcavell.dev.Options
{
    /// <summary>
    /// Account Options
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.3.0 | 11/11/2022 | User Registration Development |~ 
    /// | Christopher D. Cavell | 1.0.2.0 | 10/08/2022 | Duende IdentityServer Development |~ 
    /// </revision>
    public class AccountOptions
    {
        private static bool allowLocalLogin = false;
        private static bool allowRememberLogin = true;
        private static TimeSpan rememberMeLoginDuration = TimeSpan.FromDays(30);
        private static bool showLogoutPrompt = false;
        private static bool automaticRedirectAfterSignOut = true;
        private static string invalidCredentialsErrorMessage = "Invalid email address or password";

        /// <value>bool</value>
        public static bool AllowLocalLogin { get => allowLocalLogin; set => allowLocalLogin = value; }
        /// <value>bool</value>
        public static bool AllowRememberLogin { get => allowRememberLogin; set => allowRememberLogin = value; }
        /// <value>TimeSpan</value>
        public static TimeSpan RememberMeLoginDuration { get => rememberMeLoginDuration; set => rememberMeLoginDuration = value; }

        /// <value>bool</value>
        public static bool ShowLogoutPrompt { get => showLogoutPrompt; set => showLogoutPrompt = value; }
        /// <value>bool</value>
        public static bool AutomaticRedirectAfterSignOut { get => automaticRedirectAfterSignOut; set => automaticRedirectAfterSignOut = value; }

        /// <value>string</value>
        public static string InvalidCredentialsErrorMessage { get => invalidCredentialsErrorMessage; set => invalidCredentialsErrorMessage = value; }
    }
}
