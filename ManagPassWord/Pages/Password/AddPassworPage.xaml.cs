using ManagPassWord.ServiceLocators;
using ManagPassWord.ViewModels.Password;

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
        Task<int> _ = mainPageViewModel.Load();
        AddPasswordViewModel addPasswordViewModel = (AddPasswordViewModel)BindingContext;
        addPasswordViewModel.ClearFields();
        return base.OnBackButtonPressed();
    }
}