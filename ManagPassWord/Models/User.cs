using MVVM;

namespace ManagPassWord.Models
{
    public class UserDTO: BaseViewModel
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => UpdateObservable(ref _id, value);
        }
        private string _site;
        public string Site
        {
            get => _site;
            set => UpdateObservable(ref _site, value);
        }
        private string _userName;
        public string Username
        {
            get => _userName;
            set => UpdateObservable(ref _userName, value);
        }
        private string _passWord;
        public string Password
        {
            get => _passWord;
            set => UpdateObservable(ref _passWord, value);
        }
        private string _note;
        public string Note
        {
            get => _note;
            set => UpdateObservable(ref _note, value);  
        }
        public UserDTO(string site, string username, string password, string note)
        {
            Site = site;
            Username = username;
            Password = password;
            Note = note;
        }
        public UserDTO()
        {

        }
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Site) && !string.IsNullOrEmpty(Password);
        }
        public override string ToString()
        {
            return Site;
        }
        public void Reset()
        {
            Id = 0;
            Username = string.Empty;
            Password = string.Empty;
            Note = string.Empty;
            Site = string.Empty;
        }
    }

    public class User:BaseEntity
    {
        public string Site { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Note { get; set; }
        public User(string site, string username, string password, string note)
        {
            Site = site;
            Username = username;
            Password = password;
            Note = note;
        }
        public User()
        {

        }
        public override string ToString()
        {
            return Site;
        }
    }
}
