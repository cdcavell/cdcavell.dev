
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using System.Net;
using System.Security.Claims;

namespace dis.cdcavell.dev.Services
{
    /// <summary>
    /// Custom event store class
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.2.0 | 10/01/2022 | Duende IdentityServer Development |~ 
    /// </revision>
    public class CustomEventService : IEventSink
    {
        private readonly ILogger<CustomEventService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor method.
        /// </summary>
        /// <param name="logger">ILogger&lt;CustomEventService&gt;</param>
        /// <param name="httpContextAccessor">HttpContextAccessor</param>
        /// <returns>Task</returns>
        /// <method>
        /// CustomEventService(
        ///     ILogger&lt;CustomEventService&gt; logger,
        ///     IHttpContextAccessor httpContextAccessor
        /// )
        /// </method>
        public CustomEventService(
            ILogger<CustomEventService> logger,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Method used to act on event.
        /// </summary>
        /// <param name="evt">Event</param>
        /// <returns>Task</returns>
        /// <method>PersistAsync(Event evt)</method>
        public Task PersistAsync(Event evt)
        {
            ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
            string logMessage = $"{_httpContextAccessor.HttpContext?.Request.LogMessageHeader()} - CustomEventService.PersistAsync(Event {nameof(evt)}) [User Name]: {user?.Identity?.Name} [Claims]: {user?.Claims}";

            if (evt.EventType == EventTypes.Success || evt.EventType == EventTypes.Information)
                _logger.LogDebug("{@logMessage} [Name]: {@Name} [Id]: {@id} [Details]: {@details}", logMessage, evt.Name, evt.Id, evt);
            else
                _logger.LogError("{@logMessage} [Name]: {@Name} [Id]: {@id} [Details]: {@details}", logMessage, evt.Name, evt.Id, evt);

            return Task.CompletedTask;
        }
    }
}
