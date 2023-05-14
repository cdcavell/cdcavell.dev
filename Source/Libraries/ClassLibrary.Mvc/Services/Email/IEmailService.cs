using MimeKit;

namespace ClassLibrary.Mvc.Services.Email
{
    /// <summary>
    /// Email Web Service Interface
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.3.0 | 11/13/2022 | User Registration Development |~ 
    /// </revision>
    public interface IEmailService
    {
        /// <summary>
        /// Send mail message
        /// </summary>
        /// <param name="mailMessage">MimeMessage</param>
        /// <returns>Task</returns>
        Task Send(MimeMessage mailMessage);
    }
}
