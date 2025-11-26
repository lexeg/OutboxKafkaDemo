using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using OutboxKafka.DataAccess.Contexts;
using OutboxKafka.DataAccess.Entities;

namespace OutboxKafka.DataAccess.Repositories;

public class OutboxMessageRepository : IOutboxMessageRepository
{
    private readonly ApplicationDbContext _context;
    private IReadOnlyCollection<OutboxMessageEntity> _outboxMessages;

    public OutboxMessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<OutboxMessageEntity> OutboxMessages
    {
        get
        {
            return _outboxMessages ??= new
                ReadOnlyCollection<OutboxMessageEntity>
                (_context.OutboxMessages.ToList());
        }
    }

    public async Task<IReadOnlyCollection<OutboxMessageEntity>> GetUnsentMessagesAsync()
    {
        var unsentMessages = await _context.OutboxMessages.Where(e => e.IsMessageDispatched != true).ToListAsync();
        var result = new ReadOnlyCollection<OutboxMessageEntity>(unsentMessages);
        return result;
    }

    public async Task<IReadOnlyCollection<OutboxMessageEntity>> GetMessagesByIdsAsync(IEnumerable<int> ids)
    {
        var orders = await _context.OutboxMessages.ToListAsync();
        return new ReadOnlyCollection<OutboxMessageEntity>(orders);
    }

    public async Task UpdateAsync(OutboxMessageEntity entity, bool status)
    {
        var messageEntity = _context.OutboxMessages.FirstOrDefault(o => o.Id == entity.Id);

        if (messageEntity != null)
        {
            messageEntity.Id = entity.Id;
            messageEntity.Date = entity.Date;
            messageEntity.Payload = entity.Payload;
            messageEntity.IsMessageDispatched = entity.IsMessageDispatched;
            await _context.SaveChangesAsync();
        }
    }
}