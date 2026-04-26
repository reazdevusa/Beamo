using SQLite;

namespace Beamo.Models;

[Table("Profiles")]
public record Profile : BaseEntity
{
    [NotNull][Indexed] public string UserId { get; init; } = string.Empty;

    [MaxLength(200)] public string? FullName { get; set; }
    [MaxLength(255)] public string? Email { get; set; }
    [MaxLength(50)]  public string? Phone { get; set; }
    [MaxLength(200)] public string? Company { get; set; }
    [MaxLength(200)] public string? JobTitle { get; set; }
    [MaxLength(500)] public string? Website { get; set; }
    [MaxLength(500)] public string? LinkedIn { get; set; }
    [MaxLength(100)] public string? Twitter { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Bio { get; set; }
    [MaxLength(255)] public string? AddressLine1 { get; set; }
    [MaxLength(255)] public string? AddressLine2 { get; set; }
    [MaxLength(100)] public string? City { get; set; }
    [MaxLength(100)] public string? State { get; set; }
    [MaxLength(20)]  public string? PostalCode { get; set; }
    [MaxLength(100)] public string? Country { get; set; }
    public string? CustomFields { get; set; }
}