namespace CurrencyService.Application.Contracts;

public sealed record GetUserFavoriteCurrenciesResult(IReadOnlyList<CurrencyDto> Currencies);