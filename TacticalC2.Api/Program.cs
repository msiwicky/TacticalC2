using Scalar.AspNetCore;
using TacticalC2.Api.Hubs;
using TacticalC2.Api.InMemory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers(); 
builder.Services.AddSignalR(); 

builder.Services.AddSingleton<InMemoryUnitStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapHub<UnitsHub>("/hubs/units");
app.MapControllers();

app.Run();
