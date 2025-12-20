using Plugin.Firebase.Auth;
using System.Windows.Input;
using LeftLovers.Services;
using LeftLovers.Views;

namespace LeftLovers.ViewModels;

public class ProfileViewModel : BaseViewModel
{
    private readonly IFirebaseAuth _auth;
    private readonly UserProfileService _userService;

    private string _username = "Loading...";
    private string _email = "-";

    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(); }
    }

    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); }
    }

    public ICommand LogoutCommand { get; }

    public ProfileViewModel(
        IFirebaseAuth auth,
        UserProfileService userService)
    {
        _auth = auth;
        _userService = userService;

        Title = "Profile";
        LogoutCommand = new Command(async () => await LogoutAsync());

        Initialize();
    }

    private void Initialize()
    {
        var user = _auth.CurrentUser;
        if (user == null)
            return;

        Email = user.Email ?? "-";

        _userService.StartListeningCurrentUser();

        Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
        {
            Username = _userService.GetUsername();
            return _userService.CurrentProfile == null;
        });
    }

    private async Task LogoutAsync()
    {
        _userService.StopListening();
        await _auth.SignOutAsync();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Application.Current.MainPage = new LoginPage();
        });
    }
}
