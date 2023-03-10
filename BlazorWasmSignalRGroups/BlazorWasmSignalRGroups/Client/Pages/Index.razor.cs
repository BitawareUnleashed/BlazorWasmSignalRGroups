using BlazorWasmSignalRGroups.Shared.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

namespace BlazorWasmSignalRGroups.Client.Pages;

public partial class Index
{
    private HubConnection? hubConnection;
    private HubConnection? newsHubConnection;
    private string watch = string.Empty;
    private Article article;
    private bool isNewsDisabled;
    private bool isWatchDisabled;


    public async Task CreateWatchClient()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(new Uri("https://localhost:7014/communicationhub"))
            .Build();

        hubConnection.On<string>("SendMessage", message =>
        {
            watch = message ?? string.Empty;
            Console.WriteLine("TIME group event");
            StateHasChanged();
        });
        await hubConnection.StartAsync();


        // Add to group
        await hubConnection.SendAsync("AddClientToGroup", "TIME");

        isWatchDisabled = true;
    }

    public async Task CreateNewsClient()
    {
        newsHubConnection = new HubConnectionBuilder()
            .WithUrl(new Uri("https://localhost:7014/communicationhub"))
            .Build();

        //newsHubConnection.On<string>("SendMessage", message =>
        //{
        //    article = JsonSerializer.Deserialize<Article>(message);
        //    Console.WriteLine("NEWS group event");
        //    StateHasChanged();
        //});

        newsHubConnection.On<Article>("SendNews", message =>
        {
            article = message;
            Console.WriteLine("NEWS group event");
            StateHasChanged();
        });

        await newsHubConnection.StartAsync();


        // Add to group
        await newsHubConnection.SendAsync("AddClientToGroup", "NEWS");

        isNewsDisabled = true;
    }
}

