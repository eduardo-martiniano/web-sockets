using Microsoft.AspNetCore.SignalR;

namespace RealTimeBroker.Hubs
{
    public class BrokerHub : Hub
    {
        public Task ConnectToStock(string symbol)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, symbol);

            return Task.CompletedTask;
        }
    }
}
