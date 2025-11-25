namespace OutboxKafkaDemo;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(x => x.AddJsonFile("appsettings.Development.json"))
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
}