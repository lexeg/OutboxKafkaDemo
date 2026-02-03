using System.Text.Json.Serialization;

namespace OutboxKafka.DataAccess.Entities;

public class OrderEntity
{
    [JsonPropertyName("Order_Id")]
    public long Id { get; set; }

    [JsonPropertyName("Customer_Id")]
    public int CustomerId { get; set; }

    [JsonPropertyName("Order_Date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("Order_Amount")]
    public int Amount { get; set; }
}