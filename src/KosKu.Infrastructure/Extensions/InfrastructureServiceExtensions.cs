using KosKu.Application.Interfaces;
using KosKu.Application.Interfaces.Services;
using KosKu.Infrastructure.Persistence;
using KosKu.Infrastructure.Repositories;
using KosKu.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KosKu.Infrastructure.Extensions;

/// <summary>Extension methods untuk registrasi semua dependency Infrastructure.</summary>
public static class InfrastructureServiceExtensions
{
    /// <summary>
    /// Mendaftarkan semua service Infrastructure ke DI container.
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
            // options.UseNpgsql(
            //     configuration.GetConnectionString("DefaultConnection"),
            //     b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            // )
            options.UseMySql(
                configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))
            )
        );

        // Unit of Work & Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}
