using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using NextPost.Application.Dtos;
using NextPost.Application.Exceptions;

namespace NextPost.Api.Middlewares
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            var (statusCode, title) = exception switch
            {
                ValidationException => (StatusCodes.Status400BadRequest, "Bad Request"),
                InvalidAuthorIdException => (StatusCodes.Status400BadRequest, "Bad Request"),
                InvalidPostIdException => (StatusCodes.Status400BadRequest, "Bad Request"),
                InvalidCommentIdException => (StatusCodes.Status400BadRequest, "Bad Request"),
                InvalidRefreshTokenException => (StatusCodes.Status400BadRequest, "Bad Request"),
                NullUserException => (StatusCodes.Status400BadRequest, "Bad Request"),

                AuthorNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                CommentNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                PostNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                UnactiveRefreshTokenException => (StatusCodes.Status404NotFound, "Not Found"),

                InvalidCredentialsException => (StatusCodes.Status401Unauthorized, "Unauthorized"),

                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };


            var response = new ErrorResponseDto
            {
                StatusCode = statusCode,
                Title = title,
                ExceptionMessages = new List<string>()
            };

            if(exception is ValidationException validationException)
                response.ExceptionMessages.AddRange(validationException.Errors.Select(e => e.ErrorMessage).ToList());
            else
                response.ExceptionMessages.Add(exception.Message);


            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}
