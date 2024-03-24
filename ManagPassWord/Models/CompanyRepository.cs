using SQLite;

namespace ManagPassWord.Models
{
    public class CompanyRepository
    {
        SQLiteAsyncConnection Database;
        async Task Init()
        {
            if (Database is not null)
                return;

            try
            {
                Database = new SQLiteAsyncConnection(Constants.DatabasePathCompany, Constants.Flags);
                var result = await Database.CreateTableAsync<CompanyModel>();
                
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error",ex.Message , "ok");
            }
        }
        public async Task<List<CompanyModel>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<CompanyModel>().OrderByDescending(x => x.Id).ToListAsync();
        }
        public async Task<int> DeleteItemAsync(CompanyModel item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
        
        public async Task<int> SaveItemAsync(CompanyModel item)
        {
            await Init();
            
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }
    }
}
