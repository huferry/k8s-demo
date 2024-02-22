using System.Text.Json;

namespace QuoteFeeder;

public record Instrument(string Name, string Symbol, Quotes Quotes)
{
    public static IEnumerable<Instrument> ReadFromJson(string path) => JsonSerializer
        .Deserialize<InstrumentDto[]>(File.ReadAllText(path), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        })!
        .Select(data => new Instrument(data.Name, data.Symbol, Quotes.CreateRandom(data.Quote)))
        .ToArray();

    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed record InstrumentDto(string Name, string Symbol, double Quote);
};

public class Quotes
{
    private double _last;
    
    public double Last
    {
        get => _last;
        set
        {
            if (value < Low || Low == 0.0) Low = value;
            if (value > High || High == 0.0) High = value;
            if (Open == 0.0) Open = value;
            LastTimestamp = DateTime.UtcNow;
            _last = value;
        }
    }

    public DateTime LastTimestamp { get; private set; }

    public double Open { get; private set; }
    public double High { get; private set; }
    public double Low { get; private set; }

    public void Randomize() => Last = _last + CreateQuoteMovement(_last);

    public static Quotes CreateRandom(double last) => new()
        {
            Last = last,
            Open = last + CreateQuoteMovement(last),
        };

    private static double CreateQuoteMovement(double price) => price * new Random(DateTime.Now.Millisecond).Next(-5, 6) / 100;
}