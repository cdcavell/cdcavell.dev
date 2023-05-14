using dis.cdcavell.dev.Models.External;

namespace dis.cdcavell.dev.Models.Account
{
    /// <summary>
    /// Login View Model
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
    public class LoginViewModel : LoginInputModel
    {
        /// <value>bool</value>
        public bool AllowRememberLogin { get; set; } = true;
        /// <value>bool</value>
        public bool EnableLocalLogin { get; set; } = true;

        /// <value>IEnumerable&lt;ExternalProvider&gt;</value>
        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
        /// <value>IEnumerable&lt;ExternalProvider&gt;</value>
        public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName));

        /// <value>bool</value>
        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
        /// <value>string</value>
        public string? ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;
    }
}
