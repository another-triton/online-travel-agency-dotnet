using RezLive.Adapter.Api.Common;
using RezLive.Adapter.Api.Services.Contracts;
using System.Reflection;

namespace RezLive.Adapter.Api.Extensions;

public static class WebApplicationExtensions
{

    public static WebApplication RegisterRoutes(this WebApplication application)
    {
        application.MapGet($"/{Constants.ACCOMODATION_BY_SUPPLIER}/{{id}}", async (ISimulatorService service, int id, IHttpContextAccessor httpContextAccessor) =>
        {
            return await service.GetAccomodationBySupplier(id);
        })
        .WithName("GetAccomodationBySupplier")
        .WithOpenApi();

        application.MapGet("/static2", async () =>
        {
            return await Task.FromResult("<Adapter></Adapter>");
        })
        .WithName("Static2")
        .WithOpenApi();

        application.MapGet("/static3", async (ISimulatorService service) =>
        {
            return await service.Static3();
        })
        .WithName("Static3")
        .WithOpenApi();

        application.MapGet("/build", async (ISimulatorService service) =>
        {
            DateTime buildDate =
               new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime.ToUniversalTime();
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version?.ToString();
            return $"Adapter: {version}.{buildDate.ToString("yyyy-MM-dd-HH-mm-ss")} UTC\r\n{await service.Build()}";
        })
        .WithName("Build")
        .WithOpenApi();

        return application;
    }
}
