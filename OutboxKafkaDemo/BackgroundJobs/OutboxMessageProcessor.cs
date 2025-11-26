using OutboxKafka.DataAccess.Contexts;
using OutboxKafka.DataAccess.Entities;
using OutboxKafkaDemo.Infrastructure;

namespace OutboxKafkaDemo.BackgroundJobs;

public class OutboxMessageProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    // private readonly IKafkaProducer _producer;
    private IKafkaProducer _producer;

    public OutboxMessageProcessor(IServiceScopeFactory scopeFactory/*, IKafkaProducer producer*/)
    {
        _scopeFactory = scopeFactory;
        // _producer = producer;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await PublishOutboxMessagesAsync(cancellationToken);
        }
    }

    private async Task PublishOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            _producer = scope.ServiceProvider.GetRequiredService<IKafkaProducer>();
            await using var _dbContext =
                scope.ServiceProvider.GetRequiredService
                    <ApplicationDbContext>();

            var entities = _dbContext.OutboxMessages.Where(om => om.IsMessageDispatched != true).ToList();

            foreach (var entity in entities)
            {
                try
                {
                    await _producer.SendMessageToKafkaAsync(entity);

                    entity.IsMessageDispatched = true;
                    entity.Date = DateTime.UtcNow;

                    _dbContext.OutboxMessages.Update(entity);
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        catch
        {
            throw;
        }

        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    }
}