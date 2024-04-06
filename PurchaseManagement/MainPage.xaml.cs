using PurchaseManagement.Services;

namespace PurchaseManagement
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = RegisterViewModels.GetMainViewModel();
        }
        
    }
}