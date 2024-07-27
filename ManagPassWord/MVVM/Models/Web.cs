using Microsoft.EntityFrameworkCore;
using MVVM;

namespace ManagPassWord.MVVM.Models
{
    public static class Extension
    {
        public static Web ToWeb(this WebDto webDto)
        {
            Web web = new Web();
            web.Id = webDto.Id;
            web.Url = webDto.Url;
            web.Passwords = webDto.Passwords.ToListPasswords();
            return web;
        }
        public static IList<Password> ToListPasswords(this IList<PasswordDto> passwords)
        {
            List<Password> passwords1 = new List<Password>();
            foreach (var password in passwords)
            {
                passwords1.Add(password.ToPassword());
            }
            return passwords1;
        }
        public static Password ToPassword(this PasswordDto pass)
        {
            Password password = new();
            //password.Web = pass.Web.ToWeb();
            password.Note = pass.Note;
            password.PasswordName = pass.PasswordName;
            password.Date = pass.Date;  
            password.UserName = pass.UserName;
            password.Id = pass.Id;
            return password;
        }
    }
    public class WebDto: BaseViewModel
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => UpdateObservable(ref _id, value);
        }
        private string _site;
        public string Url
        {
            get => _site;
            set => UpdateObservable(ref _site, value);
        }
        private IList<PasswordDto> _passwords;
        public IList<PasswordDto> Passwords
        {
            get => _passwords;
            set => UpdateObservable(ref _passwords, value);
        }
        
        public WebDto(string site)
        {
            Url = site;
        }
        public WebDto()
        {

        }
        public bool IsValid()
        {
            return false;
        }
        public override string ToString()
        {
            return Url;
        }
        public void Add(PasswordDto password)
        {
            Passwords ??= new List<PasswordDto>();
            Passwords.Add(password);
        }
        
    }

    [Index(nameof(Url), IsUnique = true)]
    public class Web:BaseEntity
    {
        public string Url { get; set; }
        public virtual IList<Password> Passwords { get; set; } = new List<Password>();

        public Web(string site)
        {
            Url = site;
        }
        public Web()
        {

        }
        public override string ToString()
        {
            return Url;
        }
        public void Add(Password password)
        {
            Passwords ??= new List<Password>();
            Passwords.Add(password);
        }
        public void DeletePasswordItem(PasswordDto password)
        {
            int index = Passwords.IndexOf(Passwords.Where(x => x.Id == password.Id).FirstOrDefault());
            if(index > -1)
            {
                Passwords.RemoveAt(index);
            }
            
        }
        public void UpdatePasswordItem(Password password)
        {
            for(int i = 0; i < Passwords.Count; i++)
            {
                if(password.Id == Passwords[i].Id)
                {
                    Passwords[i].Note = password.Note;
                    Passwords[i].PasswordName = password.PasswordName;
                    Passwords[i].UserName = password.UserName;
                    Passwords[i].Date = password.Date;
                    break;
                }
            }
        }
    }

    public class PasswordDto:BaseViewModel
    {
        public int Id { get; set; }
        private string _userName = "djaliloua@gmail.com";
        private string _password = "**************";
        private DateTime _dateOnly;
        private string _note = "Hello world";
        private WebDto _web;

        public string UserName
        {
            get => _userName;
            set => UpdateObservable(ref _userName, value);
        }
        public string PasswordName
        {
            get => _password;
            set => UpdateObservable(ref _password, value);
        }
        public DateTime Date
        {
            get => _dateOnly;
            set => UpdateObservable(ref _dateOnly, value);
        }

        public string Note
        {
            get => _note;
            set => UpdateObservable(ref _note, value);
        }
        public WebDto Web
        {
            get => _web;
            set => UpdateObservable(ref _web, value);
        }

        public void CreateUpdateDate()
        {
            Date = DateTime.Now;
        }
    }
    public class Password:BaseEntity
    {
        public string UserName { get; set; } 
        public string PasswordName { get; set; } 
        public string Note { get; set; } 
        public DateTime Date { get; set; }  
        public virtual Web Web { get; set; }
        
    }
}
