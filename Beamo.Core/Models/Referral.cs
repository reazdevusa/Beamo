using SQLite;

namespace Beamo.Core.Models;

[Table("Referrals")]
public class Referral : BaseEntity
{
    public string ReferrerId { get; set; } = string.Empty;
    public string ReferredEmail { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
}