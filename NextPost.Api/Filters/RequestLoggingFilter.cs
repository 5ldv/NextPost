using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace NextPost.Api.Filters
{
    public class RequestLoggingFilter(ILogger<RequestLoggingFilter> logger) : IAsyncActionFilter
    {
        private readonly ILogger<RequestLoggingFilter> _logger = logger;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();

            await next();

            stopwatch.Stop();

            var elapsed = stopwatch.ElapsedMilliseconds;
            var path = context.HttpContext.Request.Path;

            _logger.LogInformation("Request [{Path}] completed in {ElapsedMilliseconds} ms", path, elapsed);

        }
    }
}
