using System.Text.Json.Serialization;

namespace Project.Server.Services.StackExchange.Users.Models;
public class Token {
    [JsonPropertyName("account_id")]
    public int AccountId {
        get; set;
    }

    [JsonPropertyName("expires_on_date")]
    public int ExpiresOnDate {
        get; set;
    }

    [JsonPropertyName("access_token")]
    public required string AccessToken {
        get; set;
    }
}