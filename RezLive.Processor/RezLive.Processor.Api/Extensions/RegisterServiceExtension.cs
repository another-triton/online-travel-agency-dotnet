using Microsoft.Extensions.Options;
using RezLive.Processor.Api.Common;
using RezLive.Processor.Api.DTOs;
using RezLive.Processor.Api.Repository;
using RezLive.Processor.Api.Repository.Contracts;
using RezLive.Processor.Api.Services;
using RezLive.Processor.Api.Services.Contracts;

namespace RezLive.Processor.Api.Extensions;
public static class RegisterServiceExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IAdapterRepository, AdapterRepository>();
        services.AddSingleton<IAdapterService, AdapterService>();
        return services;
    }

    public static IServiceCollection AddAdapterHttpClient(this IServiceCollection services)
    {
        services.AddHeaderPropagation(options => options.Headers.Add(Constants.CORRELATION_ID_HEADER));
        services.AddTransient<CustomHttpMessageHandler>();

        services.AddHttpClient<IAdapterRepository, AdapterRepository>((serviceProvider, client) =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<AdapterSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseURL);
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new SocketsHttpHandler()
            {
                PooledConnectionLifetime = TimeSpan.FromSeconds(15)
            };
        })
        .AddHttpMessageHandler<CustomHttpMessageHandler>()
        .SetHandlerLifetime(Timeout.InfiniteTimeSpan)
        .AddHeaderPropagation();

        return services;
    }


}