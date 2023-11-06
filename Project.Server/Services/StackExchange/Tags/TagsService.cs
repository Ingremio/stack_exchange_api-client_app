using Project.Server.Services.StackExchange.Common.Models;
using Project.Server.Services.StackExchange.Tags.Models;

namespace Project.Server.Services.StackExchange.Tags;
public class TagsService : ITagsService {
    private readonly ILogger _logger;
    private readonly StackExchangeHttpApiService _httpApiService;

    public TagsService(ILogger<TagsService> logger, StackExchangeHttpApiService httpApiService) {
        _logger = logger;
        _httpApiService = httpApiService;
    }

    public async Task<Root<Tag>?> GetTagsFromStackOverflow(string? accessToken, Pagination? pagination) {
        CancellationTokenSource cts = new CancellationTokenSource();

        try {
            return await _httpApiService.GetTagsFromStackOverflow(accessToken, pagination, cts.Token);
        } catch (Exception ex) {
            _logger.LogError(ex, ex.Message);
        }

        return null;
    }
}