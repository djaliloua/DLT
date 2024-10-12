namespace CameraApp.ViewModels
{
    public class DialogListOfCameraViewModel:BaseViewModel
    {
        public ComboBoViewModel ListOfCamera { get; private set; }
        public DialogListOfCameraViewModel()
        {
            ListOfCamera = ServiceLocation.ComboBoViewModel;
        }
    }
}
