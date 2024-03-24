namespace ManagPassWord;

public partial class SearchPage : ContentPage
{
	public SearchPage()
	{
		InitializeComponent();
		BindingContext = ViewModelServices.GetSearchViewModel();
	}
}