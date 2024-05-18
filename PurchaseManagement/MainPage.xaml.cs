using PurchaseManagement.ServiceLocator;
using PurchaseManagement.MVVM.Models.DTOs;

namespace PurchaseManagement
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigatedTo += MainPage_NavigatedTo;
        }
        //private void OrderBy()
        //{
        //    ViewModelLocator.MainViewModel.Purchases.Clear();
        //    var sort = ViewModelLocator.MainViewModel.Purchases.OrderByDescending(x => x.Purchase_Date).ToList();
        //    foreach (PurchasesDTO p in sort)
        //    {
        //        ViewModelLocator.MainViewModel.Purchases.Add(p);
        //    }
        //}
        private void MainPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            ViewModelLocator.MainViewModel.SelectedDate = DateTime.Now;
            //OrderBy();
        }
    }
}