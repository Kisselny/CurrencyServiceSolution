using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<UserLoginUseCase> _logger;

    /// Обрабатывает процесс входа пользователя в систему.
    /// Проверяет учетные данные и возвращает JWT токен
    public UserLoginUseCase(IUserRepository userRepository, IJwtTokenGenerator jwt, ILogger<UserLoginUseCase> logger)
    {
        _userRepository = userRepository;
        _jwt = jwt;
        _logger = logger;
    }

    /// Запускает процесс входа пользователя в системы
    /// <param name="cmd">Команда с именем и паролем пользователя</param>
    /// <param name="ct">Токен отмены</param>
    /// <seealso cref="LoginUserCommand"/>
    /// <returns>Результат со сгенерированным JWT токеном</returns>
    /// <exception cref="ValidationException">Выбрасывается при отсутствии имени или пароля</exception>
    /// <exception cref="Exception">Выбрасывается при неверных учетных данных</exception>
    public async Task<LoginUserResult> ExecuteAsync(LoginUserCommand cmd, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(cmd.Name))
        {
            _logger.LogError("Необходимо ввести имя пользователя.");
            throw new ValidationException("Необходимо ввести имя пользователя.");
        }
        if (string.IsNullOrWhiteSpace(cmd.Password))
        {
            _logger.LogError("Необходимо ввести пароль.");
            throw new ValidationException("Необходимо ввести пароль.");
        }
        
        var user = await _userRepository.GetByNameAsync(cmd.Name, ct);

        if (user == null)
        {
            _logger.LogError("Польозватель с таким именем не найден: {Name}", cmd.Name);
            throw new Exception("Пользователь с таким именем не найден.");
        }

        if (!user.IsPasswordCorrect(cmd.Password))
        {
            _logger.LogError("Был введен неверный пароль пользователем {Name}.", cmd.Name);
            throw new Exception("Неверный пароль.");
        }
        
        var token = _jwt.GenerateToken(user.Id, user.Name);
        _logger.LogInformation("Пользователь {Name} залогировался в систему", cmd.Name);
        return new LoginUserResult(token);
    }
}