using FluentValidation;
using KosKu.Application.Common;
using KosKu.Application.Features.Fines.DTOs;
using KosKu.Application.Features.Fines.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KosKu.Api.Controllers;

/// <summary>Controller untuk manajemen denda.</summary>
[ApiController]
[Route("api/fines")]
[Authorize]
[Produces("application/json")]
public class FinesController : ControllerBase
{
    private readonly FineService _service;
    private readonly IValidator<CreateFineRequest> _createValidator;

    public FinesController(FineService service, IValidator<CreateFineRequest> createValidator)
    {
        _service = service;
        _createValidator = createValidator;
    }

    /// <summary>Mendapatkan semua denda (Admin only).</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<FineDto>>.Ok(result));
    }

    /// <summary>Mendapatkan denda berdasarkan sewa.</summary>
    [HttpGet("rental/{rentalId:guid}")]
    public async Task<IActionResult> GetByRental(Guid rentalId)
    {
        var result = await _service.GetByRentalAsync(rentalId);
        return Ok(ApiResponse<IEnumerable<FineDto>>.Ok(result));
    }

    /// <summary>Membuat denda baru (Admin only).</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateFineRequest request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation Error",
                validation.Errors.Select(e => e.ErrorMessage)));

        var result = await _service.CreateAsync(request);
        return Ok(ApiResponse<FineDto>.Ok(result, "Denda berhasil dibuat."));
    }

    /// <summary>Melunasi denda.</summary>
    [HttpPost("{id:guid}/pay")]
    public async Task<IActionResult> Pay(Guid id)
    {
        var result = await _service.PayAsync(id);
        return Ok(ApiResponse<FineDto>.Ok(result, "Denda berhasil dilunasi."));
    }
}
