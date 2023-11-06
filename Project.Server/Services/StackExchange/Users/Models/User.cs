using System.Text.Json.Serialization;
using Project.Server.Services.StackExchange.Tags.Models;

namespace Project.Server.Services.StackExchange.Users.Models;
public class User {
    [JsonPropertyName("badge_counts")]
    public BadgeCounts? BadgeCounts {
        get; set;
    }

    [JsonPropertyName("view_count")]
    public int ViewCount {
        get; set;
    }

    [JsonPropertyName("down_vote_count")]
    public int DownVoteCount {
        get; set;
    }

    [JsonPropertyName("up_vote_count")]
    public int UpVoteCount {
        get; set;
    }

    [JsonPropertyName("answer_count")]
    public int AnswerCount {
        get; set;
    }

    [JsonPropertyName("question_count")]
    public int QuestionCount {
        get; set;
    }

    [JsonPropertyName("account_id")]
    public int AccountId {
        get; set;
    }

    [JsonPropertyName("is_employee")]
    public bool IsEmployee {
        get; set;
    }

    [JsonPropertyName("last_modified_date")]
    public int LastModifiedDate {
        get; set;
    }

    [JsonPropertyName("last_access_date")]
    public int LastAccessDate {
        get; set;
    }

    [JsonPropertyName("reputation_change_year")]
    public int ReputationChangeYear {
        get; set;
    }

    [JsonPropertyName("reputation_change_quarter")]
    public int ReputationChangeQuarter {
        get; set;
    }

    [JsonPropertyName("reputation_change_month")]
    public int ReputationChangeMonth {
        get; set;
    }

    [JsonPropertyName("reputation_change_week")]
    public int ReputationChangeWeek {
        get; set;
    }

    [JsonPropertyName("reputation_change_day")]
    public int ReputationChangeDay {
        get; set;
    }

    [JsonPropertyName("reputation")]
    public int Reputation {
        get; set;
    }

    [JsonPropertyName("creation_date")]
    public int CreationDate {
        get; set;
    }

    [JsonPropertyName("user_type")]
    public required string UserType {
        get; set;
    }

    [JsonPropertyName("user_id")]
    public int UserId {
        get; set;
    }

    [JsonPropertyName("accept_rate")]
    public int AcceptRate {
        get; set;
    }

    [JsonPropertyName("about_me")]
    public required string AboutMe {
        get; set;
    }

    [JsonPropertyName("location")]
    public required string Location {
        get; set;
    }

    [JsonPropertyName("website_url")]
    public required string WebsiteUrl {
        get; set;
    }

    [JsonPropertyName("link")]
    public required string Link {
        get; set;
    }

    [JsonPropertyName("profile_image")]
    public required string ProfileImage {
        get; set;
    }

    [JsonPropertyName("display_name")]
    public required string DisplayName {
        get; set;
    }
}