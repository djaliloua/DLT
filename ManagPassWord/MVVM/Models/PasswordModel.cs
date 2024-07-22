using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using ManagPassWord.ViewModels;
using PasswordGenerator;
using SQLite;
using System.Text.Json;
using System.Windows.Input;

namespace ManagPassWord.Models
{
    [Table("Passwords")]
    public class LocalDataBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(250), Unique]
        public string Site { get; set; }
        [MaxLength(250)]
        public string Username { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        [MaxLength(500)]
        public string Note { get; set; }
        public LocalDataBase(string site, string username, string password, string note)
        {
            Site = site;
            Username = username;
            Password = password;
            Note = note;
        }
        public LocalDataBase()
        {
            
        }
    }
    [Table("Company")]
    public class CompanyModel
    {
        public static event Action Refresh;
        protected virtual void OnRefresh() => Refresh?.Invoke();

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(250), Unique]
        public string Name { get; set; }
        public ICommand DeleteCommand { get; private set; }
        public CompanyModel()
        {
            DeleteCommand = new Command(delete);
        }
        private async void delete(object o)
        {
            await StartPageViewModel.companyDatabase.DeleteItemAsync(this);
            Refresh();
        }
    }
    [Table("Passwords")]
    public class PasswordModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Date { get; set; }= DateTime.Now.ToString("d");
        public string CompanyName { get; set; }
        public string Password { get; set; }
        
        public ICommand ShowCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public PasswordModel()
        {
            
            ShowCommand = new Command(show);
            DeleteCommand = new Command(delete);
        }
        private async void delete(object o)
        {
            await MainViewModel.database.DeleteItemAsync(this);
        }
        private async void show(object o)
        {
            await MainPage.Authentificate(FactoryMethod.Create(Date, CompanyName, Password));
        }
        
        
    }
    public class Utility
    {
        static string filename = "path.json";
        static string Dir = FileSystem.Current.AppDataDirectory;
        public static string Filename = Path.Join(Dir, filename);
        public static async Task SerializeObjectToJson(IEnumerable<PasswordModel> personList)
        {
            if (personList.Count() != 0)
            {
                using FileStream fileStream = File.Create(filename);
                await JsonSerializer.SerializeAsync(fileStream, personList);
                //await ShowSnackBar("Saved");
            }
            else
            {
                //if (File.Exists(Filename))
                //    File.Delete(Filename);
            }

        }
        public static IEnumerable<PasswordModel> Load()
        {
            if (File.Exists(filename))
            {
                string jsonString = File.ReadAllText(filename);
                if (jsonString == "")
                    return new List<PasswordModel>();
                return JsonSerializer.Deserialize<IEnumerable<PasswordModel>>(jsonString)!;
            }
            return new List<PasswordModel>();

        }
        private static string generateStrongPassword(string comp)
        {
            if (string.IsNullOrEmpty(comp))
                return "Not set";
            string numbers = "0123456789";
            string letters = "abcdefghijklmopqrstuvwyz";
            string Letters = "ABCDEFGHIJKLMNOPQRSTUVYWZ";
            string specialChar = ",;:.-_#@$£&!";
            if(comp == "Capgemini")
                return generaterandomValues(numbers, 6) + generaterandomValues(specialChar, 1) + generaterandomValues(letters, 3);
            return generaterandomValues(Letters, 1) + generaterandomValues(numbers, 6) + generaterandomValues(specialChar, 1)+ generaterandomValues(letters, 3);
            //return generaterandomValues(Letters, 8) + generaterandomValues(letters, 5) + generaterandomValues(numbers, 2) + generaterandomValues(specialChar, 1);
        }
        public static string GenerateStrongPassword(string comp, int? passWord_len)
        {
            Password pwd;
           
            if (string.IsNullOrEmpty(comp))
                return "Not set";
            //if (comp.Equals("Iveco"))
            //    return generateStrongPassword(comp);
            //if (comp.Equals("Capgemini"))
            //    return generateStrongPassword(comp);
            if(passWord_len.HasValue)
                pwd = new Password(passWord_len.Value);
            else
                pwd = new Password();


            return pwd.Next();
        }
        public static string GenerateStrongPassword(string comp)
        {
            if (string.IsNullOrEmpty(comp))
                return "Not set";
            
            if (comp.Equals("Iveco"))
                return generateStrongPassword(comp);
            if(comp.Equals("Capgemini"))
                return generateStrongPassword(comp);

            Password pwd = new Password();
            return pwd.Next();
        }
        private static string generaterandomValues(string value, int length)
        {
            string result = "";
            Random random = new Random();
            
            for(int j = 0; j < length; j++)
            {
                int i = random.Next(0, value.Length);
                result += value[i];
            }
            return result;
        }
        
    }
    public class Dialog
    {
        public static async Task ShowSnackBar(string msg, Action action)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.White,
                ActionButtonTextColor = Colors.Yellow,
                CornerRadius = new CornerRadius(10),
                Font = Microsoft.Maui.Font.SystemFontOfSize(14),
                ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(14),
                CharacterSpacing = 0.5
            };
            //string text = "This is a Snackbar";
            string actionButtonText = "Undo";
            TimeSpan duration = TimeSpan.FromSeconds(3);
            //
            var snackbar = Snackbar.Make(msg, action, actionButtonText, duration, snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
        }
        public static async Task ShowSnackBar()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Colors.LightCoral,
                TextColor = Colors.White,
                ActionButtonTextColor = Colors.White,
                CornerRadius = new CornerRadius(10),
                Font = Microsoft.Maui.Font.SystemFontOfSize(14),
                ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(14),
                CharacterSpacing = 0.5
            };
            string text = "This is a Snackbar";
            string actionButtonText = "Undo";
            TimeSpan duration = TimeSpan.FromSeconds(3);
            Action action = async () => await Shell.Current.DisplayAlert("Snackbar ActionButton Tapped", "The user has tapped the Snackbar ActionButton", "OK");
            var snackbar = Snackbar.Make(text, action, actionButtonText, duration, snackbarOptions);
            await snackbar.Show(cancellationTokenSource.Token);
        }
        public static async Task ShowToast(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            //string text = "This is a Toast";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(message, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }
    }

}
