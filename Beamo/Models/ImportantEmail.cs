using SQLite;

namespace Beamo.Models;

[Table("ImportantEmails")]
public record ImportantEmail : BaseEntity
{
    [NotNull][Indexed] public string UserId { get; init; } = string.Empty;
    [NotNull][MaxLength(500)] public string Subject { get; set; } = string.Empty;
    [NotNull][MaxLength(255)] public string From { get; set; } = string.Empty;
    [MaxLength(255)] public string? To { get; set; }
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public string? BodyPreview { get; set; }
    public int ConfidenceScore { get; set; }
    [MaxLength(100)] public string Category { get; set; } = "Urgent";
    public string? ActionItems { get; set; }
    public bool IsNotified { get; set; }
    public DateTime? NotifiedAt { get; set; }
    [MaxLength(255)] public string? MessageId { get; set; }
}