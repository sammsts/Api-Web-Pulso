using Application.Interfaces;
using Domain.Entities;
using System.Security.Claims;

namespace ApiWebBase.Middleware
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditMiddleware> _logger;

        public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IAuditService auditService)
        {
            var request = context.Request;

            request.EnableBuffering();

            await _next(context);

            if (request.Method == HttpMethods.Post || request.Method == HttpMethods.Put || request.Method == HttpMethods.Delete &&
                context.Response.StatusCode is >= 200 and < 300)
            {
                var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous";
                var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                var userAgent = request.Headers["User-Agent"].ToString();
                var entityName = request.Path.Value?.Split('/').Skip(1).FirstOrDefault() ?? "unknown";
                var entityId = context.Request.RouteValues["id"]?.ToString() ?? "";

                request.Body.Position = 0;
                var reader = new StreamReader(request.Body);
                var bodyAsText = await reader.ReadToEndAsync();
                request.Body.Position = 0;

                var auditLog = new AuditLog
                {
                    UserId = userId,
                    Action = $"{request.Method} {request.Path}",
                    EntityName = entityName,
                    EntityId = entityId,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    NewValues = bodyAsText
                };

                await auditService.LogAsync(auditLog);
            }
        }
    }
}
