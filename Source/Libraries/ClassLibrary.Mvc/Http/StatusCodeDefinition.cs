using ClassLibrary.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace ClassLibrary.Mvc.Http
{
    /// <summary>
    /// Class for returning defintion of given Html status code
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.0.0 | 08/20/2022 | Initial Development |~ 
    /// </revision>
    public static class StatusCodeDefinition
    {
        private static string _culture = "en-US";
        private static readonly List<KeyValuePair<int, string>> _StatusCodeList = new();

        private static void Load(string culture, IStringLocalizer<SharedResource> sharedLocalizer)

        {
            if (!_StatusCodeList.Any() || _culture != culture)
            {
                _culture = culture;
                _StatusCodeList.Clear();

                _StatusCodeList.Add(new KeyValuePair<int, string>(100, sharedLocalizer["StatusCode-100"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(101, sharedLocalizer["StatusCode-101"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(200, sharedLocalizer["StatusCode-200"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(201, sharedLocalizer["StatusCode-201"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(202, sharedLocalizer["StatusCode-202"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(203, sharedLocalizer["StatusCode-203"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(204, sharedLocalizer["StatusCode-204"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(205, sharedLocalizer["StatusCode-205"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(206, sharedLocalizer["StatusCode-206"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(300, sharedLocalizer["StatusCode-300"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(301, sharedLocalizer["StatusCode-301"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(302, sharedLocalizer["StatusCode-302"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(303, sharedLocalizer["StatusCode-303"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(304, sharedLocalizer["StatusCode-304"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(305, sharedLocalizer["StatusCode-305"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(306, sharedLocalizer["StatusCode-306"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(307, sharedLocalizer["StatusCode-307"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(400, sharedLocalizer["StatusCode-400"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(401, sharedLocalizer["StatusCode-401"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(402, sharedLocalizer["StatusCode-402"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(403, sharedLocalizer["StatusCode-403"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(404, sharedLocalizer["StatusCode-404"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(405, sharedLocalizer["StatusCode-405"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(406, sharedLocalizer["StatusCode-406"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(407, sharedLocalizer["StatusCode-407"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(408, sharedLocalizer["StatusCode-408"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(409, sharedLocalizer["StatusCode-409"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(410, sharedLocalizer["StatusCode-410"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(411, sharedLocalizer["StatusCode-411"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(412, sharedLocalizer["StatusCode-412"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(413, sharedLocalizer["StatusCode-413"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(414, sharedLocalizer["StatusCode-414"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(415, sharedLocalizer["StatusCode-415"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(416, sharedLocalizer["StatusCode-416"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(417, sharedLocalizer["StatusCode-417"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(500, sharedLocalizer["StatusCode-500"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(501, sharedLocalizer["StatusCode-501"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(502, sharedLocalizer["StatusCode-502"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(503, sharedLocalizer["StatusCode-503"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(504, sharedLocalizer["StatusCode-504"]));
                _StatusCodeList.Add(new KeyValuePair<int, string>(505, sharedLocalizer["StatusCode-505"]));
            }
        }

        /// <returns>
        /// KeyValuePair\&lt;int, string\&gt;
        /// </returns>
        /// <param name="code">int</param>
        /// <param name="culture">string</param>
        /// <param name="sharedLocalizer">IStringLocalizer&lt;SharedResource&gt;</param>
        /// <method>GetCodeDefinition(int code, string culture, IStringLocalizer&lt;SharedResource&gt; sharedLocalizer)</method>
        public static KeyValuePair<int, string> GetCodeDefinition(int code, string culture, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            Load(culture, sharedLocalizer);

            KeyValuePair<int, string> definition = _StatusCodeList.Find(x => x.Key == code);
            if (definition.Key != code || code == 0)
                definition = new KeyValuePair<int, string>(600, sharedLocalizer["StatusCode-600"]);

            return definition;
        }

        /// <returns>
        /// string
        /// </returns>
        /// <param name="code">int</param>
        /// <param name="culture">string</param>
        /// <param name="sharedLocalizer">IStringLocalizer&lt;SharedResource&gt;</param>
        /// <method>ToString(int code, string culture, IStringLocalizer&lt;SharedResource&gt; sharedLocalizer)</method>
        public static string ToString(int code, string culture, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            KeyValuePair<int, string> definition = GetCodeDefinition(code, culture, sharedLocalizer);
            return $"[{definition.Key}] {definition.Value}";
        }
    }
}
