using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeightTracker.Models;
using WeightTracker.Services;

namespace WeightTracker.ViewModels
{
    public partial class UserProfileViewModel : ObservableObject
    {
        private IUserProfileService userProfileService;

        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private double height;
        [ObservableProperty]
        private double weight;
        [ObservableProperty]
        private DateTime weightDate;
        [ObservableProperty]
        private double refWeight;
        [ObservableProperty]
        private DateTime refDate;

        // Derived data: 
        [ObservableProperty]
        private double weightLoss = 0;
        [ObservableProperty]
        private double weightLossRate = 0;
        [ObservableProperty]
        private double weightLossPercent = 0;
        [ObservableProperty]
        private double bmi = 0;

        // New input: 
        [ObservableProperty]
        private double newWeight;
        [ObservableProperty]
        private DateTime selectedDate;
        [ObservableProperty]
        private TimeSpan selectedTime;

        // UI trick: 
        [ObservableProperty]
        private bool isSavedMessageVisible = false;

        [ObservableProperty]
        private MeasurementsViewModel measurementsViewModel;

        private UserProfile tempUserProfile;

        public UserProfileViewModel(IUserProfileService userProfileService, MeasurementsViewModel measurementsViewModel)
        {
            MeasurementsViewModel = measurementsViewModel;
            this.userProfileService = userProfileService;
        }

        public async Task LoadUserProfile()  // for main page 
        {
            await MeasurementsViewModel.FetchMeasurementsAsync();

            tempUserProfile = this.userProfileService.GetUserProfile();
            CopyToObservableProperties();
            CalculateDerivedData();

            SelectedDate = DateTime.Today;
            SelectedTime = DateTime.Now.TimeOfDay;
            NewWeight = tempUserProfile.Weight;

        }

        public void LoadTempProfile() // for edit profile page 
        {
            tempUserProfile = this.userProfileService.GetUserProfile();
            CopyToObservableProperties();
            CalculateDerivedData();

            SelectedDate = tempUserProfile.RefDate.Date;
            SelectedTime = tempUserProfile.RefDate.TimeOfDay;
        }

        public void CopyToObservableProperties()
        {
            if (tempUserProfile is null) return;
            Name = tempUserProfile.Name;
            Height = tempUserProfile.Height;
            Weight = tempUserProfile.Weight;
            WeightDate = tempUserProfile.WeightDate;
            RefWeight = tempUserProfile.RefWeight;
            RefDate = tempUserProfile.RefDate;
        }

        public void CopyToTempUserProfile()
        {
            if (tempUserProfile is null) return;
            tempUserProfile.Name = Name; 
            tempUserProfile.Height = Height;
            tempUserProfile.Weight = Weight;
            tempUserProfile.WeightDate = WeightDate;
            tempUserProfile.RefWeight = RefWeight;
            tempUserProfile.RefDate = SelectedDate.Add(SelectedTime); 
        }

        public void CalculateDerivedData()
        {
            if (MeasurementsViewModel.Measurements.Count == 0)
            {
                Weight = 0;
                WeightLoss = 0;
                WeightLossPercent = 0;
                WeightLossRate = 0;
                Bmi = 0;
                return; 
            }

            WeightLoss = RefWeight - Weight;
            WeightLossPercent = RefWeight > 0 ? WeightLoss / RefWeight : 0;
            var weeksBetween = (WeightDate - RefDate).Days / 7.0;
            WeightLossRate = Math.Abs(weeksBetween) > 0 ? WeightLoss / weeksBetween : 0;
            Bmi = Height > 0 ? Weight / Math.Pow(Height / 100.0, 2) : 0;
        }

        public void UpdateUsersWeight()
        {
            if (MeasurementsViewModel is null) return;
            if (MeasurementsViewModel.Measurements.Count == 0) return; 

            var latest = MeasurementsViewModel.GetLatestMeasurement();
            if (latest.Weight > 0)
            {
                Weight = latest.Weight;
                WeightDate = latest.TimePoint;
                tempUserProfile.Weight = Weight;
                tempUserProfile.WeightDate = WeightDate; 
            }
        }

        [RelayCommand]
        public async Task SaveTempUserProfile() // for edit page 
        {
            CopyToTempUserProfile(); 
            //tempUserProfile.Name = Name;
            //tempUserProfile.Height = Height;
            //UpdateUsersWeight(); 
            //tempUserProfile.RefWeight = RefWeight;
            //tempUserProfile.RefDate = SelectedDate.Add(SelectedTime);

            userProfileService.SaveUserProfile(tempUserProfile);

            IsSavedMessageVisible = true;
            await Task.Delay(2000);
            IsSavedMessageVisible = false;
            await GoBackAsync(); 
        }

        public async Task SaveUserProfileAsync()  // for main page 
        {
            UpdateUsersWeight();
            userProfileService.SaveUserProfile(tempUserProfile);

            IsSavedMessageVisible = true;
            await Task.Delay(1000);
            IsSavedMessageVisible = false;
        }

        [RelayCommand]
        public async Task AddNewMeasurementAsync() // for main page 
        {
            if (NewWeight <= 0) return;
            await MeasurementsViewModel.AddNewMeasurementAsync(NewWeight, SelectedDate.Add(SelectedTime));

            UpdateUsersWeight();
            CalculateDerivedData();
            await SaveUserProfileAsync();
        }

        [RelayCommand]
        public async Task DeleteMeasurementAsync(Measurement measurement) // for main page 
        {
            if (measurement is null) return; 
            if (measurement.Weight == 0) return;

            await MeasurementsViewModel.DeleteMeasurementAsync(measurement); 

            UpdateUsersWeight();
            CalculateDerivedData();
            await SaveUserProfileAsync();
        }

        [RelayCommand]
        public async Task MakeReferenceAsync(Measurement measurement)
        {
            if (measurement is null) return;
            if (measurement.Weight == 0) return;

            RefWeight = measurement.Weight;
            RefDate = measurement.TimePoint;
            CalculateDerivedData();
            CopyToTempUserProfile(); 
            await SaveUserProfileAsync();

        }

        [RelayCommand]
        public async Task GotoEditProfilePageAsync()
        {
            await Shell.Current.GoToAsync("editprofile");
        }

        [RelayCommand]
        public async Task GoBackAsync() // for edit profile page 
        {
            await Shell.Current.GoToAsync(".."); 
        }
    }
}
