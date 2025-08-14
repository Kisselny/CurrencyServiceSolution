namespace UserService.Domain.Models;

public class User
{
    #region properties

    public int Id { get; set; }
    public string Name { get; private set; }
    private string Password { get; set; }

    #endregion

    #region .ctor

    public User(string name, string password)
    {
        ValidateUsernameIsNotNullOrEmpty(name);
        ValidateNameLength(name);
        Name = name;

        ValidatePasswordIsNotNullOrEmpty(password);
        ValidatePasswordLength(password);
        Password = password;
    }

    #endregion

    #region public methods
    
    public bool IsPasswordCorrect(string password)
    {
        return Password == password;
    }

    public void ChangeName(string newName)
    {
        ValidateUsernameIsNotNullOrEmpty(newName);
        ValidateNameLength(newName);
        EnsureNewNameIsDifferent(newName);
        Name = newName;
    }
    
    public void ChangePassword(string newPassword)
    {
        ValidatePasswordIsNotNullOrEmpty(newPassword);
        ValidatePasswordLength(newPassword);
        EnsureNewPasswordIsDifferent(newPassword);
        Password = newPassword;
    }

    #endregion

    #region private methods

    private static void ValidatePasswordLength(string password)
    {
        if (password.Length < 8 || password.Length > 64)
        {
            throw new Exception("Пароль не должен быть короче 8 символов или длиннее 64 символов.");
        }
    }

    private static void ValidatePasswordIsNotNullOrEmpty(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new Exception("Пароль не может быть пустым, null или набором пробелов.");
        }
    }

    private static void ValidateNameLength(string name)
    {
        if (name.Length < 3 || name.Length > 22)
        {
            throw new Exception("Имя пользователя не может быть короче 3 символов или длиннее 22 символов.");
        }
    }

    private static void ValidateUsernameIsNotNullOrEmpty(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception("Имя пользователя не может быть пустым, null или набором пробелов.");
        }
    }
    
    private void EnsureNewNameIsDifferent(string newName)
    {
        if (newName == Name)
        {
            throw new Exception("Новое имя пользователя должно отличаться от старого.");
        }
    }

    private void EnsureNewPasswordIsDifferent(string newPassword)
    {
        if (newPassword == Password)
        {
            throw new Exception("Новый пароль должен отличаться от старого.");
        }
    }
    #endregion
}