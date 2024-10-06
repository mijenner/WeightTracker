using WeightTracker.Views;

namespace WeightTracker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("EditProfile", typeof(EditProfilePage));
        }
    }
}
