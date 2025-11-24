using OutnoxKafkaDemo.DataAccess.Entities;

namespace OutnoxKafkaDemo.DataAccess.Repositories;

public interface IOutboxMessageRepository
{
    Task<IReadOnlyCollection<OutboxMessage>> GetUnsentMessagesAsync();
    Task<IReadOnlyCollection<OutboxMessage>> GetMessagesByIdsAsync(IEnumerable<int> ids);
    Task UpdateAsync(OutboxMessage message, bool status);
}