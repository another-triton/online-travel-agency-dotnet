using RezLive.Simulator.Api.Abstractions.Common;
using RezLive.Simulator.Api.Extensions;
using RezLive.Simulator.Api.Middleware;
using RezLive.Simulator.Domain.DTOs;
using RezLive.Simulator.Domain.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

///Configuring the AppSettings for the Dependency Injection
builder.ConfigureAppSettings();

///Configure Serilog with AWS CloudWatch
builder.RegisterSeriLog();

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterServices();
builder.Services.AddHeaderPropagation(options => options.Headers.Add("rezlive-correlation-id"));
builder.Services.AddHeaderPropagation();
//builder.Services.AddResponseCompression(options =>
//{
//    options.EnableForHttps = false;
//    options.Providers.Add<GzipCompressionProvider>();
//});
//builder.Services.Configure<GzipCompressionProviderOptions>(options =>
//{
//    options.Level = CompressionLevel.Optimal;
//});
var hostingEnvironment = builder.Environment;

var app = builder.Build();

///Uncomment following If condition once Swagger is not needed on Production
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseSerilogRequestLogging();

app.UseMiddleware<RequestLogContextMiddleware>();
//app.UseResponseCompression();
app.RegisterRoutes();
var settings = builder.Configuration
    .GetSection(Constants.APP_SETTINGS_KEY)
    .Get<AppSettings>();

SupplierData.PopulateXmlList(Path.Combine(hostingEnvironment.WebRootPath, settings?.XmlPath ?? ""));

await app.RunAsync();
