using Microsoft.AspNetCore.Mvc;
using TeaPost.DTOs.User;
using TeaPost.Models;

namespace TeaPost.Interfaces
{
    public interface IUserService
    {
        public GenericResponse Login(string email, string password);
        public GenericResponse Register(User userObj);
        public GenericResponse GetUserDetails(int id);
        public GenericResponse UpdateUser(int id, UpdateUserPayload payload);
        public GenericResponse GetAllUsers();
        public Task<GenericResponse> AddFollower(int follower, int followed);
        public Task<GenericResponse> RemoveFollower(int follower, int followed);
        public GenericResponse GetFollowers(int id);
    }
}
