using ClassLibrary.Common;
using ClassLibrary.Data;
using ClassLibrary.Data.Models;
using ClassLibrary.Mvc.Localization;
using ClassLibrary.Mvc.Services.AppSettings;
using ClassLibrary.Mvc.Services.Email;
using dis.cdcavell.dev.Models.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MimeKit;
using Newtonsoft.Json;

namespace dis.cdcavell.dev.Controllers
{
    /// <summary>
    /// Registration controller class
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 05/11/2023 | User Role Claims Development |~ 
    /// | Christopher D. Cavell | 1.0.3.1 | 12/13/2022 | User Registration Development |~ 
    /// | Christopher D. Cavell | 1.0.3.0 | 12/12/2022 | User Registration Development |~ 
    /// </revision>
    public class RegistrationController : ApplicationBaseController<RegistrationController>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="webHostEnvironment">IWebHostEnvironment</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        /// <param name="appSettingsService">IAppSettingsService</param>
        /// <param name="localizer">IStringLocalizer&lt;T&gt;</param>
        /// <param name="sharedLocalizer">IStringLocalizer&lt;SharedResource&gt;</param>
        /// <param name="userManager">UserManager&lt;ApplicationUser&gt;</param>
        /// <param name="dbContext">ApplicationDbContext</param>
        /// <param name="emailService">IEmailService</param>
        /// <method>
        /// public RegistrationController(
        ///     ILogger&lt;RegistrationController&gt; logger,
        ///     IWebHostEnvironment webHostEnvironment,
        ///     IHttpContextAccessor httpContextAccessor,
        ///     IAppSettingsService appSettingsService,
        ///     IStringLocalizer&lt;RegistrationController&gt; localizer,
        ///     IStringLocalizer&lt;SharedResource&gt; sharedLocalizer,
        ///     UserManager&lt;ApplicationUser&gt; userManager,
        ///     ApplicationDbContext dbContext,
        ///     IEmailService emailService
        /// ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService, localizer, sharedLocalizer)
        /// </method>
        public RegistrationController(
            ILogger<RegistrationController> logger,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IAppSettingsService appSettingsService,
            IStringLocalizer<RegistrationController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext,
            IEmailService emailService
        ) : base(logger, webHostEnvironment, httpContextAccessor, appSettingsService, localizer, sharedLocalizer)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _emailService = emailService;
        }

        /// <summary>
        /// Entry point into the new registration workflow 
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpGet("Registration/New")]
        [HttpGet("Registration/New/{id?}")]
        public IActionResult New()
        {
            ApplicationUser? user;
            if (TempData.ContainsKey("User"))
            {
                string provider = TempData["Provider"]?.ToString() ?? string.Empty;
                string providerUserId = TempData["ProviderUserId"]?.ToString() ?? string.Empty;

                string tempDataUser = TempData["User"]?.ToString() ?? throw new ArgumentNullException("TempData[\"User\"]");
                user = JsonConvert.DeserializeObject<ApplicationUser>(tempDataUser);

                RegistrationViewModel vm = new()
                {
                    Id = user?.Id ?? string.Empty,
                    DisplayName = user?.DisplayName ?? string.Empty,
                    Email = user?.Email ?? string.Empty,
                    LockNameEmail = true,
                    Provider = provider,
                    ProviderUserId = providerUserId
                };

                if (!vm.Email.IsValidEmail())
                {
                    vm.Email = string.Empty;
                    vm.LockNameEmail = false;
                }

                return View(vm);
            }

            return Unauthorized();
        }

        /// <summary>
        /// Handle postback from new registration
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpPost("Registration/New")]
        [HttpPost("Registration/New/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([Bind(RegistrationViewModel.BindProperties)] RegistrationViewModel model)
        {
            // Check if cancled
            bool cancel = false;
            if (!model.IsSubmit)
                cancel = true;

            if (cancel)
            {
                string publicUri = await _dbContext.Clients
                    .Where(x => x.ClientId == "cdcavell.dev")
                    .Select(x => x.ClientUri)
                    .FirstOrDefaultAsync() ?? string.Empty;

                if (!string.IsNullOrEmpty(publicUri))
                    return Redirect(publicUri);
                else
                    return Redirect("~/");
            }

            // Check email is valid
            if (!model.Email.Clean().IsValidEmail())
                ModelState.AddModelError(nameof(model.Email), _sharedLocalizer["ErrorMessage.Invalid"]);

            if (ModelState.IsValid)
            {
                bool userUpdate = false;

                // setup results
                IdentityResult identityResult = new();

                // Check for existing user
                ApplicationUser user = await _userManager.FindByEmailAsync(model.Email.Clean());
                if (user != null)
                {
                    if (user.Id != model.Id.Clean())
                    {
                        ApplicationUser removeUser = await _userManager.FindByIdAsync(model.Id.Clean());
                        if (removeUser != null)
                        {
                            identityResult = await _userManager.DeleteAsync(removeUser);
                            if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
                        }
                    }
                }
                else
                {
                    user = await _userManager.FindByIdAsync(model.Id.Clean());
                    if (user == null)
                    {
                        _logger.LogWarning(" User Key Not Found [Key]: {@Key} [Value]: {@Value}", nameof(model.Id), model.Id);
                        return Unauthorized();
                    }

                    if (!user.NormalizedUserName.Equals(model.Email.Clean().ToUpper()))
                        if (!user.UserName.IsValidEmail())
                        { 
                            user.UserName = model.Email.Clean();
                            user.NormalizedUserName = model.Email.Clean().ToUpper();
                            userUpdate = true;
                        }

                    if (!user.NormalizedEmail.Equals(model.Email.Clean().ToUpper()))
                        if (!user.Email.IsValidEmail())
                        {
                            user.Email = model.Email.Clean();
                            user.NormalizedEmail = model.Email.Clean().ToUpper();
                            userUpdate = true;
                        }

                    if (!user.DisplayName.Equals(model.DisplayName.Clean()))
                    {
                        user.DisplayName = model.DisplayName.Clean();
                        userUpdate = true;
                    }
                }

                if (user.Status == UserStatus.New)
                {
                    user.Status = UserStatus.Pending;
                    userUpdate = true;
                }

                if (userUpdate)
                {
                    identityResult = await _userManager.UpdateAsync(user);
                    if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
                }

                Registration registration = await _dbContext.Registration
                    .Where(x => x.UserId == user.Id)
                    .Where(x => x.Email == user.Email)
                    .Where(x => x.ProviderUserId == model.ProviderUserId.Clean())
                    .FirstOrDefaultAsync() ?? new();

                registration.UserId = user.Id;
                registration.Email = user.Email;
                registration.DisplayName = user.DisplayName;
                registration.Provider = model.Provider.Clean();
                registration.ProviderUserId = model.ProviderUserId.Clean();
                registration.GeneratedOn = DateTime.Now;

                registration.AddUpdate(_dbContext);

                TempData["Email"] = user.Email;
                return RedirectToAction(nameof(EmailValidation));
            }

            return View(model);
        }

        /// <summary>
        /// Email validation notice 
        /// </summary>
        /// <returns>Task&lt;IActionResult&gt;</returns>
        /// <method>EmailValidation()</method>
        [AllowAnonymous]
        [HttpGet("Registration/EmailValidation")]
        [HttpGet("Registration/EmailValidation/{id?}")]
        public async Task<IActionResult> EmailValidation()
        {
            var email = TempData["Email"]?.ToString();
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var user = await _userManager.FindByEmailAsync(email.Clean());
            if (user == null)
                return Unauthorized();

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Registration", new { token, email = user.Email }, Request.Scheme);

            MimeMessage mailMessage = new();
            mailMessage.From.Add(new MailboxAddress(_appSettings.EmailService.Email, _appSettings.EmailService.Email));
            mailMessage.To.Add(new MailboxAddress(user.DisplayName, user.Email));
            mailMessage.Subject = _sharedLocalizer["Email.Verify.Subject"];
            mailMessage.Body = new TextPart("plain")
            {
                Text =
                AsciiCodes.CRLF
                + _sharedLocalizer["Email.Verify.Body"]
                + AsciiCodes.CRLF + AsciiCodes.CRLF + confirmationLink
                + AsciiCodes.CRLF + AsciiCodes.CRLF
                + AsciiCodes.CRLF + AsciiCodes.CRLF
                + _sharedLocalizer["Email.Noreply"]
            };

            if (_appSettings.IsProduction)
                await _emailService.Send(mailMessage);
            else
                ViewData["ConfirmationLink"] = confirmationLink;

            ViewData["Email"] = user.Email;
            return View();
        }


        /// <summary>
        /// Entry point into the pending registration workflow 
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpGet("Registration/Pending")]
        [HttpGet("Registration/Pending/{id?}")]
        public async Task<IActionResult> Pending()
        {
            ApplicationUser? user;
            if (TempData.ContainsKey("User"))
            {
                string provider = TempData["Provider"]?.ToString() ?? string.Empty;
                string providerUserId = TempData["ProviderUserId"]?.ToString() ?? string.Empty;

                string tempDataUser = TempData["User"]?.ToString() ?? throw new ArgumentNullException("TempData[\"User\"]");
                user = JsonConvert.DeserializeObject<ApplicationUser>(tempDataUser);

                if (user == null)
                    return Unauthorized();

                Registration registration = await _dbContext.Registration
                    .Where(x => x.UserId == user.Id)
                    .Where(x => x.Email == user.Email)
                    .Where(x => x.ProviderUserId == providerUserId)
                    .FirstOrDefaultAsync() ?? new();

                if (registration.IsNew)
                {
                    registration.UserId = user.Id;
                    registration.Email = user.Email;
                    registration.DisplayName = user.DisplayName;
                    registration.Provider = provider;
                    registration.ProviderUserId = providerUserId;
                }

                registration.GeneratedOn = DateTime.Now;
                registration.AddUpdate(_dbContext);

                TempData["Email"] = user.Email;
                return RedirectToAction(nameof(EmailValidation));
            }

            return Unauthorized();
        }

        /// <summary>
        /// Email confirmation
        /// </summary>
        /// <param name="token">string</param>
        /// <param name="email">string</param>
        /// <returns>Task&lt;IActionResult&gt;</returns>
        /// <method>ConfirmEmail(string token, string email)</method>
        [AllowAnonymous]
        [HttpGet("Registration/ConfirmEmail")]
        [HttpGet("Registration/ConfirmEmail/{id?}")]
        public async Task<IActionResult> ConfirmEmail([FromQuery(Name = "token")] string token, [FromQuery(Name = "email")] string email)
        {
            // setup results
            IdentityResult identityResult = new();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Unauthorized();

            if (await _userManager.IsEmailConfirmedAsync(user))
                return RedirectToAction("SignIn", "Account");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return Unauthorized();

            var userLogins = await _userManager.GetLoginsAsync(user);

            List<Registration> registrations = await _dbContext.Registration
                .Where(x => x.UserId == user.Id)
                .ToListAsync();

            foreach(Registration item in registrations)
            {
                if (!string.IsNullOrEmpty(item.ProviderUserId))
                {
                    var userLogin = userLogins
                        .Where(x => x.ProviderKey == item.ProviderUserId)
                        .FirstOrDefault();

                    if (userLogin == null)
                    {
                        userLogin = new(item.Provider, item.ProviderUserId, item.Provider);
                        identityResult = await _userManager.AddLoginAsync(user, userLogin);
                        if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
                    }
                }
            }

            _dbContext.Registration.RemoveRange(registrations);
            await _dbContext.SaveChangesAsync();

            user.EmailConfirmed = true;
            user.Status = UserStatus.Active;

            identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);

            string publicUri = await _dbContext.Clients
                .Where(x => x.ClientId == "cdcavell.dev")
                .Select(x => x.ClientUri)
                .FirstOrDefaultAsync() ?? string.Empty;

            if (!string.IsNullOrEmpty(publicUri)) 
                return Redirect(publicUri);
            else
                return Redirect("~/");
        }
    }
}
