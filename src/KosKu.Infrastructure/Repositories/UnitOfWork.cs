using KosKu.Application.Interfaces;
using KosKu.Application.Interfaces.Repositories;
using KosKu.Infrastructure.Persistence;

namespace KosKu.Infrastructure.Repositories;

/// <summary>Implementasi Unit of Work yang mengkoordinasi semua repository dalam satu DbContext.</summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private bool _disposed;

    public IUserRepository Users { get; }
    public IRoomTypeRepository RoomTypes { get; }
    public IRoomRepository Rooms { get; }
    public IBookingRepository Bookings { get; }
    public IRentalRepository Rentals { get; }
    public IPaymentRepository Payments { get; }
    public IFineRepository Fines { get; }
    public IMaintenanceRepository Maintenance { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new UserRepository(context);
        RoomTypes = new RoomTypeRepository(context);
        Rooms = new RoomRepository(context);
        Bookings = new BookingRepository(context);
        Rentals = new RentalRepository(context);
        Payments = new PaymentRepository(context);
        Fines = new FineRepository(context);
        Maintenance = new MaintenanceRepository(context);
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
            _context.Dispose();
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
