using System.Collections.ObjectModel;
using CurrencyService.Application.Contracts;
using CurrencyService.Application.Interfaces;
using CurrencyService.Domain.Models;

namespace CurrencyService.Application.UseCases;

/// <summary>
/// Представляет сценарий получения текущих курсов всех валют
/// </summary>
public class JustGetAllCurrenciesUseCase
{
    private readonly ICurrencyRepository _currencyRepository;

    /// <summary>
    /// Представляет сценарий получения текущих курсов всех валют
    /// </summary>
    public JustGetAllCurrenciesUseCase(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    /// <summary>
    /// Получает все доступные валюты
    /// </summary>
    public async Task<ReadOnlyCollection<CurrencyDto>> ExecuteAsync(CancellationToken ct)
    {
        var currencies = await _currencyRepository.GetAllAsync(ct);
        
        var result = currencies
            .Select(c => new CurrencyDto(c.Name, Rate: c.Rate))
            .ToList()
            .AsReadOnly();

        return result;
    }
}