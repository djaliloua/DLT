using PurchaseManagement.Services;

namespace PurchaseManagement
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = RegisterViewModels.GetMainViewModel();
        }
        
    }
}