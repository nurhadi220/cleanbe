using KosKu.Application.Features.Auth.Services;
using KosKu.Application.Features.Bookings.Services;
using KosKu.Application.Features.Dashboard.Services;
using KosKu.Application.Features.Fines.Services;
using KosKu.Application.Features.Maintenance.Services;
using KosKu.Application.Features.Payments.Services;
using KosKu.Application.Features.Rentals.Services;
using KosKu.Application.Features.Rooms.Services;
using KosKu.Application.Features.RoomTypes.Services;
using KosKu.Application.Features.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KosKu.Application.Extensions;

/// <summary>Extension methods untuk registrasi semua Application services ke DI container.</summary>
public static class ApplicationServiceExtensions
{
    /// <summary>Mendaftarkan semua Application service.</summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<UserService>();
        services.AddScoped<RoomTypeService>();
        services.AddScoped<RoomService>();
        services.AddScoped<BookingService>();
        services.AddScoped<RentalService>();
        services.AddScoped<PaymentService>();
        services.AddScoped<FineService>();
        services.AddScoped<MaintenanceService>();
        services.AddScoped<DashboardService>();

        return services;
    }
}
