namespace UserService.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(int userId, string userName);
}