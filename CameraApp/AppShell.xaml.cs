namespace CameraApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DialogCam), typeof(DialogCam));
            Routing.RegisterRoute(nameof(DialogListOfCamera), typeof(DialogListOfCamera));
        }
    }
}
