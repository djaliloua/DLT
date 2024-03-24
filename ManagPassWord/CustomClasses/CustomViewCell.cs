#if ANDROID
using Android.Views;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif

namespace ManagPassWord.CustomClasses
{
    public class CustomViewCell : ViewCell
    {
        public CustomViewCell()
        {
#if ANDROID
    HandlerChanged += CustomViewCell_HandlerChanged;
#endif
        }

#if ANDROID
  private void CustomViewCell_HandlerChanged(object sender, EventArgs e)
  {
    ViewGroup viewGroup = (ViewGroup)((ViewCell)sender).Handler.PlatformView;
    viewGroup.SetBackgroundColor(Color.FromRgb(255,0,0).ToAndroid()); // Set desired color
  }
#endif


    }
}
