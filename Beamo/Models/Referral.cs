using SQLite;

namespace Beamo.Models;

[Table("Referrals")]
public record Referral : BaseEntity
{
    [NotNull][Indexed] public string ReferrerId { get; init; } = string.Empty;
    [NotNull][Indexed] public string ReferredUserId { get; init; } = string.Empty;
    public int Level { get; set; } = 1;
    [MaxLength(20)] public string? ReferralCode { get; set; }
    [MaxLength(50)] public string Status { get; set; } = "Active";
    public DateTime? ConvertedAt { get; set; }
    public bool IsConverted { get; set; }
}