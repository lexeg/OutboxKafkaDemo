namespace OutboxKafkaConsole;

public class Order
{
    public long Order_Id { get; set; }

    public int Customer_Id { get; set; }

    public DateTime Order_Date { get; set; }

    public int Amount { get; set; }
}