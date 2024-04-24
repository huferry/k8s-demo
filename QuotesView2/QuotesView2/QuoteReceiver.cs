using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SignalR;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QuotesView2;

public class QueueConfig
{
    public string HostName => Environment.GetEnvironmentVariable("QUEUE_HOST") ?? "localhost";
    public string UserName => Environment.GetEnvironmentVariable("QUEUE_USER") ?? "guest";
    public string Password => Environment.GetEnvironmentVariable("QUEUE_PASSWORD") ?? string.Empty;
}


public class QuoteReceiver
{
    private readonly IHubContext<QuotesHub> _hubContext;
    private readonly ILogger<QuoteReceiver> _logger;
    private readonly QueueConfig _queueConfig;

    public QuoteReceiver(IHubContext<QuotesHub> hubContext, ILogger<QuoteReceiver> logger, QueueConfig queueConfig)
    {
        _hubContext = hubContext;
        _logger = logger;
        _queueConfig = queueConfig;
    }
    
    public void Setup()
    {
        var factory = new ConnectionFactory
        {
            HostName = _queueConfig.HostName,
            UserName = _queueConfig.UserName,
            Password = _queueConfig.Password,
            RequestedConnectionTimeout = TimeSpan.FromSeconds(10),
        };
        
        var connection = Policy<IConnection>
            .Handle<Exception>()
            .WaitAndRetry(20, 
                _ => TimeSpan.FromSeconds(10), 
                (_, _) => _logger.LogWarning("Connection timeout.. retrying"))
            .Execute(() => factory.CreateConnection());

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

    private IConnection CreateConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = _queueConfig.HostName,
            UserName = _queueConfig.UserName,
            Password = _queueConfig.Password,
        };

        return factory.CreateConnection();
    }
}

public record InstrumentQuotes(string Name, string Symbol, Quotes Quotes, 
    [property: JsonPropertyName("quote")]
    double Last);
public record Quotes(double Last, DateTime LastTimestamp, double Open, double High, double Low, int Volume);