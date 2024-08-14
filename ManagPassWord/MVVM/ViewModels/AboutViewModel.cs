using MVVM;

namespace ManagPassWord.MVVM.ViewModels
{
    public class AboutViewModel:BaseViewModel
    {
        public string Version { get; private set; }
        public string MarkDown { get; private set; } = "## Password Management System(PMS)\r\n### Project description\r\nPassword Management System(PMS) is an application for managing and creating password on the fly.\r\n### Technologies\r\n- WPF\r\n- .Net8\r\n- MVVM\r\n- Sqlite\r\n- Urannium\r\n- Entity framework\r\n- FingerPrint\r\n- Mapster\r\n- CommunityToolkit.Maui\r\n- CommunityToolkit.Mvvm\r\n- Indiko.Maui.Controls.Markdown\r\n\r\n### Author\r\n- Software developer @self employement\r\n- Abdou Djalilou Ali\r\n- djalilouagmail.com";
        public AboutViewModel()
        {
            Version = AppInfo.Current.VersionString;
        }
    }
}
