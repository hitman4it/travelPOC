using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetryTests.Middleware;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OpenTelemetryTests"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddJaegerExporter(options =>
            {
                options.AgentHost = "localhost";
                options.AgentPort = 6831;
            });
    })
        .WithMetrics(metricsProviderBuilder =>
        {
            metricsProviderBuilder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OpenTelemetryTests"))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddPrometheusExporter();
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();
app.UseMiddleware<RequestMiddleware>();
app.UseSwagger();

app.UseSwaggerUI();

app.MapControllers();
app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.UseHttpMetrics();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapMetrics(); // This maps the /metrics endpoint
});

app.Run();
