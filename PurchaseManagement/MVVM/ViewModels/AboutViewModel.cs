using MVVM;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class AboutViewModel:BaseViewModel
    {
        public string DevName { get; set; }
        public AboutViewModel()
        {
            DevName = "Abdou Djalilou Ali";
        }
    }
}
