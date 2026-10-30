namespace KosKu.Domain.Common;

/// <summary>Base entity dengan Id dan timestamp.</summary>
public abstract class BaseEntity
{
    /// <summary>Primary key entity.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Waktu entity dibuat.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
