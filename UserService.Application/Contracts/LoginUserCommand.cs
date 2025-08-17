namespace UserService.Application.Contracts;

/// <summary>
/// Инкапсулирует данные, необходимые для команды входа. Используется для аутентификации пользователя путем предоставления учетных данных.
/// </summary>
/// <param name="Name">Имя пользователя, пытающегося войти в систему.</param>
/// <param name="Password">Пароль, соответствующий указанному имени пользователя.</param>
public sealed record LoginUserCommand(string Name, string Password);