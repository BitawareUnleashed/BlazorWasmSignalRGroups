using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorWasmSignalRGroups.Shared.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace BlazorWasmSignalRGroups.Server.Workers;
public class NewsWorker
{
    private HubConnection? hubConnection;
    string? apiKey;

    public NewsWorker(IConfiguration configuration)
    {
        apiKey = configuration["NewsApiKey"];
    }

    //public void SetHub(HubConnection hub, string apiKey)
    //{
    //    this.hubConnection = hub;
    //    this.apiKey = apiKey;
    //}


    /// <summary>
    /// Gets the news caller.
    /// </summary>
    /// <param name="stoppingToken">The stopping token.</param>
    public async Task GetNewsCaller(string baseAddress,  CancellationToken stoppingToken)
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(new Uri(baseAddress))
            .Build();
        await hubConnection.StartAsync();
        
        _ = Task.Run(async () =>
        {
            var url = "https://newsapi.org/v2/everything?" +
                      "sources=ansa&" +
                      "apiKey=" + apiKey;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var news = JsonConvert.DeserializeObject<Root>(json);

            if (news is not null && news.articles is not null)
            {
                foreach (var item in news.articles)
                {
                    await Task.Delay(15000, stoppingToken);
                    var serializedArticle = JsonConvert.SerializeObject(item);

                    _ = hubConnection?.SendAsync("SendToGroup", "NEWS", serializedArticle);
                }
            }
        });
    }
}
