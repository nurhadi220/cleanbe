using FluentValidation;
using KosKu.Application.Common;
using KosKu.Application.Features.Rooms.DTOs;
using KosKu.Application.Features.Rooms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KosKu.Api.Controllers;

/// <summary>Controller untuk manajemen kamar kos.</summary>
[ApiController]
[Route("api/rooms")]
[Authorize]
[Produces("application/json")]
public class RoomsController : ControllerBase
{
    private readonly RoomService _service;
    private readonly IValidator<CreateRoomRequest> _createValidator;
    private readonly IValidator<UpdateRoomRequest> _updateValidator;

    public RoomsController(
        RoomService service,
        IValidator<CreateRoomRequest> createValidator,
        IValidator<UpdateRoomRequest> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>Mendapatkan semua kamar.</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<RoomDto>>.Ok(result));
    }

    /// <summary>Mendapatkan kamar berdasarkan Id.</summary>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(ApiResponse<RoomDto>.Ok(result));
    }

    /// <summary>Membuat kamar baru (Admin only).</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequest request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation Error",
                validation.Errors.Select(e => e.ErrorMessage)));

        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<RoomDto>.Ok(result, "Kamar berhasil dibuat."));
    }

    /// <summary>Memperbarui data kamar (Admin only).</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Penghuni")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomRequest request)
    {
        var validation = await _updateValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation Error",
                validation.Errors.Select(e => e.ErrorMessage)));

        var result = await _service.UpdateAsync(id, request);
        return Ok(ApiResponse<RoomDto>.Ok(result, "Kamar berhasil diperbarui."));
    }

    /// <summary>Menghapus kamar (Admin only).</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return Ok(ApiResponse<object>.Ok("Kamar berhasil dihapus."));
    }
}
