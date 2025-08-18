using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using CurrencyService.Application.Contracts;
using CurrencyService.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyService.Api.Controllers;

/// <summary>
/// Представляет контроллер для работы с валютами
/// </summary>
[ApiController]
[Route("[controller]")]
public class CurrencyController : ControllerBase
{
    private readonly ILogger<CurrencyController> _logger;
    private readonly GetUserFavoriteCurrenciesUseCase _byUser;

    /// Контроллер для операций над валютами
    public CurrencyController(ILogger<CurrencyController> logger, GetUserFavoriteCurrenciesUseCase byUser)
    {
        _logger = logger;
        _byUser = byUser;
    }

    /// <summary>
    /// Возвращает избранные валюты текущего пользователя
    /// </summary>
    /// <param name="ct">Токен отмены операции</param>
    /// <return>Результат выполнения с коллекцией избранных валют</return>
    [Authorize]
    [HttpGet("by-user")]
    public async Task<ActionResult<GetUserFavoriteCurrenciesResult>> GetByUser(CancellationToken ct)
    {
        var userId = TryGetUserId();
        if (userId is null) return Unauthorized();

        try
        {
            var result = await _byUser.ExecuteAsync(new GetUserFavoritesCommand(userId.Value), ct);
            return Ok(result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    private int? TryGetUserId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("sub");

        return int.TryParse(sub, out var id) ? id : null;
    }
}