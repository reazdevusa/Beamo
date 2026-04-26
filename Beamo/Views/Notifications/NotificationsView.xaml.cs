using Beamo.ViewModels;

namespace Beamo.Views.Notifications;

public partial class NotificationsView : ContentPage
{
    private readonly NotificationsViewModel _vm;

    public NotificationsView(NotificationsViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }
}