using System.Collections.Concurrent;

namespace InventorySystem.API.Middleware;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private static readonly ConcurrentDictionary<string, ClientRequestInfo> _clients = new();
    private const int MaxRequests = 100;
    private static readonly TimeSpan TimeWindow = TimeSpan.FromMinutes(1);

    public RateLimitingMiddleware(
        RequestDelegate next,
        ILogger<RateLimitingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientId = GetClientId(context);

        if (string.IsNullOrEmpty(clientId))
        {
            await _next(context);
            return;
        }

        var clientInfo = _clients.GetOrAdd(clientId, _ => new ClientRequestInfo());

        lock (clientInfo)
        {
            if (clientInfo.LastRequest.Add(TimeWindow) < DateTime.UtcNow)
            {
                clientInfo.RequestCount = 0;
                clientInfo.LastRequest = DateTime.UtcNow;
            }

            clientInfo.RequestCount++;

            if (clientInfo.RequestCount > MaxRequests)
            {
                _logger.LogWarning("Rate limit exceeded for client: {ClientId}", clientId);
                context.Response.StatusCode = 429;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(new
                    {
                        error = "Rate limit exceeded. Try again later.",
                        retryAfter = TimeWindow.TotalSeconds
                    }));
                return;
            }
        }

        await _next(context);
    }

    private static string GetClientId(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString();
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        return $"{ip}_{userAgent}";
    }

    private class ClientRequestInfo
    {
        public int RequestCount { get; set; }
        public DateTime LastRequest { get; set; } = DateTime.UtcNow;
    }
}