using System.Collections;

namespace QuoteFeeder;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Instrument[] _instruments;
    private readonly Random _random = new(DateTime.Now.Millisecond);

    public Worker(ILogger<Worker> logger, Instrument[] instruments)
    {
        _logger = logger;
        _instruments = instruments;
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
        inst?.Quotes.Randomize();
        if (inst is not null) _logger.Log(LogLevel.Information, "{Symbol}={LastPrice}", inst.Symbol, inst.Quotes.Last);
    }

    private bool ShouldChange => _random.Next(100) < 40;
}