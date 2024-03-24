namespace ManagPassWord.Pages.Debt;

public partial class DebtDetailsPage : ContentPage
{
	public DebtDetailsPage()
	{
		InitializeComponent();
		BindingContext = ViewModelServices.GetDebtDetailsViewModel();


    }
}