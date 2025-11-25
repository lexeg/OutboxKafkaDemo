using System.Text.Json;

namespace OutboxKafkaConsole;

class Program
{
    private const string RequestUri = "http://localhost:5014";
    private const string CreateOrderRequest = $"{RequestUri}/api/Order/CreateOrder";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Start sending messages to OutboxKafka... Press ENTER to start");
        Console.ReadLine();
        const int requestCount = 1_000_000;
        var messages = CreateMessages(requestCount);
        await SendMessages(CreateOrderRequest, messages);
        Console.WriteLine("Sending messages complete.");
    }

    private static List<string> CreateMessages(int requestCount)
    {
        var messages = new List<string>();
        for (var i = 0; i < requestCount; i++)
        {
            var order = new Order
            {
                Order_Id = i + 1,
                Customer_Id = 45,
                Order_Date = new DateTime(2024, 6, 5),
                Amount = 4567
            };
            messages.Add(JsonSerializer.Serialize(order));
        }

        return messages;
    }

    private static async Task SendMessages(string createOrderRequest, List<string> messages)
    {
        var chunks = Enumerable.Range(1, messages.Count).Chunk(messages.Count / Environment.ProcessorCount);
        var tasks = chunks.Select(chunk => Send(createOrderRequest, messages, chunk)).ToList();
        await Task.WhenAll(tasks);
    }

    private static async Task Send(string createOrderRequest, List<string> messages, int[] chunk)
    {
        var client = new HttpClient();
        foreach (var ch in chunk)
        {
            var message = messages[ch];
            var request = new HttpRequestMessage(HttpMethod.Post, createOrderRequest);
            var content = new StringContent(message, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine($"Order {ch} has been sent successfully.");
        }
    }
}