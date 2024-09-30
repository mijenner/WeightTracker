using System.Collections.Concurrent;
using WeightTracker.Models;

namespace WeightTracker.Services
{
    class InMemoryStorage : IStorageService
    {
        private readonly ConcurrentDictionary<DateTime, Measurement> _measurements;

        public InMemoryStorage()
        {
            _measurements = new ConcurrentDictionary<DateTime, Measurement>();
        }

        public Task AddMeasurementAsync(Measurement measurement)
        {
            _measurements[measurement.TimePoint] = measurement; 
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Measurement>> GetMeasurementsAsync()
        {
            return Task.FromResult<IEnumerable<Measurement>>(_measurements.Values.ToList());
        }

        public Task UpdateMeasurementAsync(Measurement measurement)
        {
            if (_measurements.ContainsKey(measurement.TimePoint))
            {
                _measurements[measurement.TimePoint] = measurement;
            }
            return Task.CompletedTask;
        }

        public Task DeleteMeasurementAsync(Measurement measurement)
        {
            _measurements.TryRemove(measurement.TimePoint, out _);
            return Task.CompletedTask;
        }
    }
}
