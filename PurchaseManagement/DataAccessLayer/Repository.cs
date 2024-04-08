using PurchaseManagement.MVVM.Models;
using SQLite;

namespace PurchaseManagement.DataAccessLayer
{
    public class Repository : IRepository
    {
        SQLiteAsyncConnection Database;
        public static string folderName;
        public async Task<IEnumerable<Purchases>> GetAllPurchases()
        {
            await Init();
            return await Database.Table<Purchases>().OrderByDescending(p => p.Purchase_Id).ToListAsync();
        }
        public async Task<IEnumerable<Purchase_Items>> GetAllPurchaseItemById(int purchaseId)
        {
            await Init();
            string sql = $"SELECT PI.*\r\nFROM purchases P\r\nINNER JOIN\r\npurchase_items PI ON P.purchase_id = PI.purchase_id\r\n WHERE P.purchase_id = {purchaseId}\r\n order by PI.Item_Id desc\r\n;\r\n";
            return await Database.QueryAsync<Purchase_Items>(sql);
        }
        public async Task<int> SavePurchaseAsync(Purchases purchase)
        {
            await Init();
            if(purchase.Purchase_Id != 0)
            {
                return await Database.UpdateAsync(purchase);
            }
           return await Database.InsertAsync(purchase);
        }
        public async Task<IEnumerable<Purchases>> GetPurchasesByDate()
        {
            string sql = $"select *\r\nfrom purchases p\r\nwhere p.Purchase_Date = '{DateTime.Now.ToString("yyy-MM-dd")}'";
            //p => p.Purchase_Date == DateTime.Now.ToString("yyy-MM-dd")
            return await Database.QueryAsync<Purchases>(sql);
        }
        public async Task<int> SavePurchaseItemAsync(Purchase_Items purchase_item)
        {
            await Init();
            if(purchase_item.Item_Id != 0)
                return await Database.UpdateAsync(purchase_item);
            return await Database.InsertAsync(purchase_item);
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
