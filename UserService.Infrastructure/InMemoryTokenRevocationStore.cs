using System.Collections.Concurrent;
using UserService.Application.Interfaces;

namespace UserService.Infrastructure;

public class InMemoryTokenRevocationStore : ITokenRevocationStore
{
    private readonly ConcurrentDictionary<string, DateTime> _revoked = new();
    
    public Task RevokeAsync(string jti, DateTime expiresAt, int userId, CancellationToken ct = default)
    {
        _revoked[jti] = expiresAt;
        return Task.CompletedTask;
    }
    
    public Task<bool> IsRevokedAsync(string jti, CancellationToken ct = default)
    {
        return Task.FromResult(_revoked.TryGetValue(jti, out var exp) && exp > DateTime.UtcNow);
    }
}