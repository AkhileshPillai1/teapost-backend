using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeaPost.DatabaseConnection;
using TeaPost.DTOs.User;
using TeaPost.Interfaces;
using TeaPost.Mappers;
using TeaPost.Models;

namespace TeaPost.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _jwtHandler;
        public UserService(ApplicationDBContext dBContext, IConfiguration configuration, JwtSecurityTokenHandler jwtHandler)
        {
            _dbContext = dBContext;
            _configuration = configuration;
            _jwtHandler = jwtHandler;
        }
        public GenericResponse Login(string email, string password)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == email && u.Pass == password);
                if (user == null)
                {
                    return new GenericResponse()
                    {
                        isSuccess = false,
                        message = "Invalid credentials!"
                    };
                }
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"] ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId",user.Id.ToString()),
                new Claim("Email",user.Email)
            };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, null, DateTime.UtcNow.AddMinutes(240), signIn);
                string tokenStr = _jwtHandler.WriteToken(token);
                return new GenericResponse()
                {
                    data = new
                    {
                        token = tokenStr,
                        user = user.ToUserResponse()
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
        public GenericResponse Register(User userObj)
        {
            try {

                var duplicateUser = _dbContext.Users.FirstOrDefault(user => user.Email == userObj.Email || user.UserName == userObj.UserName || user.PhoneNumber == userObj.PhoneNumber);
                if(duplicateUser != null)
                {
                    StringBuilder duplicateFields = new StringBuilder();
                    if(duplicateUser.Email == userObj.Email)
                    {
                        duplicateFields.Append("Email, ");
                    }
                    if (duplicateUser.UserName == userObj.UserName)
                    {
                        duplicateFields.Append("Username, ");
                    }
                    if (duplicateUser.PhoneNumber == userObj.PhoneNumber)
                    {
                        duplicateFields.Append("Phone number, ");
                    }
                    duplicateFields.Remove(duplicateFields.Length-2,2);
                    return new GenericResponse()
                    {
                        isSuccess = false,
                        message = $"A user with this {duplicateFields} already exists!"
                    };
                }

                _dbContext.Users.Add(userObj);
                int res = _dbContext.SaveChanges();
                if(res != 1)
                {
                    return new GenericResponse()
                    {
                        isSuccess = false,
                        message = "Something went Wrong"
                    };
                }
                return new GenericResponse()
                {
                    message = "User Registered succesfully!"
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
        public GenericResponse GetUserDetails(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null) {
                return new GenericResponse()
                {
                    isSuccess = false,
                    message = "User doesn't exist!"
                };
            }
            return new GenericResponse()
            {
                data = user.ToUserResponse()
            };
        }
        public GenericResponse UpdateUser(int id, UpdateUserPayload payload)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null)
            {
                return new GenericResponse()
                {
                    isSuccess = false,
                    message = "User doesn't exist!"
                };
            }
            user.FirstName = payload.FirstName;
            user.LastName = payload.LastName;
            user.Email = payload.Email;
            user.UserName = payload.UserName;
            user.PhoneNumber = payload.PhoneNumber;
            _dbContext.SaveChangesAsync();
            return new GenericResponse()
            {
                message = "User updated successfully"
            };
        }
        public GenericResponse GetAllUsers() {
            return new GenericResponse()
            {
                data = _dbContext.Users.ToList().Select(x => x.ToUserResponse())
            };
        }

        public async Task<GenericResponse> AddFollower(int follower, int followed)
        {
            var followerRelationship = _dbContext.Followers.FirstOrDefault(fol => fol.Follower == follower && fol.Followed == followed);
            if (followerRelationship != null)
            {
                return new GenericResponse()
                {
                    isSuccess = false,
                    message = "User already follows this account!"
                };
            }
            _dbContext.Followers.Add(new Followers()
            {
                Follower = follower,
                Followed = followed,
                FollowedAt = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();
            return new GenericResponse()
            {
                message = "Follower added!"
            };
        }

        public async Task<GenericResponse> RemoveFollower(int follower, int followed)
        {
            var followerRelationship = _dbContext.Followers.FirstOrDefault(fol => fol.Follower == follower && fol.Followed == followed);
            if(followerRelationship == null)
            {
                return new GenericResponse()
                {
                    isSuccess = false,
                    message = "User doesn't follow the requested account!"
                };
            }
            _dbContext.Followers.Remove(followerRelationship);
            await _dbContext.SaveChangesAsync();
            return new GenericResponse()
            {
                message = "Unfollowed successfully"
            };
        }
        public GenericResponse GetFollowers(int id)
        {
            var followers = _dbContext.Followers.Where(fol => fol.Followed == id).Select(obj => obj.Follower).ToList();
            var users = _dbContext.Users.Where(user => followers.Contains(user.Id)).Select(obj => obj.ToUserResponse()).ToList();
            if(users.Count > 0)
            {
                return new GenericResponse()
                {
                    data = users
                };
            }
            else
            {
                return new GenericResponse()
                {
                };
            }
        }
    }
}
