using ManagPassWord.ViewModels;

namespace ManagPassWord;

public partial class Details : ContentPage
{
	public Details(DetailsViewModel detailsView)
	{
		InitializeComponent();
		BindingContext = detailsView;
	}
}