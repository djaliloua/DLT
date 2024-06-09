using SQLite;
using MarketModels = PurchaseManagement.MVVM.Models.MarketModels;

namespace PurchaseManagement.DataAccessLayer.Repository
{

    public class LocationRepository : IGenericRepository<MarketModels.Location>
    {

        public async Task DeleteItem(MarketModels.Location item)
        {
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<MarketModels.Location>();
                connection.EnableWriteAheadLogging();
                connection.Delete(item);
            }
        }
        public async Task<IEnumerable<MarketModels.Location>> GetAllItems()
        {
            await Task.Delay(1);
            List<MarketModels.Location> items;
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<MarketModels.Location>();
                connection.EnableWriteAheadLogging();
                items = connection.Table<MarketModels.Location>().ToList();
            }
            return items;
        }
        public async Task<MarketModels.Location> GetItemById(int id)
        {
            await Task.Delay(1);
            MarketModels.Location loc = new();
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<MarketModels.Location>();
                connection.EnableWriteAheadLogging();
                loc = connection.Table<MarketModels.Location>().FirstOrDefault(s => s.Location_Id == id);
            }
            return loc;
        }

        public async Task<MarketModels.Location> SaveOrUpdateItem(MarketModels.Location item)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<MarketModels.Location>();
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
