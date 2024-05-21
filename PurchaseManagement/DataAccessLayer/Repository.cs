using PurchaseManagement.MVVM.Models;
using SQLite;
using System.Diagnostics;
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
                location = connection.Table<MarketLocation>().FirstOrDefault(loc => loc.Purchase_Id == purchase_id && loc.Purchase_Item_Id == purchase_item_id);
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
        public async Task<IList<Purchases>> GetAllPurchases()
        {
            List<Purchases> purchases = null;
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchases>();
                connection.CreateTable<PurchaseStatistics>();
                connection.CreateTable<Purchase_Items>();
                connection.EnableWriteAheadLogging();
                purchases = connection.Table<Purchases>().OrderByDescending(p => p.Purchase_Date).ToList();
                for (int i = 0; i < purchases.Count; i++)
                {
                    purchases[i].PurchaseStatistics = await GetPurchaseStatistics(purchases[i].Purchase_Id);
                    IList<Purchase_Items> purchase_items = await GetAllPurchaseItemById(purchases[i].Purchase_Id);
                    foreach (Purchase_Items purchase_item in purchase_items)
                    {
                        purchase_item.Purchase = purchases[i];
                        purchase_item.Location = await GetMarketLocationAsync(purchases[i].Purchase_Id, purchase_item.Item_Id);
                        purchases[i].Purchase_Items.Add(purchase_item);
                    }
                }
            }
            return purchases;
        }
        public async Task<IList<Purchase_Items>> GetAllPurchaseItemById(int purchaseId)
        {
            List<Purchase_Items> purchase_items = null;
            await Task.Delay(1);
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchase_Items>();
                connection.EnableWriteAheadLogging();
                purchase_items = connection.Table<Purchase_Items>().Where(p => p.Purchase_Id == purchaseId).OrderByDescending(x => x.Item_Id).ToList();
            }
            return purchase_items;
        }

        public async Task<Purchases> SavePurchaseAsync(Purchases purchase)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchases>();
                connection.CreateTable<PurchaseStatistics>();
                connection.CreateTable<Purchase_Items>();
                connection.CreateTable<MarketLocation>();
                connection.EnableWriteAheadLogging();

                try
                {
                    if (purchase.Purchase_Id != 0)
                        res = connection.Update(purchase);
                    else
                        res = connection.Insert(purchase);

                    //for (int i = 0; i < purchase.Purchase_Items.Count; i++)
                    //{
                    //    purchase.Purchase_Items[i].Purchase = purchase;
                    //    purchase.Purchase_Items[i].Purchase_Id = purchase.Purchase_Id;
                    //    await SavePurchaseItemAsync(purchase.Purchase_Items[i]);
                    //}
                    //PurchaseStatistics purchaseStatistics = await GetPurchaseStatistics(purchase.Purchase_Id);
                    //await SavePurchaseStatisticsItemAsyn(purchase, purchaseStatistics);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            return purchase;
        }
        public async Task<Purchases> GetPurchasesByDate(DateTime dt)
        {
            var d = await GetAllPurchases();
            Purchases purchases = d.FirstOrDefault(p => p.Purchase_Date.Contains($"{dt:yyyy-MM-dd}"));

            return purchases;
        }
        public async Task<Purchase_Items> SavePurchaseItemAsync(Purchase_Items purchase_item)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchases>();
                connection.CreateTable<PurchaseStatistics>();
                connection.CreateTable<Purchase_Items>();
                connection.CreateTable<MarketLocation>();
                connection.EnableWriteAheadLogging();
                if (purchase_item.Item_Id != 0)
                    res = connection.Update(purchase_item);
                else
                    res = connection.Insert(purchase_item);
                PurchaseStatistics purchaseStatistics = await GetPurchaseStatistics(purchase_item.Purchase.Purchase_Id);
                await SavePurchaseStatisticsItemAsyn(purchase_item.Purchase, purchaseStatistics);
                if (purchase_item.Location != null)
                {
                    if (purchase_item.Location.Location_Id != 0)
                        res = connection.Update(purchase_item.Location);
                    else
                        res = connection.Insert(purchase_item.Location);
                }

            }
            return purchase_item;
        }
        public async Task<PurchaseStatistics> SavePurchaseStatisticsItemAsyn(Purchases purchase, PurchaseStatistics purchaseStatistics)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchases>();
                connection.CreateTable<PurchaseStatistics>();
                connection.CreateTable<Purchase_Items>();
                connection.CreateTable<MarketLocation>();
                connection.EnableWriteAheadLogging();
                purchaseStatistics ??= new();
                purchaseStatistics.Purchase_Id = purchase.Purchase_Id;
                purchaseStatistics.PurchaseCount = await CountPurchaseItems(purchase.Purchase_Id);
                purchaseStatistics.TotalPrice = await GetTotalValue(purchase, "Price");
                purchaseStatistics.TotalQuantity = await GetTotalValue(purchase, "Quantity");
                purchase.PurchaseStatistics = purchaseStatistics;
                if (purchase.PurchaseStatistics.Id != 0)
                    res = connection.Update(purchase.PurchaseStatistics);
                else
                    res = connection.Insert(purchase.PurchaseStatistics);
                if (purchaseStatistics.Id != 0)
                    res = connection.Update(purchaseStatistics);
                else
                    res = connection.Insert(purchaseStatistics);
            }
            return purchaseStatistics;
        }
        public async Task<double> GetTotalValue(Purchases purchases, string colname)
        {
            var d = await GetAllPurchaseItemById(purchases.Purchase_Id);
            double result = 0;
            if (colname == "Price")
                result = d.Sum(x => x.Item_Price);
            else
                result = d.Sum(x => x.Item_Quantity);
            return result;
        }
        public async Task<PurchaseStatistics> GetPurchaseStatistics(int id)
        {
            await Task.Delay(1);
            PurchaseStatistics p;
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchases>();
                connection.CreateTable<PurchaseStatistics>();
                connection.CreateTable<Purchase_Items>();
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
                PurchaseStatistics purchaseStatistics = await GetPurchaseStatistics(purchase.Purchase_Id);
                await SavePurchaseStatisticsItemAsyn(purchase.Purchase, purchaseStatistics);
                MarketLocation loc = await GetMarketLocationAsync(purchase.Purchase.Purchase_Id, purchase.Item_Id);
                if (loc != null)
                    res = connection.Delete(loc);
            }
            return res;
        }

    }
}
