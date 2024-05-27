using MVVM;
using Microsoft.Maui.ApplicationModel;

namespace ManagPassWord.ViewModels
{
    public class AboutViewModel:BaseViewModel
    {
        public string Version { get; private set; }
        public AboutViewModel()
        {
            Version = AppInfo.Current.VersionString;
        }
    }
}
