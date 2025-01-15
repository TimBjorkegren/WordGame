using System.Text.Json.Serialization;

namespace app;

public class WordRequest
{
    [JsonPropertyName("word")]
    public string Word { get; set; }
}
