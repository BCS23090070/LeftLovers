using LeftLovers.Models;
using LeftLovers.Services;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using System.Collections.ObjectModel;

namespace LeftLovers.ViewModels;

public class MapViewModel : BaseViewModel
{
    private readonly FoodPostService _foodService;

    public ObservableCollection<Pin> Pins { get; } = new();

    public MapViewModel(FoodPostService foodService)
    {
        _foodService = foodService;

        Title = "Food Map";

        // React when posts change
        _foodService.AllPosts.CollectionChanged += (_, __) =>
        {
            LoadPins();
        };

        LoadPins();
    }

    private void LoadPins()
    {
        Pins.Clear();

        foreach (var post in _foodService.AllPosts.Where(p => p.IsActive))
        {
            if (post.Latitude == 0 || post.Longitude == 0)
                continue;

            Pins.Add(new Pin
            {
                Label = post.Title,
                Address = post.PickupTime,
                Location = new Location(post.Latitude, post.Longitude),
                Type = PinType.Place
            });
        }
    }
}
