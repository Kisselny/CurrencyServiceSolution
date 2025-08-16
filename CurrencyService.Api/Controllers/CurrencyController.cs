using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using CurrencyService.Application.Contracts;
using CurrencyService.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CurrencyController : ControllerBase
{
    private readonly ILogger<CurrencyController> _logger;
    private readonly GetUserFavoriteCurrenciesUseCase _byUser;
    //private readonly GetRatesByCodesUseCase _byCodes;

    public CurrencyController(ILogger<CurrencyController> logger, GetUserFavoriteCurrenciesUseCase byUser)
    {
        _logger = logger;
        _byUser = byUser;
    }

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
        // Если в AddJwtBearer включено MapInboundClaims=false → "sub"
        // Иначе .NET маппит "sub" в ClaimTypes.NameIdentifier
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("sub");

        return int.TryParse(sub, out var id) ? id : null;
    }
}