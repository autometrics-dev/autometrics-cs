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
  - [Instrumenting Classes](#instrumenting-classes)
  - [Adding SLO Information](#adding-slo-information)

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

### Instrumenting Classes
The instrumentation attribute can be added to classes and will apply to all methods within a class.  The `SkipInjection` attribute from the AspectInjector package can be added to methods to prevent them from being instrumented.
```csharp
[Autometrics]
public class BusinessLayer
{ 
    public DataAccessLayer? DataAccessLayer { get; set; }

    public void ProcessRequest()
    {
        // Your instrumented code here
    }

    [SkipInjection]
    public void CalculateShippingCost()
    {
        // Your non instrumented code here
    }
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