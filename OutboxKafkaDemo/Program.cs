using Microsoft.EntityFrameworkCore;
using OutboxKafka.DataAccess.Contexts;
using OutboxKafka.DataAccess.Repositories;
using OutboxKafkaDemo.BackgroundJobs;
using OutboxKafkaDemo.Infrastructure;
using OutboxKafkaDemo.Services;

namespace OutboxKafkaDemo;

public class Program
{
    public static void Main(string[] args)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
        builder.Services.AddScoped<IKafkaProducer, KafkaProducer>();
        builder.Services.AddSingleton<IHostedService, OutboxMessageProcessor>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseAuthorization();
        app.MapControllers();
        app.Run();
        /*var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped <IOutboxMessageRepository, OutboxMessageRepository>();
        builder.Services.AddScoped<IKafkaProducer, KafkaProducer>();
        builder.Services.AddSingleton<IHostedService, OutboxMessageProcessor>();

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                        new WeatherForecast
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            TemperatureC = Random.Shared.Next(-20, 55),
                            Summary = summaries[Random.Shared.Next(summaries.Length)]
                        })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast");

        app.Run();*/
    }
}