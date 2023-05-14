using Microsoft.Extensions.DependencyInjection;

namespace ClassLibrary.Mvc.Services.Email
{
    /// <summary>
    /// Email Web Service Options Extension
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.3.0 | 11/13/2022 | User Registration Development |~ 
    /// </revision>
    public static class EmailServiceOptionsExtention
    {
        /// <summary>
        /// Add Email Web Service Options Extention
        /// </summary>
        /// <param name="serviceCollection">IServiceCollection</param>
        /// <param name="options">Action&lt;UserAuthorizationServiceOptions&gt;</param>
        /// <method>AddAppSettingsService(this IServiceCollection serviceCollection, Action&lt;UserAuthorizationServiceOptions&gt; options)</method>
        public static IServiceCollection AddEmailService(this IServiceCollection serviceCollection, Action<EmailServiceOptions> options)
        {
            serviceCollection.AddScoped<IEmailService, EmailService>();
            if (options == null)
                throw new ArgumentNullException(nameof(options), @"Missing required options for EmailService.");

            serviceCollection.Configure(options);
            return serviceCollection;
        }
    }
}
