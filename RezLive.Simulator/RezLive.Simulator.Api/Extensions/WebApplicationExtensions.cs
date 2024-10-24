using Microsoft.AspNetCore.Mvc;
using RezLive.Simulator.Api.Abstractions.Common;
using RezLive.Simulator.Domain.Services.Contracts;
using System.Reflection;

namespace RezLive.Simulator.Api.Extensions;

public static class WebApplicationExtensions
{

    public static WebApplication RegisterRoutes(this WebApplication application)
    {
        application.MapPost($"/{Constants.ACCOMODATION_BY_SUPPLIER}", async ([FromBody] int id, ISupplierService _service) =>
        {
            return await _service.GetAccomodationsAsync(id);
        })
        .WithName("GetAccomodationBySupplier")
        .WithOpenApi();

        application.MapGet("/static3", async () =>
        {
            return await Task.FromResult("<Simulator></Simulator>");
        })
        .WithName("Static3")
        .WithOpenApi();

        application.MapGet("/build", async () =>
        {
            DateTime buildDate =
               new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime.ToUniversalTime();
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version?.ToString();
            return await Task.FromResult($"Simulator: {version}.{buildDate.ToString("yyyy-MM-dd-HH-mm-ss")} UTC");
        })
        .WithName("Build")
        .WithOpenApi();

        return application;
    }
}
