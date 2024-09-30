using WeightTracker.Models;

namespace WeightTracker.Services
{
    class UserProfileService : IUserProfileService
    {
        public void SaveUserProfile(UserProfile userProfile)
        {
            Preferences.Set(nameof(userProfile.Name), userProfile.Name);
            Preferences.Set(nameof(userProfile.Height), userProfile.Height);
            Preferences.Set(nameof(userProfile.Weight), userProfile.Weight);
            Preferences.Set(nameof(userProfile.RefWeight), userProfile.RefWeight);
            Preferences.Set(nameof(userProfile.RefDate), userProfile.RefDate);
            return; 
        }

        public UserProfile GetUserProfile()
        {
            UserProfile userProfile = new();
            userProfile.Name = Preferences.Get(nameof(userProfile.Name), GetFallbackUserName());
            userProfile.Height = Preferences.Get(nameof(userProfile.Height), 0.0);
            userProfile.Weight = Preferences.Get(nameof(userProfile.Weight), 0.0);
            userProfile.RefWeight = Preferences.Get(nameof(userProfile.RefWeight), 0.0);
            userProfile.RefDate = Preferences.Get(nameof(userProfile.RefDate), DateTime.Now);
            return userProfile;
        }

        private string GetFallbackUserName()
        {
#if WINDOWS || MACOS
            return Environment.UserName;
#else
            return "User"; 
#endif
        }
    }
}
