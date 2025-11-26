namespace OutboxKafka.DataAccess.Entities;

public class OutboxMessageEntity
{
    public long Id { get; set; }

    public string Payload { get; set; }

    public DateTime Date { get; set; }

    public bool IsMessageDispatched { get; set; }
}