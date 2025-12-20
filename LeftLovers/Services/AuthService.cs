using Plugin.Firebase.Auth;
using Plugin.Firebase.Firestore;
using LeftLovers.Models;

namespace LeftLovers.Services;

public class AuthService
{
    private IFirebaseAuth Auth => CrossFirebaseAuth.Current;
    private IFirebaseFirestore Firestore => CrossFirebaseFirestore.Current;

    // 🔐 LOGIN
    public async Task<bool> LoginAsync(string email, string password)
    {
        await Auth.SignInWithEmailAndPasswordAsync(email, password);
        return Auth.CurrentUser != null;
    }

    // 🆕 REGISTER + SAVE USER PROFILE
    public async Task<bool> RegisterAsync(
        string email,
        string password,
        string username)
    {
        await Auth.CreateUserAsync(email, password);

        var user = Auth.CurrentUser;
        if (user == null)
            return false;

        // ✅ WRITE STRONGLY-TYPED USER PROFILE
        var profile = new UserProfile
        {
            Id = user.Uid,
            Username = username,
            Email = email,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await Firestore
            .GetCollection("users")
            .GetDocument(user.Uid)
            .SetDataAsync(profile);

        return true;
    }

    public bool IsLoggedIn()
    {
        return Auth.CurrentUser != null;
    }

    public async Task LogoutAsync()
    {
        await Auth.SignOutAsync();
    }
}
