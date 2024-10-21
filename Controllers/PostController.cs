using Microsoft.AspNetCore.Mvc;
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
        public PostController(IPostService postService) { 
            _postService = postService;
        }

        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost(CreatePostDTO postObj)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(postObj.Caption) && string.IsNullOrWhiteSpace(postObj.ImageUrl))
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
            catch (Exception ex) { 
                return StatusCode(500, new GenericResponse()
                {
                    isSuccess = false,
                    message = ex.Message
                });
            }
        }
    }
}
