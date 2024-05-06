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
        public async Task<IEnumerable<Purchases>> GetPurchasesByDate(DateTime dt)
        {
            string sql = $"select *\r\nfrom purchases p\r\nwhere p.Purchase_Date = '{dt:yyyy-MM-dd}'";
            List<Purchases> purchases = null;
            await Task.Delay(1);
            using(var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchases>();
                connection.EnableWriteAheadLogging();
                purchases = connection.Query<Purchases>(sql);
            }
            return purchases;
        }
        public async Task<int> SavePurchaseItemAsync(Purchase_Items purchase_item)
        {
            int res = 0;
            await Task.Delay(100);
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
        public async Task<long> GetTotalValue(Purchases purchases, string colname)
        {
            int res = 0;
            string sql;
            List<Result> results = new List<Result>();
            if(colname == "price")
                sql = $"select sum(pi.Item_price) Value\r\nfrom purchases p\r\ninner join purchase_items pi on pi.Purchase_Id=p.Purchase_Id\r\nwhere pi.Purchase_Id={purchases.Purchase_Id};";
            else
                sql = $"select sum(pi.Item_Quantity) Value\r\nfrom purchases p\r\ninner join purchase_items pi on pi.Purchase_Id=p.Purchase_Id\r\nwhere pi.Purchase_Id={purchases.Purchase_Id};";
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.EnableWriteAheadLogging();
                results = connection.Query<Result>(sql).ToList();
                if(results.Count == 1)
                {
                    res = results.ElementAt(0).Value;
                }
            }
            await Task.Delay(1);
            return res;
        }
        public async Task<PurchaseStatistics> GetPurchaseStatistics(int id)
        {
            await Task.Delay(1);
            PurchaseStatistics p;
            string sql = $"select ps.*\r\nfrom purchases p\r\ninner join PurchaseStatistics ps on p.Purchase_Id = ps.Purchase_Id\r\nwhere ps.Purchase_Id = {id};";
            using(SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.EnableWriteAheadLogging();
                p = connection.Query<PurchaseStatistics>(sql).ElementAt(0);
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
