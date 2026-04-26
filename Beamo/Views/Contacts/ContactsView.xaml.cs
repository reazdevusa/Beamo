using Beamo.ViewModels;

namespace Beamo.Views.Contacts;

public partial class ContactsView : ContentPage
{
    private readonly ContactsViewModel _vm;

    public ContactsView(ContactsViewModel vm)
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