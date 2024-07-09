
using ManagPassWord.Models;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;

namespace ManagPassWord.ViewModels
{
    public class FactoryObject
    {
        public static CompanyModel Create(string name)
        {
            CompanyModel obj = new CompanyModel();
            obj.Name = name;
            return obj;
        }
    }
    public class StartPageViewModel:BaseViewModel
    {
        public static event Action<List<CompanyModel>> UpdateComboxBox;
        protected virtual void OnUpdateComboxBox(List<CompanyModel> s) => UpdateComboxBox?.Invoke(s);
        private string _name;
        public string Name
        {
            get => _name;
            set => UpdateObservable(ref _name, value);
        }
        private bool _isEnable;
        public bool IsEnable
        {
            get => _isEnable;
            set => UpdateObservable(ref _isEnable, value);
        }
        public static  CompanyRepository companyDatabase;
        private List<CompanyModel> _companyList;
        public List<CompanyModel> CompanyList
        {
            get => _companyList;
            set => UpdateObservable(ref _companyList, value, () =>
            {
                IsEnable = value.Count > 0;
            });
        }
        private CompanyModel _company;
        public CompanyModel Company
        {
            get => _company;
            set => UpdateObservable(ref _company, value);
        }

        public ICommand AddCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public StartPageViewModel(CompanyRepository companyDatabase1)
        {
            AddCommand = new Command(add);
            DeleteCommand = new Command(delete);
            SaveCommand = new Command(save);
            companyDatabase = companyDatabase1;
            CompanyModel.Refresh += CompanyModel_Refresh;
            Refresh();
        }
        private async void save(object o)
        {
            
            OnUpdateComboxBox(CompanyList);
            //await Shell.Current.DisplayAlert("Saved", "Saved", "Ok");
            await Dialog.ShowToast("Done");
            //var ok = await Vapolia.UserInteraction.Confirm("Are you sure?");
        }

        private void CompanyModel_Refresh()
        {
            Refresh();
        }

        private async void Refresh()
        {
            CompanyList = await companyDatabase.GetItemsAsync();
        }
        private async void delete(object o)
        {
            if(Company != null)
            {
                await companyDatabase.DeleteItemAsync(Company);
                Refresh();
                OnUpdateComboxBox(CompanyList);
            }
        }
        private bool checkDuplicate(string name)
        {
            return CompanyList.Where(x => x.Name == name).Any();
        }
        private async void add(object o)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                
                if (!checkDuplicate(Name))
                {
                    await companyDatabase.SaveItemAsync(FactoryObject.Create(Name));
                    Refresh();
                }
                else
                    await Shell.Current.DisplayAlert("Warning", "Please, do not add same name twice", "Ok");
                Name = string.Empty;
            }
            else
                await Shell.Current.DisplayAlert("Empty", "Please, write the company's name", "Ok");

        }
    }
}
