using System.ComponentModel.DataAnnotations;
using CurrencyService.Application.Contracts;
using CurrencyService.Application.Interfaces;

namespace CurrencyService.Application.UseCases;

public class GetUserFavoriteCurrenciesUseCase
{
    private readonly IUserFavoritesClient _favoritesClient;
    private readonly ICurrencyRepository _currencyRepository;

    public GetUserFavoriteCurrenciesUseCase(IUserFavoritesClient favoritesClient, ICurrencyRepository currencyRepository)
    {
        _favoritesClient = favoritesClient;
        _currencyRepository = currencyRepository;
    }
    
    public async Task<GetUserFavoriteCurrenciesResult> ExecuteAsync(
        GetUserFavoritesCommand cmd,
        CancellationToken ct = default)
    {
        if (cmd.UserId <= 0)
            throw new ValidationException("UserId должен быть положительным.");

        var codes = await _favoritesClient.GetFavoritesAsync(cmd.UserId, ct);
        
        if (codes is null || codes.Count == 0)
            return new GetUserFavoriteCurrenciesResult(Array.Empty<CurrencyDto>());
        
        var currencies = await _currencyRepository.GetByCodesAsync(codes, ct);
        
        var result = currencies
            .Select(c => new CurrencyDto(c.Name, Rate: c.Rate))
            .ToList()
            .AsReadOnly();

        return new GetUserFavoriteCurrenciesResult(result);
    }
}