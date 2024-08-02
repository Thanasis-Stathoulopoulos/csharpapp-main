using CSharpApp.Application.Models;

namespace CSharpApp.Core.Interfaces;

public interface IPostService
{
    Task<ReadOnlyCollection<PostRecord>?> GetAllPosts();
    Task<PostRecord?> GetPostById(int id);
    Task<PostRecord> CreatePost(CreatePostModel createPostModel);
}