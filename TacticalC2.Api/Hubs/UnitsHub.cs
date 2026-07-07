using Microsoft.AspNetCore.SignalR;

namespace TacticalC2.Api.Hubs;

public class UnitsHub : Hub
{
    public async Task Subscribe()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "units-subscribers");
    }
}