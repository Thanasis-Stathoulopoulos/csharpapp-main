namespace CSharpApp.Core.Interfaces;

public interface IPostService
{
    Task<ReadOnlyCollection<PostRecord>?> GetAllPosts();
}