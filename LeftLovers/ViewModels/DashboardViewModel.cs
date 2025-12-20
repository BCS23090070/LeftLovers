using LeftLovers.Models;
using LeftLovers.Services;
using System.Collections.ObjectModel;

namespace LeftLovers.ViewModels;

public class DashboardViewModel : BaseViewModel
{
    private readonly FoodPostService _foodService;

    // ✅ Dashboard only shows ACTIVE posts
    public ObservableCollection<FoodPost> FoodList { get; } = new();

    public DashboardViewModel(FoodPostService foodService)
    {
        _foodService = foodService;
        Title = "Dashboard";

        // =========================
        // ✅ E5: LOAD CACHED POSTS FIRST (OFFLINE)
        // =========================
        var cachedPosts = PostCacheService.Load();

        foreach (var post in cachedPosts.Where(p => p.IsActive))
        {
            FoodList.Add(post);
        }

        // =========================
        // 🔥 START FIRESTORE LISTENER
        // =========================
        _foodService.StartListening();

        // 🔁 React to Firestore updates
        _foodService.AllPosts.CollectionChanged += (_, __) =>
        {
            RefreshActivePosts();
        };
    }

    // =========================
    // ✅ FILTER ACTIVE POSTS
    // =========================
    private void RefreshActivePosts()
    {
        FoodList.Clear();

        foreach (var post in _foodService.AllPosts.Where(p => p.IsActive))
        {
            FoodList.Add(post);
        }
    }
}
