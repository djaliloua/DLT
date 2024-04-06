﻿namespace ManagPassWord.Models
{
    public class Constants
    {
        public const string DatabaseFilename = "PassWordSqlite.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
        public static string DatabasePathCompany =>
            Path.Combine(FileSystem.AppDataDirectory, "company.db3");
        public static string PasswordDataBasePath =>
            Path.Combine(FileSystem.AppDataDirectory, "password.db3");
    }
}
