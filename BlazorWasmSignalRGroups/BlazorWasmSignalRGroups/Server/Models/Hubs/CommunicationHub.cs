using BlazorWasmSignalRGroups.Shared.Models;
using Microsoft.AspNetCore.SignalR;

namespace BlazorWasmSignalRGroups.Server.Models.Hubs;
public class CommunicationHub : Hub
{
    public async Task SendMessage(string message) 
        => await Clients.All.SendAsync("SendMessage",message);

    public async Task SendNews(Article message)
        => await Clients.All.SendAsync("SendNews", message);

    /*GROUPS*/

    public async Task AddClientToGroup(string groupName) => 
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

    public async Task SendToGroup(string groupName, string message) =>
        await Clients.Group(groupName).SendAsync("SendMessage", message);

    public async Task SendToNews(string groupName, Article message) =>
        await Clients.Group(groupName).SendAsync("SendNews", message);

    public async Task RemoveClientToGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Client(Context.ConnectionId).SendAsync("SendMessage", "Removed from group");
    }
}

