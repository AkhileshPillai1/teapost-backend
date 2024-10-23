using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TeaPost.DTOs.Post;
using TeaPost.Interfaces;
using TeaPost.Models;

namespace TeaPost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly JwtSecurityTokenHandler _jwtHandler;
        public PostController(IPostService postService, JwtSecurityTokenHandler jwtHandler)
        {
            _postService = postService;
            _jwtHandler = jwtHandler;
        }

        [Authorize]
        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost(CreatePostDTO postObj)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(postObj.Caption) && string.IsNullOrWhiteSpace(postObj.ImageUrl))
                {
                    return BadRequest(new GenericResponse()
                    {
                        isSuccess = false,
                        message = "Please enter either image or caption!"
                    });
                }
                else
                {
                    return Ok(await _postService.CreatePost(postObj));
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
        [HttpPut("UpdatePostCaption")]
        public async Task<IActionResult> UpdatePostCaption([FromQuery] int postId, [FromBody] string caption)
        {
            try
            {
                return Ok(await _postService.UpdatePostCaption(postId, caption));
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
        [HttpGet("GetPostsForUser")]
        public IActionResult GetPostsForUser()
        {
            try
            {
                var authToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var jwtToken = _jwtHandler.ReadJwtToken(authToken);
                int id = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value);

                return Ok(_postService.GetPostsByUserId(id));
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
        [HttpGet("GetAllPosts")]
        public IActionResult GetAllPosts()
        {
            try
            {
                return Ok(_postService.GetAllPosts());
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
