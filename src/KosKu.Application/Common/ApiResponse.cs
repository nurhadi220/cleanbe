namespace KosKu.Application.Common;

/// <summary>Response wrapper standar untuk semua API endpoint.</summary>
/// <typeparam name="T">Tipe data yang dikembalikan.</typeparam>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public IEnumerable<string>? Errors { get; set; }

    /// <summary>Buat response sukses dengan data.</summary>
    public static ApiResponse<T> Ok(T data, string message = "Berhasil")
        => new() { Success = true, Message = message, Data = data };

    /// <summary>Buat response sukses tanpa data.</summary>
    public static ApiResponse<T> Ok(string message = "Berhasil")
        => new() { Success = true, Message = message };

    /// <summary>Buat response error dengan pesan.</summary>
    public static ApiResponse<T> Fail(string message, IEnumerable<string>? errors = null)
        => new() { Success = false, Message = message, Errors = errors };
}
