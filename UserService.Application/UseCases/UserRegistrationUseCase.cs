using UserService.Application.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.UseCases;

/// Представляет сценарий регистрации нового пользователя
public class UserRegistrationUseCase
{
    private readonly IUserRepository _userRepository;

    /// Представляет сценарий регистрации нового пользователя
    public UserRegistrationUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// Выполняет регистрацию нового пользователя
    /// <param name="name">Имя пользователя</param>
    /// <param name="password">Пароль</param>
    /// <param name="confirmPassword">Подтверждение пароля</param>
    /// <param name="ct">Токен отмены</param>
    /// <exception cref="Exception">Выбрасывается при несовпадении пароля и подтверждения или при наличии пользователя с таким именем</exception>
    public async Task ExecuteAsync(string name, string password, string confirmPassword, CancellationToken ct)
    {
        if (password != confirmPassword)
        {
            throw new Exception("Пароль и подтверждение пароля не совпадают.");
        }
        
        if (await _userRepository.ExistsByNameAsync(name, ct))
        {
            throw new Exception("Пользователь с таким именем уже существует.");
        }

        var user = new User(name, password);

        await _userRepository.AddNewUserAsync(user, ct);
    }
}