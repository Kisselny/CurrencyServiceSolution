using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Contracts;
using UserService.Application.Interfaces;
using UserService.Application.UseCases;

namespace UserService.Api.Controllers;

/// <summary>
/// Предоставляет конечные точки для управления регистрацией, аутентификацией и другими действиями пользователя
/// </summary>
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserLoginUseCase _loginUser;
    private readonly UserRegistrationUseCase _registerUser;

    /// Предоставляет конечные точки для управления регистрацией, аутентификацией и другими действиями пользователя
    public UserController(ILogger<UserController> logger, UserLoginUseCase loginUser,
        UserRegistrationUseCase registerUser)
    {
        _logger = logger;
        _loginUser = loginUser;
        _registerUser = registerUser;
    }

    /// <summary>
    /// Обрабатывает запрос на регистрацию нового пользователя
    /// </summary>
    /// <param name="command">Команда, содержащая имя, пароль и подтверждение пароля для регистрации</param>
    /// <param name="ct">Токен отмены для управления длительностью выполнения операции</param>
    /// <returns>Результат выполнения операции регистрации</returns>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken ct)
    {
        try
        {
            await _registerUser.ExecuteAsync(command.Name, command.Password, command.ConfirmPassword, ct);
            return Created();

        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Выполняет вход пользователя на платформу
    /// </summary>
    /// <param name="command">Данные для аутентификации пользователя</param>
    /// <param name="ct">Токен отмены операции</param>
    /// <returns>Результат с JWT токеном в случае успешной аутентификации</returns>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResult>> Login([FromBody] LoginUserCommand command, CancellationToken ct)
    {
        try
        {
            var result = await _loginUser.ExecuteAsync(command, ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Завершает текущий сеанс пользователя, отзывая активный токен
    /// </summary>
    /// <param name="store">Хранилище для отзыва токенов</param>
    /// <param name="ct">Токен отмены</param>
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromServices] ITokenRevocationStore store, CancellationToken ct)
    {
        var jti = User.FindFirstValue(JwtRegisteredClaimNames.Jti);
        var expUnix = User.FindFirst("exp")?.Value;
        if (jti is null || expUnix is null) return NoContent();

        var expUtc = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expUnix)).UtcDateTime;
        var userId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        await store.RevokeAsync(jti, expUtc, userId, ct);
        return NoContent();
    }


    /// <summary>
    /// Добавляет валюту в список избранных пользователя
    /// </summary>
    /// <param name="code">Код валюты для добавления в избранное</param>
    /// <param name="useCase">Используемый сценарий добавления в избранное</param>
    /// <param name="ct">Токен для отмены операции</param>
    [Authorize]
    [HttpPost("me/favorites/{code}")]
    public async Task<IActionResult> AddFavorite(
        [FromRoute] string code,
        [FromServices] AddFavoriteUseCase useCase,
        CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub");
        if (!int.TryParse(userId, out var id))
        {
            return Unauthorized();
        }

        await useCase.Handle(new AddFavoriteCommand(id, code), ct);
        
        return NoContent();
    }

    /// <summary>
    /// Удаляет все записи из избранного для авторизованного пользователя
    /// </summary>
    /// <param name="useCase">Вариант использования для выполнения операции очистки избранного</param> 
    /// <param name="ct">Токен отмены операции</param>
    /// <returns>HTTP результат, указывающий на исход операции</returns>
    [Authorize]
    [HttpDelete("me/favorites")]
    public async Task<IActionResult> ClearFavorites(
        [FromServices] ClearFavoritesUseCase useCase,
        CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub");
        if (!int.TryParse(userId, out var id)) return Unauthorized();

        var deleted = await useCase.Handle(new ClearFavoritesCommand(id), ct);
        return Ok(new { deleted });
    }
    
    /// <summary>
    /// test shit
    /// </summary>
    [Authorize]
    [HttpGet("me/ping")]
    public ActionResult<object> MePing()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub");
        var userName = User.Identity?.Name 
                       ?? User.FindFirstValue("unique_name");
        
        return Ok(new
        {
            ok = true,
            userId,
            userName
        });
    }
    
    /// <summary>
    /// debug shit
    /// </summary>
    [Authorize]
    [HttpGet("me/claims")]
    public IActionResult ClaimsDump()
    {
        return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
    }
}