using System.Collections.ObjectModel;
using OutnoxKafkaDemo.DataAccess.Contexts;
using OutnoxKafkaDemo.DataAccess.Entities;

namespace OutnoxKafkaDemo.DataAccess.Repositories;

public class OutboxMessageRepository : IOutboxMessageRepository
{
    private readonly ApplicationDbContext _context;
    private IReadOnlyCollection<OutboxMessage> _outboxMessages;

    public OutboxMessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<OutboxMessage> OutboxMessages
    {
        get
        {
            return _outboxMessages ??= new
                ReadOnlyCollection<OutboxMessage>
                (_context.OutboxMessages.ToList());
        }
    }

    public async Task<IReadOnlyCollection<OutboxMessage>> GetUnsentMessagesAsync()
    {
        List<OutboxMessage>? unsentMessages =
            _context.OutboxMessages.Where
                (e => e.IsMessageDispatched != true).ToList();
        ReadOnlyCollection<OutboxMessage>? result = new ReadOnlyCollection
            <OutboxMessage>(unsentMessages);
        return result;
    }

    public async Task<IReadOnlyCollection<OutboxMessage>> GetMessagesByIdsAsync(IEnumerable<int> ids)
    {
        List<OutboxMessage>? orders = _context.OutboxMessages.ToList();
        var readOnlyOrders = new ReadOnlyCollection<OutboxMessage>(orders);
        return readOnlyOrders;
    }

    public async Task UpdateAsync(OutboxMessage message, bool status)
    {
        var entity = _context.OutboxMessages.FirstOrDefault
            (o => o.Event_Id == message.Event_Id);

        if (entity != null)
        {
            entity.Event_Id = message.Event_Id;
            entity.Event_Date = message.Event_Date;
            entity.Event_Payload = message.Event_Payload;
            entity.IsMessageDispatched = message.IsMessageDispatched;
            await _context.SaveChangesAsync();
        }
    }
}