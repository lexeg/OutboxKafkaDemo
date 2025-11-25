using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using OutboxKafka.DataAccess.Contexts;
using OutboxKafka.DataAccess.Entities;

namespace OutboxKafka.DataAccess.Repositories;

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
        var unsentMessages = await _context.OutboxMessages.Where(e => e.IsMessageDispatched != true).ToListAsync();
        var result = new ReadOnlyCollection<OutboxMessage>(unsentMessages);
        return result;
    }

    public async Task<IReadOnlyCollection<OutboxMessage>> GetMessagesByIdsAsync(IEnumerable<int> ids)
    {
        var orders = await _context.OutboxMessages.ToListAsync();
        return new ReadOnlyCollection<OutboxMessage>(orders);
    }

    public async Task UpdateAsync(OutboxMessage message, bool status)
    {
        var entity = _context.OutboxMessages.FirstOrDefault(o => o.Id == message.Id);

        if (entity != null)
        {
            entity.Id = message.Id;
            entity.Date = message.Date;
            entity.Payload = message.Payload;
            entity.IsMessageDispatched = message.IsMessageDispatched;
            await _context.SaveChangesAsync();
        }
    }
}