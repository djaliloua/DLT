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
        MainViewModel mainPageViewModel = ViewModelLocator.MainViewModel;
        AddPasswordViewModel addPasswordViewModel = (AddPasswordViewModel)BindingContext;
        return base.OnBackButtonPressed();
    }
}