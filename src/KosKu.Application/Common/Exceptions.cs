namespace KosKu.Application.Common;

/// <summary>Exception untuk error validasi business logic.</summary>
public class ValidationException : Exception
{
    public IEnumerable<string> Errors { get; }

    public ValidationException(string message) : base(message)
        => Errors = new[] { message };

    public ValidationException(IEnumerable<string> errors) : base("Validation failed")
        => Errors = errors;
}

/// <summary>Exception untuk data tidak ditemukan (404).</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"{name} dengan id '{key}' tidak ditemukan.") { }
}

/// <summary>Exception untuk akses tidak terotorisasi (403).</summary>
public class ForbiddenException : Exception
{
    public ForbiddenException(string message = "Akses ditolak.") : base(message) { }
}
