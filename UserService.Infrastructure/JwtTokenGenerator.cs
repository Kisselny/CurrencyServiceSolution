using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using UserService.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace UserService.Infrastructure;

/// <inheritdoc />
public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _config;

    /// <summary>
    /// Предоставляет функциональность для генерации JWT токено
    /// </summary>
    /// <param name="config">Параметры конфигурации</param>
    public JwtTokenGenerator(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Генерирует JWT токен для указанного пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="userName">Имя пользователя</param>
    /// <returns>Сгенерированный JWT токен</returns>
    public string GenerateToken(int userId, string userName)
    {
        var jwtSection = _config.GetSection("Jwt");
        var jti = Guid.NewGuid().ToString();
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, userName),
            new Claim(JwtRegisteredClaimNames.Jti, jti)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSection["ExpiresMinutes"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}