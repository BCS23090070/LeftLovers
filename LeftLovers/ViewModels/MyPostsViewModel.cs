using LeftLovers.Models;
using LeftLovers.Services;
using Plugin.Firebase.Auth;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LeftLovers.ViewModels;

public class MyPostsViewModel : BaseViewModel
{
    private readonly FoodPostService _service;
    private readonly IFirebaseAuth _auth;

    public ObservableCollection<FoodPost> ActivePosts { get; } = new();
    public ObservableCollection<FoodPost> PastPosts { get; } = new();

    public ICommand DeactivateCommand { get; }

    public MyPostsViewModel(
        FoodPostService service,
        IFirebaseAuth auth)
    {
        _service = service;
        _auth = auth;

        Title = "My Posts";

        DeactivateCommand = new Command<FoodPost>(async post =>
        {
            await _service.DeactivatePostAsync(post);
            Refresh();
        });

        _service.AllPosts.CollectionChanged += (_, __) => Refresh();
        Refresh();
    }

    private void Refresh()
    {
        ActivePosts.Clear();
        PastPosts.Clear();

        var currentUserId = _auth.CurrentUser?.Uid;
        if (string.IsNullOrEmpty(currentUserId))
            return;

        foreach (var post in _service.AllPosts
                     .Where(p => p.OwnerUserId == currentUserId))
        {
            if (post.IsActive)
                ActivePosts.Add(post);
            else
                PastPosts.Add(post);
        }
    }
}
