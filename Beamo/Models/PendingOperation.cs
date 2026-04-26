using SQLite;

namespace Beamo.Models;

[Table("PendingOperations")]
public record PendingOperation : BaseEntity
{
    [NotNull][MaxLength(100)] public string EntityType { get; set; } = string.Empty;
    [NotNull] public string EntityId { get; set; } = string.Empty;
    [NotNull][MaxLength(50)] public string Operation { get; set; } = string.Empty;
    [NotNull] public string Data { get; set; } = string.Empty;

    public int RetryCount { get; set; }
    public int MaxRetries { get; set; } = 3;
    public string? LastError { get; set; }
    public DateTime? NextRetryAt { get; set; }
    [MaxLength(50)] public string Status { get; set; } = "Pending";
    [MaxLength(50)] public string Priority { get; set; } = "Normal";
}