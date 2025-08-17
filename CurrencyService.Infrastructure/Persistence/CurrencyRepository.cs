using CurrencyService.Application.Interfaces;
using CurrencyService.Domain;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.Infrastructure.Persistence;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly CurrencyDbContext _dbContext;
    public CurrencyRepository(CurrencyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Currency>> GetByCodesAsync(IEnumerable<string> codes, CancellationToken ct = default)
    {
        var normalized = codes
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => c.Trim().ToUpperInvariant())
            .Distinct()
            .ToArray();

        if (normalized.Length == 0)
        {
            return Array.Empty<Currency>();
        }

        return await _dbContext.Currencies
            .AsNoTracking()
            .Where(c => normalized.Contains(c.Name.Trim().ToUpper()
            ))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Currency>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbContext.Currencies.AsNoTracking().ToListAsync(ct);
    }
}