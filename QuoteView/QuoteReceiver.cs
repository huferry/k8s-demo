using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QuoteView;

public class QuoteReceiver
{
    private readonly IHubContext<QuotesHub> _hubContext;
    private readonly ILogger<QuoteReceiver> _logger;

    public QuoteReceiver(IHubContext<QuotesHub> hubContext, ILogger<QuoteReceiver> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }
    
    public void Setup()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "queue-user",
            Password = "ToPSecreT"
        };

        var connection = factory.CreateConnection();

        var channel = connection.CreateModel();
        
        channel.ExchangeDeclare("quote-feed",ExchangeType.Fanout, true, false, null);

        var queueName = channel.QueueDeclare().QueueName;
        channel.QueueBind(queue: queueName, exchange: "quote-feed", routingKey: string.Empty);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var instrument = JsonSerializer.Deserialize<InstrumentQuotes>(message);
            
            if (instrument is not null)
            {
                _hubContext.Clients.All.SendAsync("update", instrument);
                _logger.LogInformation("{Symbol}={Last}", instrument.Symbol, instrument.Quotes.Last);
            }
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }
}

public record InstrumentQuotes(string Name, string Symbol, Quotes Quotes, 
    [property: JsonPropertyName("quote")]
    double Last);
public record Quotes(double Last, DateTime LastTimestamp, double Open, double High, double Low, int Volume);