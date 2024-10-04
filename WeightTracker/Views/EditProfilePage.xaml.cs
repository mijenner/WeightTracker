using WeightTracker.ViewModels;

namespace WeightTracker.Views;

public partial class EditProfilePage : ContentPage
{
	public EditProfilePage(UserProfileViewModel viewModel)
	{
		InitializeComponent();
		Title = "Edit User Profile"; 
		BindingContext = viewModel; 
	}
}
