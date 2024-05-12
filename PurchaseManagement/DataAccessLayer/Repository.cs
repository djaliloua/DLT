using PurchaseManagement.MVVM.Models;
using SQLite;
using System.Linq;

namespace PurchaseManagement.DataAccessLayer
{
    class Result
    {
        public int Value { get; set; }
    }
    public class Repository : IRepository
    {
        public static string folderName;
        public async Task<MarketLocation> GetMarketLocationAsync(int purchase_id, int purchase_item_id)
        {
            MarketLocation location = null;
            await Task.Delay(1);
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchases>();
                connection.CreateTable<PurchaseStatistics>();
                connection.CreateTable<MarketLocation>();
                connection.EnableWriteAheadLogging();
                location = connection.Table<MarketLocation>().FirstOrDefault(loc => loc.Purchase_Id==purchase_id && loc.Purchase_Item_Id==purchase_item_id);
            }
            return location;
        }
        public async Task<int> SaveAndUpdateLocationAsync(MarketLocation location)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<MarketLocation>();
                connection.EnableWriteAheadLogging();
                if (location.Location_Id != 0)
                    res = connection.Update(location);
                else
                    res = connection.Insert(location);
            }
            return res;
        }
        public async Task<IEnumerable<Purchases>> GetAllPurchases()
        {
            List<Purchases> purchases = null;
            await Task.Delay(1);
            using(SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchases>();
                connection.CreateTable<PurchaseStatistics>();
                connection.EnableWriteAheadLogging();
                purchases = connection.Table<Purchases>().OrderByDescending(p => p.Purchase_Date).ToList();
            }
            return purchases;
        }
        public async Task<IList<Purchase_Items>> GetAllPurchaseItemById(int purchaseId)
        {
            List<Purchase_Items> purchase_items = null;
            await Task.Delay(1);
            string sql = $"SELECT PI.*\r\nFROM purchases P\r\nINNER JOIN\r\npurchase_items PI ON P.purchase_id = PI.purchase_id\r\n WHERE P.purchase_id = {purchaseId}\r\n order by PI.Item_Id desc\r\n;\r\n";
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchase_Items>();
                connection.EnableWriteAheadLogging();
                purchase_items = connection.Query<Purchase_Items>(sql);
            }
            return purchase_items;
        }
        public async Task<int> SavePurchaseAsync(Purchases purchase)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchases>();
                connection.EnableWriteAheadLogging();
                if (purchase.Purchase_Id != 0)
                    res = connection.Update(purchase);
                else
                    res = connection.Insert(purchase);
            }
            return res;
        }
        public async Task<Purchases> GetPurchasesByDate(DateTime dt)
        {
            var d = await GetAllPurchases();
            Purchases purchases = d.FirstOrDefault(p => p.Purchase_Date == $"{dt:yyyy-MM-dd}");
            return purchases;
        }
        public async Task<int> SavePurchaseItemAsync(Purchase_Items purchase_item)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchase_Items>();
                connection.EnableWriteAheadLogging();
                if (purchase_item.Item_Id != 0)
                    res = connection.Update(purchase_item);
                else
                    res = connection.Insert(purchase_item);
            }
            return res;
        }
        public async Task<int> SavePurchaseStatisticsItemAsyn(PurchaseStatistics purchaseStatistics)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<PurchaseStatistics>();
                connection.EnableWriteAheadLogging();
                if (purchaseStatistics.Id != 0)
                    res = connection.Update(purchaseStatistics);
                else
                    res = connection.Insert(purchaseStatistics);
            }
            return res;
        }
        public async Task<double> GetTotalValue(Purchases purchases, string colname)
        {
            var d = await GetAllPurchaseItemById(purchases.Purchase_Id);
            double result = 0;
            if (colname == "Price")
                result  = d.Sum(x => x.Item_Price);
            else
                result = d.Sum(x => x.Item_Quantity);
            return result;
        }
        public async Task<PurchaseStatistics> GetPurchaseStatistics(int id)
        {
            await Task.Delay(1);
            PurchaseStatistics p;
            using(SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.EnableWriteAheadLogging();
                p = connection.Table<PurchaseStatistics>().FirstOrDefault(s => s.Purchase_Id == id);
            }
            return p;
        }
        public async Task<int> CountPurchaseItems(int purchase_id)
        {
            IList<Purchase_Items> items = await GetAllPurchaseItemById(purchase_id);
            return items.Count();
        }
        public async Task<int> DeletePurchaseItemAsync(Purchase_Items purchase)
        {
            int res = 0;
            await Task.Delay(1);
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.EnableWriteAheadLogging();
                res = connection.Delete(purchase);
            }
            return res;
        }

    }
}
