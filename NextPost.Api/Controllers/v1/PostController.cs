using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextPost.Application.Constants;
using NextPost.Application.DTO_s;
using NextPost.Application.Dtos;
using NextPost.Application.Interfaces;
using NextPost.Application.Services;

namespace NextPost.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/post")]
    public class PostController(IPostService postService) : ControllerBase
    {
        private readonly IPostService _postService = postService;


        /// <summary>
        /// Retrieves an post by post id.
        /// </summary>
        /// <response code="200">Returns the post details.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">post is not found.</response>
        /// <response code="500">internal server error occurs.</response>
        /// 
        [HttpGet]
        [Route("by-id")]
        [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPostByIdAsync(int postId)
        {
            if(!ModelState.IsValid)
                throw new BadHttpRequestException("Invalid request parameters.");

            var post = await _postService.GetPostByIdAsync(postId);

            if(post is null)
                return NotFound($"Post with Id {postId} not found.");

            return Ok(post);
        }


        /// <summary>
        /// Adds a new post.
        /// </summary>
        /// <response code="200">Post added successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Resource not found.</response>
        /// <response code="500">Internal server error occurs.</response>
        [HttpPost]
        [Route("add-post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = UserRoles.Author)]
        public async Task<IActionResult> AddPostAsync(AddPostDto dto)
        {
            await _postService.AddNewPostAsync(dto);
            return Ok();
        }

        /// <summary>
        /// Updates an existing post.
        /// </summary>
        /// <response code="200">Post updated successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Post not found.</response>
        /// <response code="500">Internal server error occurs.</response>
        [HttpPatch]
        [Route("update-post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = UserRoles.Author)]
        public async Task<IActionResult> UpdatePostAsync(UpdatePostDto dto)
        {
            await _postService.UpdatePostAsync(dto);
            return Ok();
        }


        /// <summary>
        /// Deletes a post by post id.
        /// </summary>
        /// <response code="200">Post deleted successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Post not found.</response>
        /// <response code="500">Internal server error occurs.</response>
        [HttpDelete]
        [Route("delete-post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = UserRoles.Author)]
        public async Task<IActionResult> DeletePostAsync(int postId)
        {
            await _postService.DeletePostAsync(postId);
            return Ok();
        }


    }
}
