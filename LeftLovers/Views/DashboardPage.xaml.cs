using LeftLovers.ViewModels;   // ✅ ADD THIS

namespace LeftLovers.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashboardViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
