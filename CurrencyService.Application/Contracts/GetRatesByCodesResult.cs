namespace CurrencyService.Application.Contracts;

public sealed record GetRatesByCodesResult(IReadOnlyList<CurrencyDto> Currencies);