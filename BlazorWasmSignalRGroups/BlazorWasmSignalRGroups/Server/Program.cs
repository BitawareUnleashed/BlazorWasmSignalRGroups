using BlazorWasmSignalRGroups.Server.Models.Hubs;
using BlazorWasmSignalRGroups.Server.Workers;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSignalR();
builder.Services.AddSingleton<WatchWorker>();
builder.Services.AddSingleton<NewsWorker>();

var app = builder.Build();
app.Services.GetRequiredService<WatchWorker>().ExecuteAsync("https://localhost:7014/communicationhub",new CancellationToken());
app.Services.GetRequiredService<NewsWorker>().GetNewsCaller("https://localhost:7014/communicationhub",new CancellationToken());


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.MapHub<CommunicationHub>("/communicationhub");
app.Run();
