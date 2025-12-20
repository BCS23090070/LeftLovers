using LeftLovers.Models;
using LeftLovers.Services;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Storage;
using Plugin.Firebase.Auth;
using System.Windows.Input;

namespace LeftLovers.ViewModels;

public class PostFoodViewModel : BaseViewModel
{
    private readonly FoodPostService _service;
    private readonly ImageUploadService _imageService;
    private readonly UserProfileService _profileService;
    private readonly IFirebaseAuth _auth;

    private string _titleText;
    private string _description;
    private string _pickupTime;
    private string _imagePath;

    public string TitleText
    {
        get => _titleText;
        set { _titleText = value; OnPropertyChanged(); }
    }

    public string Description
    {
        get => _description;
        set { _description = value; OnPropertyChanged(); }
    }

    public string PickupTime
    {
        get => _pickupTime;
        set { _pickupTime = value; OnPropertyChanged(); }
    }

    public string ImagePath
    {
        get => _imagePath;
        set { _imagePath = value; OnPropertyChanged(); }
    }

    public ICommand PickImageCommand { get; }
    public ICommand SubmitCommand { get; }

    public PostFoodViewModel(
        FoodPostService service,
        ImageUploadService imageService,
        UserProfileService profileService,
        IFirebaseAuth auth)
    {
        _service = service;
        _imageService = imageService;
        _profileService = profileService;
        _auth = auth;

        PickImageCommand = new Command(async () => await PickImageAsync());
        SubmitCommand = new Command(async () => await SubmitAsync());
    }

    // 📸 Gallery only
    private async Task PickImageAsync()
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Select food image",
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
            ImagePath = result.FullPath;
    }

    // ➕ Submit food post
    private async Task SubmitAsync()
    {
        if (IsBusy || string.IsNullOrWhiteSpace(TitleText))
            return;

        var user = _auth.CurrentUser;
        if (user == null)
            return;

        var profile = _profileService.CurrentProfile;
        if (profile == null)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                "User profile not loaded yet. Please try again.",
                "OK");
            return;
        }

        try
        {
            IsBusy = true;

            // 📍 Get location
            var location = await Geolocation.GetLastKnownLocationAsync()
                           ?? await Geolocation.GetLocationAsync(
                               new GeolocationRequest(
                                   GeolocationAccuracy.Medium,
                                   TimeSpan.FromSeconds(10)));

            if (location == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Location error",
                    "Unable to get your location. Please enable GPS.",
                    "OK");
                return;
            }

            // ☁️ Upload image (optional)
            string imageUrl = "food_placeholder.png";

            if (!string.IsNullOrEmpty(ImagePath))
            {
                var uploadedUrl = await _imageService.UploadImageAsync(
                    ImagePath,
                    user.Uid);

                if (!string.IsNullOrEmpty(uploadedUrl))
                    imageUrl = uploadedUrl;
            }

            await _service.AddPostAsync(new FoodPost
            {
                Title = TitleText,
                Description = Description,
                PickupTime = PickupTime,
                Image = imageUrl,
                OwnerUserId = user.Uid,
                OwnerUsername = profile.Username,
                IsActive = true,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                CreatedAt = DateTimeOffset.UtcNow
            });

            // ✅ E6: ASYNC FEEDBACK ON POST PAGE
            await Application.Current.MainPage.DisplayAlert(
                "Success",
                "Food posted successfully 🎉",
                "OK");

            // Clear form
            TitleText = string.Empty;
            Description = string.Empty;
            PickupTime = string.Empty;
            ImagePath = string.Empty;

        }
        finally
        {
            IsBusy = false;
        }

        // ✅ Return to Dashboard tab
        Shell.Current.CurrentItem = Shell.Current.Items[0];
    }
}


