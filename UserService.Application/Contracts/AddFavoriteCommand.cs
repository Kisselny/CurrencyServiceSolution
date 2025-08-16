namespace UserService.Application.Contracts;

public sealed record AddFavoriteCommand(int UserId, string CurrencyCode);