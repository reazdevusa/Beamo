using SQLite;

namespace Beamo.Models;

/// <summary>
/// Base entity for all models with common properties
/// </summary>
public abstract record BaseEntity
{
    /// <summary>
    /// Unique identifier (syncs with PocketBase)
    /// </summary>
    [PrimaryKey]
    public string Id { get; init; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Creation timestamp (UTC)
    /// </summary>
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Last update timestamp (UTC)
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Soft delete flag
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Sync status: 0=Synced, 1=PendingCreate, 2=PendingUpdate, 3=PendingDelete
    /// </summary>
    public int SyncStatus { get; set; }

    /// <summary>
    /// Last sync timestamp (UTC)
    /// </summary>
    public DateTime? LastSyncedAt { get; set; }
}