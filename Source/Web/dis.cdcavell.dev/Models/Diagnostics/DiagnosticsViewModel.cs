using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.Text.Json;

namespace dis.cdcavell.dev.Models.Diagnostics
{
    /// <summary>
    /// Redirect link information.
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
    public class DiagnosticsViewModel
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="result">AuthenticateResult</param>
        public DiagnosticsViewModel(AuthenticateResult result)
        {
            AuthenticateResult = result;

            if (result.Properties != null)
                if (result.Properties.Items.ContainsKey("client_list"))
                {
                    var encoded = result.Properties.Items["client_list"];
                    var bytes = Base64Url.Decode(encoded);
                    var value = Encoding.UTF8.GetString(bytes);

                    IEnumerable<string>? clientList = JsonSerializer.Deserialize<string[]>(value);
                    if (clientList != null)
                        Clients = clientList;
                }
        }

        /// <value>AuthenticateResult</value>
        public AuthenticateResult AuthenticateResult { get; }

        /// <value>IEnumerable&lt;string&gt;</value>
        public IEnumerable<string> Clients { get; } = new List<string>();
    }
}
