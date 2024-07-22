using Microsoft.EntityFrameworkCore;
using MVVM;

namespace ManagPassWord.MVVM.Models
{
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
        private IList<PasswordDto> Passwords { get; set; }
        
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
        
    }

    [Index(nameof(Url), IsUnique = true)]
    public class Web:BaseEntity
    {
        public string Url { get; set; }
        public virtual IList<Password> Passwords { get; set; }

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
    }

    public class PasswordDto:BaseViewModel
    {
        public int Id { get; set; }
        private string _userName = "djaliloua@gmail.com";
        private string _password = "**************";
        private DateOnly _dateOnly;
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
        public DateOnly Date
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
    }
    public class Password:BaseEntity
    {
        public string Username { get; set; } 
        public string PasswordName { get; set; } 
        public string Note { get; set; } 
        public DateOnly Date { get; set; }  
        public virtual Web Web { get; set; }
    }
}
