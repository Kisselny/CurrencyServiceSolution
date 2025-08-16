namespace CurrencyService.Application.Contracts;

public sealed record GetRatesByCodesCommand(IReadOnlyCollection<string> Codes);