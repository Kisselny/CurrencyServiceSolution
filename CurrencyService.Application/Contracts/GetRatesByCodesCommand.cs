namespace CurrencyService.Application.Contracts;

/// <summary>
/// Представляет команду для получения обменных курсов для указанных кодов валют
/// </summary>
/// <param name="Codes">Коллекция кодов валют для получения курсов</param>
public sealed record GetRatesByCodesCommand(IReadOnlyCollection<string> Codes);