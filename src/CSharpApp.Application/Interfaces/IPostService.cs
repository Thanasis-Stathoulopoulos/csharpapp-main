using CSharpApp.Application.Models;

namespace CSharpApp.Core.Interfaces;

public interface IPostService
{
    Task<PostRecordResponseModel> GetAllPostsAsync();
    Task<PostRecordResponseModel> GetPostByIdAsync(int id);
    Task<PostRecordResponseModel> CreatePostAsync(CreatePostModel createPostModel);
    Task<PostRecordResponseModel> DeletePostAsync(int id);

}