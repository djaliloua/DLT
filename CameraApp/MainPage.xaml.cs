#if ANDROID
using CameraApp.Platforms.Android;
#endif
namespace CameraApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
#if ANDROID
            // Request the permission
            ScreenControl.KeepScreenOn();
#endif
        }
#if ANDROID
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Optionally allow the screen to sleep again when leaving the page
            ScreenControl.AllowScreenSleep();
        }
#endif
    }

}
