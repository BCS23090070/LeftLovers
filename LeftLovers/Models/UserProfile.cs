using Plugin.Firebase.Firestore;

namespace LeftLovers.Models;

public class UserProfile : IFirestoreObject
{
    [FirestoreDocumentId]
    public string Id { get; set; }   // ✅ this IS the userId

    [FirestoreProperty("username")]
    public string Username { get; set; }

    [FirestoreProperty("email")]
    public string Email { get; set; }

    [FirestoreProperty("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }
}
