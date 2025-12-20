using LeftLovers.Services;
using LeftLovers.ViewModels;
using LeftLovers.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Firestore;
using Plugin.Firebase.Storage;
using Microsoft.Maui.Controls.Maps;

#if ANDROID
using Plugin.Firebase.Core.Platforms.Android;
#endif

namespace LeftLovers;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiMaps()

            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // 🔥 Firebase initialization (Android-safe)
        builder.ConfigureLifecycleEvents(events =>
        {
#if ANDROID
            events.AddAndroid(android =>
                android.OnCreate((activity, _) =>
                    CrossFirebase.Initialize(activity)));
#endif
        });

        // 🔐 Firebase services
        builder.Services.AddSingleton<IFirebaseAuth>(_ => CrossFirebaseAuth.Current);
        builder.Services.AddSingleton<IFirebaseFirestore>(_ => CrossFirebaseFirestore.Current);
        builder.Services.AddSingleton<IFirebaseStorage>(_ => CrossFirebaseStorage.Current);

        // 🧠 App services
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<FoodPostService>();
        builder.Services.AddSingleton<ImageUploadService>();
        builder.Services.AddSingleton<UserProfileService>();

        // 📦 ViewModels
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<PostFoodViewModel>();
        builder.Services.AddTransient<MyPostsViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<MapViewModel>();

        // 📄 Pages (ONLY if you resolve via DI)
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<PostFoodPage>();
        builder.Services.AddTransient<MyPostsPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MapPage>();

        return builder.Build();
    }
}
