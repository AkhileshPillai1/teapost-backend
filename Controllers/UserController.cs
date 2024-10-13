using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TeaPost.DatabaseConnection;
using TeaPost.Models;
using Microsoft.AspNetCore.Authorization;
using TeaPost.Utilities;
using TeaPost.Interfaces;
using TeaPost.DTOs.User;

namespace TeaPost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public UserController(ApplicationDBContext dbContext, IConfiguration configuration, IUserService userService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet("Login")]
        public IActionResult Login([FromQuery] string email, [FromQuery] string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || !email.validateEmail())
                {
                    return BadRequest(new GenericResponse()
                    {
                        isSuccess = false,
                        message = "Invalid Email!"
                    });
                }
                if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                {
                    return BadRequest(new GenericResponse()
                    {
                        isSuccess = false,
                        message = "Invalid Password!"
                    });
                }
                return Ok(_userService.Login(email, password));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse()
                {
                    isSuccess = false,
                    message = ex.Message
                });
            }
        }

        [Authorize]
        [HttpGet("GetUserDetails")]
        public IActionResult GetUserDetails()
        {
            try
            {
                var authToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(authToken);
                int id = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value);

                return Ok(_userService.GetUserDetails(id));
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

        [HttpPost("Register")]
        public IActionResult Register(User userObj)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userObj.UserName) || string.IsNullOrWhiteSpace(userObj.FirstName) || string.IsNullOrWhiteSpace(userObj.Email)
                    || string.IsNullOrWhiteSpace(userObj.PhoneNumber) || string.IsNullOrWhiteSpace(userObj.Pass))
                {
                    return BadRequest(new GenericResponse()
                    {
                        isSuccess = false,
                        message = "Please enter all mandatory fields!"
                    });
                }

                if (!userObj.Email.validateEmail())
                {
                    return BadRequest(new GenericResponse()
                    {
                        isSuccess = false,
                        message = "Invalid Email!"
                    });
                }
                if (userObj.Pass.Length < 8)
                {
                    return BadRequest(new GenericResponse()
                    {
                        isSuccess = false,
                        message = "Invalid Password!"
                    });
                }

                return Ok(_userService.Register(userObj));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse()
                {
                    isSuccess = false,
                    message = ex.Message
                });
            }
        }

        [Authorize]
        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser(UpdateUserPayload payload)
        {
            try
            {
                var authToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(authToken);
                int id = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value);
                return Ok(_userService.UpdateUser(id, payload));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse()
                {
                    isSuccess = false,
                    message = ex.Message
                });
            }
        }

        [Authorize]
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                return Ok(_userService.GetAllUsers());
            }
            catch (Exception ex) {
                return StatusCode(500, new GenericResponse() { 
                    isSuccess = false,
                    message = ex.Message
                });
            }
        }
        [Authorize]
        [HttpGet("AddOrRemoveFollower")]
        public async Task<IActionResult> AddOrRemoveFollower(int followed, bool isFollow = true)
        {
            try
            {
                var authToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(authToken);
                int follower = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value);
                if (isFollow)
                {
                    return Ok(await _userService.AddFollower(follower, followed));
                }
                else
                {
                    return Ok(await _userService.RemoveFollower(follower, followed));
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse()
                {
                    isSuccess = false,
                    message = ex.Message
                });
            }
        }
        [Authorize]
        [HttpGet("GetFollowers")]
        public IActionResult GetFollowers(int id) {
            try
            {
                var authToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(authToken);
                int follower = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value);
                return Ok(_userService.GetFollowers(follower));
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new GenericResponse() 
                { 
                    isSuccess = false,
                    message = ex.Message 
                });
            }
        }
    }
}
