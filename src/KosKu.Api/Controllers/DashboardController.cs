using KosKu.Application.Common;
using KosKu.Application.Features.Dashboard.DTOs;
using KosKu.Application.Features.Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KosKu.Api.Controllers;

/// <summary>Controller untuk data statistik dashboard.</summary>
[ApiController]
[Route("api/dashboard")]
[Authorize(Roles = "Admin")]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _service;

    public DashboardController(DashboardService service) => _service = service;

    /// <summary>Mendapatkan ringkasan statistik sistem.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<DashboardDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary()
    {
        var result = await _service.GetSummaryAsync();
        return Ok(ApiResponse<DashboardDto>.Ok(result));
    }
}
