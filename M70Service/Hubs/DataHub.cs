using Microsoft.AspNetCore.SignalR;

namespace M70Service.Hubs
{
    // To be used with SignalR for secure data sending
    public class DataHub : Hub
    {
        public async Task<bool> SendData(string data) {
            int x = 23;
            return true;
        }
    }
}
