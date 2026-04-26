using SQLite;

namespace Beamo.Models;

[Table("ReferralEarnings")]
public record ReferralEarning : BaseEntity
{
    [NotNull][Indexed] public string UserId { get; init; } = string.Empty;
    [NotNull][Indexed] public string ReferralId { get; init; } = string.Empty;
    public decimal Amount { get; set; }
    [MaxLength(10)] public string Currency { get; set; } = "USD";
    [MaxLength(50)] public string Type { get; set; } = "Subscription";
    public decimal CommissionRate { get; set; }
    public int Level { get; set; } = 1;
    [MaxLength(50)] public string Status { get; set; } = "Pending";
    public DateTime? PaidAt { get; set; }
    [MaxLength(255)] public string? StripeTransactionId { get; set; }
}