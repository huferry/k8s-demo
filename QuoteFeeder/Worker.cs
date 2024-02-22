namespace QuoteFeeder;

public class Worker : BackgroundService
{
    private const int Liquidity = 30;
    private readonly ILogger<Worker> _logger;
    private readonly Instrument[] _instruments;
    private readonly Random _random = new(DateTime.Now.Millisecond);
    private readonly Sender _sender;

    public Worker(ILogger<Worker> logger, Instrument[] instruments, Sender sender)
    {
        _logger = logger;
        _instruments = instruments;
        _sender = sender;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            RandomizeQuotes();
            await Task.Delay(100, stoppingToken);
        }
    }

    private Instrument? GetNext()
    {
        if (!ShouldChange) return default;
        
        var idx = _random.Next(_instruments.Length);
        return _instruments[idx];
    }

    private void RandomizeQuotes()
    {
        var inst = GetNext();
        var oldValue = inst?.Quotes.Last;
        inst?.Quotes.Randomize();
        if (inst is not null && !oldValue.Equals(inst.Quotes.Last))
        {
            var diff = Math.Round((double)(inst.Quotes.Last - oldValue), 5);
            var sign = diff >= 0 ? "+" : string.Empty;
            _sender.Publish(inst);
            _logger.Log(LogLevel.Information, "{Symbol}={LastPrice} ({Sign}{Diff}) [{Volume}]", 
                inst.Symbol, inst.Quotes.Last, sign, diff, inst.Quotes.Volume);
        }
    }

    private bool ShouldChange => _random.Next(100) < Liquidity;
}