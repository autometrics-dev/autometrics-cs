using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autometrics.Instrumentation.Tests.Utils
{
    internal class MetricListener
    {
        private MeterListener meterListener;
        private MetricBatch activeBatch;

        public MetricListener()
        {
            meterListener = new MeterListener();
            meterListener.InstrumentPublished += MeterListener_InstrumentPublished;
            meterListener.SetMeasurementEventCallback<double>(OnDoubleMeasurementEvent);
            meterListener.SetMeasurementEventCallback<long>(OnLongMeasurementEvent);
            meterListener.SetMeasurementEventCallback<int>(OnIntMeasurementEvent);
            
            activeBatch = new MetricBatch();
            
            meterListener.Start();
        }

        private void OnIntMeasurementEvent(Instrument instrument, int measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        {
            activeBatch.AddMeasurement(instrument, measurement, tags);
        }

        private void OnLongMeasurementEvent(Instrument instrument, long measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        {
            activeBatch.AddMeasurement(instrument, measurement, tags);
        }

        private void OnDoubleMeasurementEvent(Instrument instrument, double measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
        {
            activeBatch.AddMeasurement(instrument, measurement, tags);
        }

        private void MeterListener_InstrumentPublished(Instrument instrument, MeterListener meterListener)
        {
            Console.WriteLine($"Subscribing to {instrument.Meter.Name}\\{instrument.Name}");
            meterListener.EnableMeasurementEvents(instrument);
        }

        internal MetricBatch GetMetricBatch()
        {
            return activeBatch;
        }

        internal void Reset()
        {
            activeBatch = new MetricBatch();
        }
    }
}
