using ManagPassWord.ServiceLocators;

namespace ManagPassWord.Pages;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
		//BindingContext = ViewModelLocator.GetAboutViewModel();
	}
}