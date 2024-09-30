using System.Text.Json;
using WeightTracker.Models;

namespace WeightTracker.Services
{
    class JSONFileStorage : IStorageService
    {
        private readonly string _filePath;

        public JSONFileStorage(string filePath)
        {
            _filePath = filePath;
        }

        public async Task AddMeasurementAsync(Measurement measurement)
        {
            List<Measurement> measurements = (await GetMeasurementsAsync()).ToList();
            measurements.Add(measurement);
            await SaveMeasurementsAsync(measurements);
        }

        public async Task<IEnumerable<Measurement>> GetMeasurementsAsync()
        {
            if (!File.Exists(_filePath))
                return new List<Measurement>();

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Measurement>>(json) ?? new List<Measurement>();
        }

        public async Task UpdateMeasurementAsync(Measurement measurement)
        {
            var measurements = await GetMeasurementsAsync();
            var index = measurements.ToList().FindIndex(m => m.TimePoint == measurement.TimePoint);
            if (index >= 0)
            {
                measurements.ToList()[index] = measurement; 
                await SaveMeasurementsAsync(measurements);
            }
        }

        public async Task DeleteMeasurementAsync(Measurement measurement)
        {
            var measurements = await GetMeasurementsAsync();
            var updatedMeasurements = measurements.Where(m => m.TimePoint != measurement.TimePoint).ToList();
            await SaveMeasurementsAsync(updatedMeasurements);
        }

        private async Task SaveMeasurementsAsync(IEnumerable<Measurement> measurements)
        {
            var json = JsonSerializer.Serialize(measurements, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
