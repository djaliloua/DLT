using ManagPassWord.Models;
using System.Windows.Input;

namespace ManagPassWord.ViewModels
{
    public class FactoryMethod
    {
        //PasswordModel password;
        public static PasswordModel Create(string comp) 
        { 
            PasswordModel model = new PasswordModel();
            model.Password = Utility.GenerateStrongPassword(comp);
            model.CompanyName = comp;
            
            return model;
        }
        public static PasswordModel Create(string comp, int len)
        {
            PasswordModel model = new PasswordModel();
            model.Password = Utility.GenerateStrongPassword(comp, len);
            model.CompanyName = comp;

            return model;
        }
        public static PasswordModel Create(string comp, string passw)
        {
            PasswordModel model = new PasswordModel();
            model.Password = passw;
            model.CompanyName = comp;

            return model;
        }
        public static PasswordModel Create(string d, string name, string passw)
        {
            PasswordModel model = new PasswordModel();
            model.Date = d;
            model.Password = passw;
            model.CompanyName = name;
            //model.ShowCommand = new Command(async ()=> await Shell.Current.DisplayAlert("Hello", "hh", "ok"));
            return model;
        }
    }
    public class MainViewModel: BaseViewModel
    {
        public static PasswordRepository database;
        public static CompanyRepository companyDatabase;
        private List<CompanyModel> companies = new List<CompanyModel>();
        public List<CompanyModel> Companies
        {
            get => companies;
            set => UpdateObservable(ref companies, value, () =>
            {
                
                IsEnable = value.Count != 0;
                if (value.Count > 0)
                {
                    SelectedComp = value[0];
                }
            });
        }
        private CompanyModel selectedComp;
       
        public CompanyModel SelectedComp
        {
            get => selectedComp;
            set => UpdateObservable(ref selectedComp, value, async () =>
            {
                ListOfPasswords = await database.GetItemsAsync(SelectedComp);
            });
        }
        private List<PasswordModel> listOfPasswords;
        public List<PasswordModel> ListOfPasswords
        {
            get => listOfPasswords ?? new List<PasswordModel>();
            set => UpdateObservable(ref listOfPasswords, value, () =>
            {
                if (value is not null)
                    value.Reverse();
            });
            
        }
        private PasswordModel selectedPassword;
        public PasswordModel SelectedPassword
        {
            get => selectedPassword;
            set => UpdateObservable(ref selectedPassword, value);
        }

        private bool _isEnable;
        public bool IsEnable
        {
            get => _isEnable;
            set => UpdateObservable(ref _isEnable, value);
        }
        public ICommand AddCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public MainViewModel(PasswordRepository _db, CompanyRepository companyRepository)
        {
            database = _db;
            companyDatabase = companyRepository;
            init();
            Console.WriteLine($"Hello {DeviceInfo.Current.DeviceType}");
            AddCommand = new Command(add);
            DeleteCommand = new Command(delete);
            StartPageViewModel.UpdateComboxBox += StartPageViewModel_UpdateComboxBox;
            DetailsViewModel.DetailsChanged += DetailsViewModel_DetailsChanged;
        }

        private async void DetailsViewModel_DetailsChanged(PasswordModel obj)
        {
            SelectedPassword.Password = obj.Password;
            await database.SaveItemAsync(SelectedPassword);
            ListOfPasswords = await database.GetItemsAsync(SelectedComp);
        }

        private void StartPageViewModel_UpdateComboxBox(List<CompanyModel> obj)
        {
            Companies = obj;
        }

        private async void delete(object o)
        {
            if(SelectedPassword != null)
            {
                await database.DeleteItemAsync(SelectedPassword);
                ListOfPasswords = await database.GetItemsAsync(SelectedComp);
            }
            await Dialog.ShowToast("Please, Select an Item");
            
        }
        private async void add(object o)
        {
            string action = await Shell.Current.DisplayActionSheet("Options", "Cancel", null, "Random", "User");
            if(action == "Random")
            {
                await Task.Delay(100);
                string passWord_length = await Shell.Current.DisplayPromptAsync(action, "Length:>= 8");
                if(!string.IsNullOrEmpty(passWord_length))
                {
                    int n = int.Parse(passWord_length);
                    if(n >= 8)
                    {
                        await database.SaveItemAsync(FactoryMethod.Create(SelectedComp.Name, n));
                    }
                    else
                    {
                        await Task.Delay(100);
                        await Shell.Current.DisplayAlert("Warning", "Length must be greater than 8 characters", "Ok");
                    }
                }
                else
                {
                    await database.SaveItemAsync(FactoryMethod.Create(SelectedComp.Name));
                }
                
            }
            if(action == "User")
            {
                string _password = await Shell.Current.DisplayPromptAsync("User", "New password?");
                await Task.Delay(100);
                await database.SaveItemAsync(FactoryMethod.Create(SelectedComp.Name, _password));
            }
            if (action == "Cancel")
            {
                return;
            }
            ListOfPasswords = await database.GetItemsAsync(SelectedComp);

        }
        private async void init()
        {
            Companies = await companyDatabase.GetItemsAsync();
            var x = await database.GetItemsAsync(SelectedComp);
            if( x != null)
            {
                ListOfPasswords = x;
            }
            
            
        }
        
    }
}
