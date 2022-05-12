using Microsoft.AspNetCore.SignalR;

namespace hangnow_back.Hubs;

public class OfferHub : Hub
{
    public void Send(string name, string message, string connId)
    {
        Clients.Client(connId).SendAsync("Message", name, message);
    }
}