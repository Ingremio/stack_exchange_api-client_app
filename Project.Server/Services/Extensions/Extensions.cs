using System.IdentityModel.Tokens.Jwt;

namespace Project.Server.Services.Extensions;
public static class Extensions {
    public static bool IsTokenValid(this JwtSecurityToken token) => token != null && token.ValidTo > DateTime.Now;
}