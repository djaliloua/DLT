using CommunityToolkit.Maui.Storage;
using CsvHelper;
using ManagPassWord.Models;
using SQLite;
using System.Globalization;

namespace ManagPassWord.Data_AcessLayer
{
    public class UserRepository: IRepository<User>
    {
        SQLiteAsyncConnection Database;
        public static string folderName;
        public async Task<int> CountItemAsync()
        {
            return await Database.Table<User>().CountAsync();
        }
        public UserRepository()
        {
            //this.fileSaver = fileSaver;
        }
        public async Task<int> SaveItemAsync(User item)
        {
            await Init();
            //
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteAll()
        {
            await Init();
            return await Database.DeleteAllAsync<User>();
        }
        async Task<string> pickFolder(CancellationToken cancellationToken)
        {
            var result = await FolderPicker.Default.PickAsync(cancellationToken);
            return result.Folder.Path;
        }
        public async Task<int> SaveToCsv()
        {
            CancellationToken cancellationToken = CancellationToken.None;
            string folder;
            folder = await pickFolder(cancellationToken);
            if (folder == null)
                return 0;
            folderName = folder;
            try
            {
                await Init();
                string sqlcmd = "SELECT * FROM Passwords";
                List<User> data = await Database.QueryAsync<User>(sqlcmd);
                string filepath = Path.Combine(folderName, "passwords.txt");
                using (var writer = new CsvWriter(new StreamWriter(filepath), CultureInfo.InvariantCulture))
                {
                    // Write header row (optional)
                    await writer.WriteRecordsAsync(data); // Automatically infers names from properties

                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> DeleteById(User item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            await Init();
            return await Database.Table<User>().OrderByDescending(user => user.Id).ToListAsync();
        }

        public User GetById(int id)
        {
            return new();
        }
        public async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.PasswordDataBasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<User>();
        }
    }
}
