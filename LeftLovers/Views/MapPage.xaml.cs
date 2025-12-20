using LeftLovers.ViewModels;

namespace LeftLovers.Views;

public partial class MapPage : ContentPage
{
    public MapPage(MapViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
