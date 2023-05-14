using ClassLibrary.Mvc.Services.AppSettings;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Security.Cryptography;

namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    /// Extension methods for existing HttpRequest types.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.2.0 | 10/02/2022 | Duende IdentityServer Development |~ 
    /// | Christopher D. Cavell | 1.0.0.0 | 09/04/2022 | Initial Development |~ 
    /// </revision>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Method to determine if string is a local url
        /// </summary>
        /// <param name="request">this HttpRequest</param>
        /// <param name="url">string</param>
        /// <returns>bool</returns>
        /// <method>IsLocalUrl(this HttpRequest request, string url)</method>
        public static bool IsLocalUrl(this HttpRequest request, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            Uri? absoluteUri;
            if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
            {
                return String.Equals(request.GetUri().Host, absoluteUri.Host,
                            StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                bool isLocal = !url.StartsWith("http:", StringComparison.OrdinalIgnoreCase)
                    && !url.StartsWith("https:", StringComparison.OrdinalIgnoreCase)
                    && Uri.IsWellFormedUriString(url, UriKind.Relative);
                return isLocal;
            }
        }

        /// <summary>
        /// Method to return Uri of HttpRequest
        /// </summary>
        /// <param name="request">this HttpRequest</param>
        /// <returns>bool</returns>
        /// <method>GetUri(this HttpRequest request)</method>
        public static Uri GetUri(this HttpRequest request)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port.GetValueOrDefault(80),
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };
            return uriBuilder.Uri;
        }

        /// <summary>
        /// Method to return IPAddress of reomote address for HttpRequest
        /// </summary>
        /// <param name="request">HttpRequest</param>
        /// <returns>IPAddress</returns>
        /// <method>GetRemoteAddress(this IPAddress ipAddress)</method>
        public static IPAddress GetRemoteAddress(this HttpRequest request)
        {
            IPAddress ipAddress = request.HttpContext.Connection.RemoteIpAddress ?? throw new ArgumentNullException("RemoteIpAddress is null");
            KeyValuePair<string, StringValues> xForwardedForHeader = request.HttpContext.Request.Headers
                .Where(x => x.Key.ToLower() == "x-forwarded-for")
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(xForwardedForHeader.Key))
            {
                if (!string.IsNullOrEmpty(xForwardedForHeader.Value))
                {
                    UriHostNameType uriType = Uri.CheckHostName(xForwardedForHeader.Value);
                    switch (uriType)
                    {
                        case UriHostNameType.IPv4:
                            // strip any port from xForwardedForHeader IP Address
                            string[] hostParts = xForwardedForHeader.Value.ToString().Split(':');
                            ipAddress = IPAddress.Parse(hostParts[0]);
                            break;
                        case UriHostNameType.IPv6:
                            ipAddress = IPAddress.Parse(xForwardedForHeader.Value);
                            break;
                    }
                }
            }

            return ipAddress;
        }

        /// <summary>
        /// Method to return if HttpRequest is an AJax request
        /// </summary>
        /// <param name="httpRequest">this HttpRequest</param>
        /// <returns>bool</returns>
        /// <method>IsAjaxRequest(this HttpRequest httpRequest)</method>
        public static bool IsAjaxRequest(this HttpRequest httpRequest)
        {
            return string.Equals(httpRequest.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) ||
                string.Equals(httpRequest.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
        }

        /// <summary>
        /// Method to return standard log message header
        /// </summary>
        /// <param name="httpRequest">this HttpRequest</param>
        /// <returns>string</returns>
        /// <method>LogMessageHeader(this HttpRequest httpRequest)</method>
        public static string LogMessageHeader(this HttpRequest httpRequest)
        {
            IPAddress remoteIPAddress = httpRequest.GetRemoteAddress();

            string controller = httpRequest.RouteValues
                .Where(x => x.Key.Equals("controller", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value?.ToString())
                .FirstOrDefault() ?? string.Empty;

             string action = httpRequest.RouteValues
                .Where(x => x.Key.Equals("action", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value?.ToString())
                .FirstOrDefault() ?? string.Empty;

             string id = httpRequest.RouteValues
                .Where(x => x.Key.Equals("id", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value?.ToString())
                .FirstOrDefault() ?? string.Empty;

            string pathQueryString = httpRequest.GetEncodedPathAndQuery().ToString();
            string queryString = httpRequest.QueryString.ToString();

            string logMessageHeader = $"[Remote IP]: {remoteIPAddress.MapToIPv4()}";
            if (!string.IsNullOrEmpty(controller))
                logMessageHeader += $" [Controller]: {controller}";
            if (!string.IsNullOrEmpty(action))
                logMessageHeader += $" [Action]: {action}";
            if (!string.IsNullOrEmpty(id))
                logMessageHeader += $" [Id]: {id}";
            if (!string.IsNullOrEmpty(id))
                logMessageHeader += $" [Id]: {id}";
            if (!string.IsNullOrEmpty(queryString))
                logMessageHeader += $" [Query String]: {queryString}";

            return logMessageHeader;
        }

        /// <summary>
        /// Method to return if request is for authority server
        /// </summary>
        /// <param name="httpRequest">this HttpRequest</param>
        /// <returns>bool</returns>
        /// <method>IsAuthority(this HttpRequest httpRequest)</method>
        public static bool IsAuthority(this HttpRequest httpRequest)
        {
            IServiceScopeFactory? scopeFactory = httpRequest.HttpContext.RequestServices
                .GetService<IServiceScopeFactory>();

            if (scopeFactory != null)
                using (var serviceScope = scopeFactory.CreateScope())
                {
                    var appSettingsService = serviceScope.ServiceProvider.GetRequiredService<IAppSettingsService>();

                    List<Client> clients = appSettingsService.GetClients();
                    if (clients.Any())
                    {
                        Client? client = clients
                            .Where(x => x.ClientId.Trim().ToLower() == httpRequest.Host.Value.Trim().ToLower())
                            .FirstOrDefault();

                        if (client == null)
                            return true;
                    }
                }

            return false;
        }
    }
}
