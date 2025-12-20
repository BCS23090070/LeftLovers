using LeftLovers.Models;
using Plugin.Firebase.Firestore;
using Plugin.Firebase.Auth;
using System.Collections.ObjectModel;
using LeftLovers.Services;

namespace LeftLovers.Services;

public class FoodPostService
{
    private readonly IFirebaseFirestore _firestore;
    private readonly IFirebaseAuth _auth;

    private IDisposable? _listener;

    public ObservableCollection<FoodPost> AllPosts { get; } = new();
    public ObservableCollection<FoodPost> MyActivePosts { get; } = new();
    public ObservableCollection<FoodPost> MyPastPosts { get; } = new();

    private const string COLLECTION = "food_posts";

    public FoodPostService(
        IFirebaseFirestore firestore,
        IFirebaseAuth auth)
    {
        _firestore = firestore;
        _auth = auth;
    }

    // =========================
    // 🔥 START REAL-TIME LISTENER
    // =========================
    public void StartListening()
    {
        _listener?.Dispose();

        _listener = _firestore
            .GetCollection(COLLECTION)
            .AddSnapshotListener<FoodPost>(snapshot =>
            {
                if (snapshot == null)
                    return;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    AllPosts.Clear();

                    foreach (var doc in snapshot.Documents)
                    {
                        var post = doc.Data;          // ✅ Typed FoodPost
                        post.Id = doc.Reference.Id;  // ✅ Firestore ID
                        AllPosts.Add(post);
                    }

                    PostCacheService.Save(AllPosts.ToList());

                    SyncMyPosts();
                });
            });
    }


    // =========================
    // ➕ ADD POST
    // =========================
    public async Task AddPostAsync(FoodPost post)
    {
        if (post == null)
            return;

        post.CreatedAt = DateTimeOffset.UtcNow;
        post.IsActive = true;

        await _firestore
            .GetCollection(COLLECTION)
            .AddDocumentAsync(post);
        // ❌ DO NOT touch AllPosts here
    }

    // =========================
    // ❌ DEACTIVATE POST
    // =========================
    public async Task DeactivatePostAsync(FoodPost post)
    {
        if (post == null || string.IsNullOrEmpty(post.Id))
            return;

        await _firestore
            .GetCollection(COLLECTION)
            .GetDocument(post.Id)
            .UpdateDataAsync(new Dictionary<object, object>
            {
                { "isActive", false }
            });
        // ❌ DO NOT manually update collections
    }

    // =========================
    // 🔁 SYNC MY POSTS
    // =========================
    private void SyncMyPosts()
    {
        MyActivePosts.Clear();
        MyPastPosts.Clear();

        var currentUserId = _auth.CurrentUser?.Uid;
        if (string.IsNullOrEmpty(currentUserId))
            return;

        foreach (var post in AllPosts.Where(p => p.OwnerUserId == currentUserId))
        {
            if (post.IsActive)
                MyActivePosts.Add(post);
            else
                MyPastPosts.Add(post);
        }
    }
}
