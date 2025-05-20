using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using NextPost.Application.Dtos;

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
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                ValidationException => (StatusCodes.Status400BadRequest, "Bad Request"),
                ArgumentNullException => (StatusCodes.Status400BadRequest, "Bad Request"),
                ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request"),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
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
