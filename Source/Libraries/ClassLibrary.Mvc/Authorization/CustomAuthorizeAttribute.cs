using Microsoft.AspNetCore.Mvc;

namespace ClassLibrary.Mvc.Authorization
{
    /// <summary>
    /// Custom  authorization attribute
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/26/2022 | User Role Claims Development |~ 
    /// </revision>
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="permissions">string</param>
        /// <method>CustomAuthorizeAttribute(string permissions) : base(typeof(CustomAuthorizeFilter))</method>
        public CustomAuthorizeAttribute(string permissions) : base(typeof(CustomAuthorizeFilter)) 
        { 
            Arguments = new object[] { permissions.Split(',').ToList() };
        }
    }
}
