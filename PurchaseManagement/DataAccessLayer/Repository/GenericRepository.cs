using PurchaseManagement.MVVM.Models;
using PurchaseManagement.DataAccessLayer.Abstractions;
using SQLite;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        public async Task DeleteItem(T item)
        {
            await Task.Delay(1);
            using (SQLiteConnection connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<T>();
                connection.Delete(item);
            }
        }

        public async Task<IEnumerable<T>> GetAllItems()
        {
            await Task.Delay(1);
            List<T> items;
            using (SQLiteConnection connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                var x = connection.CreateTable<T>();
                items = connection.Table<T>().ToList();
            }
            return items;
        }

        public async Task<T> GetItemById(int id)
        {
            await Task.Delay(1);
            T product;
            using (SQLiteConnection connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<T>();
                product = connection.Find<T>(id);
            }
            return product;
        }

        public async Task<T> SaveOrUpdateItem(T item)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<T>();
                if (item.Id != 0)
                    res = connection.Update(item);
                else
                    res = connection.Insert(item);
            }
            return item;
        }
    }
}
