using Microsoft.Extensions.Options;
using RezLive.Adapter.Api.DTOs;
using RezLive.Adapter.Api.Repository.Contracts;
using RezLive.Adapter.Api.Repository;
using RezLive.Adapter.Api.Services.Contracts;
using RezLive.Adapter.Api.Services;
using RezLive.Adapter.Api.Common;

namespace RezLive.Adapter.Api.Extensions;
public static class RegisterServiceExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<ISimulatorRepository, SimulatorRepository>();
        services.AddSingleton<ISimulatorService, SimulatorService>();

        return services;
    }

    public static IServiceCollection AddSimulatorHttpClient(this IServiceCollection services)
    {
        services.AddHeaderPropagation(options => options.Headers.Add(Constants.CORRELATION_ID_HEADER));
        services.AddTransient<CustomHttpMessageHandler>();

        services.AddHttpClient<ISimulatorRepository, SimulatorRepository>((serviceProvider, client) => {
            var settings = serviceProvider.GetRequiredService<IOptions<SimulatorSettings>>().Value;

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