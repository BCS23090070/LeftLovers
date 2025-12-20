using LeftLovers.ViewModels;

namespace LeftLovers.Views;

public partial class PostFoodPage : ContentPage
{
    public PostFoodPage()
    {
        InitializeComponent();

        // ✅ Correct MAUI DI resolution
        BindingContext = Application.Current?
            .Handler?
            .MauiContext?
            .Services
            .GetService<PostFoodViewModel>();
    }
}
