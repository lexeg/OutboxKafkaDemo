using System.ComponentModel.DataAnnotations.Schema;

namespace OutboxKafka.DataAccess.Entities;

public class Order
{
    public long Order_Id { get; set; }

    public int Customer_Id { get; set; }

    public DateTime Order_Date { get; set; }

    [Column("Order_Amount")]
    public int Amount { get; set; }
}