using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;

namespace UserService.Infrastructure.Persistence;

public class FavoritesRepository : IFavoritesRepository
{
    private readonly UserDbContext _userDbContext;
    public FavoritesRepository(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext;
    }

    public async Task<bool> ExistsAsync(int userId, string code, CancellationToken ct = default)
    {
        return await _userDbContext.Favorites.AsNoTracking()
            .AnyAsync(x => x.UserId == userId && x.CurrencyName == code, ct);
    }

    public async Task AddAsync(int userId, string code, CancellationToken ct = default)
    {
        _userDbContext.Favorites.Add(new FavoriteRow { UserId = userId, CurrencyName = code });
        await _userDbContext.SaveChangesAsync(ct);
    }
    
    public async Task<int> ClearAsync(int userId, CancellationToken ct = default)
    {
        var rows = await _userDbContext.Favorites.Where(x => x.UserId == userId).ToListAsync(ct);
        if (rows.Count == 0) return 0;
        _userDbContext.Favorites.RemoveRange(rows);
        return await _userDbContext.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<string>> InternalGetByUserAsync(int userId, CancellationToken ct = default)
        => await _userDbContext.Favorites.AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => x.CurrencyName)
            .ToListAsync(ct);
}