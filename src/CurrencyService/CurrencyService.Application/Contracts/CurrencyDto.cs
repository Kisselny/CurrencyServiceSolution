namespace CurrencyService.Application.Contracts;

/// <summary>
/// Объект передачи данных о валюте
/// </summary>
/// <param name="Name">Название валюты</param>
/// <param name="Rate">Обменный курс</param>
public sealed record CurrencyDto(string Name, decimal Rate);