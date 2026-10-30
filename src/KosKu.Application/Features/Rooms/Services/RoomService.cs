using KosKu.Application.Common;
using KosKu.Application.Features.Rooms.DTOs;
using KosKu.Application.Interfaces;
using KosKu.Domain.Entities;
using KosKu.Domain.Enums;

namespace KosKu.Application.Features.Rooms.Services;

/// <summary>Service untuk manajemen kamar kos.</summary>
public class RoomService
{
    private readonly IUnitOfWork _uow;

    public RoomService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<RoomDto>> GetAllAsync()
    {
        var rooms = await _uow.Rooms.GetAllWithTypeAsync();
        return rooms.Select(MapToDto);
    }

    public async Task<RoomDto> GetByIdAsync(Guid id)
    {
        var room = await _uow.Rooms.GetByIdWithTypeAsync(id)
            ?? throw new NotFoundException("Kamar", id);
        return MapToDto(room);
    }

    public async Task<RoomDto> CreateAsync(CreateRoomRequest request)
    {
        var tipeExists = await _uow.RoomTypes.ExistsAsync(rt => rt.Id == request.TipeKamarId);
        if (!tipeExists)
            throw new NotFoundException("TipeKamar", request.TipeKamarId);

        var nomorExists = await _uow.Rooms.ExistsAsync(r => r.NomorKamar == request.NomorKamar);
        if (nomorExists)
            throw new ValidationException($"Nomor kamar '{request.NomorKamar}' sudah digunakan.");

        var room = new Room
        {
            NomorKamar = request.NomorKamar,
            Lantai = request.Lantai,
            TipeKamarId = request.TipeKamarId,
            FotoUrl = request.FotoUrl
        };
        await _uow.Rooms.AddAsync(room);
        await _uow.SaveChangesAsync();

        var roomWithType = await _uow.Rooms.GetByIdWithTypeAsync(room.Id);
        return MapToDto(roomWithType!);
    }

    public async Task<RoomDto> UpdateAsync(Guid id, UpdateRoomRequest request)
    {
        var room = await _uow.Rooms.GetByIdAsync(id)
            ?? throw new NotFoundException("Kamar", id);

        var nomorExists = await _uow.Rooms.ExistsAsync(r => r.NomorKamar == request.NomorKamar && r.Id != id);
        if (nomorExists)
            throw new ValidationException($"Nomor kamar '{request.NomorKamar}' sudah digunakan.");

        room.NomorKamar = request.NomorKamar;
        room.Lantai = request.Lantai;
        room.TipeKamarId = request.TipeKamarId;
        room.Status = request.Status;
        room.FotoUrl = request.FotoUrl;

        _uow.Rooms.Update(room);
        await _uow.SaveChangesAsync();

        var roomWithType = await _uow.Rooms.GetByIdWithTypeAsync(room.Id);
        return MapToDto(roomWithType!);
    }

    public async Task DeleteAsync(Guid id)
    {
        var room = await _uow.Rooms.GetByIdAsync(id)
            ?? throw new NotFoundException("Kamar", id);
        _uow.Rooms.Delete(room);
        await _uow.SaveChangesAsync();
    }

    private static RoomDto MapToDto(Room r) =>
        new(r.Id, r.NomorKamar, r.Lantai, r.TipeKamarId,
            r.TipeKamar?.NamaTipe ?? "", r.TipeKamar?.HargaPerBulan ?? 0,
            r.Status.ToString(), r.FotoUrl, r.CreatedAt);
}
