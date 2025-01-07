namespace TruthOrDrink_Mobile.Pages;

public partial class DrunkMeterPage : ContentPage
{
    private readonly QuoteService _quoteService;
    private readonly DatabaseService _databaseService;

    public DrunkMeterPage(QuoteService quoteService, DatabaseService databaseService)
    {
        InitializeComponent();
        _quoteService = quoteService;
        _databaseService = databaseService;
        BindingContext = new DrunkMeterViewModel(_quoteService, _databaseService);
    }
}
