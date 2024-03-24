using ManagPassWord.ViewModels.Password;

namespace ManagPassWord;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
    }
}