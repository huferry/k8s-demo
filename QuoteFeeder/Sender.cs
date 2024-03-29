using System.Text;
using System.Text.Json;
using Polly;
using RabbitMQ.Client;

namespace QuoteFeeder;

public class QueueConfig
{
    public string HostName => Environment.GetEnvironmentVariable("QUEUE_HOST") ?? "localhost";
    public string UserName => Environment.GetEnvironmentVariable("QUEUE_USER") ?? "guest";
    public string Password => Environment.GetEnvironmentVariable("QUEUE_PASSWORD") ?? string.Empty;
}

public class Sender
{
    private readonly ILogger<Sender> _logger;
    private IModel? _channel;
    private readonly QueueConfig _config;
    private IBasicProperties? _properties;

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

        var conn = Policy<IConnection>
            .Handle<Exception>()
            .WaitAndRetry(20, 
                _ => TimeSpan.FromSeconds(10), 
                (_, _) => _logger.LogWarning("Connection timeout.. retrying"))
            .Execute(() => factory.CreateConnection());
        
        _channel = conn.CreateModel();
        _channel.ExchangeDeclare("quote-feed", ExchangeType.Fanout, true, false, null);

        _properties = _channel.CreateBasicProperties();
        _properties.Persistent = false;
    }

    public void Publish(Instrument instrument)
    {
        if (_channel is null)
        {
            _logger.LogInformation("Channel not setup, will setup now");
            Setup();
        }

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(instrument));

        _channel.BasicPublish("quote-feed", string.Empty, _properties, body);
    }
}