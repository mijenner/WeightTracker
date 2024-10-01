﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WeightTracker.Models;
using WeightTracker.Services;

namespace WeightTracker.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private IStorageService storageService;
        public ObservableCollection<Measurement> Measurements { get; set; } = new();

        [ObservableProperty]
        private double newWeight;

        [ObservableProperty]
        private DateTime selectedDate;

        [ObservableProperty]
        private TimeSpan selectedTime;

        public MainViewModel(IStorageService storageService)
        {
            Title = "Weight Tracker";
            this.storageService = storageService;
            SetDateAndTime(); 
        }

        private void SetDateAndTime() {
            SelectedDate = DateTime.Today;
            SelectedTime = DateTime.Now.TimeOfDay;
        }

        [RelayCommand]
        async Task AddNewMeasurementAsync()
        {
            try
            {
                var timePoint = SelectedDate.Date + SelectedTime;
                var meas = new Measurement(NewWeight, timePoint); 
                await storageService.AddMeasurementAsync(meas);
                await FetchMeasurementsAsync(); 
            }
            catch (Exception ex)
            {
                throw;
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
                foreach (var measurement in meas)
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
    }
}
