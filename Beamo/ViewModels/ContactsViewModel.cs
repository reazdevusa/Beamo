using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Beamo.Services.Database;
using System.Collections.ObjectModel;
using Contact = Beamo.Models.Contact;

namespace Beamo.ViewModels;

public partial class ContactsViewModel : BaseViewModel
{
    private readonly IDatabaseService _db;
    private List<Contact> _allContacts = [];

    [ObservableProperty] public partial ObservableCollection<Contact> Contacts   { get; set; }
    [ObservableProperty] public partial string                        SearchText { get; set; }
    [ObservableProperty] public partial bool                          IsEmpty    { get; set; }

    public ContactsViewModel(IDatabaseService db)
    {
        _db        = db;
        Title      = "Contacts";
        Contacts   = [];
        SearchText = string.Empty;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            _allContacts = await _db.GetContactsAsync("local");
            ApplyFilter();
        }
        finally { IsBusy = false; }
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? _allContacts
            : _allContacts.Where(c =>
                (c.FullName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.Email?.Contains(SearchText, StringComparison.OrdinalIgnoreCase)    ?? false) ||
                (c.Company?.Contains(SearchText, StringComparison.OrdinalIgnoreCase)  ?? false)).ToList();

        Contacts = new ObservableCollection<Contact>(filtered);
        IsEmpty  = Contacts.Count == 0;
    }

    [RelayCommand]
    private async Task DeleteContactAsync(Contact contact)
    {
        if (IsBusy) return;
        bool confirmed = await Shell.Current.DisplayAlertAsync(
            "Delete Contact", $"Are you sure you want to delete {contact.FullName}?", "Delete", "Cancel");
        if (!confirmed) return;
        await _db.SoftDeleteAsync<Contact>(contact.Id);
        _allContacts.Remove(contact);
        ApplyFilter();
    }

    [RelayCommand]
    private async Task ToggleFavoriteAsync(Contact contact)
    {
        var updated = contact with { IsFavorite = !contact.IsFavorite };
        await _db.SaveAsync(updated);
        var index = _allContacts.IndexOf(contact);
        if (index >= 0) _allContacts[index] = updated;
        ApplyFilter();
    }
}