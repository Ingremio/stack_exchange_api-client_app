using Microsoft.Extensions.Options;
using Project.Server.Helpers;
using Project.Server.Services.Common;
using Project.Server.Services.StackExchange.Common.Models;
using Project.Server.Services.StackExchange.Common.Slugs;
using Project.Server.Services.StackExchange.Tags.Models;
using Project.Server.Services.StackExchange.Users.Models;

namespace Project.Server.Services.StackExchange;
public partial class StackExchangeHttpApiService : HttpApiService {
    private readonly IOptions<ApiSettings> _apiSettings;

    public StackExchangeHttpApiService(ILogger<StackExchangeHttpApiService> logger, HttpClient client, IOptions<ApiSettings> appSettings) : base(logger, client) {
        _apiSettings = appSettings;
    }

    public async Task<Root<Token>?> GetAccessTokenProperties(string? accessToken, CancellationToken cancellationToken) {
        if (accessToken == null)
            throw new ArgumentNullException(nameof(accessToken), "The access token cannot be null");

        if (_apiSettings.Value.AccessTokenPropertiesEndpoint == null)
            throw new Exception("The access token properties endpoint is not set");

        var httpApiResult = await GetData<Root<Token>>(string.Format(_apiSettings.Value.AccessTokenPropertiesEndpoint, accessToken), cancellationToken, setHeaders: SetAdditionalHeaders);

        if (httpApiResult != null && httpApiResult.IsSuccessStatusCode) {
            return httpApiResult.Result;
        }

        return null;
    }

    public async Task<Root<Token>?> InvalidateAccessToken(string? accessToken, CancellationToken cancellationToken) {
        if (accessToken == null)
            throw new ArgumentNullException(nameof(accessToken), "The access token cannot be null");

        if (_apiSettings.Value.InvalidateAccessTokenEndpoint == null)
            throw new Exception("The invalidate access token endpoint is not set");

        var httpApiResult = await GetData<Root<Token>>(string.Format(_apiSettings.Value.InvalidateAccessTokenEndpoint, accessToken), cancellationToken, setHeaders: SetAdditionalHeaders);

        if (httpApiResult != null && httpApiResult.IsSuccessStatusCode) {
            return httpApiResult.Result;
        }

        return null;
    }

    public async Task<Root<Tag>?> GetTagsFromStackOverflow(string? accessToken, Pagination? pagination, CancellationToken cancellationToken) {
        if (accessToken == null)
            throw new ArgumentNullException(nameof(accessToken), "The access token cannot be null");

        if (_apiSettings.Value.TagsEndpoint == null)
            throw new Exception("The tags endpoint is not set");

        var httpApiResult = await GetData<Root<Tag>>(string.Format(_apiSettings.Value.TagsEndpoint, accessToken), cancellationToken, (requestMessage) => {
            if (_apiSettings.Value.Key == null)
                throw new Exception("The stack exchange api key is not set");

            var page = (pagination?.Page ?? 0).ToString();
            var pageSize = (pagination?.PageSize ?? 0).ToString();

            var parameters = new Dictionary<string, string> {
                { "key", _apiSettings.Value.Key },
                { "access_token", accessToken },
                { "page" , page },
                { "pagesize" , pageSize },
                { "order",  OrderSlugs.Desc },
                { "sort", SortSlugs.Popular },
                { "site", SiteSlugs.Stackoverflow }
            };

            requestMessage.Content = new FormUrlEncodedContent(parameters);
        }, SetAdditionalHeaders);

        if (httpApiResult != null && httpApiResult.IsSuccessStatusCode) {
            return httpApiResult.Result;
        }

        return null;
    }

    protected override void SetContent(HttpRequestMessage requestMessage, HttpContent? httpContent) {
        if (_apiSettings.Value.Key == null)
            throw new Exception("The stack exchange api key is not set");

        var parameters = new Dictionary<string, string> {
            { "key", _apiSettings.Value.Key }
        };

        requestMessage.Content = new FormUrlEncodedContent(parameters);
    }

    private bool SetAdditionalHeaders(HttpRequestMessage message) {
        message.Headers.Add("cache-control", "no-cache");
        return true;
    }
}