# Autometrics Web Application Sample
The `Autometrics.Samples.WebApp` sample demonstrates how to use the Autometrics framework to instrument a .NET Web Application. The instrumentation of methods remains the same as in the other examples; the only difference lies in the configuration of the metrics exporter. 

## Instrumenting Methods

Autometrics makes it easy to instrument your methods with the `Autometrics` attribute. Just add the attribute to any method you want to instrument. Here is an example:

```csharp
[Autometrics]
public IActionResult Index()
{
    // Your code here
    return View();
}
```

## Configuring the Exporter

To configure the exporter for a web application, you'll need to modify the `Startup.cs` or `Program.cs` file. Below is an example of how to do this using the OpenTelemetry exporter:

```csharp
using OpenTelemetry;
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
```


