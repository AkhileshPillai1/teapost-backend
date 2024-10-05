using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeaPost.DatabaseConnection;
using TeaPost.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace TeaPost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IConfiguration _configuration;
        public UserController(ApplicationDBContext dbContext, IConfiguration configuration) {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpGet("Login")]
        public IActionResult Login([FromQuery]string email, [FromQuery]string password)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == email && u.Pass == password);
                if (user == null)
                {
                    return BadRequest(new GenericResponse()
                    {
                        isSuccess = false,
                        message = "Invalid credentials!"
                    });
                }
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"] ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId",user.Id.ToString()),
                    new Claim("Email",user.Email)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
                var signIn = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, null, DateTime.UtcNow.AddMinutes(10), signIn);
                string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new GenericResponse()
                {
                    data = new
                    {
                        token = tokenStr,
                        user = user
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new GenericResponse()
                {
                    isSuccess = false,
                    message = ex.Message
                });
            }
        }
        [Authorize]
        [HttpGet("GetUserDetails")]
        public IActionResult GetUserDetails([FromQuery] int id)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);
                if (user == null)
                {
                    return BadRequest(new GenericResponse()
                    {
                        isSuccess = false,
                        message = "User doesn't exist!"
                    });
                }
                return Ok(new GenericResponse()
                {
                    data = user
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new GenericResponse()
                {
                    isSuccess = false,
                    message = ex.Message
                });
            }
        }
    }
}
