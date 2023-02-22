using BlazorWasmSignalRGroups.Shared.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

namespace BlazorWasmSignalRGroups.Client.Pages;

public partial class Index
{
    private HubConnection? hubConnection;
    private HubConnection? newsHubConnection;
    private string Watch = string.Empty;
    private Article article;


    protected override async Task OnInitializedAsync()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(new Uri("https://localhost:7014/communicationhub"))
        .Build();

        hubConnection.On<string>("SendMessage", message =>
        {
            Watch = message ?? string.Empty;
            StateHasChanged();
        });
        await hubConnection.StartAsync();


        // Add to group
        await hubConnection.SendAsync("AddClientToGroup", "TIME");
    }


    public async Task CreateNewsClient()
    {
        newsHubConnection = new HubConnectionBuilder()
            .WithUrl(new Uri("https://localhost:7014/communicationhub"))
            .Build();

        newsHubConnection.On<string>("SendMessage", message =>
        {
            article = JsonSerializer.Deserialize<Article>(message);
            StateHasChanged();
        });
        await newsHubConnection.StartAsync();


        // Add to group
        await newsHubConnection.SendAsync("AddClientToGroup", "NEWS");
    }
}

