using SQLite;

namespace Beamo.Core.Models;

[Table("ImportantEmails")]
public class ImportantEmail : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? Sender { get; set; }
    public string? Preview { get; set; }
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
}