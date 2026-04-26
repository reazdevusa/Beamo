using SQLite;
using System.ComponentModel.DataAnnotations;

namespace Beamo.Models;

/// <summary>
/// Contact entity for storing business cards and contact information
/// </summary>
[Table("Contacts")]
public record Contact : BaseEntity
{
    /// <summary>
    /// Owner user ID (references Users table)
    /// </summary>
    [Required]
    [Indexed]
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Full name
    /// </summary>
    [Required]
    [System.ComponentModel.DataAnnotations.MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Email address
    /// </summary>
    [System.ComponentModel.DataAnnotations.MaxLength(255)]
    public string? Email { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    [System.ComponentModel.DataAnnotations.MaxLength(50)]
    public string? Phone { get; set; }

    /// <summary>
    /// Company name
    /// </summary>
    [System.ComponentModel.DataAnnotations.MaxLength(200)]
    public string? Company { get; set; }

    /// <summary>
    /// Job title
    /// </summary>
    [System.ComponentModel.DataAnnotations.MaxLength(200)]
    public string? JobTitle { get; set; }

    /// <summary>
    /// Website URL
    /// </summary>
    [System.ComponentModel.DataAnnotations.MaxLength(500)]
    public string? Website { get; set; }

    /// <summary>
    /// LinkedIn profile URL
    /// </summary>
    [System.ComponentModel.DataAnnotations.MaxLength(500)]
    public string? LinkedIn { get; set; }

    /// <summary>
    /// Twitter/X handle
    /// </summary>
    [System.ComponentModel.DataAnnotations.MaxLength(100)]
    public string? Twitter { get; set; }

    /// <summary>
    /// Profile photo URL or base64
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Custom notes
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Tags (comma-separated)
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// Is this a favorite contact?
    /// </summary>
    public bool IsFavorite { get; set; }

    /// <summary>
    /// Source of contact: Manual, QR, NFC, BLE, Import
    /// </summary>
    [System.ComponentModel.DataAnnotations.MaxLength(50)]
    public string Source { get; set; } = "Manual";

    /// <summary>
    /// Custom fields (JSON serialized)
    /// </summary>
    public string? CustomFields { get; set; }
}