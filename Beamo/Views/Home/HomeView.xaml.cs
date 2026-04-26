using Beamo.ViewModels;

namespace Beamo.Views.Home;

public partial class HomeView : ContentPage
{
    private readonly HomeViewModel _vm;

    public HomeView(HomeViewModel vm)
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