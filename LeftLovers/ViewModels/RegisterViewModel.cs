using System.Text.RegularExpressions;
using System.Windows.Input;
using LeftLovers.Services;
using LeftLovers.Views;

namespace LeftLovers.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    private readonly AuthService _authService;

    private string _username;
    private string _email;
    private string _password;
    private string _confirmPassword;

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
            ValidateForm();
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
            ValidateForm();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
            ValidateForm();
        }
    }

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            OnPropertyChanged();
            ValidateForm();
        }
    }

    // 🔴 Validation messages
    public string UsernameError { get; private set; }
    public string EmailError { get; private set; }
    public string PasswordError { get; private set; }

    public bool IsFormValid { get; private set; }

    public ICommand RegisterCommand { get; }
    public ICommand BackCommand { get; }

    public RegisterViewModel(AuthService authService)
    {
        _authService = authService;

        RegisterCommand = new Command(async () => await RegisterAsync(), () => IsFormValid);
        BackCommand = new Command(() =>
        {
            Application.Current.MainPage = new LoginPage();
        });
    }

    // =========================
    // 🔍 VALIDATION LOGIC
    // =========================
    private void ValidateForm()
    {
        UsernameError = string.IsNullOrWhiteSpace(Username)
            ? "Username is required"
            : string.Empty;

        EmailError = string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, @"^\S+@\S+\.\S+$")
            ? "Enter a valid email"
            : string.Empty;

        PasswordError = Password?.Length < 6
            ? "Password must be at least 6 characters"
            : Password != ConfirmPassword
                ? "Passwords do not match"
                : string.Empty;

        IsFormValid =
            string.IsNullOrEmpty(UsernameError) &&
            string.IsNullOrEmpty(EmailError) &&
            string.IsNullOrEmpty(PasswordError);

        OnPropertyChanged(nameof(UsernameError));
        OnPropertyChanged(nameof(EmailError));
        OnPropertyChanged(nameof(PasswordError));
        OnPropertyChanged(nameof(IsFormValid));

        ((Command)RegisterCommand).ChangeCanExecute();
    }

    // =========================
    // 🆕 REGISTER
    // =========================
    private async Task RegisterAsync()
    {
        if (!IsFormValid)
            return;

        var success = await _authService.RegisterAsync(
            Email, Password, Username);

        if (success)
        {
            Application.Current.MainPage = new LoginPage();
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                "Registration failed",
                "OK");
        }
    }
}
