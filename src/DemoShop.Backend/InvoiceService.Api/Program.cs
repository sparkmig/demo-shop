using Common.CommandHandler;
using InvoiceService.Api.Consumers;
using InvoiceService.Domain;
using InvoiceService.Infrastructure;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDispatcher();
builder.Services.AddCommandHandlers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderFulfilledConsumer>();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
