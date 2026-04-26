using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Beamo.Services.Database;

namespace Beamo.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly IDatabaseService _db;

    [ObservableProperty] public partial string WelcomeMessage           { get; set; }
    [ObservableProperty] public partial int    ContactCount             { get; set; }
    [ObservableProperty] public partial int    UnreadNotificationCount  { get; set; }

    public HomeViewModel(IDatabaseService db)
    {
        _db            = db;
        Title          = "Home";
        WelcomeMessage = "Welcome to Beamo 👋";
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var contacts      = await _db.GetContactsAsync("local");
            var notifications = await _db.GetUnreadNotificationsAsync("local");
            ContactCount            = contacts.Count;
            UnreadNotificationCount = notifications.Count;
        }
        finally { IsBusy = false; }
    }
}