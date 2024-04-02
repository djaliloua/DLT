using ManagPassWord.ViewModels.Password;

namespace ManagPassWord.Views;

public partial class PasswordSettingView : ContentView
{
	public PasswordSettingView()
	{
		InitializeComponent();
		BindingContext = ServiceLocator.GetService<PasswordSettingViewModel>();
	}
}