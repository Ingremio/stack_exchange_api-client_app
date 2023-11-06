using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Project.Server.Services.StackExchange.Users;

namespace Project.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase {
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService) {
        _usersService = usersService;
    }

    [EnableCors("ClientPermission")]
    [HttpGet(nameof(Login))]
    public async Task<IActionResult> Login() {
        if (!User.Identity?.IsAuthenticated ?? false)
            return Unauthorized();

        var token = await HttpContext.GetTokenAsync("access_token");
        var data = User.Claims.ToDictionary(x => x.Type, x => x.Value);

        data.TryGetValue(ClaimTypes.NameIdentifier, out var id);
        data.TryGetValue(ClaimTypes.Name, out var name);

        return base.Ok(new ApiUser() { Id = id, Name = name, Token = token });
    }

    [EnableCors("ClientPermission")]
    [HttpPost(nameof(Logout))]
    public async Task<IActionResult> Logout(ApiUser user) {
        if (User.Identity?.IsAuthenticated ?? false) {
            if (User.Claims.Any(c => c.Value == user.Id || c.Value == user.Name)) {
                var response = await _usersService.InvalidateAccessToken(user.Token);
                var accessTokens = response?.Items?.Select(x => x.AccessToken).ToArray();

                if (accessTokens != null && accessTokens.Contains(user.Token)) {
                    await this.HttpContext.SignOutAsync();
                    return Ok(true);
                }
            }
        }

        return Ok(!(User.Identity?.IsAuthenticated ?? false));
    }


    [EnableCors("ClientPermission")]
    [HttpPost(nameof(GetAccessTokenProperties))]
    public async Task<IActionResult> GetAccessTokenProperties(string accessToken) {
        var response = await _usersService.GetAccessTokenProperties(accessToken);

        if (response == null)
            return NoContent();

        return Ok(response);
    }

    [EnableCors("ClientPermission")]
    [HttpPost(nameof(InvalidateAccessToken))]
    public async Task<IActionResult> InvalidateAccessToken(string accessToken) {
        var response = await _usersService.InvalidateAccessToken(accessToken);

        if (response == null)
            return NoContent();

        return Ok(response);
    }
}