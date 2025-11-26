namespace OutboxKafka.DataAccess.Entities;

public class OrderEntity
{
    public long Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime Date { get; set; }

    public int Amount { get; set; }
}