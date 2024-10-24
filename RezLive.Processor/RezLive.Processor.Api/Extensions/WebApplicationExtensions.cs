using Microsoft.AspNetCore.Mvc;
using RezLive.Processor.Api.Services.Contracts;
using System.Diagnostics;
using System.Reflection;

namespace RezLive.Processor.Api.Extensions;

public static class WebApplicationExtensions
{

    public static WebApplication RegisterRoutes(this WebApplication application)
    {

        application.MapPost("/", async ([FromForm] string XML, IAdapterService service) =>
        {
            return await service.GetAccomodations();
        })
        .DisableAntiforgery()
        .WithName("GetAccomodations")
        .WithOpenApi();

        application.MapGet("/static1", async (IAdapterService service) =>
        {
            return await Task.FromResult("<Processor></Processor>");
        })
        .WithName("Static1")
        .WithOpenApi();

        application.MapGet("/build", async (IAdapterService service) =>
        {
            DateTime buildDate =
               new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime.ToUniversalTime();
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version?.ToString();
            return $"Processor: {version}.{buildDate.ToString("yyyy-MM-dd-HH-mm-ss")} UTC\r\n{await service.Build()}";
        })
        .WithName("Build")
        .WithOpenApi();

        /// Uncomment the following code to test the Processor Layer without Interacting with Other Layers.
        /*
        

        application.MapGet("/static2", async (IAdapterService service) =>
        {
            return $"<Processor>{await service.Static2()}</Processor>";
        })
        .WithName("Static2")
        .WithOpenApi();

        application.MapGet("/static3", async (IAdapterService service) =>
        {
            return $"<Processor>{await service.Static3()}</Processor>";
        })
        .WithName("Static3")
        .WithOpenApi();
        */
        return application;
    }
}
