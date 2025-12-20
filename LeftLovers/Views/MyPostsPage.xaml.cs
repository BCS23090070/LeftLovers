using LeftLovers.ViewModels;

namespace LeftLovers.Views;

public partial class MyPostsPage : ContentPage
{
    public MyPostsPage()
    {
        InitializeComponent();

        BindingContext = App.Services.GetRequiredService<MyPostsViewModel>();
    }
}
