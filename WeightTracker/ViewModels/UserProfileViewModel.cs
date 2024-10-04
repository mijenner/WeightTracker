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
        private double weightLoss = 0;
        [ObservableProperty]
        private double weightLossRate = 0;
        [ObservableProperty]
        private double weightLossPercent = 0;
        [ObservableProperty]
        private double bmi = 0; 

        [ObservableProperty]
        private MeasurementsViewModel measurementsViewModel;

        public UserProfileViewModel(IUserProfileService userProfileService, MeasurementsViewModel measurementsViewModel)
        {
            this.userProfileService = userProfileService; 
            UserProfile = this.userProfileService.GetUserProfile();
            this.measurementsViewModel = measurementsViewModel;
            UpdateData();
        }

        public void UpdateData()
        {
            var latest = MeasurementsViewModel.GetLatestMeasurement();
            if (latest.Weight > 0)
            {
                UserProfile.Weight = latest.Weight;
                UserProfile.WeightDate = latest.TimePoint; 
            }
            WeightLoss = UserProfile.Weight - UserProfile.RefWeight;
            WeightLossPercent = WeightLoss / UserProfile.RefWeight;
            WeightLossRate = WeightLoss / (UserProfile.WeightDate - UserProfile.RefDate).Days / 7;
            Bmi = UserProfile.Weight / (Math.Pow(UserProfile.Height, 2)); 
        }
    }
}
