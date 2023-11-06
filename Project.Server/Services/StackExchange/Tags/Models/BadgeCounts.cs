using System.Text.Json.Serialization;

namespace Project.Server.Services.StackExchange.Tags.Models;
public class BadgeCounts
{
    [JsonPropertyName("bronze")]
    public int Bronze
    {
        get; set;
    }

    [JsonPropertyName("silver")]
    public int Silver
    {
        get; set;
    }

    [JsonPropertyName("gold")]
    public int Gold
    {
        get; set;
    }
}
