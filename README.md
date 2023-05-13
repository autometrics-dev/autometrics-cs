# Autometrics for .NET

Autometrics-CS is a C# instrumentation of the [Autometrics](https://github.com/autometrics-dev) observability micro-framework. It makes it quick and easy to instrument your code to collect standardized metrics, including function call counts, durations, and build information. This project includes a sample application which shows examples for generating metrics to the console, Prometheus, or hosting and endpoint for scraping.

## Table of Contents
- [Aspect-Oriented Programming and AspectInjector](#aspect-oriented-programming-and-aspectinjector)
- [How it Works](#how-it-works)
  - [Metrics Instrumentation](#metrics-instrumentation)
  - [Metrics Collection and OpenTelemetry](#metrics-collection-and-opentelemetry)
- [Getting Started](#getting-started)
  - [Usage](#usage)
  - [Instrumenting Methods](#instrumenting-methods)
  - [Adding SLO Information](#adding-slo-information)
- [Examples](#examples)
  - [Usage in a Library](#usage-in-a-library)
  - [Exporting Metrics in a Console Application](#exporting-metrics-in-a-console-application)
  - [Exporting Metrics in a Web Application](#exporting-metrics-in-a-web-application)
- [Running the Sample Console Application and looking at the Grafana Dashboards](#running-the-sample-console-application-and-looking-at-the-grafana-dashboards)

## Aspect-Oriented Programming and AspectInjector

Autometrics-CS utilizes Aspect-Oriented Programming (AOP) techniques to provide non-invasive and modular instrumentation. AOP allows you to inject additional behavior (in our case, metrics collection) into your methods without altering the original code. This project uses the [AspectInjector](https://github.com/pamidur/aspect-injector) library to achieve AOP in C#.

## How it Works

### Metrics Instrumentation

Metrics instrumentation is performed using Microsoft's `System.Diagnostics.Metrics` library. This provides a lightweight and well-tested method of instrumentation that's used as a .NET standard.

Read more on Microsoft's [Metrics Overview](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/metrics) page.

### Metrics Collection and OpenTelemetry

When using `System.Diagnostics.Metrics`, the metrics instrumentation occurs but the data isn't listened to or sent anywhere. We've provided examples below of using OpenTelemetry's .NET exporters to export this data in an Otel-friendly format. Additional options such as using the `dotnet-counters` command-line tool or the `MeterListener` API can be seen on the [Metrics Collection](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/metrics-collection) page.

## Getting Started

### Usage
To use Autometrics, follow these simple steps:
1. Import the Autometrics package and its dependencies in your code.
2. Add the `[Autometrics]` attribute and any SLOs to the methods you want to instrument.
3. Configure the Collection method and Exporter in your application.

### Instrumenting Methods
The instrumentation attribute can be added to any method and its usage doesn't change for Libraries, Console Applications, or Web Applications.  The only difference is how the metrics are exported.  The attribute class is `AutometricsAttribute`, however Visual Studio will show it as only `Autometrics`.

```csharp
[Autometrics]
private bool CheckRedisCache()
{
    // Your unchanged code here
}
```

### Adding SLO Information
Autometrics makes it easy to add Prometheus Service-Level Objectives (SLOs) to your methods. SLOs are a great way to track the performance of your code and ensure that it meets your expectations. SLOs can be added as three different types:

Success Rate: The percentage of calls that succeed.
```csharp
[Autometrics("UserAuth", ObjectivePercentile.P99)]
public void UserAuthentication()
{
    // Your code here
}
```

Latency Threshold: The amount of time it takes for a call to complete.
```csharp
[Autometrics("OrderCreation", ObjectivePercentile.P99, ObjectiveLatency.Ms500, ObjectiveType.LatencyThreshold)]
public void OrderGeneration()
{
    // Your code here
}
```

Both Latency and Success Rate: The amount of time it takes for a call to complete and the percentage of calls that succeed.  This is the default if a ObjectiveLatency is specifyed but no ObjectiveType is specified.
```csharp
[Autometrics("OrderCreation", ObjectivePercentile.P99, ObjectiveLatency.Ms500, ObjectiveType.SuccessAndLatency)]
public void OrderGeneration()
{
    // Your code here
}
```

## Examples

Check out the `Autometrics.Samples` projects for examples of how to use Autometrics in your code.  Varying examples of how this works and using it can be seen on the [ConsoleApp Sample](ConsoleAppSample.md), Library Sample, and [WebApp Sample](WebAppSample.md) pages. page.

### Usage in a Library

The Autometrics Instrumentation works well in a library, and the metrics `module` tag will hold the libraries name when used as a dependency.  No exporter configuration is required within the library, the application using the library will be responsible for exporting the metrics.

A library instrumented with Autometrics can be used in an application that doesn't add a meter or listen to those metrics.


### Exporting Metrics in a Console Application

Here's an example of how to instrument Autometrics in a C# Console application and export the metrics to the OpenTelemetry collector.

```csharp
using OpenTelemetry;

namespace MyProject
{
    class Program
    {
        static void Main(string[] args)
        {

            // Create a meter provider with the Otlp exporter connected to the Autometrics.Instrumentation
            using var meterProvider = Sdk.CreateMeterProviderBuilder()
                .AddMeter("Autometrics.Instrumentation")
                .AddOtlpExporter()
                .Build();

            // Call your instrumented method(s)
            MyInstrumentedMethod();
        }
    }
}
```

### Exporting Metrics in a Web Application

Here's and example of exporting the metrics to the OpenTelemetry collector in a C# Web application.
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


## Running the Sample Console Application and looking at the Grafana Dashboards

Running the application generates a steady flow of activity with the ocassional error
![image](https://user-images.githubusercontent.com/17866458/236915271-244170f4-1345-4817-8b5c-84e70d715339.png)

The pre-build Grafana Dashboards display the activity the same as all over Autometrics projects 
![image](https://user-images.githubusercontent.com/17866458/236915571-f20dfb9e-7639-4406-80f3-34ca1315d25c.png)
![image](https://user-images.githubusercontent.com/17866458/236916166-2a387d96-de70-405e-bdb6-f54faa98545d.png)
