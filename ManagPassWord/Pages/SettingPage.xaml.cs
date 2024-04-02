using MVVM;

namespace ManagPassWord.Pages;

public partial class SettingPage : ContentPage
{
    public SettingPage()
    {
        InitializeComponent();
        BindingContext = ViewModelServices.GetSettingViewModel();
    }
   
}