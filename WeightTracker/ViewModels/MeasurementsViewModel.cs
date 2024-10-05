using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WeightTracker.Models;
using WeightTracker.Services;

namespace WeightTracker.ViewModels
{
    public partial class MeasurementsViewModel : BaseViewModel
    {
        private IStorageService storageService;
        public ObservableCollection<Measurement> Measurements { get; set; } = new();

        // constructor 
        public MeasurementsViewModel(IStorageService storageService)
        {
            Title = "Weight Tracker";
            this.storageService = storageService;
        }


        public async Task AddNewMeasurementAsync(double newWeight, DateTime timePoint)
        {
            if (newWeight == 0) return; 
            try
            {
                var meas = new Measurement(newWeight, timePoint); 
                await storageService.AddMeasurementAsync(meas);
                await FetchMeasurementsAsync(); 
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {

            }

        }

        [RelayCommand]
        async Task FetchMeasurementsAsync()
        {
            try
            {
                var meas = await storageService.GetMeasurementsAsync();
                Measurements.Clear();
                foreach (var measurement in meas.OrderByDescending(m => m.TimePoint))
                {
                    Measurements.Add(measurement);
                }

            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
            }
            finally
            {

            }

        }

        public Measurement GetLatestMeasurement()
        {
            if (Measurements.Count == 0)
                return new Measurement();

            var recentMeasurement = Measurements.OrderByDescending(m => m.TimePoint).FirstOrDefault();

            return recentMeasurement ?? new Measurement();
        }

        public async Task DeleteMeasurementAsync(Measurement measurement)
        {
            Measurements.Remove(measurement);
            await storageService.DeleteMeasurementAsync(measurement);
        }
    }
}
