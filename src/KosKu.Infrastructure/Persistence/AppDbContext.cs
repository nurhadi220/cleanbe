using KosKu.Domain.Entities;
using KosKu.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace KosKu.Infrastructure.Persistence;

/// <summary>
/// AppDbContext untuk KosKu — mengelola seluruh entity dan konfigurasi EF Core.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>Tabel pengguna sistem.</summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>Tabel tipe kamar.</summary>
    public DbSet<RoomType> RoomTypes => Set<RoomType>();

    /// <summary>Tabel kamar kos.</summary>
    public DbSet<Room> Rooms => Set<Room>();

    /// <summary>Tabel booking.</summary>
    public DbSet<Booking> Bookings => Set<Booking>();

    /// <summary>Tabel sewa aktif.</summary>
    public DbSet<Rental> Rentals => Set<Rental>();

    /// <summary>Tabel pembayaran.</summary>
    public DbSet<Payment> Payments => Set<Payment>();

    /// <summary>Tabel denda.</summary>
    public DbSet<Fine> Fines => Set<Fine>();

    /// <summary>Tabel laporan perbaikan.</summary>
    public DbSet<MaintenanceRequest> MaintenanceRequests => Set<MaintenanceRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Terapkan semua konfigurasi Fluent API dari assembly ini
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
