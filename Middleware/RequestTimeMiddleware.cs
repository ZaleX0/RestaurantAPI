using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RestaurantAPI.Middleware;

public class RequestTimeMiddleware : IMiddleware
{
    private readonly ILogger<RequestTimeMiddleware> _logger;

    public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        await next.Invoke(context);
        stopwatch.Stop();

        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        if (elapsedMilliseconds / 1000 > 4)
        {
            var message = $"Request [{context.Request.Method}] at {context.Request.Path} took {elapsedMilliseconds} ms";
            _logger.LogInformation(message);
        }
    }
}
