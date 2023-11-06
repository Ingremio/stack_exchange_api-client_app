using System.Text.Json.Serialization;

namespace Project.Server;
public class ApiUser {
    [JsonPropertyName("id")]
    public string? Id {
        get; set;
    }
    [JsonPropertyName("name")]
    public string? Name {
        get; set;
    }
    [JsonPropertyName("token")]
    public string? Token {
        get; set;
    }
}