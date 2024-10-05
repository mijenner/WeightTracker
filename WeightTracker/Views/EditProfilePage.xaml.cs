using WeightTracker.ViewModels;

namespace WeightTracker.Views;

public partial class EditProfilePage : ContentPage
{
	UserProfileViewModel viewModel; 

	public EditProfilePage(UserProfileViewModel viewModel)
	{
		InitializeComponent();
		Title = "Edit User Profile"; 
		BindingContext = viewModel; 
		this.viewModel = viewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
		viewModel?.LoadTempProfile(); 
    }
}
