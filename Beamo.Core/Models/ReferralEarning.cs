using SQLite;

namespace Beamo.Core.Models;

[Table("ReferralEarnings")]
public class ReferralEarning : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string ReferralId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Pending";
}