using CameraApp.DataAccess.Repository;
using CameraApp.ExtensionMethods;
using CameraApp.Models;
using System.Windows.Input;

namespace CameraApp.ViewModels
{
    public class DialogViewModel:BaseViewModel, IQueryAttributable
    {
        private CameraViewModel _cameraVM = new();
        public CameraViewModel Camera
        {
            get => _cameraVM;
            set => UpdateObservable(ref _cameraVM, value);
        }
        private bool _isSave = true;
        public bool IsSave
        {
            get => _isSave;
            set => UpdateObservable(ref _isSave, value);
        }
        public ICommand AddCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public DialogViewModel()
        {
            AddCommand = new Command(OnAdd);
            UpdateCommand = new Command(OnUpdate);
        }
        private async void OnUpdate(object parameter)
        {
            if(Camera.Error == null)
            {
                ServiceLocation.ComboBoViewModel.Update(Camera);
                using CameraRepository cameraRepository = new CameraRepository();
                Camera cam = await cameraRepository.SaveOrUpdateItemAsync(Camera.FromDto());
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Info", Camera.Error, "Cancel");
            }
        }
        private async void OnAdd(object parameter)
        {
            if (Camera.Error == null)
            {
                if(!CheckDuplicateName(Camera.Name))
                {
                    using CameraRepository cameraRepository = new CameraRepository();
                    Camera cam = await cameraRepository.SaveOrUpdateItemAsync(Camera.FromDto());
                    ServiceLocation.ComboBoViewModel.Add(cam.ToDto());
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Info", "Please, change that name", "Cancel");
                }

            }
            else
            {
                await Shell.Current.DisplayAlert("Info", Camera.Error, "Cancel");
            }
            
        }
        private static bool CheckDuplicateName(string name)
        {
            CameraViewModel viewModel = ServiceLocation.ComboBoViewModel.Items.FirstOrDefault(x => x.Name.Trim().ToLower() == name.Trim().ToLower());
            return viewModel != null;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.Count() > 0)
            {
                IsSave = (bool)query["IsSave"];
                Camera = (CameraViewModel)query["CamVM"];
            }
        }
    }
}
