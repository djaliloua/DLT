using PurchaseManagement.MVVM.Models;
using SQLite;

namespace PurchaseManagement.DataAccessLayer
{
    class SqliteAsyncConnection : SQLiteAsyncConnection, IDisposable
    {
        public SqliteAsyncConnection(string databasepath, SQLiteOpenFlags flag):base(databasepath, flag)
        {
            
        }
        public void Dispose()
        {
            
        }
    }
    public class Repository : IRepository
    {
        SQLiteAsyncConnection Database;
        public static string folderName;
        public async Task<IEnumerable<Purchases>> GetAllPurchases()
        {
            List<Purchases> purchases = null;
            using(SqliteAsyncConnection connection = new SqliteAsyncConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                await connection.CreateTableAsync<Purchases>();
                purchases = await connection.Table<Purchases>().OrderByDescending(p => p.Purchase_Id).ToListAsync();
            }
            return purchases;
        }
        public async Task<IEnumerable<Purchase_Items>> GetAllPurchaseItemById(int purchaseId)
        {
            List<Purchase_Items> purchase_items = null;
            await Task.Delay(100);
            string sql = $"SELECT PI.*\r\nFROM purchases P\r\nINNER JOIN\r\npurchase_items PI ON P.purchase_id = PI.purchase_id\r\n WHERE P.purchase_id = {purchaseId}\r\n order by PI.Item_Id desc\r\n;\r\n";
            using (SqliteAsyncConnection connection = new SqliteAsyncConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                await connection.CreateTableAsync<Purchase_Items>();
                purchase_items = await connection.QueryAsync<Purchase_Items>(sql);
            }
            return purchase_items;
        }
        public async Task<int> SavePurchaseAsync(Purchases purchase)
        {
            int res = 0;
            await Task.Delay(100);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchases>();
                if (purchase.Purchase_Id != 0)
                    res = connection.Update(purchase);
                else
                    res = connection.Insert(purchase);
            }
            return res;
        }
        public async Task<IEnumerable<Purchases>> GetPurchasesByDate()
        {
            string sql = $"select *\r\nfrom purchases p\r\nwhere p.Purchase_Date = '{DateTime.Now.ToString("yyy-MM-dd")}'";
            List<Purchases> purchases = null;
            await Task.Delay(100);
            using(var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchases>();
                purchases = connection.Query<Purchases>(sql).ToList();
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
                if (purchase_item.Item_Id != 0)
                    res = connection.Update(purchase_item);
                else
                    res = connection.Insert(purchase_item);
            }
            return res;
        }
        public async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePurchase, Constants.Flags);
            await Database.CreateTableAsync<Purchase_Items>();
            await Database.CreateTableAsync<Purchases>();
            
        }
        
    }
}
