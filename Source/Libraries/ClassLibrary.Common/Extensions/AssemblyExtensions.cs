using System.Diagnostics;

namespace System.Reflection
{
    /// <summary>
    /// Extension methods for existing Assembly types.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.2.0 | 09/07/2022 | Duende IdentityServer Development |~ 
    /// </revision>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Method to return parent assembly in chain
        /// </summary>
        /// <param name="value">Assembly</param>
        /// <method>GetParentAssembly(this Assembly assembly)</method>
        public static Assembly GetParentAssembly(this Assembly value)
        {
            List<Assembly> callerAssemblies = new StackTrace().GetFrames()
                        .Select(x => x.GetMethod()?.ReflectedType?.Assembly ?? value).Distinct()
                        .Where(x => x.GetReferencedAssemblies().Any(y => y.FullName == value.FullName))
                        .ToList();

            Assembly initialAssembly = value;
            if (callerAssemblies.Any())
                initialAssembly = callerAssemblies.Last();

            return initialAssembly;
        }
    }
}
