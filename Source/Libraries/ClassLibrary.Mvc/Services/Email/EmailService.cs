using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;

namespace ClassLibrary.Mvc.Services.Email
{
    /// <summary>
    /// Email Web Service
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.3.0 | 11/13/2022 | User Registration Development |~ 
    /// </revision>
    public class EmailService : IEmailService
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _host;
        private readonly int _port;
        private readonly NetworkCredential _credentials;
        private readonly bool _enableSsl;

        /// <value>MailMessage</value>
        public MimeMessage MailMessage { get; set; } = new();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">ILogger&lt;EmailService&gt;</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        /// <param name="options">IOptions&lt;EmailServiceOptions&gt;</param>
        /// <method>EmailService(ILogger&lt;EmailService&gt; logger, IHttpContextAccessor httpContextAccessor, IOptions&lt;EmailServiceOptions&gt; options)</method>
        public EmailService(ILogger<EmailService> logger, IHttpContextAccessor httpContextAccessor, IOptions<EmailServiceOptions> options)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _host = options.Value.Host;
            _port = options.Value.Port;
            _credentials = options.Value.Credentials;
            _enableSsl = options.Value.EnableSsl;
        }

        /// <summary>
        /// Send mail message
        /// </summary>
        /// <param name="mailMessage">MimeMessage</param>
        /// <returns>Task</returns>
        /// <exception cref="Exception">Invalid Property</exception>
        public async Task Send(MimeMessage mailMessage)
        {
            if (mailMessage.To.Count < 1)
                throw new Exception("Invalid Property", new Exception("MailMessage.To required"));

            if (mailMessage.From.Count < 1)
                throw new Exception("Invalid Property", new Exception("MailMessage.From.Address required"));

            if (string.IsNullOrEmpty(mailMessage.Subject))
                throw new Exception("Invalid Property", new Exception("MailMessage.Subject required"));

            if (mailMessage.Body == null)
                throw new Exception("Invalid Property", new Exception("MailMessage.Body required"));

            using var client = new SmtpClient();
            //await client.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
            await client.ConnectAsync(_host, _port, _enableSsl);
            await client.AuthenticateAsync(_credentials);
            await client.SendAsync(mailMessage);
            await client.DisconnectAsync(true);

            string logMessageHeader = _httpContextAccessor.HttpContext?.Request.LogMessageHeader() ?? string.Empty;
            if (!string.IsNullOrEmpty(logMessageHeader))
            {
                foreach (MailboxAddress item in mailMessage.From.Cast<MailboxAddress>())
                    _logger.LogTrace("{@logMessageHeader} - Send({@mailMessage}) [From]: {@from} [Subject]: {@subject}",
                        logMessageHeader,
                        nameof(mailMessage),
                        item.Address,
                        mailMessage.Subject
                    );

                foreach (MailboxAddress item in mailMessage.To.Cast<MailboxAddress>())
                    _logger.LogTrace("{@logMessageHeader} - Send({@mailMessage}) [To]: {@to} [Subject]: {@subject}",
                        logMessageHeader,
                        nameof(mailMessage),
                        item.Address,
                        mailMessage.Subject
                    );
            }
        }
    }
}
