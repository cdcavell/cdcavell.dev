using ClassLibrary.Mvc.Http;
using ClassLibrary.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace ClassLibrary.Mvc.Models.Home
{
    /// <summary>
    /// Error view model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.0.0 | 08/20/2022 | Initial Development |~ 
    /// </revision>
    public class ErrorViewModel
    {
        /// <value>int</value>
        public int StatusCode { get; set; } = 0;
        /// <value>string</value>
        public string StatusMessage { get; set; } = string.Empty;
        /// <value>Exception</value>
        public Exception Exception { get; set; } = new Exception("Empty Exception");
        /// <value>string</value>
        public string RequestId { get; set; }  = string.Empty;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="statusCode">int</param>
        /// <param name="culture">string</param>
        /// <param name="sharedLocalizer">IStringLocalizer&lt;SharedResource&gt;</param>
        /// <method>ErrorViewModel(int statusCode, string culture, IStringLocalizer&lt;SharedResource&gt; )</method>
        public ErrorViewModel(int statusCode, string culture, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            KeyValuePair<int, string> kvp = StatusCodeDefinition.GetCodeDefinition(statusCode, culture, sharedLocalizer);
            StatusCode = kvp.Key;
            StatusMessage = kvp.Value;
        }
    }
}
