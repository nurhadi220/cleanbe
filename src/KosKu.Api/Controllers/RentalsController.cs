using System.Security.Claims;
using FluentValidation;
using KosKu.Application.Common;
using KosKu.Application.Features.Rentals.DTOs;
using KosKu.Application.Features.Rentals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KosKu.Api.Controllers;

/// <summary>Controller untuk manajemen sewa aktif.</summary>
[ApiController]
[Route("api/rentals")]
[Authorize]
[Produces("application/json")]
public class RentalsController : ControllerBase
{
    private readonly RentalService _service;
    private readonly IValidator<ExtendRentalRequest> _extendValidator;

    public RentalsController(RentalService service, IValidator<ExtendRentalRequest> extendValidator)
    {
        _service = service;
        _extendValidator = extendValidator;
    }

    private Guid GetCurrentUserId()
        => Guid.Parse(User.FindFirstValue("userId") ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Mendapatkan semua sewa aktif (Admin only).</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<RentalDto>>.Ok(result));
    }

    /// <summary>Mendapatkan sewa berdasarkan Id.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(ApiResponse<RentalDto>.Ok(result));
    }

    /// <summary>Mendapatkan sewa milik pengguna yang sedang login.</summary>
    [HttpGet("my")]
    public async Task<IActionResult> GetMyRentals()
    {
        var result = await _service.GetByUserAsync(GetCurrentUserId());
        return Ok(ApiResponse<IEnumerable<RentalDto>>.Ok(result));
    }

    /// <summary>Menyelesaikan sewa aktif (Admin only).</summary>
    [HttpPost("{id:guid}/finish")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Finish(Guid id)
    {
        var result = await _service.FinishAsync(id);
        return Ok(ApiResponse<RentalDto>.Ok(result, "Sewa berhasil diselesaikan."));
    }

    /// <summary>Memperpanjang sewa.</summary>
    [HttpPost("{id:guid}/extend")]
    public async Task<IActionResult> Extend(Guid id, [FromBody] ExtendRentalRequest request)
    {
        var validation = await _extendValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation Error",
                validation.Errors.Select(e => e.ErrorMessage)));

        var result = await _service.ExtendAsync(id, request);
        return Ok(ApiResponse<RentalDto>.Ok(result, "Sewa berhasil diperpanjang."));
    }
}
