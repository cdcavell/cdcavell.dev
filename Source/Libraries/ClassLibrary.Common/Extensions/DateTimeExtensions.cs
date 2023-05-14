namespace System
{
    /// <summary>
    /// Extension methods for existing DateTime types.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.0.0 | 08/20/2022 | Initial Development |~ 
    /// </revision>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Method to return timestamp of current DateTime value
        /// </summary>
        /// <param name="value">this DateTime</param>
        /// <returns>string</returns>
        /// <method>Timestamp(this DateTime value)</method>
        public static string Timestamp(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        /// <summary>
        /// Method to return age of current DateTime value in years
        /// </summary>
        /// <param name="value">this DateTime</param>
        /// <returns>string</returns>
        /// <method>Timestamp(this DateTime value)</method>
        public static int Age(this DateTime value)
        {
            return DateTime.Now.AddYears(-value.Year).Year;
        }
    }
}
