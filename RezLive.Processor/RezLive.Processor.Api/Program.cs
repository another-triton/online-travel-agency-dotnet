using Prometheus;
using Prometheus.SystemMetrics;
using Prometheus.SystemMetrics.Collectors;
using RezLive.Processor.Api.Extensions;
using RezLive.Processor.Api.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

///Configuring the AppSettings for the Dependency Injection
builder.ConfigureAppSettings();

///Configure Serilog with AWS CloudWatch
builder.RegisterSeriLog();

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.UseHttpClientMetrics(); //gc
builder.Services.AddSystemMetrics(registerDefaultCollectors: false); //gc
builder.Services.AddSystemMetricCollector<LinuxCpuUsageCollector>(); //gc
builder.Services.AddSystemMetricCollector<LinuxMemoryCollector>();  //gc
builder.Services.RegisterServices();
builder.Services.AddAdapterHttpClient();
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
app.UseHeaderPropagation();

app.UseRouting();
app.UseEndpoints(endpoints => { _ = endpoints.MapMetrics(); }); //gc
app.UseMetricServer(); //gc
app.UseSwagger();
app.UseSwaggerUI();
app.UseSerilogRequestLogging();
app.UseMiddleware<RequestLogContextMiddleware>();
//app.UseResponseCompression();
app.RegisterRoutes();

app.UseHttpMetrics(metrics => {
    metrics.AddCustomLabel("host", ctx => ctx.Request.Host.Host);
}); //gc



/*var gcType = GCSettings.IsServerGC == true ? "Server" : "Workstation";
WriteLine($"GC Type - {gcType} ");
WriteLine($"GC Latency Type - {GCSettings.LatencyMode}");
WriteLine($"GC Run Count{System.GC.CollectionCount}");*/



app.Run();


