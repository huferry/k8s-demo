using System.Text.Json;

namespace QuoteFeeder;

// ReSharper disable once NotAccessedPositionalProperty.Global
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
            if (value < Low || Low == 0.0) Low = Math.Round(value, 5);
            if (value > High || High == 0.0) High = Math.Round(value, 5);
            if (Open == 0.0) Open = Math.Round(value, 5);
            LastTimestamp = DateTime.UtcNow;
            _last = Math.Round(value, 5);
        }
    }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public DateTime LastTimestamp { get; private set; }

    public double Open { get; private set; }
    public double High { get; private set; }
    public double Low { get; private set; }
    public int Volume { get; private set; }

    public void Randomize()
    {
        Last = Math.Max(0.1, _last + CreateQuoteMovement(_last));
        Volume += new Random().Next(1, 100);
    }

    public static Quotes CreateRandom(double last) => new()
        {
            Last = last,
            Open = Math.Round(last + CreateQuoteMovement(last), 5),
            Volume = new Random().Next(10, 5000),
        };

    private static double CreateQuoteMovement(double price) => price * new Random(DateTime.Now.Millisecond).Next(-1, 2) / 1000;
}