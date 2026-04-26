using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Beamo.Models;
using Beamo.Services.Database;

namespace Beamo.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly IDatabaseService _db;
    private Profile? _currentProfile;
    private const string UserId = "local";

    [ObservableProperty] public partial string FullName    { get; set; }
    [ObservableProperty] public partial string Email       { get; set; }
    [ObservableProperty] public partial string Phone       { get; set; }
    [ObservableProperty] public partial string Company     { get; set; }
    [ObservableProperty] public partial string JobTitle    { get; set; }
    [ObservableProperty] public partial string Website     { get; set; }
    [ObservableProperty] public partial string LinkedIn    { get; set; }
    [ObservableProperty] public partial string Twitter     { get; set; }
    [ObservableProperty] public partial string Bio         { get; set; }
    [ObservableProperty] public partial bool   IsEditing   { get; set; }

    public ProfileViewModel(IDatabaseService db)
    {
        _db = db;
        Title = "Profile";
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var profiles = await _db.GetAllAsync<Profile>();
            _currentProfile = profiles.FirstOrDefault(static p => p.UserId == UserId);
            if (_currentProfile is not null)
                PopulateFields(_currentProfile);
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private void ToggleEdit() => IsEditing = !IsEditing;

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var profile = (_currentProfile ?? new Profile { UserId = UserId }) with
            {
                FullName = FullName, Email    = Email,   Phone   = Phone,
                Company  = Company,  JobTitle = JobTitle, Website = Website,
                LinkedIn = LinkedIn, Twitter  = Twitter, Bio     = Bio
            };
            await _db.SaveAsync(profile);
            _currentProfile = profile;
            IsEditing = false;
            await Shell.Current.DisplayAlertAsync("Saved", "Profile updated successfully.", "OK");
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private void CancelEdit()
    {
        if (_currentProfile is not null)
            PopulateFields(_currentProfile);
        IsEditing = false;
    }

    private void PopulateFields(Profile p)
    {
        FullName = p.FullName  ?? string.Empty;
        Email    = p.Email     ?? string.Empty;
        Phone    = p.Phone     ?? string.Empty;
        Company  = p.Company   ?? string.Empty;
        JobTitle = p.JobTitle  ?? string.Empty;
        Website  = p.Website   ?? string.Empty;
        LinkedIn = p.LinkedIn  ?? string.Empty;
        Twitter  = p.Twitter   ?? string.Empty;
        Bio      = p.Bio       ?? string.Empty;
    }
}