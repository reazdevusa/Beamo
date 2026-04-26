using SQLite;

namespace Beamo.Core.Models;

[Table("PendingOperations")]
public class PendingOperation
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string OperationType { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public int Priority { get; set; } = 0;
    public int RetryCount { get; set; } = 0;
    public int MaxRetries { get; set; } = 3;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Payload { get; set; }
}