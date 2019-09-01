// Standard
using System;

// Vendor
using Microsoft.AspNetCore.SignalR;
using Serilog.Configuration;

// Internal
using Serilog.Sinks.SignalR.Core.Interfaces;


namespace Serilog.Sinks.SignalR.Core.Extensions
{
    public static class SignalRSinkExtensions
    {
        /// <summary>
        /// Add the SignalR sink to the Logger's sinks collection.
        /// </summary>
        /// <typeparam name="THub">The type of SignalR hub to use.</typeparam>
        /// <param name="loggerConfiguration">The logger sink configuration.</param>
        /// <param name="hub">The Hub to use.</param>
        /// <param name="formatProvider">The format provider to use.</param>
        /// <param name="groups">The groups to which the log events are sent.</param>
        /// <param name="userIds">The users to which the log events are sent.</param>
        /// <param name="excludedConnectionIds">The excluded ids from the dispatch.</param>
        /// <returns>The instance of LoggerConfiguration</returns>
        public static LoggerConfiguration SignalR<THub, T>(
            this LoggerSinkConfiguration loggerConfiguration,
            IHubContext<THub, T> hub,
            IFormatProvider formatProvider = null,
            string[] groups = null,
            string[] userIds = null,
            string[] excludedConnectionIds = null
        ) where THub : Hub<T> where T : class
        {
            if (loggerConfiguration == null) { throw new ArgumentNullException(nameof(loggerConfiguration)); }
            if (hub == null) { throw new ArgumentNullException(nameof(hub)); }

            return loggerConfiguration.Sink(new SignalRSink<THub, T>(
                hub,
                formatProvider,
                groups,
                userIds,
                excludedConnectionIds
            ));
        }
    }
}
