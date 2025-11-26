using OutboxKafka.DataAccess.Entities;

namespace OutboxKafka.DataAccess.Repositories;

public interface IOutboxMessageRepository
{
    Task<IReadOnlyCollection<OutboxMessageEntity>> GetUnsentMessagesAsync();
    Task<IReadOnlyCollection<OutboxMessageEntity>> GetMessagesByIdsAsync(IEnumerable<int> ids);
    Task UpdateAsync(OutboxMessageEntity entity, bool status);
}