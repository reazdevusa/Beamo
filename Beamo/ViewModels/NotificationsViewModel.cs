using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Beamo.Models;
using Beamo.Services.Database;
using System.Collections.ObjectModel;

namespace Beamo.ViewModels;

public partial class NotificationsViewModel : BaseViewModel
{
    private readonly IDatabaseService _db;
    private const string UserId = "local";

    [ObservableProperty] public partial ObservableCollection<Notification> Notifications { get; set; }
    [ObservableProperty] public partial bool                               IsEmpty       { get; set; }
    [ObservableProperty] public partial int                                UnreadCount   { get; set; }

    public NotificationsViewModel(IDatabaseService db)
    {
        _db           = db;
        Title         = "Notifications";
        Notifications = [];
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var items     = await _db.GetUnreadNotificationsAsync(UserId);
            Notifications = new ObservableCollection<Notification>(
                items.OrderByDescending(static n => n.CreatedAt));
            UnreadCount = items.Count(static n => !n.IsRead);
            IsEmpty     = Notifications.Count == 0;
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task MarkReadAsync(Notification notification)
    {
        if (notification.IsRead) return;
        await _db.MarkNotificationReadAsync(notification.Id);
        var index = Notifications.IndexOf(notification);
        if (index >= 0)
        {
            Notifications[index] = notification with { IsRead = true, ReadAt = DateTime.UtcNow };
            UnreadCount = Notifications.Count(static n => !n.IsRead);
        }
    }

    [RelayCommand]
    private async Task MarkAllReadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            foreach (var n in Notifications.Where(static n => !n.IsRead))
                await _db.MarkNotificationReadAsync(n.Id);
            await LoadAsync();
        }
        finally { IsBusy = false; }
    }
}