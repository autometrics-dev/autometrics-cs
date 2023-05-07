# Autometrics.Samples Application Documentation

The `Autometrics.Samples` application is a testing suite for evaluating the performance, output, and overhead of the `AutometricsMethod` attribute. The application covers various test scenarios, including simple and recursive methods, with and without the attribute.

## Table of Contents

- [Application Overview](#application-overview)
- [Test Types](#test-types)
  - [ConsoleMetrics](#consolemetrics)
  - [PrometheusMetrics](#prometheusmetrics)
  - [ScrapableMetrics](#scrapablemetrics)
- [Usage](#usage)
  - [MySampleApplication](#mysampleapplication)
  - [Load Testing](#load-testing)
    - [PerformSimpleTest](#performsimpletest)
    - [PerformRecursiveTest](#performrecursivetest)

## Application Overview

The `Autometrics.Samples` application contains the following main components:

- `MySampleApplication`: A sample application that simulates a multi-layered system with Presentation, Business, and Data Access layers. It generates activity that can be instrumented with the `AutometricsMethod` attribute.
- `Load Testing`: A set of test scenarios that evaluate the overhead of using the `AutometricsMethod` attribute. The tests cover simple and recursive methods, and their performance is compared with and without the attribute.

## Test Types

### ConsoleMetrics

This test type generates activity metrics from the `MySampleApplication` and outputs the results to the console.

*Documentation for this test type is under construction.*

### PrometheusMetrics

This test type generates activity metrics from the `MySampleApplication` and exports the results to a Prometheus-compatible format.

*Documentation for this test type is under construction.*

### ScrapableMetrics

This test type generates activity metrics from the `MySampleApplication`, and the results are provided in a format that can be easily scraped by monitoring tools.

*Documentation for this test type is under construction.*

## Usage

### MySampleApplication

The `MySampleApplication` class simulates a multi-layered system with Presentation, Business, and Data Access layers. It generates activity that can be instrumented with the `AutometricsMethod` attribute.

To run the `MySampleApplication`, call the `DoApplicationStuff()` method. The method will continuously generate activity and simulate potential errors, restarting the process after a short delay.

### Load Testing

The `Load Testing` component contains two methods for evaluating the overhead of the `AutometricsMethod` attribute: `PerformSimpleTest` and `PerformRecursiveTest`.

#### PerformSimpleTest

`PerformSimpleTest` is designed to test the overhead of the `AutometricsMethod` attribute on simple, non-recursive methods. It accepts an optional parameter, `numberOfIterations`, which defaults to 1000.

The method runs the test scenario with and without the `AutometricsMethod` attribute and calculates the average time for each case. The overhead is displayed as the difference in average time between the two cases.

#### PerformRecursiveTest

`PerformRecursiveTest` is designed to test the overhead of the `AutometricsMethod` attribute on recursive methods. It accepts two optional parameters: `numberOfIterations`, which defaults to 100, and `maxDepth`, which defaults to 5.

The method runs the test scenario with and without the `AutometricsMethod` attribute, using the specified depth for recursion, and calculates the average time for each case. The overhead is displayed as the difference in average time between the two cases.
