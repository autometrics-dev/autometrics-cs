# Testing with Autometrics.Samples Application

The `Autometrics.Samples` application is a testing suite for evaluating the performance, output, and overhead of the `AutometricsMethod` attribute. The application covers various test scenarios, including simple and recursive methods, with and without the attribute.

## Table of Contents

- [Application Overview](#application-overview)
- [Usage](#usage)
  - [MySampleApplication](#mysampleapplication)
  - [Load Testing](#load-testing)
    - [Perform a Simple Test](#performing-a-simple-test)
    - [Perform Recursive Call Test](#performing-a-recursive-call-test)
- [Test Types](#test-types)
  - [Export to Console](#exporting-to-the-console)
  - [Export to the Otel Collector](#exporting-to-the-otel-collector)
  - [Host Scrapable Metrics](#hosting-scrapeable-prometheus-metrics)

## Application Overview

The `Autometrics.Samples` application contains the following main components:

- `MySampleApplication`: A sample application that simulates a multi-layered system with Presentation, Business, and Data Access layers. It generates activity that can be instrumented with the `AutometricsMethod` attribute.
- `Load Testing`: A set of test scenarios that evaluate the overhead of using the `AutometricsMethod` attribute. The tests cover simple and recursive methods, and their performance is compared with and without the attribute.

## Usage

### Running the Console Application

The `Autometrics.Samples` application provides a console menu for users to interact with and execute various test scenarios. The menu lists available options to generate activity, perform load tests, and export test results.

#### Using the Console Menu

When you run the `Autometrics.Samples` application, you will be presented with the following console menu:

```
Please select an option:
1. Generate Activity Metrics to Console
2. Generate Activity Metrics for the Otel Collector
3. Generate Activity Metrics to be Scraped by Prometheus
4. Test AutometricsMethod Overhead (Simple)
5. Test AutometricsMethod Overhead (Recursive / Complex)
6. Exit
```

To execute a specific test, enter the corresponding option number and press `Enter`. Depending on the selected option, you may be prompted to provide additional information, such as the number of iterations or recursion depth for the load tests.

### Menu Options

1. **Generate Activity Metrics to Console**: This option runs the `MySampleApplication` and outputs the activity metrics to the console using the OpenTelemetry .NET Console Exporter. [See how](#exporting-to-the-console)

2. **Generate Activity Metrics for the Otel Collector**: This option runs the `MySampleApplication` and exports the activity metrics to a locally running Otel Collector using the OpenTelemetry .NET OTLP Exporter. [See how](#exporting-to-the-otel-collector)

3. **Generate Activity Metrics to be Scraped by Prometheus**: This option runs the `MySampleApplication` and hosts the activity metrics in a format that can be scraped by Prometheus using the OpenTelemetry .NET Prometheus Exporter (Experimental). [See how](#hosting-scrapeable-prometheus-metrics)

4. **Test AutometricsMethod Overhead (Simple)**: This option performs a load test on simple, non-recursive methods using the `PerformSimpleTest` method. You will be prompted to enter the number of iterations for the test. [See how](#performing-a-simple-test)

5. **Test AutometricsMethod Overhead (Recursive / Complex)**: This option performs a load test on recursive methods using the `PerformRecursiveTest` method. You will be prompted to enter the number of iterations and the recursion depth for the test. [See how](#performing-a-recursive-call-test)

6. **Exit**: This option exits the `Autometrics.Samples` application.

Choose an option by entering the corresponding number and pressing `Enter`. The application will execute the selected test and display the results accordingly.


### MySampleApplication

The `MySampleApplication` class simulates a multi-layered system with Presentation, Business, and Data Access layers. It generates activity that can be instrumented with the `AutometricsMethod` attribute.

To run the `MySampleApplication`, call the `DoApplicationStuff()` method. The method will continuously generate activity and simulate potential errors, restarting the process after a short delay.

### Load Testing

The `Load Testing` component contains two methods for evaluating the overhead of the `AutometricsMethod` attribute: `PerformSimpleTest` and `PerformRecursiveTest`.

#### Performing a Simple Test

`PerformSimpleTest` is designed to test the overhead of the `AutometricsMethod` attribute on simple, non-recursive methods. It accepts an optional parameter, `numberOfIterations`, which defaults to 1000.

The method runs the test scenario with and without the `AutometricsMethod` attribute and calculates the average time for each case. The overhead is displayed as the difference in average time between the two cases.

#### Performing a Recursive Call Test

`PerformRecursiveTest` is designed to test the overhead of the `AutometricsMethod` attribute on recursive methods. It accepts two optional parameters: `numberOfIterations`, which defaults to 100, and `maxDepth`, which defaults to 5.

The method runs the test scenario with and without the `AutometricsMethod` attribute, using the specified depth for recursion, and calculates the average time for each case. The overhead is displayed as the difference in average time between the two cases.


## Export Test Types

### Exporting to the Console

This test type generates activity metrics from the `MySampleApplication` and outputs the results to the console. 
This uses the Open Telementy .NET [console exporter](https://opentelemetry.io/docs/instrumentation/net/exporters/#console-exporter).

```csharp
public static void GenerateActivity()
{
    // Create a meter provider with the console exporter connected to the Autometrics.Instrumentation meter
    using var meterProvider = Sdk.CreateMeterProviderBuilder()
        .AddMeter("Autometrics.Instrumentation")
        .AddConsoleExporter()
        .Build();

    Console.WriteLine("Listener ready, starting to generate metrics.");

    // Run our sample application
    MySampleApplication.DoApplicationStuff();
}
```


### Exporting to the Otel Collector

This test type generates activity metrics from the `MySampleApplication` and exports the data to a locally running Otel collector. 
The destination can be configured when starting the provider, see the [Otel exporter documentation](https://opentelemetry.io/docs/instrumentation/net/exporters/#otlp-endpoint) for more details.

```csharp
public static void GenerateActivity()
{
    // Create a meter provider with the Otlp exporter connected to the Autometrics.Instrumentation
    using var meterProvider = Sdk.CreateMeterProviderBuilder()
        .AddMeter("Autometrics.Instrumentation")
        .AddOtlpExporter()
        .Build();

    Console.WriteLine("Listener ready, starting to generate metrics.");
            
    // Run our sample application
    MySampleApplication.DoApplicationStuff();
}
```


### Hosting Scrapeable Prometheus Metrics

This test type generates activity metrics from the `MySampleApplication`, and the results are provided in a format that can be easily scraped by Prometheus. 
This is currently in an Experimental stage and dependent on the OpenTelemetry specification to be made stable.  More details are available on the [OpenTelemetry .NET Prometheus exporter](https://opentelemetry.io/docs/instrumentation/net/exporters/#prometheus-experimental).


```csharp
public static void GenerateActivity()
{
    // Create a meter provider with the console exporter connected to the Autometrics.Instrumentation meter
    using var meterProvider = Sdk.CreateMeterProviderBuilder()
        .AddMeter("Autometrics.Instrumentation")
        .AddPrometheusHttpListener(
        options =>
        {
            options.UriPrefixes = new string[] { "http://localhost:9091/" };
        })
        .Build();

    Console.WriteLine("Listener ready, starting to generate metrics.");

    // Run our sample application
    MySampleApplication.DoApplicationStuff();
}
```
