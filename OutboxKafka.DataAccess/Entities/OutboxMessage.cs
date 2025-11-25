using System.ComponentModel.DataAnnotations.Schema;

namespace OutboxKafka.DataAccess.Entities;

public class OutboxMessage
{
    [Column("Event_Id")]
    public long Id { get; set; }

    [Column("Event_Payload")]
    public string Payload { get; set; }

    [Column("Event_Date")]
    public DateTime Date { get; set; }

    [Column("IsMessageDispatched")]
    public bool IsMessageDispatched { get; set; }
}