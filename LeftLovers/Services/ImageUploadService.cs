using Plugin.Firebase.Storage;

namespace LeftLovers.Services;

public class ImageUploadService
{
    private readonly IFirebaseStorage _storage;

    public ImageUploadService(IFirebaseStorage storage)
    {
        _storage = storage;
    }

    public async Task<string?> UploadImageAsync(string localPath, string userId)
    {
        if (string.IsNullOrEmpty(localPath) || string.IsNullOrEmpty(userId))
            return null;

        var fileName = $"{Guid.NewGuid()}.jpg";

        using var stream = File.OpenRead(localPath);

        var reference = _storage
            .GetRootReference()
            .GetChild("food_images")
            .GetChild(userId)
            .GetChild(fileName);

        // ✅ CORRECT upload method
        var uploadTask = reference.PutStream(stream);
        await uploadTask.AwaitAsync();

        // ✅ Get public download URL
        return await reference.GetDownloadUrlAsync();
    }
}
