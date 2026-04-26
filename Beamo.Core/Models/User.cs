using SQLite;

namespace Beamo.Core.Models;

[Table("Users")]
public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
}