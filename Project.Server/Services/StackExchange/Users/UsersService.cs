using Project.Server.Services.StackExchange.Common.Models;
using Project.Server.Services.StackExchange.Users.Models;

namespace Project.Server.Services.StackExchange.Users;
public class UsersService : IUsersService {
    private readonly ILogger _logger;
    private readonly StackExchangeHttpApiService _httpApiService;

    public UsersService(ILogger<UsersService> logger, StackExchangeHttpApiService httpApiService) {
        _logger = logger;
        _httpApiService = httpApiService;
    }

    public async Task<Root<Token>?> GetAccessTokenProperties(string? accessToken) {
        CancellationTokenSource cts = new CancellationTokenSource();

        try {
            return await _httpApiService.GetAccessTokenProperties(accessToken, cts.Token);
        } catch (Exception ex) {
            _logger.LogError(ex, ex.Message);
        }

        return null;
    }

    public async Task<Root<Token>?> InvalidateAccessToken(string? accessToken) {
        CancellationTokenSource cts = new CancellationTokenSource();

        try {
            return await _httpApiService.InvalidateAccessToken(accessToken, cts.Token);
        } catch (Exception ex) {
            _logger.LogError(ex, ex.Message);
        }

        return null;
    }
}