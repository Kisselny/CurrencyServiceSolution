using Microsoft.AspNetCore.Mvc;
using UserService.Application.Interfaces;

namespace UserService.Api.Controllers;

/// <summary>
/// Внутренний контроллер для обработки запросов, связанных с избранным пользователей в системе
/// </summary>
/// <remarks>Предназначен для микросервиса валют</remarks>
[ApiController]
[Route("internal/users")]
//[ApiExplorerSettings(IgnoreApi = true)] //это позже
public class UserFavoritesInternalController : ControllerBase
{
    /// <summary>
    /// Получает список избранных записей пользователя (используется микросервисом валют)
    /// </summary>
    /// <param name="id">Идентификатор пользователя (берется из JWT)</param>
    /// <param name="repo">Репозиторий для работы с избранным</param>
    /// <param name="ct">Токен отмены операции</param>
    /// <returns>Список имен избранных записей пользователя</returns>
    [HttpGet("{id:int}/favorites")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetFavorites(
        [FromRoute] int id,
        [FromServices] IFavoritesRepository repo,
        CancellationToken ct)
    {
        if (id <= 0)
        {
            return BadRequest(new { error = "UserId должен быть положительным." });
        }

        var list = await repo.InternalGetByUserAsync(id, ct);
        
        var normalized = list
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim().ToUpperInvariant())
            .Distinct()
            .ToArray();

        return Ok(normalized);
    }
}