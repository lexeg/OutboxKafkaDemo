using Microsoft.EntityFrameworkCore;
using OutboxKafka.DataAccess.Contexts;
using OutboxKafka.DataAccess.Repositories;
using OutboxKafkaDemo.BackgroundJobs;
using OutboxKafkaDemo.Configuration;
using OutboxKafkaDemo.Infrastructure;
using OutboxKafkaDemo.Services;

namespace OutboxKafkaDemo;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.Configure<ProducerConfigSettings>(configuration.GetSection(ProducerConfigSettings.Key));

        services.AddControllers();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
        services.AddScoped<IKafkaProducer, KafkaProducer>();
        services.AddSingleton<IHostedService, OutboxMessageProcessor>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}