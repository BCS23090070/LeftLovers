using Android.App;
using Android.Content.PM;
using Android.OS;
using Firebase;
using Firebase.Crashlytics;
using Plugin.Firebase.Core.Platforms.Android;

namespace LeftLovers;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize |
                           ConfigChanges.Orientation |
                           ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout |
                           ConfigChanges.SmallestScreenSize)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // 🔥 1️⃣ Disable Crashlytics BEFORE anything else
        FirebaseCrashlytics.Instance.SetCrashlyticsCollectionEnabled(false);

        // 🔥 2️⃣ Initialize Firebase (required by Plugin.Firebase)
        CrossFirebase.Initialize(this);


    }
}
