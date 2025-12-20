using LeftLovers.Services;
using LeftLovers.Views;

namespace LeftLovers;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }

    public App(IServiceProvider serviceProvider, AuthService authService)
    {
        InitializeComponent();

        Services = serviceProvider;

        if (authService.IsLoggedIn())
        {
            // ✅ Logged-in app (Shell)
            MainPage = new AppShell();

            // 🔥 Start global Firestore listeners ONCE
            MainThread.BeginInvokeOnMainThread(() =>
            {
                serviceProvider
                    .GetService<FoodPostService>()
                    ?.StartListening();

                serviceProvider
                    .GetService<UserProfileService>()
                    ?.StartListeningCurrentUser();
            });
        }
        else
        {
            // ❌ NO Shell when not logged in
            MainPage = new LoginPage();
        }
    }
}
