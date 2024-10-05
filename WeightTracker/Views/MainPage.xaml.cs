using WeightTracker.Models;
using WeightTracker.Services;
using WeightTracker.ViewModels;

namespace WeightTracker.Views
{
    public partial class MainPage : ContentPage
    {
        UserProfileViewModel viewModel; 
        public MainPage(UserProfileViewModel viewModel)
        {
            InitializeComponent();
            Title = "Main page"; 
            BindingContext = viewModel;
            this.viewModel = viewModel; 
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            viewModel?.LoadUserProfile();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel?.LoadUserProfile();
            viewModel?.CalculateDerivedData(); 
        }
    }
}
