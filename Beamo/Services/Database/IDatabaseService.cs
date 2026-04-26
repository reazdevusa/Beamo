using Beamo.Models;
using BeamoContact = Beamo.Models.Contact;
using Microsoft.Extensions.Logging;
using SQLite;

namespace Beamo.Services.Database;

public interface IDatabaseService
{
    // ─── Initialization ───────────────────────────────────────
    Task InitializeAsync();

    // ─── Generic CRUD ──────────────────────────────────────────
    Task<List<T>> GetAllAsync<T>(CancellationToken ct = default) where T : BaseEntity, new();
    Task<T?> GetByIdAsync<T>(string id, CancellationToken ct = default) where T : BaseEntity, new();
    Task<int> SaveAsync<T>(T entity, CancellationToken ct = default) where T : BaseEntity, new();
    Task<int> SaveAllAsync<T>(List<T> entities, CancellationToken ct = default) where T : BaseEntity, new();
    Task<int> DeleteAsync<T>(string id, CancellationToken ct = default) where T : BaseEntity, new();
    Task<int> SoftDeleteAsync<T>(string id, CancellationToken ct = default) where T : BaseEntity, new();

    // ─── Contacts ──────────────────────────────────────────────
    Task<List<BeamoContact>> GetContactsAsync(string userId, CancellationToken ct = default);
    Task<List<BeamoContact>> SearchContactsAsync(string userId, string query, CancellationToken ct = default);
    Task<List<BeamoContact>> GetFavoriteContactsAsync(string userId, CancellationToken ct = default);

    // ─── Exchanges ─────────────────────────────────────────────
    Task<List<Exchange>> GetExchangesAsync(string userId, CancellationToken ct = default);
    Task<List<Exchange>> GetExchangesForContactAsync(string contactId, CancellationToken ct = default);

    // ─── Referrals ─────────────────────────────────────────────
    Task<List<Referral>> GetReferralsAsync(string userId, CancellationToken ct = default);
    Task<List<ReferralEarning>> GetEarningsAsync(string userId, CancellationToken ct = default);
    Task<decimal> GetTotalEarningsAsync(string userId, CancellationToken ct = default);

    // ─── Notifications ─────────────────────────────────────────
    Task<List<Notification>> GetUnreadNotificationsAsync(string userId, CancellationToken ct = default);
    Task MarkNotificationReadAsync(string notificationId, CancellationToken ct = default);

    // ─── Important Emails ──────────────────────────────────────
    Task<List<ImportantEmail>> GetImportantEmailsAsync(string userId, CancellationToken ct = default);

    // ─── Sync Queue ────────────────────────────────────────────
    Task<List<PendingOperation>> GetPendingOperationsAsync(CancellationToken ct = default);
    Task ClearCompletedOperationsAsync(CancellationToken ct = default);
    Task<int> GetPendingOperationsCountAsync(CancellationToken ct = default);
}