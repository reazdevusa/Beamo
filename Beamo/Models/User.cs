using SQLite;

namespace Beamo.Models;

[Table("Users")]
public record User : BaseEntity
{
    [NotNull][MaxLength(255)][Indexed(Unique = true)]
    public string Email { get; set; } = string.Empty;
    [MaxLength(200)] public string? FullName { get; set; }
    [MaxLength(500)] public string? PhotoUrl { get; set; }
    [MaxLength(50)]  public string SubscriptionTier { get; set; } = "Free";
    public DateTime? SubscriptionExpiresAt { get; set; }
    [MaxLength(255)] public string? StripeCustomerId { get; set; }
    [MaxLength(20)][Indexed(Unique = true)] public string? ReferralCode { get; set; }
    public string? ReferredByUserId { get; set; }
    public decimal TotalEarnings { get; set; }
    public decimal AvailableBalance { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool IsPartner { get; set; }
    public DateTime? LastLoginAt { get; set; }
}