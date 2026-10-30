using FluentValidation;
using KosKu.Application.Common;
using KosKu.Application.Features.RoomTypes.DTOs;
using KosKu.Application.Features.RoomTypes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KosKu.Api.Controllers;

/// <summary>Controller untuk manajemen tipe kamar.</summary>
[ApiController]
[Route("api/room-types")]
[Authorize]
[Produces("application/json")]
public class RoomTypesController : ControllerBase
{
    private readonly RoomTypeService _service;
    private readonly IValidator<CreateRoomTypeRequest> _createValidator;
    private readonly IValidator<UpdateRoomTypeRequest> _updateValidator;

    public RoomTypesController(
        RoomTypeService service,
        IValidator<CreateRoomTypeRequest> createValidator,
        IValidator<UpdateRoomTypeRequest> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>Mendapatkan semua tipe kamar.</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<RoomTypeDto>>.Ok(result));
    }

    /// <summary>Mendapatkan tipe kamar berdasarkan Id.</summary>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(ApiResponse<RoomTypeDto>.Ok(result));
    }

    /// <summary>Membuat tipe kamar baru (Admin only).</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateRoomTypeRequest request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation Error",
                validation.Errors.Select(e => e.ErrorMessage)));

        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<RoomTypeDto>.Ok(result, "Tipe kamar berhasil dibuat."));
    }

    /// <summary>Memperbarui tipe kamar (Admin only).</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomTypeRequest request)
    {
        var validation = await _updateValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation Error",
                validation.Errors.Select(e => e.ErrorMessage)));

        var result = await _service.UpdateAsync(id, request);
        return Ok(ApiResponse<RoomTypeDto>.Ok(result, "Tipe kamar berhasil diperbarui."));
    }

    /// <summary>Menghapus tipe kamar (Admin only).</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return Ok(ApiResponse<object>.Ok("Tipe kamar berhasil dihapus."));
    }
}
