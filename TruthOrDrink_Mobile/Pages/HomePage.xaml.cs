namespace TruthOrDrink_Mobile.Pages;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}