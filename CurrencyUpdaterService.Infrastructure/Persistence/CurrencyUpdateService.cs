using CurrencyUpdaterService.Application;
using CurrencyUpdaterService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyUpdaterService.Infrastructure.Persistence;

public class CurrencyUpdateService : ICurrencyUpdateService
{
    private readonly CurrencyDbContext _dbContext;
    public CurrencyUpdateService(CurrencyDbContext dbContext) => _dbContext = dbContext;
    
    public async Task UpsertCurrenciesAsync(IEnumerable<Currency> newCurrencies)
    {
        var oldCurrencies = await _dbContext.Currencies.ToDictionaryAsync(x => x.Name);

        foreach (var currency in newCurrencies)
        {
            if (oldCurrencies.TryGetValue(currency.Name, out var entity))
            {
                entity.Rate = currency.Rate;
            }
            else
            {
                _dbContext.Currencies.Add(currency);
            }
        }
        await _dbContext.SaveChangesAsync();
    }
}