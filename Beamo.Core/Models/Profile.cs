using SQLite;

namespace Beamo.Core.Models;

[Table("Profiles")]
public class Profile : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? Website { get; set; }
    public string? PhoneNumber { get; set; }
}