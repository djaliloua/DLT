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
        MainPageViewModel mainPageViewModel = ViewModelServices.GetMainPageViewModel();
        Task<int> _ = mainPageViewModel.Load();
        AddPasswordViewModel addPasswordViewModel = (AddPasswordViewModel)BindingContext;
        addPasswordViewModel.ClearFields();
        return base.OnBackButtonPressed();
    }
}