namespace CurrencyService.Application.Contracts;

/// Представляет результат получения избранных валют пользователя
/// <param name="Currencies">Коллекция DTO избранных валют пользователя</param>
public sealed record GetUserFavoriteCurrenciesResult(IReadOnlyList<CurrencyDto> Currencies);