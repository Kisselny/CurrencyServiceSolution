using CurrencyService.Domain;

namespace CurrencyService.Application.Interfaces;

public interface ICurrencyRepository
{
    Task<IReadOnlyList<Currency>> GetByCodesAsync(
        IEnumerable<string> codes,
        CancellationToken ct = default);

    Task<IReadOnlyList<Currency>> GetAllAsync(
        CancellationToken ct = default);
}