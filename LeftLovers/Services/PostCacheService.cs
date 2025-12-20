using Microsoft.Maui.Storage;
using System.Text.Json;
using LeftLovers.Models;

namespace LeftLovers.Services;

public static class PostCacheService
{
    private const string KEY = "cached_posts";

    public static void Save(List<FoodPost> posts)
    {
        var json = JsonSerializer.Serialize(posts);
        Preferences.Set(KEY, json);
    }

    public static List<FoodPost> Load()
    {
        if (!Preferences.ContainsKey(KEY))
            return new();

        var json = Preferences.Get(KEY, string.Empty);
        return string.IsNullOrEmpty(json)
            ? new()
            : JsonSerializer.Deserialize<List<FoodPost>>(json) ?? new();
    }
}
