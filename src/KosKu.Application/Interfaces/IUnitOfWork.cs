using KosKu.Application.Interfaces.Repositories;

namespace KosKu.Application.Interfaces;

/// <summary>
/// Unit of Work pattern — mengelola semua repository dalam satu transaksi database.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IRoomTypeRepository RoomTypes { get; }
    IRoomRepository Rooms { get; }
    IBookingRepository Bookings { get; }
    IRentalRepository Rentals { get; }
    IPaymentRepository Payments { get; }
    IFineRepository Fines { get; }
    IMaintenanceRepository Maintenance { get; }

    /// <summary>Menyimpan semua perubahan dalam satu transaksi.</summary>
    Task<int> SaveChangesAsync();
}
