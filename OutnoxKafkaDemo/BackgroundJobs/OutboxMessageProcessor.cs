using OutnoxKafkaDemo.DataAccess.Contexts;
using OutnoxKafkaDemo.DataAccess.Entities;
using OutnoxKafkaDemo.Infrastructure;

namespace OutnoxKafkaDemo.BackgroundJobs;

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

            List<OutboxMessage> messages = _dbContext.OutboxMessages.Where
                (om => om.IsMessageDispatched != true).ToList();

            foreach (OutboxMessage outboxMessage in messages)
            {
                try
                {
                    await _producer.SendMessageToKafkaAsync(outboxMessage);

                    outboxMessage.IsMessageDispatched = true;
                    outboxMessage.Event_Date = DateTime.UtcNow;

                    _dbContext.OutboxMessages.Update(outboxMessage);
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