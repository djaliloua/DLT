using PurchaseManagement.ServiceLocator;

namespace PurchaseManagement
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigatedTo += MainPage_NavigatedTo;
        }

        private void MainPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            ViewModelLocator.MainViewModel.SelectedDate = DateTime.Now;
        }
    }
}