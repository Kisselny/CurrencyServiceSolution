namespace UserService.Application.Contracts;

/// <summary>
/// Представляет команду для очистки списка избранных валют у пользователя
/// </summary>
/// <param name="UserId">Идентификатор пользователя</param>
public sealed record ClearFavoritesCommand(int UserId);