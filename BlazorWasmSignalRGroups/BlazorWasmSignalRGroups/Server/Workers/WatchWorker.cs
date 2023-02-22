using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorWasmSignalRGroups.Server.Workers;

public class WatchWorker
{
    private HubConnection? hubConnection;
    
    public async Task ExecuteAsync(string baseAddress,CancellationToken stoppingToken)
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(new Uri(baseAddress))
            .Build();
        await hubConnection.StartAsync();
        _ = Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
                _ = hubConnection?.SendAsync("SendToGroup", "TIME", DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + " - " + DateTime.Now.Second.ToString("00"), stoppingToken);
            }
        });
    }
}

