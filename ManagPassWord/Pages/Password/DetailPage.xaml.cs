using ManagPassWord.ViewModels.Password;

namespace ManagPassWord;

public partial class DetailPage : ContentPage
{
	public DetailPage()
	{
		InitializeComponent();
		BindingContext = ViewModelServices.GetDetailViewModel();
	}
}