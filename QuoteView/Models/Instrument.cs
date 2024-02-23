using System.Text.Json.Serialization;

namespace QuoteView.Models;

public record Instrument(
    [property: JsonPropertyName("name")]
    string Name, 
    [property: JsonPropertyName("symbol")]
    string Symbol
    );