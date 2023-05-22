using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

// Add OpenTelemetry services, and configure the metrics exporter for Autometrics.Instrumentation
builder.Services.AddOpenTelemetry()
    .WithMetrics(builder =>
    {
        // Additional configuration or other exporters can be added here
        builder.AddOtlpExporter();
        builder.AddMeter("Autometrics.Instrumentation");
        builder.AddView(
                    instrumentName: "function.calls.duration",
                    new ExplicitBucketHistogramConfiguration { Boundaries = new double[] { 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10 } }
                    );
    });

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.Run();