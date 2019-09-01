// Standard
using System.Threading.Tasks;


namespace Serilog.Sinks.SignalR.Core.Interfaces
{
    public interface ISerilogHub
    {
        Task PushEventLog(string message);
    }
}
