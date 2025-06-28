using Android.App;
using Android.Runtime;

namespace PurchaseManagement
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }
        public override void OnCreate()
        {
            base.OnCreate();
            // Initialize Firebase Push Notifications
            // Initialize any libraries or services here
            Platform.Init(this);
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}