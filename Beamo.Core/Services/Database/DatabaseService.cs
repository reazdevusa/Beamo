using Beamo.Core.Models;
using BeamoContact = Beamo.Core.Models.Contact;
using Microsoft.Extensions.Logging;
using SQLite;

namespace Beamo.Core.Services.Database;

public class DatabaseService : IDatabaseService
{
    // ─── Fields ───────────────────────────────────────────────
    private SQLiteAsyncConnection? _db;
    private readonly ILogger<DatabaseService> _logger;
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private bool _isInitialized;

    // ✅ Path is injected — no MAUI dependency
    private readonly string _dbPath;

    // ─── Constructor ──────────────────────────────────────────
    public DatabaseService(string dbPath, ILogger<DatabaseService> logger)
    {
        _dbPath  = dbPath;
        _logger  = logger;
    }

    // ─── Initialization ───────────────────────────────────────
    public async Task InitializeAsync()
    {
        if (_isInitialized) return;

        await _initLock.WaitAsync();
        try
        {
            if (_isInitialized) return;

            _db = new SQLiteAsyncConnection(_dbPath, SQLiteOpenFlags.ReadWrite |
                SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

            await _db.CreateTablesAsync<User, Profile, BeamoContact, Exchange>();
            await _db.CreateTablesAsync<Referral, ReferralEarning, Notification, ImportantEmail>();
            await _db.CreateTableAsync<PendingOperation>();

            _isInitialized = true;
            _logger.LogInformation("✅ Database initialized at {Path}", _dbPath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Database initialization failed");
            throw;
        }
        finally
        {
            _initLock.Release();
        }
    }

    // ─── Ensure DB Ready ──────────────────────────────────────
    private async Task<SQLiteAsyncConnection> GetDbAsync()
    {
        if (!_isInitialized) await InitializeAsync();
        return _db!;
    }

    // ─── Generic CRUD ─────────────────────────────────────────
    public async Task<List<T>> GetAllAsync<T>(CancellationToken ct = default)
        where T : BaseEntity, new()
    {
        try
        {
            var db = await GetDbAsync();
            return await db.Table<T>()
                           .Where(static e => !e.IsDeleted)
                           .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ GetAllAsync failed for {Type}", typeof(T).Name);
            return [];
        }
    }

    public async Task<T?> GetByIdAsync<T>(string id, CancellationToken ct = default)
        where T : BaseEntity, new()
    {
        try
        {
            var db = await GetDbAsync();
            return await db.Table<T>()
                           .Where(e => e.Id == id && !e.IsDeleted)
                           .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ GetByIdAsync failed for {Type} Id:{Id}", typeof(T).Name, id);
            return null;
        }
    }

    public async Task<int> SaveAsync<T>(T entity, CancellationToken ct = default)
        where T : BaseEntity, new()
    {
        try
        {
            var db = await GetDbAsync();
            entity.UpdatedAt = DateTime.UtcNow;

            var existing = await db.FindAsync<T>(entity.Id);
            return existing is null
                ? await db.InsertAsync(entity)
                : await db.UpdateAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ SaveAsync failed for {Type}", typeof(T).Name);
            return 0;
        }
    }

    public async Task<int> SaveAllAsync<T>(List<T> entities, CancellationToken ct = default)
        where T : BaseEntity, new()
    {
        try
        {
            var db = await GetDbAsync();
            entities.ForEach(static e => e.UpdatedAt = DateTime.UtcNow);
            return await db.InsertAllAsync(entities, "OR REPLACE");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ SaveAllAsync failed for {Type}", typeof(T).Name);
            return 0;
        }
    }

    public async Task<int> DeleteAsync<T>(string id, CancellationToken ct = default)
        where T : BaseEntity, new()
    {
        try
        {
            var db = await GetDbAsync();
            var entity = await db.FindAsync<T>(id);
            if (entity is null) return 0;
            return await db.DeleteAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ DeleteAsync failed for {Type} Id:{Id}", typeof(T).Name, id);
            return 0;
        }
    }

    public async Task<int> SoftDeleteAsync<T>(string id, CancellationToken ct = default)
        where T : BaseEntity, new()
    {
        try
        {
            var entity = await GetByIdAsync<T>(id, ct);
            if (entity is null) return 0;

            entity.IsDeleted = true;
            entity.SyncStatus = 3; // PendingDelete
            entity.UpdatedAt = DateTime.UtcNow;
            return await SaveAsync(entity, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ SoftDeleteAsync failed for {Type} Id:{Id}", typeof(T).Name, id);
            return 0;
        }
    }

    // ─── Contacts ─────────────────────────────────────────────
    public async Task<List<BeamoContact>> GetContactsAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            var db = await GetDbAsync();
            return await db.Table<BeamoContact>()
                           .Where(c => c.UserId == userId && !c.IsDeleted)
                           .OrderByDescending(c => c.UpdatedAt)
                           .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ GetContactsAsync failed");
            return [];
        }
    }

    public async Task<List<BeamoContact>> SearchContactsAsync(string userId, string query, CancellationToken ct = default)
    {
        try
        {
            var db = await GetDbAsync();
            var q = query.ToLower();
            return await db.Table<BeamoContact>()
                           .Where(c => c.UserId == userId
                               && !c.IsDeleted
                               && (c.FullName.ToLower().Contains(q)
                               || (c.Email != null && c.Email.ToLower().Contains(q))
                               || (c.Company != null && c.Company.ToLower().Contains(q))))
                           .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ SearchContactsAsync failed");
            return [];
        }
    }

    public async Task<List<BeamoContact>> GetFavoriteContactsAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            var db = await GetDbAsync();
            return await db.Table<BeamoContact>()
                           .Where(c => c.UserId == userId && c.IsFavorite && !c.IsDeleted)
                           .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ GetFavoriteContactsAsync failed");
            return [];
        }
    }

    // ─── Exchanges ────────────────────────────────────────────
    public async Task<List<Exchange>> GetExchangesAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            var db = await GetDbAsync();
            return await db.Table<Exchange>()
                           .Where(e => e.UserId == userId && !e.IsDeleted)
                           .OrderByDescending(e => e.CreatedAt)
                           .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ GetExchangesAsync failed");
            return [];
        }
    }

    public async Task<List<Exchange>> GetExchangesForContactAsync(string contactId, CancellationToken ct = default)
    {
        try
        {
            var db = await GetDbAsync();
            return await db.Table<Exchange>()
                           .Where(e => e.ContactId == contactId && !e.IsDeleted)
                           .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ GetExchangesForContactAsync failed");
            return [];
        }
    }

    // ─── Referrals ────────────────────────────────────────────
    public async Task<List<Referral>> GetReferralsAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            var db = await GetDbAsync();
            return await db.Table<Referral>()
                           .Where(r => r.ReferrerId == userId && !r.IsDeleted)
                           .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ GetReferralsAsync failed");
            return [];
        }
    }

    public async Task<List<ReferralEarning>> GetEarningsAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            var db = await GetDbAsync();
            return await db.Table<ReferralEarning>()
                           .Where(e => e.UserId == userId && !e.IsDeleted)
                           .OrderByDescending(e => e.CreatedAt)
                           .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ GetEarningsAsync failed");
            return [];
        }
    }

    public async Task<decimal> GetTotalEarningsAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            var earnings = await GetEarningsAsync(userId, ct);
            return earnings
                .Where(static e => e.Status == "Paid")
                .Sum(static e => e.Amount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ GetTotalEarningsAsync failed");
            return 0;
        }
    }

    // ─── Notifications ────────────────────────────────────────
    public async Task<List<Notification>> GetUnreadNotificationsAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            var db = await GetDbAsync();
            return await db.Table<Notification>()
                           .Where(n => n.UserId == userId && !n.IsRead && !n.IsDeleted)
                           .OrderByDescending(n => n.CreatedAt)
                           .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ GetUnreadNotificationsAsync failed");
            return [];
        }
    }

    public async Task MarkNotificationReadAsync(string notificationId, CancellationToken ct = default)
    {
        try
        {
            var notification = await GetByIdAsync<Notification>(notificationId, ct);
            if (notification is null) return;

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await SaveAsync(notification, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ MarkNotificationReadAsync failed");
        }
    }

    // ─── Important Emails ─────────────────────────────────────
    public async Task<List<ImportantEmail>> GetImportantEmailsAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            var db = await GetDbAsync();
            return await db.Table<ImportantEmail>()
                           .Where(e => e.UserId == userId && !e.IsDeleted)
                           .OrderByDescending(e => e.ReceivedAt)
                           .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ GetImportantEmailsAsync failed");
            return [];
        }
    }

    // ─── Sync Queue ───────────────────────────────────────────
    public async Task<List<PendingOperation>> GetPendingOperationsAsync(CancellationToken ct = default)
    {
        try
        {
            var db = await GetDbAsync();
            return await db.Table<PendingOperation>()
                           .Where(static p => p.Status == "Pending" && p.RetryCount < p.MaxRetries)
                           .OrderBy(static p => p.Priority)
                           .OrderBy(static p => p.CreatedAt)
                           .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ GetPendingOperationsAsync failed");
            return [];
        }
    }

    public async Task ClearCompletedOperationsAsync(CancellationToken ct = default)
    {
        try
        {
            var db = await GetDbAsync();
            await db.Table<PendingOperation>()
                    .Where(static p => p.Status == "Completed" || p.Status == "Failed")
                    .DeleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ ClearCompletedOperationsAsync failed");
        }
    }

    public async Task<int> GetPendingOperationsCountAsync(CancellationToken ct = default)
    {
        try
        {
            var db = await GetDbAsync();
            return await db.Table<PendingOperation>()
                           .Where(static p => p.Status == "Pending" && p.RetryCount < p.MaxRetries)
                           .CountAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ GetPendingOperationsCountAsync failed");
            return 0;
        }
    }
}