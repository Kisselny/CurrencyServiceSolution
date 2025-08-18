namespace CurrencyService.Application.Contracts;

/// Представляет команду для получения избранных валют пользователя
/// <param name="UserId">Идентификатор пользователя</param>
public sealed record GetUserFavoritesCommand(int UserId);