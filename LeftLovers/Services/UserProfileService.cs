using Plugin.Firebase.Auth;
using Plugin.Firebase.Firestore;
using LeftLovers.Models;

namespace LeftLovers.Services;

public class UserProfileService
{
    private readonly IFirebaseFirestore _firestore;
    private readonly IFirebaseAuth _auth;
    private IDisposable? _listener;

    public UserProfile? CurrentProfile { get; private set; }

    private const string COLLECTION = "users";

    public UserProfileService(
        IFirebaseFirestore firestore,
        IFirebaseAuth auth)
    {
        _firestore = firestore;
        _auth = auth;
    }

    // =========================
    // 🔥 REAL-TIME LISTENER (CURRENT USER ONLY)
    // =========================
    public void StartListeningCurrentUser()
    {
        _listener?.Dispose();

        var userId = _auth.CurrentUser?.Uid;
        if (string.IsNullOrEmpty(userId))
            return;

        _listener = _firestore
            .GetCollection(COLLECTION)
            .GetDocument(userId)
            .AddSnapshotListener<UserProfile>(snapshot =>
            {
                if (snapshot?.Data == null)
                    return;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    snapshot.Data.Id = userId;
                    CurrentProfile = snapshot.Data;
                });
            });
    }

    public void StopListening()
    {
        _listener?.Dispose();
        _listener = null;
    }

    public string GetUsername()
    {
        return CurrentProfile?.Username ?? "Loading...";
    }
}
