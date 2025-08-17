using System.ComponentModel.DataAnnotations;
using UserService.Application.Contracts;
using UserService.Application.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.UseCases;


/// <summary>
/// Обрабатывает процесс входа пользователя в систему.
/// Проверяет входные данные для входа, учетные данные пользователя и генерирует JWT токен
/// </summary>
public class UserLoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwt;

    /// Обрабатывает процесс входа пользователя в систему.
    /// Проверяет учетные данные и возвращает JWT токен
    public UserLoginUseCase(IUserRepository userRepository, IJwtTokenGenerator jwt)
    {
        _userRepository = userRepository;
        _jwt = jwt;
    }

    /// Запускает процесс входа пользователя в системы
    /// <param name="cmd">Команда с именем и паролем пользователя</param>
    /// <seealso cref="LoginUserCommand"/>
    /// <returns>Результат со сгенерированным JWT токеном</returns>
    /// <exception cref="ValidationException">Выбрасывается при отсутствии имени или пароля</exception>
    /// <exception cref="Exception">Выбрасывается при неверных учетных данных</exception>
    public async Task<LoginUserResult> ExecuteAsync(LoginUserCommand cmd, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(cmd.Name))
            throw new ValidationException("Необходимо ввести имя пользователя.");
        if (string.IsNullOrWhiteSpace(cmd.Password))
            throw new ValidationException("Необходимо ввести пароль.");
        
        var user = await _userRepository.GetByNameAsync(cmd.Name, ct);

        if (user == null)
            throw new Exception("Пользователь с таким именем не найден.");

        if (!user.IsPasswordCorrect(cmd.Password))
            throw new Exception("Неверный пароль.");
        
        var token = _jwt.GenerateToken(user.Id, user.Name);
        
        return new LoginUserResult(token);
    }
}