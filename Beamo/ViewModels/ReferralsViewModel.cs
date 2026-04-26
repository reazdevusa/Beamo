using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Beamo.Models;
using Beamo.Services.Database;
using System.Collections.ObjectModel;

namespace Beamo.ViewModels;

public partial class ReferralsViewModel : BaseViewModel
{
    private readonly IDatabaseService _db;
    private const string UserId = "local";

    [ObservableProperty] public partial ObservableCollection<Referral>        Referrals        { get; set; }
    [ObservableProperty] public partial ObservableCollection<ReferralEarning> Earnings         { get; set; }
    [ObservableProperty] public partial decimal                               TotalEarnings    { get; set; }
    [ObservableProperty] public partial decimal                               AvailableBalance { get; set; }
    [ObservableProperty] public partial string                                ReferralCode     { get; set; }
    [ObservableProperty] public partial bool                                  IsEmpty          { get; set; }

    public ReferralsViewModel(IDatabaseService db)
    {
        _db              = db;
        Title            = "Referrals";
        Referrals        = [];
        Earnings         = [];
        ReferralCode     = "BEAMO-LOCAL";
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var referrals = await _db.GetReferralsAsync(UserId);
            var earnings  = await _db.GetEarningsAsync(UserId);
            var total     = await _db.GetTotalEarningsAsync(UserId);
            Referrals        = new ObservableCollection<Referral>(referrals);
            Earnings         = new ObservableCollection<ReferralEarning>(earnings);
            TotalEarnings    = total;
            AvailableBalance = total;
            IsEmpty          = Referrals.Count == 0;
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task CopyReferralCodeAsync()
    {
        await Clipboard.SetTextAsync(ReferralCode);
        await Shell.Current.DisplayAlertAsync("Copied!", $"Referral code '{ReferralCode}' copied to clipboard.", "OK");
    }

    [RelayCommand]
    private async Task ShareReferralCodeAsync()
    {
        await Share.RequestAsync(new ShareTextRequest
        {
            Text  = $"Join Beamo using my referral code: {ReferralCode}",
            Title = "Share Referral Code"
        });
    }
}