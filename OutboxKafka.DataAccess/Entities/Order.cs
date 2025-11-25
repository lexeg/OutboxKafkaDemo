using System.ComponentModel.DataAnnotations.Schema;

namespace OutboxKafka.DataAccess.Entities;

public class Order
{
    [Column("Order_Id")]
    public long Id { get; set; }

    [Column("Customer_Id")]
    public int CustomerId { get; set; }

    [Column("Order_Date")]
    public DateTime Date { get; set; }

    [Column("Order_Amount")]
    public int Amount { get; set; }
}