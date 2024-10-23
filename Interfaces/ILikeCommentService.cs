using TeaPost.Models;

namespace TeaPost.Interfaces
{
    public interface ILikeCommentService
    {
        public Task<GenericResponse> AddLike(int userId, int postId);
        public Task<GenericResponse> RemoveLike(int userId, int postId);
    }
}
