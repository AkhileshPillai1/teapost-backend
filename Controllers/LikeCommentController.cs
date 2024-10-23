using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using TeaPost.Interfaces;
using TeaPost.Models;

namespace TeaPost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikeCommentController: ControllerBase
    {
        private readonly JwtSecurityTokenHandler _jwtHandler;
        private readonly ILikeCommentService _likeCommentService;
        public LikeCommentController(JwtSecurityTokenHandler jwtHandler, ILikeCommentService likeCommentService)
        {
            _jwtHandler = jwtHandler;
            _likeCommentService = likeCommentService;
        }

        [Authorize]
        [HttpPost("AddOrRemoveLike")]
        public async Task<IActionResult> AddOrRemoveLike([FromBody] JObject payload)
        {
            try
            {
                var authToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var jwtToken = _jwtHandler.ReadJwtToken(authToken);
                int id = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value);

                if (Convert.ToBoolean(payload["isLike"]))
                {
                    return Ok(await _likeCommentService.AddLike(id, Convert.ToInt32(payload["postId"])));
                }
                else
                {
                    return Ok(await _likeCommentService.RemoveLike(id, Convert.ToInt32(payload["postId"])));
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
    }
}
