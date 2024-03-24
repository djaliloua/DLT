namespace ManagPassWord.ViewModels
{
    public class AboutViewModel:BaseViewModel
    {
        public string Version { get; set; }
        public AboutViewModel()
        {
            Version = AppInfo.Current.VersionString;
        }
    }
}
