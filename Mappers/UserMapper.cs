using TeaPost.DTOs.User;
using TeaPost.Models;

namespace TeaPost.Mappers
{
    public static class UserMapper
    {
        public static UserResponseDTO ToUserResponse(this User user)
        {
            return new UserResponseDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
        }
    }
}
