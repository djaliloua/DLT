using SQLite;

namespace ManagPassWord.Models
{
    [Table("Passwords")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(250)]
        public string Site { get; set; }
        [MaxLength(250)]
        public string Username { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        [MaxLength(500)]
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
