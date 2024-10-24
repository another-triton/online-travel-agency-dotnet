using RezLive.Processor.Api.Common;

namespace RezLive.Processor.Api.Extensions;

public class CustomHttpMessageHandler(IHttpContextAccessor httpContextAccessor)
: DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            var traceId = string.Empty;
            if (httpContextAccessor != null && httpContextAccessor.HttpContext != null)
            {
                var header = httpContextAccessor.HttpContext.Request.Headers
                        .FirstOrDefault(
                                x => x.Key.Contains("correlation",
                                    StringComparison.CurrentCultureIgnoreCase));
                if (header.Key != null)
                    traceId = header.Value.ToString();
            }
            request.Headers.Remove(Constants.CORRELATION_ID_HEADER);
            request.Headers.Add(Constants.CORRELATION_ID_HEADER, traceId);

            var result = await base.SendAsync(request, cancellationToken);

            result.EnsureSuccessStatusCode();

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
