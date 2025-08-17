namespace UserService.Application.Interfaces;

public interface IFavoritesRepository
{
    Task<bool> ExistsAsync(int userId, string code, CancellationToken ct = default);
    Task AddAsync(int userId, string code, CancellationToken ct = default);
    Task<int> ClearAsync(int userId, CancellationToken ct = default);
    Task<IReadOnlyList<string>> InternalGetByUserAsync(int userId, CancellationToken ct = default);
}