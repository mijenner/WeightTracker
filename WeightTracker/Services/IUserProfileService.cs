using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeightTracker.Models;

namespace WeightTracker.Services
{
    interface IUserProfileService
    {
        void SaveUserProfile(UserProfile userProfile);
        UserProfile GetUserProfile();
    }
}
