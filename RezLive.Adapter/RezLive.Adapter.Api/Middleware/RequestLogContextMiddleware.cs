using RezLive.Adapter.Api.Common;
using Serilog.Context;
using System.Runtime;

namespace RezLive.Adapter.Api.Middleware;

public class RequestLogContextMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public Task InvokeAsync(HttpContext context)
    {
        var header = context.Request.Headers
                .FirstOrDefault(
                        x => x.Key.Contains("correlation",
                            StringComparison.CurrentCultureIgnoreCase));
        var headerValue = header.Value;

        if (header.Key != null)
        {
            LogContext.PushProperty(Constants.CORRELATION_ID_HEADER, headerValue);
            context.Items[header.Key] = headerValue;
            if (context.Request.Headers.ContainsKey(header.Key))
            {
                context.Response.Headers[header.Key] = headerValue;
            }
            else
            {
                context.Response.Headers.Append(header.Key, headerValue);
            }
        }
        context.Response.ContentType = "text/html; charset=UTF-8";
        var response = _next(context);
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        return response;
    }
}
