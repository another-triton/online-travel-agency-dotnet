using RezLive.Adapter.Api.Extensions;
using RezLive.Adapter.Api.Middleware;
using Serilog;
using System.Runtime;
using static System.Console;

var builder = WebApplication.CreateBuilder(args);

///Configuring the AppSettings for the Dependency Injection
builder.ConfigureAppSettings();

///Configure Serilog with AWS CloudWatch
builder.RegisterSeriLog();

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterServices();
builder.Services.AddSimulatorHttpClient();
//builder.Services.AddResponseCompression(options => {
//    options.EnableForHttps = false;
//    options.Providers.Add<GzipCompressionProvider>();
//});
//builder.Services.Configure<GzipCompressionProviderOptions>(options => {
//    options.Level = CompressionLevel.Optimal;
//});
var app = builder.Build();

///Uncomment following If condition once Swagger is not needed on Production
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHeaderPropagation();
app.UseSerilogRequestLogging();

app.UseMiddleware<RequestLogContextMiddleware>();
//app.UseResponseCompression();
app.RegisterRoutes();

var gcType = GCSettings.IsServerGC == true ? "Server" : "Workstation";
WriteLine($"GC Type - {gcType} ");
WriteLine($"GC Latency Type - {GCSettings.LatencyMode}");

app.Run();
