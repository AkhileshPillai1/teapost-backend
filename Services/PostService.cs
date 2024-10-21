﻿using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using TeaPost.DatabaseConnection;
using TeaPost.DTOs.Post;
using TeaPost.Interfaces;
using TeaPost.Models;

namespace TeaPost.Services
{
    public class PostService : IPostService
    {

        private readonly ApplicationDBContext _dbContext;
        private readonly JwtSecurityTokenHandler _jwtHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PostService(ApplicationDBContext dBContext, JwtSecurityTokenHandler jwtHandler, IHttpContextAccessor httpContextAccessor) { 
            _dbContext  = dBContext;
            _jwtHandler = jwtHandler;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<GenericResponse> CreatePost(CreatePostDTO payload)
        {
            try
            {
                //Extracting user id from JWT
                var authToken = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var jwtToken = _jwtHandler.ReadJwtToken(authToken);
                int id = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value);

                Post newPost = new Post()
                {
                    AuthorId = id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ImageUrl = payload.ImageUrl,
                    Caption = payload.Caption,
                    Likes = 0,
                    Comments = 0
                };

                _dbContext.Posts.Add(newPost);
                await _dbContext.SaveChangesAsync();

                return new GenericResponse()
                {
                    message = "Post created successfully!",
                    data = new
                    {
                        PostId = newPost.Id
                    }
                };
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database-related exceptions like constraint violations, etc.
                return new GenericResponse()
                {
                    isSuccess = false,
                    message = "Error while performing database action!",
                    detailedMessage = dbEx.Message
                };
            }
            catch (Exception ex) {
                return new GenericResponse()
                {
                    isSuccess = false,
                    message = ex.Message
                };
            }
        }
    }
}
