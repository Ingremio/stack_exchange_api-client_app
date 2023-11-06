using Project.Server.Services.StackExchange.Common.Models;
using Project.Server.Services.StackExchange.Tags.Models;

namespace Project.Server.Services.StackExchange.Tags;
public interface ITagsService {
    Task<Root<Tag>?> GetTagsFromStackOverflow(string? accessToken, Pagination? pagination);
}