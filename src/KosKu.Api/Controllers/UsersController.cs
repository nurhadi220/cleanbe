using System.Security.Claims;
using FluentValidation;
using KosKu.Application.Common;
using KosKu.Application.Features.Users.DTOs;
using KosKu.Application.Features.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KosKu.Api.Controllers;

/// <summary>Controller untuk manajemen pengguna.</summary>
[ApiController]
[Route("api/users")]
[Authorize]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IValidator<UpdateUserRequest> _updateValidator;
    private readonly IValidator<ChangePasswordRequest> _changePasswordValidator;

    public UsersController(
        UserService userService,
        IValidator<UpdateUserRequest> updateValidator,
        IValidator<ChangePasswordRequest> changePasswordValidator)
    {
        _userService = userService;
        _updateValidator = updateValidator;
        _changePasswordValidator = changePasswordValidator;
    }

    private Guid GetCurrentUserId()
        => Guid.Parse(User.FindFirstValue("userId") ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Mendapatkan semua pengguna (Admin only).</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<UserDto>>.Ok(users));
    }

    /// <summary>Mendapatkan pengguna berdasarkan Id.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    /// <summary>Mendapatkan profil pengguna yang sedang login.</summary>
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var user = await _userService.GetByIdAsync(GetCurrentUserId());
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    /// <summary>Memperbarui data pengguna.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
    {
        var validation = await _updateValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation Error",
                validation.Errors.Select(e => e.ErrorMessage)));

        var user = await _userService.UpdateAsync(id, request);
        return Ok(ApiResponse<UserDto>.Ok(user, "Data pengguna berhasil diperbarui."));
    }

    /// <summary>Menghapus pengguna (Admin only).</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.DeleteAsync(id);
        return Ok(ApiResponse<object>.Ok("Pengguna berhasil dihapus."));
    }

    /// <summary>Mengubah password pengguna yang sedang login.</summary>
    [HttpPost("me/change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var validation = await _changePasswordValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation Error",
                validation.Errors.Select(e => e.ErrorMessage)));

        await _userService.ChangePasswordAsync(GetCurrentUserId(), request);
        return Ok(ApiResponse<object>.Ok("Password berhasil diubah."));
    }
}
