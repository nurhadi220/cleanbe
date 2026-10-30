using System.Collections.Generic;
using System.Text.Json;
using KosKu.Application.Common;
using KosKu.Application.Features.RoomTypes.DTOs;
using KosKu.Application.Interfaces;
using KosKu.Domain.Entities;

namespace KosKu.Application.Features.RoomTypes.Services;

/// <summary>Service untuk manajemen tipe kamar.</summary>
public class RoomTypeService
{
    private readonly IUnitOfWork _uow;

    public RoomTypeService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<RoomTypeDto>> GetAllAsync()
    {
        var list = await _uow.RoomTypes.GetAllAsync();
        return list.Select(MapToDto);
    }

    public async Task<RoomTypeDto> GetByIdAsync(Guid id)
    {
        var rt = await _uow.RoomTypes.GetByIdAsync(id)
            ?? throw new NotFoundException("TipeKamar", id);
        return MapToDto(rt);
    }

    public async Task<RoomTypeDto> CreateAsync(CreateRoomTypeRequest request)
    {
        // 1. Validasi: Cek apakah nama tipe sudah ada
        var nameExists = await _uow.RoomTypes.ExistsAsync(rt => rt.NamaTipe == request.NamaTipe);
        if (nameExists)
            throw new ValidationException($"Tipe kamar dengan nama '{request.NamaTipe}' sudah ada.");

        var rt = new RoomType
        {
            NamaTipe = request.NamaTipe,
            Deskripsi = request.Deskripsi,
            HargaPerBulan = request.HargaPerBulan,
            Fasilitas = request.Fasilitas != null ? JsonSerializer.Serialize(request.Fasilitas) : null
        };

        await _uow.RoomTypes.AddAsync(rt);
        await _uow.SaveChangesAsync();
        return MapToDto(rt);
    }

    public async Task<RoomTypeDto> UpdateAsync(Guid id, UpdateRoomTypeRequest request)
    {
        var rt = await _uow.RoomTypes.GetByIdAsync(id)
            ?? throw new NotFoundException("TipeKamar", id);

        // 1. Validasi: Cek nama tipe ganda (kecuali untuk ID dirinya sendiri)
        var nameExists = await _uow.RoomTypes.ExistsAsync(r => r.NamaTipe == request.NamaTipe && r.Id != id);
        if (nameExists)
            throw new ValidationException($"Tipe kamar dengan nama '{request.NamaTipe}' sudah ada.");

        rt.NamaTipe = request.NamaTipe;
        rt.Deskripsi = request.Deskripsi;
        rt.HargaPerBulan = request.HargaPerBulan;
        rt.Fasilitas = request.Fasilitas != null ? JsonSerializer.Serialize(request.Fasilitas) : null;

        _uow.RoomTypes.Update(rt);
        await _uow.SaveChangesAsync();
        return MapToDto(rt);
    }

    public async Task DeleteAsync(Guid id)
    {
        var rt = await _uow.RoomTypes.GetByIdAsync(id)
            ?? throw new NotFoundException("TipeKamar", id);

        // PROTEKSI UTAMA: Cek apakah ada kamar yang menggunakan TipeKamarId ini
        var isUsedInRooms = await _uow.Rooms.ExistsAsync(r => r.TipeKamarId == id);
        if (isUsedInRooms)
        {
            throw new ValidationException(
                $"Tipe kamar '{rt.NamaTipe}' tidak dapat dihapus karena masih digunakan oleh satu atau lebih kamar."
            );
        }

        _uow.RoomTypes.Delete(rt);
        await _uow.SaveChangesAsync();
    }

    private static RoomTypeDto MapToDto(RoomType rt)
    {
        FasilitasDto? fasilitasDto = null;
        if (!string.IsNullOrWhiteSpace(rt.Fasilitas))
        {
            try
            {
                fasilitasDto = JsonSerializer.Deserialize<FasilitasDto>(rt.Fasilitas);
            }
            catch (JsonException)
            {
                fasilitasDto = new FasilitasDto { UkuranKamar = rt.Fasilitas };
            }
        }

        return new RoomTypeDto(rt.Id, rt.NamaTipe, rt.Deskripsi, rt.HargaPerBulan, fasilitasDto, rt.CreatedAt);
    }
}