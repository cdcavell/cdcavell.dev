using ClassLibrary.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace ClassLibrary.Data.Extensions
{
    /// <summary>
    /// Extension methods to log Enumerable records.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.1.0 | 09/05/2022 | Data Access Layer Development |~ 
    /// </revision>
    public static class IEnumerableExtentions
    {
        /// <value>ILogger</value>
        public static ILogger? Logger { get; set; }

        /// <summary>
        /// Log returned records meeting specific parameters.
        /// </summary>
        /// <param name="enumerable">IEnumerable&lt;T&gt;</param>
        /// <param name="expression">Expression&lt;Func&lt;T, bool&gt;&gt;</param>
        /// <returns>IEnumerable&lt;T&gt;</returns>
        /// <method>LogRecords&lt;T&gt;(this IEnumerable&lt;T&gt; enumerable, Expression&lt;Func&lt;T, bool&gt;&gt; expression)</method>
        public static IEnumerable<T> LogRecords<T>(this IEnumerable<T> enumerable, Expression<Func<T, bool>> expression)
        {
            List<string> messageResults = new();

            foreach (var item in enumerable.ToList())
            {
                if (item != null)
                {
                    Type type = item.GetType();
                    Delegate dynamicExpression = Expression.Lambda(expression.Body, expression.Parameters).Compile();
                    bool result = Convert.ToBoolean(dynamicExpression.DynamicInvoke(item));

                    if (result)
                    {
                        string record = JsonConvert.SerializeObject(item, new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            Formatting = Formatting.Indented
                        });

                        messageResults.Add($"Entity: {{\"{item.GetType().Name}\"}} Record: {record}");
                    }
                }

                yield return item;
            }

            if (Logger != null)
            {
                string message = $"LogRecords({expression}) Result(s):{AsciiCodes.CRLF}{AsciiCodes.CRLF}";
                foreach (string item in messageResults)
                    message += $"{item}{AsciiCodes.CRLF}";

                Logger.LogDebug(message);
            }
        }

        /// <summary>
        /// Log all returned records.
        /// </summary>
        /// <param name="enumerable">IEnumerable&lt;T&gt;</param>
        /// <returns>IEnumerable&lt;T&gt;</returns>
        /// <method>LogAllRecords&lt;T&gt;(this IEnumerable&lt;T&gt; enumerable)</method>
        public static IEnumerable<T> LogAllRecords<T>(this IEnumerable<T> enumerable)
        {
            List<string> messageResults = new();

            foreach (var item in enumerable.ToList())
            {
                if (item != null)
                {
                    string record = JsonConvert.SerializeObject(item, new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        Formatting = Formatting.Indented
                    });

                    messageResults.Add($"Entity: {{\"{item.GetType().Name}\"}} Record: {record}");
                }

                yield return item;
            }

            if (Logger != null)
            {
                string message = $"LogAllRecords() Result(s):{AsciiCodes.CRLF}{AsciiCodes.CRLF}";
                foreach (string item in messageResults)
                    message += $"{item}{AsciiCodes.CRLF}";

                Logger.LogDebug(message);
            }
        }
    }
}
