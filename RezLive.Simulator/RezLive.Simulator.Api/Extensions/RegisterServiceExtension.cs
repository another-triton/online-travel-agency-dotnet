using RezLive.Simulator.Api.Abstractions.Behaviours;
using RezLive.Simulator.Domain.Services;
using RezLive.Simulator.Domain.Services.Contracts;

namespace RezLive.Simulator.Api.Extensions;
public static class RegisterServiceExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        //services.AddScoped(implementationFactory: cfg => cfg.GetService<IOptions<AppSettings>>()?.Value);
        services.AddMediatR(config => {
            config.RegisterServicesFromAssemblies(typeof(RegisterServiceExtension).Assembly);
            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
        });
        services.AddScoped<ISupplierService, SupplierService>();

        return services;
    }
    
}