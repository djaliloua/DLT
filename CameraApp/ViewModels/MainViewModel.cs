using CameraApp.DataAccess.Repository;
using CameraApp.ExtensionMethods;
using Patterns.Abstractions;
using Patterns.Implementations;
using System.Windows.Input;

namespace CameraApp.ViewModels
{
    public class LoadCameraService : ILoadService<CameraViewModel>
    {
        public IList<CameraViewModel> Reorder(IList<CameraViewModel> items)
        {
            return items.OrderByDescending(a => a.Id).ToList();
        }
    }
    public class ComboBoViewModel: Loadable<CameraViewModel>
    {
        public ICommand UpdateCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ComboBoViewModel(ILoadService<CameraViewModel> loadService) : base(loadService)
        {
            Init();
            UpdateCommand = new Command(OnUpdate);
            DeleteCommand = new Command(OnDelete);
        }
        protected override int Index(CameraViewModel item)
        {
            CameraViewModel cameraViewModel = Items.FirstOrDefault(x => x.Id == item.Id);
            return base.Index(cameraViewModel);
        }
        private async void OnDelete(object parameter)
        {
            CameraViewModel cameraViewModel = (CameraViewModel)parameter;
            bool result = await Shell.Current.DisplayAlert("Info", $"Do you want to delete {cameraViewModel.Name}", "Yes", "No");
            if (result)
            {
                using CameraRepository cameraRepository = new CameraRepository();
                await cameraRepository.DeleteItemAsync(cameraViewModel.FromDto());
                DeleteItem(cameraViewModel);
            }

            
        }
        private async void OnUpdate(object parameter)
        {
            CameraViewModel viewModel = parameter as CameraViewModel;
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"IsSave", false },
                {"CamVM", viewModel.Clone() },
            };
            await Shell.Current.GoToAsync(nameof(DialogCam), parameters);
        }
        private async void Init()
        {
            using CameraRepository cameraRepository = new CameraRepository();
            List<CameraViewModel> data = cameraRepository.GetAllAsDto() ?? new List<CameraViewModel>();
            await LoadItems(data);
        }

        public void Add(CameraViewModel viewModel)
        {
            AddItem(viewModel);
        }
        public void Update(CameraViewModel viewModel)
        {
            UpdateItem(viewModel);
        }
        public override void SelectedItemCallBack(CameraViewModel item)
        {
            ServiceLocation.MainViewModel.UpdateTitle(item.Address + (item.IsActive? " Active": " Inactive"));
        }
    }
    public class MainViewModel: BaseViewModel
    {
        private string _title;
        public string Title
        {
            get => "Home " + _title;
            set => UpdateObservable(ref _title, value);
        }
        public ComboBoViewModel ComboBox {  get; private set; }
        public ICommand NewCommand { get; private set; }
        public ICommand ShowAllCommand { get; private set; }
        public MainViewModel()
        {
            ComboBox = ServiceLocation.ComboBoViewModel;
            NewCommand = new Command(OnNew);
            ShowAllCommand = new Command(OnShowAll);
        }
        public void UpdateTitle(string title)
        {
            Title = title;
        }
        private async void OnShowAll(object parameter)
        {
            await Shell.Current.GoToAsync(nameof(DialogListOfCamera));
        }
        private async void OnNew(object parameter)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"IsSave", true },
                {"CamVM", new CameraViewModel() },
            };
            await Shell.Current.GoToAsync(nameof(DialogCam), parameters);
        }
    }
}
