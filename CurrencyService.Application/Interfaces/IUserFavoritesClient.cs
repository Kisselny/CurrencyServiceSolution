namespace CurrencyService.Application.Interfaces;

public interface IUserFavoritesClient
{
    Task<IReadOnlyList<string>> GetFavoritesAsync(int userId, CancellationToken ct = default);
}