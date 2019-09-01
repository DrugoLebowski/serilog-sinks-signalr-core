// Standard
using System.Threading.Tasks;

// Vendor
using Microsoft.AspNetCore.SignalR;

// Internal
using Serilog.Sinks.SignalR.Core.Interfaces;


namespace Serilog.Sinks.SignalR.Core.Hubs
{
    public abstract class BaseSerilogHub : Hub<ISerilogHub>
    {
        public async Task SubscribeToGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }
    }
}
