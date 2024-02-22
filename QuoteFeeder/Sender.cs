using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace QuoteFeeder;

public class QueueConfig
{
    public string HostName => Environment.GetEnvironmentVariable("QUEUE_HOST") ?? "localhost";
    public string UserName => Environment.GetEnvironmentVariable("QUEUE_USER") ?? "gueest";
    public string Password => Environment.GetEnvironmentVariable("QUEUE_PASSWORD") ?? string.Empty;
}

public class Sender
{
    private readonly ILogger<Sender> _logger;
    private IModel? _channel;
    private readonly QueueConfig _config;

    public Sender(ILogger<Sender> logger, QueueConfig config)
    {
        _logger = logger;
        _config = config;
    }

    private void Setup()
    {
        var factory = new ConnectionFactory
        {
            HostName = _config.HostName,
            UserName = _config.UserName,
            Password = _config.Password,
        };
        var conn = factory.CreateConnection();
        _channel = conn.CreateModel();

        _channel.QueueDeclare(
            queue: "quote-feed",
            durable: true,
            exclusive: false,
            autoDelete: true,
            arguments: null);
    }

    public void Publish(Instrument instrument)
    {
        if (_channel is null)
        {
            _logger.LogInformation("Channel not setup, will setup now");
            Setup();
        }

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(instrument));
        
        _channel.BasicPublish
        (
            exchange: string.Empty,
            routingKey: "quote-feed",
            basicProperties: null,
            body: body);
    }
}