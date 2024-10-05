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
        private UserProfile userProfile;

        [ObservableProperty]
        private UserProfile tempUserProfile;

        [ObservableProperty]
        private double weightLoss = 0;
        [ObservableProperty]
        private double weightLossRate = 0;
        [ObservableProperty]
        private double weightLossPercent = 0;
        [ObservableProperty]
        private double bmi = 0;

        [ObservableProperty]
        private DateTime selectedDate;

        [ObservableProperty]
        private TimeSpan selectedTime;

        [ObservableProperty]
        private bool isSavedMessageVisible = false; 

        [ObservableProperty]
        private MeasurementsViewModel measurementsViewModel;

        public UserProfileViewModel(IUserProfileService userProfileService, MeasurementsViewModel measurementsViewModel)
        {
            this.userProfileService = userProfileService;
            UserProfile = this.userProfileService.GetUserProfile();
            LoadTempProfile();
            MeasurementsViewModel = measurementsViewModel;
        }

        public void LoadTempProfile()
        {
            TempUserProfile = new UserProfile
            {
                Name = UserProfile.Name,
                Height = UserProfile.Height,
                Weight = UserProfile.Weight,
                WeightDate = UserProfile.WeightDate,
                RefWeight = UserProfile.RefWeight,
                RefDate = UserProfile.RefDate,
            };

            SelectedDate = TempUserProfile.RefDate.Date;
            SelectedTime = TempUserProfile.RefDate.TimeOfDay;
        }

        partial void OnSelectedDateChanged(DateTime value)
        {
            UserProfile.RefDate = value.Add(SelectedTime);
        }

        partial void OnSelectedTimeChanged(TimeSpan value)
        {
            UserProfile.RefDate = SelectedDate.Add(value);
        }

        public void UpdateData()
        {
            SelectedDate = UserProfile.RefDate.Date;
            SelectedTime = UserProfile.RefDate.TimeOfDay;

            if (MeasurementsViewModel is null) return; 

            var latest = MeasurementsViewModel.GetLatestMeasurement();
            if (latest.Weight > 0)
            {
                UserProfile.Weight = latest.Weight;
                UserProfile.WeightDate = latest.TimePoint;
            }
            WeightLoss = UserProfile.Weight - UserProfile.RefWeight;

            WeightLossPercent = UserProfile.RefWeight > 0 ? WeightLoss / UserProfile.RefWeight : 0;

            var daysBetween = (UserProfile.WeightDate - UserProfile.RefDate).Days;
            WeightLossRate = daysBetween > 0 ? WeightLoss / (daysBetween / 7) : 0;

            if (UserProfile.Height > 0)
            {
                Bmi = UserProfile.Height > 0 ? UserProfile.Weight / Math.Pow(UserProfile.Height, 2) : 0;
            }
        }

        partial void OnUserProfileChanged(UserProfile value)
        {
            UpdateData();
        }

        [RelayCommand]
        public async Task SaveUserProfile()
        {
            if (UserProfile == null) return;
            if (UserProfile.Weight == 0 || UserProfile.Height == 0) return;

            UserProfile.Name = TempUserProfile.Name;
            UserProfile.Weight = TempUserProfile.Weight;
            UserProfile.Height = TempUserProfile.Height;
            UserProfile.WeightDate = TempUserProfile.WeightDate;
            UserProfile.RefWeight = TempUserProfile.RefWeight;
            UserProfile.RefDate = TempUserProfile.RefDate;

            userProfileService.SaveUserProfile(UserProfile);

            isSavedMessageVisible = true;
            await Task.Delay(2000);
            IsSavedMessageVisible = false;
        }
    }
}
