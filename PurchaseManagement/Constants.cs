using Microsoft.Maui.Storage;
namespace PurchaseManagement
{
    public class Constants
    {
        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;
        public static string DatabasePurchase =>
            Path.Combine(FileSystem.AppDataDirectory, "Purchase.db3");
    }
}
