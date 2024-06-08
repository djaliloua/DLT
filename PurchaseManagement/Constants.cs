﻿using Microsoft.Maui.Storage;
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
        public static string GetRestUrl(string id, string parameter)
        {
            string baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5156" : "http://localhost:5156";
            string baseUrl;
            if (id == null)
                baseUrl = $"{baseAddress}/api/{parameter}";
            else
                baseUrl = $"{baseAddress}/api/{parameter}/{id}";
            
            return baseUrl;
        }
    }
}
