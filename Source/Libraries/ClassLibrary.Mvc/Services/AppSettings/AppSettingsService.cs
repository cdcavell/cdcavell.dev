using ClassLibrary.Mvc.Services.AppSettings.Models;
using Duende.IdentityServer.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;

namespace ClassLibrary.Mvc.Services.AppSettings
{
    /// <summary>
    /// AppSettings Web Service
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
    public class AppSettingsService : IAppSettingsService
    {
        private readonly Models.AppSettings _appSettings;
        private readonly string _rawJson;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">IOptions&lt;AppSettingsServiceOptions&gt;</param>
        /// <method>UserAuthorizationService(IOptions&lt;UserAuthorizationServiceOptions&gt; options)</method>
        public AppSettingsService(IOptions<AppSettingsServiceOptions> options)
        {
            _appSettings = (Models.AppSettings)options.Value.AppSettings;
            _rawJson = JsonConvert.SerializeObject(options.Value.AppSettings);
        }

        /// <summary>
        /// Get AssemblyName value
        /// </summary>
        /// <returns>string</returns>
        public string AssemblyName()
        {
            return _appSettings.AssemblyName;
        }

        /// <summary>
        /// Get AssemblyVersion value
        /// </summary>
        /// <returns>string</returns>
        public string AssemblyVersion()
        {
            return _appSettings.AssemblyVersion;
        }

        /// <summary>
        /// Get LastModifiedDate value
        /// </summary>
        /// <returns>string</returns>
        public string LastModifiedDate()
        {
            return _appSettings.LastModifiedDateTime.ToString("MM/dd/yyyy");
        }

        /// <summary>
        /// Get EnvironmentName value
        /// </summary>
        /// <returns>string</returns>
        public string EnvironmentName()
        {
            return _appSettings.EnvironmentName;
        }

        /// <summary>
        /// Get IsDevelopment value
        /// </summary>
        /// <returns>bool</returns>
        public bool IsDevelopment()
        {
            return _appSettings.IsDevelopment;
        }

        /// <summary>
        /// Get IsProduction value
        /// </summary>
        /// <returns>bool</returns>
        public bool IsProduction()
        {
            return _appSettings.IsProduction;
        }

        /// <summary>
        /// Get ApiScopes object
        /// </summary>
        /// <returns>List&lt;ApiScope&gt;</returns>
        public List<ApiScope> GetApiScopes()
        {
            return _appSettings.ApiScopes;
        }

        /// <summary>
        /// Get Clients object
        /// </summary>
        /// <returns>List&lt;Client&gt;</returns>
        public List<Client> GetClients()
        {
            return _appSettings.Clients;
        }

        /// <summary>
        /// Get raw json string value
        /// </summary>
        /// <returns>string</returns>
        public string ToJson()
        {
            return _rawJson;
        }

        /// <summary>
        /// Get full json dynamic object
        /// </summary>
        /// <returns>T?</returns>
        /// <method>public T? ToObject&lt;T&gt;()</method>
        /// <exception>NullReferenceException</exception>
        public T ToObject<T>()
        {
            T? result = JsonConvert.DeserializeObject<T>(_rawJson);
            if (result == null)
                throw new NullReferenceException();

            return result;
        }

        /// <summary>
        /// Get if IPAddress is blocked
        /// </summary>
        /// <param name="ipAddress">string</param>
        /// <returns>bool</returns>
        public bool IsIPAddressBlocked(string ipAddress)
        {
            return _appSettings.BlockList.Any(s => ipAddress.Contains(s));
        }

        /// <summary>
        /// Get Roles object
        /// </summary>
        /// <returns>List&lt;Role&gt;</returns>
        public List<Role> GetRoles()
        {
            return _appSettings.Roles;
        }

        /// <summary>
        /// Get System Administrator object
        /// </summary>
        /// <returns>SysAdmin</returns>
        public SysAdmin GetSysAdmin()
        {
            return _appSettings.SysAdmin;
        }
    }
}
