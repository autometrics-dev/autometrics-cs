using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autometrics.Instrumentation.Tests.Utils
{
    internal class MetricBatch
    {
        private List<MetricReading<int>> intReadings;
        private List<MetricReading<long>> longReadings;
        private List<MetricReading<double>> doubleReadings;

        public MetricBatch()
        {
            intReadings = new List<MetricReading<int>>();
            longReadings = new List<MetricReading<long>>();
            doubleReadings = new List<MetricReading<double>>();
        }

        public int IntReadings
        {
            get
            {
                return intReadings.Count;
            }
        }
        public int LongReadings
        {
            get
            {
                return longReadings.Count;
            }
        }
        public int DoubleReadings
        {
            get
            {
                return doubleReadings.Count;
            }
        }

        internal void AddMeasurement(Instrument instrument, int measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags)
        {
            intReadings.Add(new MetricReading<int>(
                metricName: instrument.Meter.Name,
                reading: measurement,
                tags: tags.ToArray()
            ));
        }

        internal void AddMeasurement(Instrument instrument, long measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags)
        {
            longReadings.Add(new MetricReading<long>(
                metricName: instrument.Meter.Name,
                reading: measurement,
                tags: tags.ToArray()
            ));
        }

        internal void AddMeasurement(Instrument instrument, double measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags)
        {
            doubleReadings.Add(new MetricReading<double>(
                metricName: instrument.Meter.Name,
                reading: measurement,
                tags: tags.ToArray()
            ));
        }

        internal bool ValidateFunctionNames(string[] functionNames)
        {
            return intReadings.All(reading => functionNames.Contains(reading.Function))
                && longReadings.All(reading => functionNames.Contains(reading.Function))
                && doubleReadings.All(reading => functionNames.Contains(reading.Function));
        }

        internal bool ValidateModuleName(string moduleName)
        {
            return intReadings.All(reading => reading.Module == moduleName)
                && longReadings.All(reading => reading.Module == moduleName)
                && doubleReadings.All(reading => reading.Module == moduleName);
        }

        internal bool ValidateResult(string result)
        {
            return intReadings.All(reading => reading.Result == result)
                && longReadings.All(reading => reading.Result == result)
                && doubleReadings.All(reading => reading.Result == result);
        }

        internal bool ValidateCallers(string[] callers)
        {
            return intReadings.All(reading => callers.Contains(reading.Caller))
                && longReadings.All(reading => callers.Contains(reading.Caller))
                && doubleReadings.All(reading => callers.Contains(reading.Caller));
        }

        internal bool? ValidateDuration(double sleepDuration, double allowedVariance)
        {
            // Get the average duration of all the double readings
            var averageDuration = doubleReadings.Average(reading => reading.Reading);

            // If it is within allowedVariance give this a pass
            if (Math.Abs(averageDuration - sleepDuration) <= allowedVariance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
