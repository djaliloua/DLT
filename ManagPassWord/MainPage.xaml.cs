namespace ManagPassWord;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		BindingContext = ViewModelServices.GetMainPageViewModel();
    }
}