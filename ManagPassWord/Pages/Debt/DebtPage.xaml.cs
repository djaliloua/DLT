namespace ManagPassWord;

public partial class DebtPage : ContentPage
{
	public DebtPage()
	{
		InitializeComponent();
		BindingContext = ViewModelServices.GetDebtPageViewModel();
	}
}