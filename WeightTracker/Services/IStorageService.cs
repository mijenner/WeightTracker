using System.Collections;
using WeightTracker.Models;

namespace WeightTracker.Services
{
    interface IStorageService
    {
        Task AddMeasurementAsync(Measurement measurement);  // Create 
        Task<IEnumerable<Measurement>> GetMeasurementsAsync(); // Read
        Task UpdateMeasurementAsync(Measurement measurement); // Update
        Task DeleteMeasurementAsync(Measurement measurement); // Delete 
        Task DeleteMeasurementsAsync();  // Delete all 
    }
}
