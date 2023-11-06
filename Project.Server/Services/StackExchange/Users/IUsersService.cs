using Project.Server.Services.StackExchange.Common.Models;
using Project.Server.Services.StackExchange.Users.Models;

namespace Project.Server.Services.StackExchange.Users;
public interface IUsersService {
    Task<Root<Token>?> GetAccessTokenProperties(string? accessToken);
    Task<Root<Token>?> InvalidateAccessToken(string? accessToken);
}