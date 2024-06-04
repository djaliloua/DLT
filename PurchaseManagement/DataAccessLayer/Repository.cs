﻿using PurchaseManagement.MVVM.Models;
using SQLite;
using System.Data.SqlTypes;
using System.Diagnostics;

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
            string sqlComd = $"select *\r\nfrom Purchases P\r\nOrder by P.Purchase_Date desc\r\n;";
            List<Purchases> purchases = null;
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchases>();
                connection.EnableWriteAheadLogging();
                purchases = connection.Query<Purchases>(sqlComd);
                //purchases = connection.Table<Purchases>().OrderByDescending(p => p.Purchase_Date).ToList();
                for (int i = 0; i < purchases.Count; i++)
                {
                    purchases[i].PurchaseStatistics = await GetPurchaseStatistics(purchases[i].Purchase_Id);
                    //IList<Purchase_Items> purchase_items = await GetAllPurchaseItemById(purchases[i].Purchase_Id);
                    foreach (Purchase_Items purchase_item in await GetAllPurchaseItemById(purchases[i].Purchase_Id))
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
            string sqlCmd = $"select *\r\nfrom Purchase_items P\r\nwhere P.Purchase_Id = {purchaseId}\r\norder by P.Item_id desc;";
            List<Purchase_Items> purchase_items = null;
            await Task.Delay(1);
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchase_Items>();
                connection.EnableWriteAheadLogging();
                purchase_items = connection.Query<Purchase_Items>(sqlCmd);

                //purchase_items = connection.Table<Purchase_Items>().Where(p => p.Purchase_Id == purchaseId).OrderByDescending(x => x.Item_Id).ToList();
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
                
                connection.EnableWriteAheadLogging();

                try
                {
                    if (purchase.Purchase_Id != 0)
                        res = connection.Update(purchase);
                    else
                        res = connection.Insert(purchase);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            return purchase;
        }
        public async Task<Purchases> GetPurchaseByDate(DateTime dt)
        {
            await Task.Delay(1);
            Purchases purchases = null;
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchases>();
                foreach (var purchase in connection.Table<Purchases>())
                {
                    if(purchase.Purchase_Date == $"{dt:yyyy-MM-dd}")
                    {
                        purchases = purchase;
                        break;
                    }
                    
                }
            }
            return purchases;
        }
        public async Task<Purchases> GetFullPurchaseByDate(DateTime dt)
        {
            string sqlCmd = $"select *\r\nfrom Purchases P\r\nwhere P.Purchase_Date= '{dt:yyyy-MM-dd}';";
            Purchases purchases = null;
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchases>();
                var p = connection.Query<Purchases>(sqlCmd);
                if (p != null && p.Count > 0)
                {
                    purchases = p[0];
                    purchases.PurchaseStatistics = await GetPurchaseStatistics(purchases.Purchase_Id);
                    var data = await GetAllPurchaseItemById(purchases.Purchase_Id);
                    for (int i = 0; i < data.Count; i++)
                    {
                        data[i].Purchase = purchases;
                        data[i].Location = await GetMarketLocationAsync(purchases.Purchase_Id, data[i].Location_Id);
                        purchases.Purchase_Items.Add(data[i]);
                    }
                }
            }
            return purchases;
        }
        public async Task<Purchase_Items> SavePurchaseItemAsync(Purchase_Items purchase_item)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                //connection.CreateTable<Purchases>();
                //connection.CreateTable<PurchaseStatistics>();
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
                //connection.CreateTable<Purchases>();
                connection.CreateTable<PurchaseStatistics>();
                //connection.CreateTable<Purchase_Items>();
                //connection.CreateTable<MarketLocation>();
                connection.EnableWriteAheadLogging();
                purchaseStatistics ??= new();
                purchaseStatistics.Purchase_Id = purchase.Purchase_Id;
                purchaseStatistics.PurchaseCount = await CountPurchaseItems(purchase.Purchase_Id);
                purchaseStatistics.TotalPrice = await GetTotalValue(purchase, "Price");
                purchaseStatistics.TotalQuantity = await GetTotalValue(purchase, "Quantity");
                //purchase.PurchaseStatistics = purchaseStatistics;

                //if (purchase.PurchaseStatistics.Id != 0)
                //    res = connection.Update(purchase.PurchaseStatistics);
                //else
                //    res = connection.Insert(purchase.PurchaseStatistics);

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
                //connection.CreateTable<Purchases>();
                connection.CreateTable<PurchaseStatistics>();
                //connection.CreateTable<Purchase_Items>();
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
