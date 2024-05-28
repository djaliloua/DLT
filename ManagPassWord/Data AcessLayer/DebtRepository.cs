using CommunityToolkit.Maui.Storage;
using CsvHelper;
using ManagPassWord.Models;
using SQLite;
using System.Globalization;

namespace ManagPassWord.Data_AcessLayer
{
    public class DebtRepository : IRepository<DebtModel>
    {
        public static string folderName;
        public async Task<int> CountItemAsync()
        {
            int res = 0;
            await Task.Delay(100);
            using (var connection = new SQLiteConnection(Constants.PasswordDataBasePath, Constants.Flags))
            {
                connection.CreateTable<DebtModel>();
                res = connection.Table<DebtModel>().Count();
            }
            return res;
        }
       
        public async Task<DebtModel> SaveItemAsync(DebtModel item)
        {
            int res = 0;
            await Task.Delay(100);
            using (var connection = new SQLiteConnection(Constants.PasswordDataBasePath, Constants.Flags))
            {
                connection.CreateTable<DebtModel>();
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
                connection.CreateTable<DebtModel>();
                res = connection.DeleteAll<DebtModel>();
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
            List<DebtModel> data = new List<DebtModel>();
            CancellationToken cancellationToken = CancellationToken.None;
            string folder;
            folder = await pickFolder(cancellationToken);
            if (folder == null)
                return 0;
            folderName = folder;
            try
            {
                string sqlcmd = "SELECT * FROM Debts";
                using (var connection = new SQLiteConnection(Constants.PasswordDataBasePath, Constants.Flags))
                {
                    connection.CreateTable<DebtModel>();
                    data = connection.Query<DebtModel>(sqlcmd);
                }
                string filepath = Path.Combine(folderName, "debts.txt");
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

        public async Task<int> DeleteById(DebtModel item)
        {
            int res = 0;
            await Task.Delay(100);
            using (var connection = new SQLiteConnection(Constants.PasswordDataBasePath, Constants.Flags))
            {
                connection.CreateTable<DebtModel>();
                res = connection.Delete(item);
            }
            return res;
        }

        public async Task<IEnumerable<DebtModel>> GetAll()
        {
            List<DebtModel> res;
            await Task.Delay(100);
            using (var connection = new SQLiteConnection(Constants.PasswordDataBasePath, Constants.Flags))
            {
                connection.CreateTable<DebtModel>();
                res = connection.Table<DebtModel>().OrderByDescending(d => d.DebtDate).ToList();
            }
            return res;
        }
        public DebtModel GetById(int id)
        {
            return new();
        }
    }
}
