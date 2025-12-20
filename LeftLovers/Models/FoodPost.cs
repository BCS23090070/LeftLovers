using Plugin.Firebase.Firestore;

namespace LeftLovers.Models;

public class FoodPost : IFirestoreObject
{
    // 🔑 Firestore document ID
    [FirestoreDocumentId]
    public string Id { get; set; }

    // =========================
    // 📄 POST DATA
    // =========================
    [FirestoreProperty("title")]
    public string Title { get; set; }

    [FirestoreProperty("description")]
    public string Description { get; set; }

    [FirestoreProperty("pickupTime")]
    public string PickupTime { get; set; }

    [FirestoreProperty("image")]
    public string Image { get; set; } // old Image Database, i just keep it here incase i wanna do something 

    [FirestoreProperty("distance")]
    public string Distance { get; set; }

    // =========================
    // 👤 OWNERSHIP
    // =========================
    [FirestoreProperty("ownerUserId")]
    public string OwnerUserId { get; set; }

    // =========================
    // 🔄 STATUS
    // =========================
    [FirestoreProperty("isActive")]
    public bool IsActive { get; set; } = true;

    // =========================
    // ⏱ TIMESTAMP 
    // =========================
    [FirestoreProperty("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }

    // =========================
    // Image Database
    // =========================
    [FirestoreProperty("imageUrl")]
    public string ImageUrl { get; set; }

    // 🗺️ MAP
    [FirestoreProperty("latitude")]
    public double Latitude { get; set; }

    [FirestoreProperty("longitude")]
    public double Longitude { get; set; }

    [FirestoreProperty("ownerUsername")]
    public string OwnerUsername { get; set; }


}
