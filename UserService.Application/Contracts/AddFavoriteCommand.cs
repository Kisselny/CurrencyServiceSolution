namespace UserService.Application.Contracts;

/// <summary>
/// Представляет команду для добавления избранной валюты для заданного пользователя
/// </summary>
/// <remarks>
/// Эта команда используется для инкапсуляции данных, необходимых для добавления 
/// избранной валюты пользователя в приложении
/// </remarks>
/// <param name="UserId">Уникальный идентификатор пользователя, добавляющего избранную валюту.</param>
/// <param name="CurrencyCode">Код валюты для добавления в избранное.</param>
public sealed record AddFavoriteCommand(int UserId, string CurrencyCode);