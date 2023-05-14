using Duende.IdentityServer.Models;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ClassLibrary.Mvc.Services.AppSettings.Models
{
    /// <summary>
    /// Application settings information.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/25/2022 | User Role Claims Development |~ 
    /// | Christopher D. Cavell | 1.0.3.0 | 11/12/2022 | User Registration Development |~ 
    /// | Christopher D. Cavell | 1.0.2.1 | 10/15/2022 | Block Harassing IP Addresses |~ 
    /// | Christopher D. Cavell | 1.0.2.0 | 10/01/2022 | Duende IdentityServer Development |~ 
    /// | Christopher D. Cavell | 1.0.0.0 | 08/20/2022 | Initial Development |~ 
    /// </revision>
    public abstract class AppSettings
    {
        private static readonly string _assemblyName = Assembly.GetEntryAssembly()?.GetName()?.Name ?? String.Empty;
        private static readonly string _assemblyVersion = Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? String.Empty;
        private static readonly string _assemblyLocation = Assembly.GetEntryAssembly()?.Location ?? String.Empty;
        private static readonly string _environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? String.Empty;
        private static readonly DateTime _lastModifiedDateTime = String.IsNullOrEmpty(_assemblyLocation) ? DateTime.MinValue : File.GetLastWriteTime(_assemblyLocation);

        /// <summary>
        /// Contstructor 
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        protected AppSettings(IConfiguration configuration)
        {
            if (configuration != null)
                configuration.Bind("AppSettings", this);
        }

        /// <value>string</value>
        public string AssemblyName { get; } = _assemblyName;

        /// <value>string</value>
        public string AssemblyVersion { get; } = _assemblyVersion;

        /// <value>string</value>
        public string EnvironmentName { get; } = _environmentName;

        /// <value>bool</value>
        public bool IsDevelopment { get; } = _environmentName.Equals("Development", StringComparison.OrdinalIgnoreCase);

        /// <value>bool</value>
        public bool IsProduction { get; } = _environmentName.Equals("Production", StringComparison.OrdinalIgnoreCase);

        /// <value>DateTime</value>
        public DateTime LastModifiedDateTime { get; } = _lastModifiedDateTime;

        /// <value>List&lt;Client&gt;</value>
        public List<Client> Clients { get; set; } = new();

        /// <value>List&lt;ApiScope&gt;</value>
        public List<ApiScope> ApiScopes { get; set; } = new();

        /// <value>List&lt;string&gt;</value>
        public List<string> BlockList { get; set; } = new();

        /// <value>List&lt;Role&gt;</value>
        public List<Role> Roles { get; set; } = new();

        /// <value>SysAdmin</value>
        public SysAdmin SysAdmin { get; set; } = new();
    }
}
