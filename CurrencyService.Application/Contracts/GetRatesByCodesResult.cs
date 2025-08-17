namespace CurrencyService.Application.Contracts;

/// <summary>
/// Представляет результат получения  курсов валют
/// </summary>
/// <param name="Currencies">Коллекция DTO валют</param>
public sealed record GetRatesByCodesResult(IReadOnlyList<CurrencyDto> Currencies);