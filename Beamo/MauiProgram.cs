using Beamo.Services.Database;
using Beamo.ViewModels;
using Beamo.Views.Contacts;
using Beamo.Views.Home;
using Beamo.Views.Notifications;
using Beamo.Views.Profile;
using Beamo.Views.Referrals;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Beamo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(static fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ─── Services ─────────────────────────────────────────
        builder.Services.AddSingleton<IDatabaseService>(sp =>
            new DatabaseService(
                sp.GetRequiredService<ILogger<DatabaseService>>()
            ));

        // ─── ViewModels ───────────────────────────────────────
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<ContactsViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<ReferralsViewModel>();
        builder.Services.AddTransient<NotificationsViewModel>();

        // ─── Views ────────────────────────────────────────────
        builder.Services.AddTransient<HomeView>();
        builder.Services.AddTransient<ContactsView>();
        builder.Services.AddTransient<ProfileView>();
        builder.Services.AddTransient<ReferralsView>();
        builder.Services.AddTransient<NotificationsView>();

        // ─── Logging ──────────────────────────────────────────
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
