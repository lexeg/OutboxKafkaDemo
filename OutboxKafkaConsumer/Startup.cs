using OutboxKafkaConsumer.BackgroundWorks;
using OutboxKafkaConsumer.Configuration;

namespace OutboxKafkaConsumer;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ConsumerConfigSettings>(configuration.GetSection(ConsumerConfigSettings.Key));
        services.AddControllers();
        services.AddSingleton<IKafkaConsumer, KafkaConsumer>();
        services.AddSingleton<IHostedService, KafkaMessageProcessor>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}