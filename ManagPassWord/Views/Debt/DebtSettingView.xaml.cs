using ManagPassWord.ViewModels.Debt;

namespace ManagPassWord.Views;

public partial class DebtSettingView : ContentView
{
	public DebtSettingView()
	{
		InitializeComponent();
		BindingContext = ServiceLocator.GetService<DebtSettingViewModel>();
	}
}