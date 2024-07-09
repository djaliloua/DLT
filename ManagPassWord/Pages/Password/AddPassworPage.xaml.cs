using ManagPassWord.ServiceLocators;
using ManagPassWord.MVVM.ViewModels.Password;

namespace ManagPassWord;

public partial class AddPassworPage : ContentPage
{
	public AddPassworPage()
	{
		InitializeComponent();
	}
    protected override bool OnBackButtonPressed()
    {
        MainPageViewModel mainPageViewModel = ViewModelLocator.MainPageViewModel;
        AddPasswordViewModel addPasswordViewModel = (AddPasswordViewModel)BindingContext;
        return base.OnBackButtonPressed();
    }
}