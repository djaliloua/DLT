using CommunityToolkit.Maui.Storage;
using CsvHelper;
using ManagPassWord.Models;
using SQLite;
using System.Globalization;

namespace ManagPassWord.Data_AcessLayer
{
    public class UserRepository : IRepository<User>
    {
        public static string folderName;
        public async Task<int> CountItemAsync()
        {
            int res = 0;
            await Task.Delay(100);
            using (var connection = new SQLiteConnection(Constants.PasswordDataBasePath, Constants.Flags))
            {
                connection.CreateTable<User>();
                res = connection.Table<User>().Count();
            }
            return res;
        }
        public UserRepository()
        {
            //this.fileSaver = fileSaver;
        }
        public async Task<User> SaveItemAsync(User item)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.PasswordDataBasePath, Constants.Flags))
            {
                connection.CreateTable<User>();
                if (item.Id != 0)
                    res = connection.Update(item);
                else
                    res = connection.Insert(item);
            }
            return item;
        }

        public async Task<int> DeleteAll()
        {
            int res = 0;
            await Task.Delay(100);
            using (var connection = new SQLiteConnection(Constants.PasswordDataBasePath, Constants.Flags))
            {
                connection.CreateTable<User>();
                res = connection.DeleteAll<User>();
            }
            return res;
        }
        async Task<string> pickFolder(CancellationToken cancellationToken)
        {
            var result = await FolderPicker.Default.PickAsync(cancellationToken);
            return result.Folder.Path;
        }
        public async Task<int> SaveToCsv()
        {
            List<User> data = new List<User>();
            CancellationToken cancellationToken = CancellationToken.None;
            string folder;
            folder = await pickFolder(cancellationToken);
            if (folder == null)
                return 0;
            folderName = folder;
            try
            {
                string sqlcmd = "SELECT * FROM Passwords";
                using (var connection = new SQLiteConnection(Constants.PasswordDataBasePath, Constants.Flags))
                {
                    connection.CreateTable<User>();
                    data = connection.Query<User>(sqlcmd);
                }
                string filepath = Path.Combine(folderName, "passwords.txt");
                using (var writer = new CsvWriter(new StreamWriter(filepath), CultureInfo.InvariantCulture))
                {
                    // Write header row (optional)
                    await writer.WriteRecordsAsync(data); // Automatically infers names from properties

                }
                return 1;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return 0;
            }
        }

        public async Task<int> DeleteById(User item)
        {
            int res = 0;
            await Task.Delay(100);
            using (var connection = new SQLiteConnection(Constants.PasswordDataBasePath, Constants.Flags))
            {
                connection.CreateTable<User>();
                res = connection.Delete(item);
            }
            return res;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            List<User> res;
            await Task.Delay(100);
            using (var connection = new SQLiteConnection(Constants.PasswordDataBasePath, Constants.Flags))
            {
                connection.CreateTable<User>();
                res = connection.Table<User>().OrderByDescending(d => d.Id).ToList();
            }
            return res;
        }

        public User GetById(int id)
        {
            return new();
        }
    }
}
