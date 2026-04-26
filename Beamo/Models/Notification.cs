using SQLite;

namespace Beamo.Models;

[Table("Notifications")]
public record Notification : BaseEntity
{
    [NotNull][Indexed] public string UserId { get; init; } = string.Empty;
    [NotNull][MaxLength(255)] public string Title { get; set; } = string.Empty;
    [NotNull] public string Message { get; set; } = string.Empty;
    [MaxLength(50)] public string Type { get; set; } = "Info";
    public bool IsRead { get; set; }
    [MaxLength(500)] public string? ActionUrl { get; set; }
    public string? RelatedEntityId { get; set; }
    [MaxLength(100)] public string? RelatedEntityType { get; set; }
    public DateTime? ReadAt { get; set; }
}