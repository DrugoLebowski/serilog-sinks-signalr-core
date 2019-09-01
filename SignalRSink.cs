// Standard
using System;
using System.Collections.Generic;
using System.Linq;

// Vendor
using Microsoft.AspNetCore.SignalR;
using Serilog.Core;
using Serilog.Events;

// Internal
using Serilog.Sinks.SignalR.Core.Interfaces;


namespace Serilog.Sinks.SignalR.Core
{
    /// <summary>
    /// Writes log event to a SignalR Hub.
    /// </summary>
    /// <typeparam name="THub">The type of the Hub to use.</typeparam>
    public class SignalRSink<THub> : ILogEventSink
        where THub : Hub<ISerilogHub>
    {
        private readonly IHubContext<THub, ISerilogHub> _hubContext;
        private readonly IFormatProvider _formatProvider;
        private readonly string[] _groups;
        private readonly string[] _userIds;
        private readonly string[] _excludedConnectionIds;

        /// <summary>
        /// SignalRSink constructor.
        /// </summary>
        /// <param name="hubContext">The hub where the events are emmitted.</param>
        /// <param name="formatProvider">The format provider with which the events are formatted.</param>
        /// <param name="groups">The groups where the events are sent.</param>
        /// <param name="userIds">The users to where the events are sent.</param>
        /// <param name="excludedConnectionIds">The client ids to exclude.</param>
        public SignalRSink(
            IHubContext<THub, ISerilogHub> hubContext,
            IFormatProvider formatProvider = null,
            string[] groups = null,
            string[] userIds = null,
            string[] excludedConnectionIds = null
        )
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _formatProvider = formatProvider;
            _groups = groups ?? Array.Empty<string>();
            _userIds = userIds ?? Array.Empty<string>();
            _excludedConnectionIds = excludedConnectionIds ?? Array.Empty<string>();
        }

        /// <summary>
        /// Emit a log event to the clients registered to the hub.
        /// </summary>
        /// <param name="logEvent">The event to emit</param>
        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null) { throw new ArgumentNullException(nameof(logEvent)); }

            var targets = new List<ISerilogHub>();

            if (_groups.Any())
            {
                targets.Add(_hubContext
                    .Clients
                    .Groups(_groups
                        .Except(_excludedConnectionIds)
                        .ToArray()
                    )
                );
            }

            if (_userIds.Any())
            {
                targets.Add(_hubContext
                    .Clients
                    .Users(_userIds
                        .Except(_excludedConnectionIds)
                        .ToArray()
                    )
                );
            }

            if (!_groups.Any() && !_userIds.Any())
            {
                targets.Add(_hubContext
                    .Clients
                    .AllExcept(_excludedConnectionIds)
                );
            }

            foreach (var target in targets)
            {
                target.PushEventLog(logEvent.RenderMessage(_formatProvider));
            }
        }
    }
}
