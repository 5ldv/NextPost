using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextPost.Application.Dtos;
using NextPost.Application.Interfaces;

namespace NextPost.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/author")]
    [Authorize]
    public class AuthorController(IAuthorService authorService) : ControllerBase
    {
        private readonly IAuthorService _authorService = authorService;

        /// <summary>
        /// Retrieves an author by their unique identifier.
        /// </summary>
        /// <response code="200">Returns the author details.</response>
        /// <response code="404">author is not found.</response>
        /// <response code="500">internal server error occurs.</response>
        [HttpGet]
        [Route("by-id")]
        [ProducesResponseType(typeof(AuthorDto),  StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuthorByIdAsync(int authorId)
        {
            if(!ModelState.IsValid)
                throw new BadHttpRequestException("Invalid request parameters.");

            var author = await _authorService.GetAuthorByIdAsync(authorId);

            if(author is null)
                return NotFound($"Author with Id {authorId} not found.");

            return Ok(author);
        }
        /// <summary>
        /// Retrieves an author by their username.
        /// </summary>
        /// <response code="200">Returns the author details.</response>
        /// <response code="404">Author is not found.</response>
        /// <response code="500">Internal server error occurs.</response>
        [HttpGet]
        [Route("by-username")]
        [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuthorByUsernameAsync(string username)
        {
            if(!ModelState.IsValid)
                throw new BadHttpRequestException("Invalid request parameters.");

            var author = await _authorService.GetAuthorByUsernameAsync(username);

            if(author is null)
                return NotFound($"Author with username {username} not found.");

            return Ok(author);
        }


        /// <summary>
        /// Updates an existing author's information.
        /// </summary>
        /// <param name="dto">The updated author information.</param>
        /// <response code="200">Author updated successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="500">Internal server error occurs.</response>
        [HttpPatch]
        [Route("update-author")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAuthorAsync(UpdateAuthorDto dto)
        {
            await _authorService.UpdateAuthorAsync(dto);
            return Ok();
        }


    }
}
