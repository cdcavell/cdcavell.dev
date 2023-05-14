namespace dis.cdcavell.dev.Options
{
    /// <summary>
    /// Consent Options
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
    public class ConsentOptions
    {
        private static bool enableOfflineAccess = true;
        private static string offlineAccessDisplayName = "Offline Access";
        private static string offlineAccessDescription = "Access to your applications and resources, even when you are offline";
        private static readonly string mustChooseOneErrorMessage = "You must pick at least one permission";
        private static readonly string invalidSelectionErrorMessage = "Invalid selection";

        /// <value>bool</value>
        public static bool EnableOfflineAccess { get => enableOfflineAccess; set => enableOfflineAccess = value; }
        /// <value>string</value>
        public static string OfflineAccessDisplayName { get => offlineAccessDisplayName; set => offlineAccessDisplayName = value; }
        /// <value>string</value>
        public static string OfflineAccessDescription { get => offlineAccessDescription; set => offlineAccessDescription = value; }


        /// <value>string</value>
        public static string MustChooseOneErrorMessage { get => mustChooseOneErrorMessage; }
        /// <value>string</value>
        public static string InvalidSelectionErrorMessage { get => invalidSelectionErrorMessage; }

    }
}
