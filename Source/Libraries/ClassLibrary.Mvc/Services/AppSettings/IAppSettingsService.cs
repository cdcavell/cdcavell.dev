using ClassLibrary.Mvc.Services.AppSettings.Models;
using Duende.IdentityServer.Models;

namespace ClassLibrary.Mvc.Services.AppSettings
{
    /// <summary>
    /// AppSettings Web Service Interface
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/25/2022 | User Role Claims Development |~ 
    /// | Christopher D. Cavell | 1.0.2.1 | 10/15/2022 | Block Harassing IP Addresses |~ 
    /// | Christopher D. Cavell | 1.0.2.0 | 10/01/2022 | Duende IdentityServer Development |~ 
    /// | Christopher D. Cavell | 1.0.0.0 | 08/20/2022 | Initial Development |~ 
    /// </revision>
    public interface IAppSettingsService
    {
        /// <summary>
        /// Get AssemblyName value
        /// </summary>
        /// <returns>string</returns>
        public string AssemblyName();

        /// <summary>
        /// Get AssemblyVersion value
        /// </summary>
        /// <returns>string</returns>
        public string AssemblyVersion();

        /// <summary>
        /// Get LastModifiedDate value
        /// </summary>
        /// <returns>string</returns>
        public string LastModifiedDate();

        /// <summary>
        /// Get EnvironmentName value
        /// </summary>
        /// <returns>string</returns>
        public string EnvironmentName();

        /// <summary>
        /// Get IsDevelopment value
        /// </summary>
        /// <returns>bool</returns>
        public bool IsDevelopment();

        /// <summary>
        /// Get IsProduction value
        /// </summary>
        /// <returns>bool</returns>
        public bool IsProduction();

        /// <summary>
        /// Get ApiScopes object
        /// </summary>
        /// <returns>List&lt;ApiScope&gt;</returns>
        public List<ApiScope> GetApiScopes();

        /// <summary>
        /// Get Clients object
        /// </summary>
        /// <returns>List&lt;Client&gt;</returns>
        public List<Client> GetClients();

        /// <summary>
        /// Get raw json string value
        /// </summary>
        /// <returns>string</returns>
        public string ToJson();

        /// <summary>
        /// Get dynamic object
        /// </summary>
        /// <returns>T?</returns>
        /// <method>public T ToObject&lt;T&gt;()</method>
        public T ToObject<T>();

        /// <summary>
        /// Get if IPAddress is blocked
        /// </summary>
        /// <param name="ipAddress">string</param>
        /// <returns>bool</returns>
        public bool IsIPAddressBlocked(string ipAddress);

        /// <summary>
        /// Get Roles object
        /// </summary>
        /// <returns>List&lt;Role&gt;</returns>
        public List<Role> GetRoles();

        /// <summary>
        /// Get System Administrator object
        /// </summary>
        /// <returns>SysAdmin</returns>
        public SysAdmin GetSysAdmin();
    }
}
