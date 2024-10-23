using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TeaPost.DatabaseConnection;
using TeaPost.DTOs.Comment;
using TeaPost.Interfaces;
using TeaPost.Models;

namespace TeaPost.Services
{
    public class LikeCommentService:ILikeCommentService
    {
        private readonly ApplicationDBContext _dbContext;
        public LikeCommentService(ApplicationDBContext dBContext) 
        {
            _dbContext = dBContext;
        }
        public async Task<GenericResponse> AddLike(int userId, int postId)
        {
            try
            {
                var post = _dbContext.Posts.Find(postId);
                if (post == null)
                {
                    return new GenericResponse()
                    {
                        isSuccess = false,
                        message = "No post found with this id!"
                    };
                }
                var existingLike = _dbContext.Likes.FirstOrDefault(like => like.PostId == postId && like.UserId == userId);
                if (existingLike != null)
                {
                    return new GenericResponse()
                    {
                        isSuccess = false,
                        message = "User already likes this post!"
                    };
                }
                //using raw sql as EF doesn't support tracking/insertion/modification without primary key
                var sql = "INSERT INTO Likes (userId, postId) VALUES (@userId, @postId)";
                await _dbContext.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@userId", userId), new SqlParameter("@postId", postId));
                post.Likes++;
                await _dbContext.SaveChangesAsync();

                return new GenericResponse()
                {
                    message = "Like added successfully!"
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse()
                {
                    isSuccess = false,
                    message = ex.Message
                };
            }
        }
        public async Task<GenericResponse> RemoveLike(int userId, int postId)
        {
            try
            {
                var post = _dbContext.Posts.Find(postId);
                if (post == null)
                {
                    return new GenericResponse()
                    {
                        isSuccess = false,
                        message = "No post found with this id!"
                    };
                }
                var existingLike = _dbContext.Likes.FirstOrDefault(like => like.PostId == postId && like.UserId == userId);
                if (existingLike == null)
                {
                    return new GenericResponse()
                    {
                        isSuccess = false,
                        message = "User doesn't like this post!"
                    };
                }
                //using raw sql as EF doesn't support tracking/insertion/modification without primary key
                var sql = "DELETE FROM Likes WHERE postId = @postId AND userId = @userId";
                await _dbContext.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@postId", postId), new SqlParameter("@userId", userId));

                post.Likes--;
                await _dbContext.SaveChangesAsync();
                return new GenericResponse()
                {
                    message = "Like removed successfully!"
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse()
                {
                    isSuccess = false,
                    message = ex.Message
                };
            }
        }
        public async Task<GenericResponse> CreateComment(int userId, CreateCommentDTO comment)
        {
            try
            {
                var post = _dbContext.Posts.Find(comment.PostId);
                if (post == null)
                {
                    return new GenericResponse()
                    {
                        isSuccess = false,
                        message = "No post found with this id!"
                    };
                }
                var newComment = new Comment()
                {
                    AuthorId = userId,
                    PostId = comment.PostId,
                    Text = comment.Text,
                };
                _dbContext.Comments.Add(newComment);
                post.Comments++;
                await _dbContext.SaveChangesAsync();
                return new GenericResponse()
                {
                    message = "Comment created successfully",
                    data = new
                    {
                        commentId = newComment.CommentId
                    }
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse()
                {
                    isSuccess = false,
                    message = ex.Message
                };
            }
        }
        public async Task<GenericResponse> DeleteComment(int commentId)
        {
            try
            {
                var comment = _dbContext.Comments.Find(commentId);
                if (comment == null)
                {
                    return new GenericResponse()
                    {
                        isSuccess = false,
                        message = "No comment found with this id!"
                    };
                }
                _dbContext.Comments.Remove(comment);
                var post = _dbContext.Posts.Find(comment.PostId);
                if(post != null)
                {
                    post.Comments--;
                }
                await _dbContext.SaveChangesAsync();
                return new GenericResponse()
                {
                    message = "Commented deleted!"
                };
            }
            catch(Exception ex)
            {
                return new GenericResponse()
                {
                    isSuccess = false,
                    message = ex.Message
                };
            }
        }
    }
}
