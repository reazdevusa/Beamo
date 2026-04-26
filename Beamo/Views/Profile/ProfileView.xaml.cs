using Beamo.ViewModels;

namespace Beamo.Views.Profile;

public partial class ProfileView : ContentPage
{
    private readonly ProfileViewModel _vm;

    public ProfileView(ProfileViewModel vm)
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