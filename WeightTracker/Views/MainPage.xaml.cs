
using System.Diagnostics;
using WeightTracker.Models;
using WeightTracker.Services;

namespace WeightTracker.Views
{
    public partial class MainPage : ContentPage
    {
        // quick and dirty testing of services: 
        private readonly IStorageService storageService;
        private readonly IUserProfileService userProfileService;

        public MainPage()
        {
            InitializeComponent();

            storageService = new InMemoryStorage();
            userProfileService = new UserProfileService(); 

            TestUserProfileService();
            TestMeasurementStorage(); 

        }

        private void TestMeasurementStorage()
        {
            // set breakpoint here 
            var tp1 = DateTime.Now;
            var m1 = new Measurement { TimePoint = tp1, Weight = 70 };
            storageService.AddMeasurementAsync(m1);
            var tp2 = DateTime.Now;
            var m2 = new Measurement { TimePoint = tp2, Weight = 80 }; 
            storageService.AddMeasurementAsync(m2);
            var tp3 = DateTime.Now;
            var m3 = new Measurement { TimePoint = tp3, Weight = 90 };
            storageService.AddMeasurementAsync(m3);
            var list1 = storageService.GetMeasurementsAsync();
            m1.Weight = 71;
            storageService.UpdateMeasurementAsync(m1);
            storageService.DeleteMeasurementAsync(m2);
            list1 = storageService.GetMeasurementsAsync();
        }

        private void TestUserProfileService()
        {
            // set breakpoint here 
            var userProfile = new UserProfile { Height = 179, Weight = 85 };
            userProfile.Name = "Svend";
            userProfile.RefWeight = 95; 
            userProfile.RefDate = DateTime.Now;
            userProfileService.SaveUserProfile(userProfile);
            userProfile.Name = "Bent";
            userProfile.Height = 189;
            userProfile.Weight = 97; 
            userProfile.RefWeight = 103;
            userProfile.RefDate = DateTime.Now;

            userProfile = userProfileService.GetUserProfile();
        }
    }
}
