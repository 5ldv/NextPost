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
    [Route("api/v{version:apiVersion}/comment")]
    [Authorize]
    public class CommentController(ICommentService commentService) : ControllerBase
    {
        private readonly ICommentService _commentService = commentService;


        /// <summary>
        /// Adds a new comment.
        /// </summary>
        /// <response code="200">Comment added successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Resource not found.</response>
        /// <response code="500">Internal server error occurs.</response>
        [HttpPost]
        [Route("add-comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = UserRoles.Author)]
        public async Task<IActionResult> AddCommentAsync(AddCommentDto dto)
        {
            await _commentService.AddNewCommentAsync(dto);
            return Ok();
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <response code="200">Comment updated successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Comment not found.</response>
        /// <response code="500">Internal server error occurs.</response>
        [HttpPatch]
        [Route("update-comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = UserRoles.Author)]
        public async Task<IActionResult> UpdateCommentAsync(UpdateCommentDto dto)
        {
            await _commentService.UpdateCommentAsync(dto);
            return Ok();
        }

        /// <summary>
        /// Deletes a comment by comment id.
        /// </summary>
        /// <response code="200">Comment deleted successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Comment not found.</response>
        /// <response code="500">Internal server error occurs.</response>
        [HttpDelete]
        [Route("delete-comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = UserRoles.Author)]
        public async Task<IActionResult> DeleteCommentAsync(int commentId)
        {
            await _commentService.DeleteCommentAsync(commentId);
            return Ok();
        }

    }
}
