using TeaPost.DTOs.Post;
using TeaPost.Models;

namespace TeaPost.Interfaces
{
    public interface IPostService
    {
        public Task<GenericResponse> CreatePost(CreatePostDTO payload);
        public Task<GenericResponse> UpdatePostCaption(int id, string caption);
        public GenericResponse GetPostsByUserId(int userId);
        public GenericResponse GetAllPosts();
    }
}
