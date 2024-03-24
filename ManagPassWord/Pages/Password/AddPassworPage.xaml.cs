using ManagPassWord.ViewModels.Password;

namespace ManagPassWord;

public partial class AddPassworPage : ContentPage
{
	public AddPassworPage()
	{
		InitializeComponent();
		BindingContext = ViewModelServices.GetAddPassordViewModel();
	}
    protected override bool OnBackButtonPressed()
    {
        //ServiceLocator.RunMethod<MainPageViewModel>("Load");
        MainPageViewModel mainPageViewModel = ViewModelServices.GetMainPageViewModel();
        mainPageViewModel.Load();
        return base.OnBackButtonPressed();
    }
}