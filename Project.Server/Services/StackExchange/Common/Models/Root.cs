using System.Text.Json.Serialization;

namespace Project.Server.Services.StackExchange.Common.Models;
public class Root<T> where T : class
{
    [JsonPropertyName("items")]
    public List<T>? Items
    {
        get; set;
    }

    [JsonPropertyName("has_more")]
    public bool HasMore
    {
        get; set;
    }

    [JsonPropertyName("quota_max")]
    public int QuotaMax
    {
        get; set;
    }

    [JsonPropertyName("quota_remaining")]
    public int QuotaRemaining
    {
        get; set;
    }
}
