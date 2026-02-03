using System.Text.Json.Serialization;

namespace OutboxKafka.Common;

public class Order
{
    [JsonPropertyName("Order_Id")]
    public long OrderId { get; set; }

    [JsonPropertyName("Customer_Id")]
    public int CustomerId { get; set; }

    [JsonPropertyName("Order_Date")]
    public DateTime OrderDate { get; set; }

    [JsonPropertyName("Order_Amount")]
    public int Amount { get; set; }
}