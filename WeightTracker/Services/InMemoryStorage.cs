using System.Collections.Concurrent;
using WeightTracker.Models;

namespace WeightTracker.Services
{
    class InMemoryStorage : IStorageService
    {
        private readonly ConcurrentDictionary<DateTime, Measurement> measurements;

        public InMemoryStorage()
        {
            measurements = new ConcurrentDictionary<DateTime, Measurement>();
        }

        public Task AddMeasurementAsync(Measurement measurement)
        {
            measurements[measurement.TimePoint] = measurement; 
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Measurement>> GetMeasurementsAsync()
        {
            return Task.FromResult<IEnumerable<Measurement>>(measurements.Values.ToList());
        }

        public Task UpdateMeasurementAsync(Measurement measurement)
        {
            if (measurements.ContainsKey(measurement.TimePoint))
            {
                measurements[measurement.TimePoint] = measurement;
            }
            return Task.CompletedTask;
        }

        public Task DeleteMeasurementAsync(Measurement measurement)
        {
            measurements.TryRemove(measurement.TimePoint, out _);
            return Task.CompletedTask;
        }

        public Task DeleteMeasurementsAsync()
        {
            measurements.Clear(); 
            return Task.CompletedTask;
        }

    }
}
