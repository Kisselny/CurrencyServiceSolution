namespace UserService.Application.Interfaces;

public interface ITokenRevocationStore
{
    Task RevokeAsync(string jti, DateTime expiresAt, int userId, CancellationToken ct = default);
    Task<bool> IsRevokedAsync(string jti, CancellationToken ct = default);
}