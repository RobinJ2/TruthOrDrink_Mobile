namespace TruthOrDrink_Mobile.Pages;

public partial class ManageQuestionsPage : ContentPage
{
	public ManageQuestionsPage()
	{
		InitializeComponent();
        BindingContext = new ManageQuestionsViewModel(new DatabaseService());
    }
}