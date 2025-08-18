using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Persistence;

/// <inheritdoc />
public class UserRepository : IUserRepository
{
    private readonly UserDbContext _db;
    private readonly ILogger<UserRepository> _logger;

    /// <summary>
    /// Интерфейс для работы с репозиторием пользователей
    /// </summary>
    /// <param name="db">Контекст БД пользователей</param>
    /// <param name="logger">Логгер</param>
    public UserRepository(UserDbContext db, ILogger<UserRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct)
    {
        return await _db.Users
            .AsNoTracking()
            .AnyAsync(u => u.Name == name, ct);
    }


    /// <inheritdoc />
    public async Task<User?> GetByNameAsync(string name, CancellationToken ct)
        => await _db.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Name == name, ct);

    /// <inheritdoc />
    public async Task AddNewUserAsync(User user, CancellationToken ct)
    {
        await _db.Users.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
        _logger.LogInformation("Пользователь с именем {Name} был добавлен в систему", user.Name);
    }
}