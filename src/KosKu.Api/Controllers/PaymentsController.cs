using System.Security.Claims;
using FluentValidation;
using KosKu.Application.Common;
using KosKu.Application.Features.Payments.DTOs;
using KosKu.Application.Features.Payments.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KosKu.Api.Controllers;

/// <summary>Controller untuk manajemen pembayaran sewa.</summary>
[ApiController]
[Route("api/payments")]
[Authorize]
[Produces("application/json")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _service;
    private readonly IValidator<CreatePaymentRequest> _createValidator;

    public PaymentsController(PaymentService service, IValidator<CreatePaymentRequest> createValidator)
    {
        _service = service;
        _createValidator = createValidator;
    }

    private Guid GetCurrentUserId()
        => Guid.Parse(User.FindFirstValue("userId") ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Mendapatkan semua pembayaran (Admin only).</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<PaymentDto>>.Ok(result));
    }

    /// <summary>Mendapatkan riwayat pembayaran milik pengguna yang sedang login.</summary>
    [HttpGet("my")]
    public async Task<IActionResult> GetMyPayments()
    {
        var result = await _service.GetByUserAsync(GetCurrentUserId());
        return Ok(ApiResponse<IEnumerable<PaymentDto>>.Ok(result));
    }

    /// <summary>Mendapatkan pembayaran berdasarkan sewa.</summary>
    [HttpGet("rental/{rentalId:guid}")]
    public async Task<IActionResult> GetByRental(Guid rentalId)
    {
        var result = await _service.GetByRentalAsync(rentalId);
        return Ok(ApiResponse<IEnumerable<PaymentDto>>.Ok(result));
    }

    /// <summary>Membuat pembayaran baru.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation Error",
                validation.Errors.Select(e => e.ErrorMessage)));

        var result = await _service.CreateAsync(GetCurrentUserId(), request);
        return Ok(ApiResponse<PaymentDto>.Ok(result, "Pembayaran berhasil dicatat."));
    }
}
