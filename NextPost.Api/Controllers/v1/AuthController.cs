using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextPost.Api.Helpers;
using NextPost.Application.Dtos;
using NextPost.Application.Interfaces;

namespace NextPost.Api.Controllers.v1
{
    /// <summary>
    /// Handles user authentication operations including registration, login, token refresh, and revocation.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    [Authorize]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        /// <summary>
        /// Registers a new user and returns an authentication response.
        /// </summary>
        /// <response code="200">Registration successful.</response>
        /// <response code="400">Invalid data or user already exists.</response>
        /// <response code="500">Server error during registration.</response>
        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            _logger.LogInformation("Register endpoint called for user: {Username}", registerDto.Username);

            var authResponse = await _authService.RegisterAsync(registerDto);

            if(authResponse.RefreshToken != null && authResponse.RefreshTokenExpiration.HasValue)
            {
                CookieHelper.SetRefreshToken(Response, authResponse.RefreshToken,
                    authResponse.RefreshTokenExpiration.Value);
            }

            _logger.LogInformation("User registered successfully: {Username}", registerDto.Username);
            return Ok(authResponse);
        }

        /// <summary>
        /// Logs in an existing user and returns an authentication response.
        /// </summary>
        /// <response code="200">Login successful.</response>
        /// <response code="400">Invalid credentials or user not found.</response>
        /// <response code="500">Server error during login.</response>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            _logger.LogInformation("Login endpoint called for user: {Username}", loginDto.Username);

            var authResponse = await _authService.LoginAsync(loginDto);

            if(authResponse.RefreshToken != null && authResponse.RefreshTokenExpiration.HasValue)
            {
                CookieHelper.SetRefreshToken(Response, authResponse.RefreshToken,
                    authResponse.RefreshTokenExpiration.Value);
            }

            _logger.LogInformation("User logged in successfully: {Username}", loginDto.Username);
            return Ok(authResponse);
        }

        /// <summary>
        /// Refreshes the authentication token using a valid refresh token stored in cookies.
        /// </summary>
        /// <response code="200">Token refreshed successfully.</response>
        /// <response code="400">Missing refresh token.</response>
        /// <response code="401">Invalid or expired refresh token.</response>
        /// <response code="500">Server error during token refresh.</response>
        [HttpPost]
        [AllowAnonymous]
        [Route("refresh-token")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken()
        {
            _logger.LogInformation("RefreshToken endpoint called");

            var refreshToken = CookieHelper.GetRefreshToken(Request);

            if(string.IsNullOrEmpty(refreshToken))
                throw new ArgumentException(nameof(refreshToken), "Refresh token is missing.");

            var authResponse = await _authService.RefreshUserTokenAsync(refreshToken);


            if(authResponse.RefreshToken != null && authResponse.RefreshTokenExpiration.HasValue)
            {
                CookieHelper.SetRefreshToken(Response, authResponse.RefreshToken,
                    authResponse.RefreshTokenExpiration.Value);
            }

            _logger.LogInformation("Refresh token succeeded");
            return Ok(authResponse);

        }

        /// <summary>
        /// Revokes the user's current refresh token, preventing further use.
        /// </summary>
        /// <response code="204">Token revoked successfully.</response>
        /// <response code="400">Missing or invalid token.</response>
        /// <response code="500">Server error during token revocation.</response>
        [HttpPost]
        [Route("revoke-token")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RevokeToken()
        {
            _logger.LogInformation("RevokeToken endpoint called");

            var refreshToken = Request.Cookies["RefreshToken"];

            var isRevoked = await _authService.RevokeUserToken(refreshToken);

            if(isRevoked)
            {
                _logger.LogInformation("Refresh token revoked successfully");
                return NoContent();
            }
            else
            {
                _logger.LogWarning("Invalid token provided for revocation");
                return BadRequest("Invalid token");
            }
        }
    }
}
