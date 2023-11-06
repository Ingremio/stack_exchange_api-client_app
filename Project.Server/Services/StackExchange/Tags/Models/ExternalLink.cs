using System.Text.Json.Serialization;

namespace Project.Server.Services.StackExchange.Tags.Models;
public class ExternalLink {
    [JsonPropertyName("type")]
    public string? Type {
        get; set;
    }

    [JsonPropertyName("link")]
    public string? Link {
        get; set;
    }
}