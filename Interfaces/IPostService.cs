using TeaPost.DTOs.Post;
using TeaPost.Models;

namespace TeaPost.Interfaces
{
    public interface IPostService
    {
        public Task<GenericResponse> CreatePost(CreatePostDTO payload);
    }
}
