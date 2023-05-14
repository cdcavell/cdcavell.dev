using ClassLibrary.Common;
using ClassLibrary.Common.Html;
using ClassLibrary.Mvc.Localization;
using ClassLibrary.Mvc.Models.Home;
using ClassLibrary.Mvc.Services.AppSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace ClassLibrary.Mvc.Controllers
{
    /// <class>WebBaseController</class>
    /// <summary>
    /// Base controller class for web application
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/31/2022 | User Role Claims Development |~ 
    /// | Christopher D. Cavell | 1.0.3.0 | 12/12/2022 | User Registration Development |~ 
    /// | Christopher D. Cavell | 1.0.2.1 | 10/15/2022 | Block Harassing IP Addresses |~ 
    /// | Christopher D. Cavell | 1.0.2.0 | 10/08/2022 | Duende IdentityServer Development |~ 
    /// | Christopher D. Cavell | 1.0.0.0 | 09/04/2022 | Initial Development |~ 
    /// </revision>
    [Controller]
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public abstract partial class WebBaseController<T> : Controller where T : WebBaseController<T>
    {
        /// <value>ILogger</value>
        protected readonly ILogger _logger;
        /// <value>IWebHostEnvironment</value>
        protected readonly IWebHostEnvironment _webHostEnvironment;
        /// <value>IWebHostEnvironment</value>
        protected readonly IHttpContextAccessor _httpContextAccessor;
        /// <value>IStringLocalizer</value>
        protected readonly IStringLocalizer<T> _localizer;
        /// <value>IStringLocalizer&lt;SharedResource&gt;</value>
        protected readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        
        /// <value>string</value>
        protected string _cultureName;
        /// <value>string</value>
        protected string _invalidModelState;
        /// <value>string</value>
        protected string _logMessageHeader;
        /// <value>string</value>
        protected IPAddress? _remoteIPAddress;
        /// <value>string</value>
        protected string _controller;
        /// <value>string</value>
        protected string _action;
        /// <value>string</value>
        protected string _id;
        /// <value>string</value>
        protected string _pathQueryString;
        /// <value>string</value>
        protected string _queryString;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="webHostEnvironment">IWebHostEnvironment</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        /// <param name="localizer">IStringLocalizer&lt;T&gt;</param>
        /// <param name="sharedLocalizer">IStringLocalizer&lt;SharedResource&gt;</param>
        /// <method>
        /// WebBaseController(
        ///     ILogger&lt;T&gt; logger, 
        ///     IWebHostEnvironment webHostEnvironment, 
        ///     IHttpContextAccessor httpContextAccessor,
        ///     IStringLocalizer&lt;T&gt; localizer,
        ///     IStringLocalizer&lt;SharedResource&gt; sharedLocalizer
        /// )
        /// </method>
        protected WebBaseController(
            ILogger<T> logger, 
            IWebHostEnvironment webHostEnvironment, 
            IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<T> localizer, 
            IStringLocalizer<SharedResource> sharedLocalizer
        )
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
            
            _cultureName = string.Empty;
            _invalidModelState = string.Empty;
            _logMessageHeader = string.Empty;
            _controller = string.Empty;
            _action = string.Empty;
            _id = string.Empty;
            _pathQueryString = string.Empty;
            _queryString = string.Empty;
        }

        /// <summary>
        /// Called before the action method is invoked
        /// </summary>
        /// <param name="context">ActionExecutingContext</param>
        /// <exception>ArgumentNullException</exception>
        /// <exception>NullReferenceException</exception>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("ActionExecutingContext is null");

            _remoteIPAddress = Request.GetRemoteAddress();

            _controller = RouteData.Values
                .Where(x => x.Key.Equals("controller", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value?.ToString())
                .FirstOrDefault() ?? string.Empty;

            _action = RouteData.Values
                .Where(x => x.Key.Equals("action", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value?.ToString())
                .FirstOrDefault() ?? string.Empty;

            _id = RouteData.Values
                .Where(x => x.Key.Equals("id", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value?.ToString())
                .FirstOrDefault() ?? string.Empty;

            _pathQueryString = Request.GetEncodedPathAndQuery().ToString();
            _queryString = Request.QueryString.ToString();

            _logMessageHeader = Request.LogMessageHeader();
            _logMessageHeader += User.LogMessageHeader();

            var scopeFactory = _httpContextAccessor.HttpContext?.RequestServices.GetService<IServiceScopeFactory>();
            if (scopeFactory != null)
                using (var serviceScope = scopeFactory.CreateScope())
                {
                    var appSettingsService = serviceScope.ServiceProvider.GetRequiredService<IAppSettingsService>();
                    if (appSettingsService.IsIPAddressBlocked(_remoteIPAddress.MapToIPv4().ToString()))
                    {
                        context.HttpContext.Abort();
                        _logger.LogTrace("{@logMessageHeader} - OnActionExecuting({@context}) Remote IPAddress Blocked", _logMessageHeader, nameof(context));
                    }
                    else
                        _logger.LogTrace("{@logMessageHeader} - OnActionExecuting({@context})", _logMessageHeader, nameof(context));
                }
            else
                throw new NullReferenceException("IServiceScopeFactory is null");

            IRequestCultureFeature? requestCultureFeature = context.HttpContext.Features.Get<IRequestCultureFeature>();
            if (requestCultureFeature == null)
                throw new NullReferenceException("IRequestCultureFeature is null");

            _cultureName = CultureHelper.GetImplementedCulture(requestCultureFeature.RequestCulture.Culture.Name);
            ViewBag.CultureName = _cultureName;
            _invalidModelState = _sharedLocalizer["Invalid Model State"];

            string currentUrl = UriHelper.GetDisplayUrl(Request).Trim('/');
            currentUrl += (currentUrl.Contains('?')) ? "&" : "?";
            ViewBag.CurrentUrl = currentUrl;

            base.OnActionExecuting(context);
        }

        /// <summary>
        /// Return BadRequestObjectResult containing ModelState error messages
        /// </summary>
        /// <returns>BadRequestObjectResult</returns>
        protected BadRequestObjectResult InvalidModelState()
        {
            string message = _invalidModelState;
            List<ModelErrorCollection> errors = ModelState.Values
                .Where(x => x.RawValue != null)
                .Select(x => x.Errors)
                .Where(x => x.Any())
                .ToList();

            foreach (ModelErrorCollection errorCollection in errors)
                foreach (ModelError error in errorCollection)
                    message += AsciiCodes.CRLF + error.ErrorMessage;

            _logger.LogDebug("{@logMessageHeader} - WebBaseController.InvalidModelState() {@message}", _logMessageHeader, message);
            return new BadRequestObjectResult(message);
        }

        /// <summary>
        /// Global model validation method (View found in HomeSite.ClassLibrary.Razor)
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>KeyValuePair&lt;int, string&gt;</returns>
        /// <method>ValidateModel&lt;M&gt;(M model)</method>
        /// <exception>ArgumentNullException</exception>
        protected KeyValuePair<int, string> ValidateModel<M>(M model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            int key = 0;
            string value = string.Empty;

            bool isValid = TryValidateModel(model);
            if (!isValid)
            {
                foreach (var modelValue in ModelState.Values)
                {
                    var errors = modelValue.Errors;
                    if (errors.Count > 0)
                    {
                        foreach (var error in errors)
                        {
                            key++;
                            value += Tags.Brackets(error.ErrorMessage) + Tags.LineBreak();
                        }
                    }
                }
            }

            return new KeyValuePair<int, string>(key, value);
        }

        /// <summary>
        /// Exception Message
        /// </summary>
        /// <returns>string</returns>
        protected string ExceptionMessage(Exception exception)
        {
            _logger.LogError(exception, "ExceptionMessage");

            string errorMessage = $"Exception: {exception.Message}";
            if (exception.InnerException != null)
                errorMessage += $"{AsciiCodes.CRLF}Inner Exception: {exception.InnerException.Message}";

            _logger.LogError(exception, "{@logMessageHeader} - WebBaseController.ExceptionMessage({@exception}) {@message}", _logMessageHeader, nameof(exception), errorMessage);
            return errorMessage;
        }

        /// <summary>
        /// Set current culture language
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost("{controller}/SetCulture")]
        public virtual IActionResult SetCulture([Bind(CultureModel.BindProperties)] CultureModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Response.Cookies.Append(
                        CookieRequestCultureProvider.DefaultCookieName,
                        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(model.Culture.Clean())),
                        new CookieOptions
                        {
                            SameSite = SameSiteMode.Lax,
                            IsEssential = true,
                            Secure = true,
                            HttpOnly = true
                        }
                    );

                    model.ReturnUrl = model.ReturnUrl.Clean();

                    return Json(model);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "{@logMessageHeader} - WebBaseController.SetCulture(string {@model})", _logMessageHeader, nameof(model));
                    return BadRequest(ExceptionMessage(exception));
                }
            }

            return InvalidModelState();
        }

        /// <summary>
        /// Global error handling
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpGet("{controller}/Error/{id?}"), HttpPost("{controller}/Error/{id?}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public virtual IActionResult Error(int id)
        {
            // Instruct search engines not to index. https://developers.google.com/search/docs/crawling-indexing/block-indexing?visit_id=638042798023618216-3766845920&rd=1
            if (!HttpContext.Response.Headers.ContainsKey("X-Robots-Tag"))
            {
                HttpContext.Response.Headers.Add("X-Robots-Tag", "noindex");
            }

            if (id == 0)
                if (Request.Method.ToLower() == "post")
                    _ = int.TryParse((RouteData?.Values["id"]?.ToString()) ?? "0", out id);

            IRequestCultureFeature? requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            if (requestCultureFeature == null)
                throw new NullReferenceException();

            var vm = new ErrorViewModel(id, ViewBag.CultureName, _sharedLocalizer);

            string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            vm.RequestId = requestId;

            string requestPath = (HttpContext.Request.Query
                .Where(x => x.Key == "RequestPath")
                .Select(x => x.Value.ToString())
                .FirstOrDefault() ?? string.Empty).Clean();

            if (!requestPath.IsLocalUrl())
                if (!string.IsNullOrEmpty(requestPath))
                {
                    _logger.LogError(new Exception($"Invalid Local RequestPath: {requestPath}"), "{@logMessageHeader} [RequestId]: {@requestId} [Request Path]: {@requestPath} [Status Code]: {@statusCode} [Status Message]: {@statusMessage}", _logMessageHeader, requestId, requestPath, vm.StatusCode, vm.StatusMessage);
                    vm = new ErrorViewModel((int)HttpStatusCode.InternalServerError, ViewBag.CultureName, _sharedLocalizer);
                    vm.RequestId = requestId;
                }

            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                requestPath = exceptionFeature.Path;

                try
                {
                    switch (exceptionFeature.Error)
                    {
                        case HttpRequestException hre:
                            if (hre.StatusCode != null)
                                vm = new ErrorViewModel((int)hre.StatusCode, ViewBag.CultureName, _sharedLocalizer);
                            break;
                        case UnauthorizedAccessException uae:
                            vm = new ErrorViewModel((int)HttpStatusCode.Unauthorized, ViewBag.CultureName, _sharedLocalizer);
                            break;
                        case TaskCanceledException tce:
                            vm = new ErrorViewModel((int)HttpStatusCode.Conflict, ViewBag.CultureName, _sharedLocalizer);
                            break;
                        default:
                            vm = new ErrorViewModel((int)HttpStatusCode.InternalServerError, ViewBag.CultureName, _sharedLocalizer);
                            break;
                    }

                    vm.RequestId = requestId;
                    vm.Exception = exceptionFeature.Error;
                    _logger.LogError(exceptionFeature.Error, "{@logMessageHeader} [RequestId]: {@requestId} [Request Path]: {@requestPath} [Status Code]: {@statusCode} [Status Message]: {@statusMessage}", _logMessageHeader, requestId, requestPath, vm.StatusCode, vm.StatusMessage);
                }
                catch (Exception exception)
                {
                    vm.RequestId = requestId;
                    vm.Exception = exceptionFeature.Error;
                    _logger.LogError(exceptionFeature.Error, "{@logMessageHeader} [RequestId]: {@requestId} [Request Path]: {@requestPath} [Status Code]: {@statusCode} [Status Message]: {@statusMessage}", _logMessageHeader, requestId, requestPath, vm.StatusCode, vm.StatusMessage);
                    _logger.LogError(exception, "{@logMessageHeader} [RequestId]: {@requestId} [Request Path]: {@requestPath} [Status Code]: {@statusCode} [Status Message]: {@statusMessage}", _logMessageHeader, requestId, requestPath, vm.StatusCode, vm.StatusMessage);
                }
            }

            _logger.LogDebug("{@logMessageHeader} [RequestId]: {@requestId} [Request Path]: {@requestPath} [Status Code]: {@statusCode} [Status Message]: {@statusMessage}", _logMessageHeader, requestId, requestPath, vm.StatusCode, vm.StatusMessage);
            return View("Error", vm);
        }
    }
}
