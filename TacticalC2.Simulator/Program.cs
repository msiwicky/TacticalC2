using TacticalC2.Simulator;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient("TacticalApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5136");
});

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<TacticalApiClient>();

var host = builder.Build();
host.Run();
