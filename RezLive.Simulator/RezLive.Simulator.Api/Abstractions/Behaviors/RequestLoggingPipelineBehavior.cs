using MediatR;
using RezLive.Simulator.Api.Abstractions.Models;
using Serilog.Context;

namespace RezLive.Simulator.Api.Abstractions.Behaviours;

internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>
        (ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;

        logger.LogInformation("Processing request {RequestName}", requestName);

        TResponse result = await next();

        if (result.IsSuccess) {
            logger.LogInformation("Complete request {RequestName}", requestName);
        }
        else 
        {
            using (LogContext.PushProperty("Error", result.Error, true)) {
                logger.LogError("Complete request {RequestName} with error", requestName);
            }                
        }
        return result;
    }
}