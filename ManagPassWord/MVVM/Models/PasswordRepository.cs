using SQLite;

namespace ManagPassWord.Models
{
    public class PasswordRepository
    {
        //string _dbPath;
        SQLiteAsyncConnection Database;
        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<PasswordModel>();
        }
        
        public async Task<List<PasswordModel>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<PasswordModel>().ToListAsync();
        }
        public async Task<List<PasswordModel>> GetItemsAsync(CompanyModel comp)
        {
            await Init();
            if (comp == null)
                return null;
            return await Database.Table<PasswordModel>().Where(x => x.CompanyName==comp.Name).ToListAsync();
        }
        public async Task DeleteAll()
        {
            await Init();
            //return await Database.Table<PasswordModel>().Where(x => x.CompanyName == comp).ToListAsync();
        }
        public async Task<int> DeleteItemAsync(PasswordModel item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
        public async Task<int> SaveItemAsync(PasswordModel item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }
    }
}
