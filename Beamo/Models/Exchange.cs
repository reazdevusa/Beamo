using SQLite;

namespace Beamo.Models;

[Table("Exchanges")]
public record Exchange : BaseEntity
{
    [NotNull][Indexed] public string UserId { get; init; } = string.Empty;
    [NotNull][Indexed] public string ContactId { get; init; } = string.Empty;
    [NotNull][MaxLength(50)] public string Method { get; set; } = "Manual";
    [MaxLength(500)] public string? Location { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    [MaxLength(255)] public string? Event { get; set; }
    public string? Notes { get; set; }
    [MaxLength(50)] public string Direction { get; set; } = "Mutual";
    public DateTime? FollowUpDate { get; set; }
    public bool IsFollowUpComplete { get; set; }
}