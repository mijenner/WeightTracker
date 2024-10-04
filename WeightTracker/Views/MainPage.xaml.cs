using WeightTracker.Models;
using WeightTracker.Services;
using WeightTracker.ViewModels;

namespace WeightTracker.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(UserProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
