namespace UserService.Application.Contracts;

/// <summary>
/// Представляет команду для регистрации нового пользователя путем предоставления информации об имени и пароле.
/// </summary>
public sealed record RegisterUserCommand(string Name, string Password, string ConfirmPassword);