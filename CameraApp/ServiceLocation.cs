using CameraApp.ViewModels;

namespace CameraApp
{
    public static class ServiceLocation
    {
        public static MainViewModel MainViewModel => GetService<MainViewModel>();
        public static DialogViewModel DialogViewModel => GetService<DialogViewModel>();
        public static ComboBoViewModel ComboBoViewModel => GetService<ComboBoViewModel>();
        public static DialogListOfCameraViewModel DialogListOfCameraViewModel => GetService<DialogListOfCameraViewModel>();
        public static T GetService<T>() => Application.Current.Handler.MauiContext.Services.GetRequiredService<T>();
    }
}
