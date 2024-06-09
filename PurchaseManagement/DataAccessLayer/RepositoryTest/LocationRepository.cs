using SQLite;

namespace PurchaseManagement.DataAccessLayer.RepositoryTest
{
    public class LocationRepository : IGenericRepository<MVVM.Models.Location>
    {
        public async Task DeleteItem(MVVM.Models.Location item)
        {
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<MVVM.Models.Location>();
                connection.EnableWriteAheadLogging();
                connection.Delete(item);
            }
        }

        public async Task<IEnumerable<MVVM.Models.Location>> GetAllItems()
        {
            await Task.Delay(1);
            List<MVVM.Models.Location> items;
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<MVVM.Models.Location>();
                connection.EnableWriteAheadLogging();
                items = connection.Table<MVVM.Models.Location>().ToList();
            }
            return items;
        }

        public async Task<MVVM.Models.Location> GetItemById(int id)
        {
            await Task.Delay(1);
            MVVM.Models.Location loc = new();
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<MVVM.Models.Location>();
                connection.EnableWriteAheadLogging();
                loc = connection.Table<MVVM.Models.Location>().FirstOrDefault(s => s.Location_Id == id);
            }
            return loc;
        }
        public async Task<MVVM.Models.Location> SaveOrUpdateItem(MVVM.Models.Location item)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<MVVM.Models.Location>();
                connection.EnableWriteAheadLogging();
                if (item.Location_Id != 0)
                    res = connection.Update(item);
                else
                    res = connection.Insert(item);
            }
            return item;
        }
    }
}
