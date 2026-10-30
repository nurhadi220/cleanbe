using System.Security.Claims;
using FluentValidation;
using KosKu.Application.Common;
using KosKu.Application.Features.Bookings.DTOs;
using KosKu.Application.Features.Bookings.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KosKu.Api.Controllers;

/// <summary>Controller untuk manajemen booking kamar.</summary>
[ApiController]
[Route("api/bookings")]
[Authorize]
[Produces("application/json")]
public class BookingsController : ControllerBase
{
    private readonly BookingService _service;
    private readonly IValidator<CreateBookingRequest> _createValidator;

    public BookingsController(BookingService service, IValidator<CreateBookingRequest> createValidator)
    {
        _service = service;
        _createValidator = createValidator;
    }

    private Guid GetCurrentUserId()
        => Guid.Parse(User.FindFirstValue("userId") ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Mendapatkan semua booking (Admin only).</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<BookingDto>>.Ok(result));
    }

    /// <summary>Mendapatkan booking berdasarkan Id.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(ApiResponse<BookingDto>.Ok(result));
    }

    /// <summary>Mendapatkan booking milik pengguna yang sedang login.</summary>
    [HttpGet("my")]
    public async Task<IActionResult> GetMyBookings()
    {
        var result = await _service.GetByUserAsync(GetCurrentUserId());
        return Ok(ApiResponse<IEnumerable<BookingDto>>.Ok(result));
    }

    /// <summary>Membuat booking baru.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation Error",
                validation.Errors.Select(e => e.ErrorMessage)));

        var result = await _service.CreateAsync(GetCurrentUserId(), request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<BookingDto>.Ok(result, "Booking berhasil dibuat."));
    }

    /// <summary>Mengonfirmasi booking (Admin only).</summary>
    [HttpPost("{id:guid}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var result = await _service.ApproveAsync(id);
        return Ok(ApiResponse<BookingDto>.Ok(result, "Booking berhasil dikonfirmasi dan sewa aktif telah dibuat."));
    }

    /// <summary>Membatalkan booking.</summary>
    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var result = await _service.CancelAsync(id);
        return Ok(ApiResponse<BookingDto>.Ok(result, "Booking berhasil dibatalkan."));
    }
}
