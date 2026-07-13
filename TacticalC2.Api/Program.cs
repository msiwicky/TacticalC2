using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TacticalC2.Api.Conventions;
using TacticalC2.Api.Hubs;
using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Application.Units.Commands.UpdateUnitPosition;
using TacticalC2.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseParameterTransformer()));
});
builder.Services.AddSignalR(); 

builder.Services.AddSingleton<IUnitRepository, InMemoryUnitStore>();

builder.Services.AddDbContext<TacticalDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TacticalDb")));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateUnitPositionCommand).Assembly));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowTestClient", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

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
app.UseCors("AllowTestClient");


app.Run();
