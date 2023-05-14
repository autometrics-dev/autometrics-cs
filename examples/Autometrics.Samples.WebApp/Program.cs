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