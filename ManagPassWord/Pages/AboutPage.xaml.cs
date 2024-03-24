namespace ManagPassWord.Pages;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
		BindingContext = ViewModelServices.GetAboutViewModel();
	}
}