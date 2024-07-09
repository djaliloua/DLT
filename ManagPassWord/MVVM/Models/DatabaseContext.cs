using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagPassWord.Models
{
    public interface IContext
    {
        // CRUD
        Task<IEnumerable<LocalDataBase>> GetAll();
        LocalDataBase GetById(int id);
        Task<int> DeleteAll();
        Task<int> DeleteById(LocalDataBase item);
        Task<int> SaveItemAsync(LocalDataBase obj);
    }



    public class DatabaseContext: IContext
    {
        SQLiteAsyncConnection Database;

        public async Task<int> SaveItemAsync(LocalDataBase item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteAll()
        {
            await Init();
            return await Database.DeleteAllAsync<LocalDataBase>();
        }

        public async Task<int> DeleteById(LocalDataBase item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }

        public async Task<IEnumerable<LocalDataBase>> GetAll()
        {
            await Init();
            return await Database.Table<LocalDataBase>().ToListAsync();
        }

        public LocalDataBase GetById(int id)
        {
            return new();
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.PasswordDataBasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<LocalDataBase>();
        }
    }
}
