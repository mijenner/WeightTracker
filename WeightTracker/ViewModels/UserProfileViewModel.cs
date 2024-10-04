using CommunityToolkit.Mvvm.ComponentModel;
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
        private MeasurementsViewModel measurementsViewModel;

        public UserProfileViewModel(IUserProfileService userProfileService, MeasurementsViewModel measurementsViewModel)
        {
            this.userProfileService = userProfileService; 
            UserProfile = this.userProfileService.GetUserProfile();
            this.measurementsViewModel = measurementsViewModel;
            UpdateData();
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
    }
}
