using LeftLovers.Services;
using LeftLovers.ViewModels;

namespace LeftLovers.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();

        // ✅ PASS AuthService
        BindingContext = new RegisterViewModel(new AuthService());
    }
}
