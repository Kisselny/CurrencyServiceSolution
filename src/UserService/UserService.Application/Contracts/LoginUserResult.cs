namespace UserService.Application.Contracts;

/// <summary>
/// Представляет результат операции входа пользователя, инкапсулируя информацию, связанную с аутентификацией
/// </summary>
/// <param name="Token">JWT токен, выданный при успешной аутентификации пользователя</param>
public sealed record LoginUserResult(string Token);