using Beamo.ViewModels;

namespace Beamo.Views.Referrals;

public partial class ReferralsView : ContentPage
{
    private readonly ReferralsViewModel _vm;

    public ReferralsView(ReferralsViewModel vm)
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