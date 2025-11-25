using OutboxKafkaConsumer.BackgroundWorks;

namespace OutboxKafkaConsumer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumer>();
        builder.Services.AddSingleton<IHostedService, KafkaMessageProcessor>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}