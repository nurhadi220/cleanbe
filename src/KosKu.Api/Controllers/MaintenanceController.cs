using System.Security.Claims;
using FluentValidation;
using KosKu.Application.Common;
using KosKu.Application.Features.Maintenance.DTOs;
using KosKu.Application.Features.Maintenance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KosKu.Api.Controllers;

/// <summary>Controller untuk manajemen laporan maintenance/perbaikan.</summary>
[ApiController]
[Route("api/maintenance")]
[Authorize]
[Produces("application/json")]
public class MaintenanceController : ControllerBase
{
    private readonly MaintenanceService _service;
    private readonly IValidator<CreateMaintenanceRequest> _createValidator;

    public MaintenanceController(MaintenanceService service, IValidator<CreateMaintenanceRequest> createValidator)
    {
        _service = service;
        _createValidator = createValidator;
    }

    private Guid GetCurrentUserId()
        => Guid.Parse(User.FindFirstValue("userId") ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Mendapatkan semua laporan (Admin only).</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<MaintenanceDto>>.Ok(result));
    }

    /// <summary>Mendapatkan laporan milik pengguna yang sedang login.</summary>
    [HttpGet("my")]
    public async Task<IActionResult> GetMyReports()
    {
        var result = await _service.GetByUserAsync(GetCurrentUserId());
        return Ok(ApiResponse<IEnumerable<MaintenanceDto>>.Ok(result));
    }

    /// <summary>Membuat laporan perbaikan baru.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMaintenanceRequest request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation Error",
                validation.Errors.Select(e => e.ErrorMessage)));

        var result = await _service.CreateAsync(GetCurrentUserId(), request);
        return Ok(ApiResponse<MaintenanceDto>.Ok(result, "Laporan perbaikan berhasil dibuat."));
    }

    /// <summary>Memperbarui status laporan (Admin only).</summary>
    [HttpPut("{id:guid}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateMaintenanceStatusRequest request)
    {
        var result = await _service.UpdateStatusAsync(id, request);
        return Ok(ApiResponse<MaintenanceDto>.Ok(result, "Status laporan berhasil diperbarui."));
    }
}
