using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Mvc.Http
{
    /// <summary>
    /// Http client to handle api requests. 
    /// and can be overriden with TimeOut property.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/30/2022 | User Role Claims Development |~ 
    /// </revision>
    public class ApiClient : HttpClient
    {
        private string _returnMessage = string.Empty;
        private Dictionary<string, string> _headers;
        private HttpStatusCode _statusCode = HttpStatusCode.NoContent;
        private bool _responseSuccess = false;

        /// <value>HttpStatusCode</value>
        public HttpStatusCode StatusCode { get { return _statusCode; } }

        /// <value>bool</value>
        public bool IsResponseSuccess { get { return _responseSuccess; } }

        /// <value>TimeSpan</value>
        public TimeSpan TimeOut { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="baseUrl">string</param>
        /// <method>ApiClient(string baseUrl)</method>
        public ApiClient(string baseUrl)
        {
            string cleanBaseUrl = baseUrl.Clean();

            if (string.IsNullOrEmpty(cleanBaseUrl))
                throw new InvalidOperationException("Invalid baseUrl");

            if (cleanBaseUrl[^1] != '/' && cleanBaseUrl[^1] != '\\')
                cleanBaseUrl += "/";

            this.BaseAddress = new Uri(cleanBaseUrl);

            _headers = new Dictionary<string, string>();
        }

        /// <summary>
        /// Set Bearer Authentication Header
        /// </summary>
        /// <param name="token">string</param>
        /// <method>SetBearerToken(string token)</method>
        public void SetBearerToken(string token)
        {
            this.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Clean());
        }

        /// <summary>
        /// Add request header
        /// </summary>
        /// <param name="name">string</param>
        /// <param name="value">value</param>
        /// <method>AddRequestHeader(string name, string value)</method>
        public void AddRequestHeader(string name, string value)
        {
            _headers.Add(name, value);
        }

        /// <summary>
        /// Get response string
        /// </summary>
        /// <returns>string</returns>
        /// <method>GetResponseString()</method>
        public string GetResponseString()
        {
            if (!string.IsNullOrEmpty(_returnMessage))
                return _returnMessage;

            return string.Empty;
        }

        /// <summary>
        /// Get response object
        /// </summary>
        /// <returns>T?</returns>
        /// <method>GetResponseObject&lt;T&gt;()</method>
        public T? GetResponseObject<T>()
        {
            if (!string.IsNullOrEmpty(_returnMessage))
            {
                var result = JsonConvert.DeserializeObject<T>(_returnMessage);
                return result;
            }

            return default;
        }

        /// <summary>
        /// Send Form URL Encoded Request
        /// </summary>
        /// <param name="httpMethod">HttpMethod</param>
        /// <param name="requestUri">string</param>
        /// <param name="content">Dictionary&lt;string, string&gt;</param>
        /// <returns>Task&lt;HttpStatusCode&gt;</returns>
        /// <method>SendFormUrlEncodedRequest(HttpMethod httpMethod, string requestUri, Dictionary&lt;string, string&gt; content)</method>
        public async Task<HttpStatusCode> SendFormUrlEncodedRequest(HttpMethod httpMethod, string requestUri, Dictionary<string, string> content)
        {
            _statusCode = HttpStatusCode.BadRequest;
            HttpRequestMessage httpRequest = new(httpMethod, requestUri.Clean().Trim('/'))
            {
                Content = new FormUrlEncodedContent(content)
            };

            return await GetResponse(httpRequest);
        }

        /// <summary>
        /// Send Jason Request
        /// </summary>
        /// <param name="httpMethod">HttpMethod</param>
        /// <param name="requestUri">string</param>
        /// <param name="content">Object</param>
        /// <returns>Task&lt;HttpStatusCode&gt;</returns>
        /// <method>SendJasonRequest(HttpMethod httpMethod, string requestUri, Object content)</method>
        public async Task<HttpStatusCode> SendJasonRequest(HttpMethod httpMethod, string requestUri, Object content)
        {
            _statusCode = HttpStatusCode.BadRequest;
            HttpRequestMessage httpRequest = new(httpMethod, requestUri.Clean().Trim('/'))
            {
                Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json")
            };

            return await GetResponse(httpRequest);
        }

        /// <summary>
        /// Send Request (No Content)
        /// </summary>
        /// <param name="httpMethod">HttpMethod</param>
        /// <param name="requestUri">string</param>
        /// <returns>Task&lt;HttpStatusCode&gt;</returns>
        /// <method>SendRequest(HttpMethod httpMethod, string requestUri)</method>
        public async Task<HttpStatusCode> SendRequest(HttpMethod httpMethod, string requestUri)
        {
            _statusCode = HttpStatusCode.BadRequest;
            HttpRequestMessage httpRequest = new(httpMethod, requestUri.Clean().Trim('/'));

            return await GetResponse(httpRequest);
        }


        /// <summary>
        /// Send Raw Request (No Content)
        /// </summary>
        /// <param name="httpMethod">HttpMethod</param>
        /// <param name="requestUri">string</param>
        /// <returns>Task&lt;HttpStatusCode&gt;</returns>
        /// <method>SendRequestRaw(HttpMethod httpMethod, string requestUri)</method>
        public async Task<HttpStatusCode> SendRawRequest(HttpMethod httpMethod, string requestUri)
        {
            _statusCode = HttpStatusCode.BadRequest;
            HttpRequestMessage httpRequest = new(httpMethod, requestUri.Trim('/'));

            return await GetResponse(httpRequest);
        }

        private async Task<HttpStatusCode> GetResponse(HttpRequestMessage httpRequest)
        {
            // Adding any additional headers for request here
            if (_headers.Any())
                foreach (KeyValuePair<string, string> header in _headers)
                    httpRequest.Headers.Add(header.Key, header.Value);

            HttpResponseMessage httpResponse = await this.SendAsync(httpRequest);
            _statusCode = httpResponse.StatusCode;

            if (httpResponse.IsSuccessStatusCode)
            {
                _returnMessage = $"{await httpResponse.Content.ReadAsStringAsync()}";
                _responseSuccess = true;
            }
            else
                _returnMessage = $"{(int)httpResponse.StatusCode} - {httpResponse.StatusCode.ToString()}";

            // clear any additional request headers that may have been set
            _headers.Clear();

            return _statusCode;
        }
    }
}
