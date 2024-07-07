using ManagPassWord.DataAcessLayer;
using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.Models;
using MVVM;
using System.Windows.Input;

namespace ManagPassWord.ViewModels.Password
{
    public class PasswordSettingViewModel:BaseViewModel
    {
        private string basePath = FileSystem.AppDataDirectory;
        private readonly IPasswordRepository _userRepository;
        private string temp;
        private string _fileName = "";
        public string FileName
        {
            get => _fileName.Replace(basePath, "");
            set => UpdateObservable(ref _fileName, value);
        }
        bool isVirtual = DeviceInfo.Current.DeviceType switch
        {
            DeviceType.Physical => false,
            DeviceType.Virtual => true,
            _ => false
        };
        public ICommand ExportCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public PasswordSettingViewModel(IPasswordRepository db)
        {
            _userRepository = db;
            temp = Path.Combine(basePath, "passwords.csv");
            load();
            OpenCommand = new Command(open);
            ExportCommand = new Command(export);
        }
        private void load()
        {
            if (File.Exists(temp))
            {
                FileName = temp;
            }
        }
        private async void open(object sender)
        {
            if (File.Exists(FileName))
            {
                await Launcher.OpenAsync(new OpenFileRequest("Read csv file", new ReadOnlyFile(Path.Combine(basePath, FileName))));
                await MessageDialogs.ShowToast("Opened..");
                return;
            }
            await Shell.Current.DisplayAlert("Warning", $"File not present", "OK");
        }
        private async void export(object sender)
        {
            var res = await _userRepository.SaveToCsv();
            if (res == 1)
            {
                //FileName = Path.Combine(UserRepository.folderName, "passwords.txt");
                //await MessageDialogs.ShowToast($"{UserRepository.folderName}");
            }

        }
    }
}
