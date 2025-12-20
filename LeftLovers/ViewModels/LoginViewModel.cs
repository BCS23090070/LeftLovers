using System.Windows.Input;
using Plugin.Firebase.Auth;
using LeftLovers.Views;

namespace LeftLovers.ViewModels;

public class LoginViewModel : BaseViewModel
{
    public string Email { get; set; }
    public string Password { get; set; }

    public ICommand LoginCommand { get; }
    public ICommand GoToRegisterCommand { get; }

    public LoginViewModel()
    {
        LoginCommand = new Command(async () => await LoginAsync());

        // ✅ FIXED: NO SHELL HERE
        GoToRegisterCommand = new Command(() =>
        {
            Application.Current.MainPage = new RegisterPage();
        });
    }

    private async Task LoginAsync()
    {
        try
        {
            IsBusy = true;

            await CrossFirebaseAuth.Current
                .SignInWithEmailAndPasswordAsync(Email, Password);

            // ✅ HARD RESET INTO APP SHELL
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage = new AppShell();
            });
        }
        catch
        {
            await Application.Current.MainPage.DisplayAlert(
                "Login Failed",
                "Invalid email or password",
                "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}