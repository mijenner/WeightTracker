using System.Text.Json;
using WeightTracker.Models;

namespace WeightTracker.Services
{
    class JSONFileStorage : IStorageService
    {
        private readonly string filePath;

        public JSONFileStorage()
        {
            var directory = FileSystem.AppDataDirectory;
            filePath = Path.Combine(directory, "WeightTrackerMeas.json");
        }

        public async Task AddMeasurementAsync(Measurement measurement)
        {
            List<Measurement> measurements = (await GetMeasurementsAsync()).ToList();
            measurements.Add(measurement);
            await SaveMeasurementsAsync(measurements);
        }

        public async Task<IEnumerable<Measurement>> GetMeasurementsAsync()
        {
            if (!File.Exists(filePath))
                return new List<Measurement>();

            var json = await File.ReadAllTextAsync(filePath);

            if (string.IsNullOrWhiteSpace(json))
                return new List<Measurement>();

            try
            {
                return JsonSerializer.Deserialize<List<Measurement>>(json) ?? new List<Measurement>();
            }
            catch (JsonException)
            {
                return new List<Measurement>();
            }
        }

        public async Task UpdateMeasurementAsync(Measurement measurement)
        {
            var measurements = (await GetMeasurementsAsync()).ToList();
            var index = measurements.FindIndex(m => m.TimePoint == measurement.TimePoint);
            if (index >= 0)
            {
                measurements[index] = measurement; 
                await SaveMeasurementsAsync(measurements);
            }
        }

        public async Task DeleteMeasurementAsync(Measurement measurement)
        {
            var measurements = await GetMeasurementsAsync();
            var updatedMeasurements = measurements.Where(m => m.TimePoint != measurement.TimePoint).ToList();
            await SaveMeasurementsAsync(updatedMeasurements);
        }

        public async Task DeleteMeasurementsAsync()
        {
            await SaveMeasurementsAsync(new List<Measurement>());
        }

        private async Task SaveMeasurementsAsync(IEnumerable<Measurement> measurements)
        {
            var json = JsonSerializer.Serialize(measurements, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
        }
    }
}
