using Common.CommandHandler;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using OrderService.Domain;
using OrderService.Domain.Configure;
using OrderService.Domain.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks()
    .AddCheck<AvailabiltyHealthCheck>(AvailabiltyHealthCheck.Name)
    .AddCheck<RabbitMQHealthCheck>(RabbitMQHealthCheck.Name);


builder.Services.AddRabbitMQ();
builder.Services.AddDispatcher();
builder.Services.AddCommandHandlers();
builder.Services.AddMassTransit(x =>
{
    var rabbitMQConfig = builder.Configuration.GetSection("RabbitMQ");
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMQConfig["HostName"], "/", h =>
        {
            h.Username(rabbitMQConfig["Username"]);
            h.Password(rabbitMQConfig["Password"]);
        });

        cfg.ConfigureEndpoints(context);
    });
});
var app = builder.Build();

app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(
            new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    exception = e.Value.Exception?.Message,
                    duration = e.Value.Duration.ToString()
                })
            });
        await context.Response.WriteAsync(result);
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
