using CommunityToolkit.Mvvm.ComponentModel;

namespace Beamo.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    public partial bool IsBusy { get; set; }

    [ObservableProperty]
    public partial string Title { get; set; }

    public bool IsNotBusy => !IsBusy;

    public BaseViewModel()
    {
        Title = string.Empty;
    }
}