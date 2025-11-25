namespace OutboxKafka.DataAccess.Entities;

public class OutboxMessage
{
    public long Event_Id { get; set; }

    public string Event_Payload { get; set; }

    public DateTime Event_Date { get; set; }

    public bool IsMessageDispatched { get; set; }
}