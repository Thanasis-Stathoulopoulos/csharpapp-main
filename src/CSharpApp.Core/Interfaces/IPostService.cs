namespace CSharpApp.Core.Interfaces;

public interface IPostService
{
    Task<ReadOnlyCollection<PostRecord>?> GetAllPosts();
    Task<PostRecord?> GetPostById(int id);
}