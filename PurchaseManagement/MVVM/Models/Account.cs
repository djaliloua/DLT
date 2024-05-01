﻿using SQLite;

namespace PurchaseManagement.MVVM.Models
{
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Money { get; set; }
        public DateTime DateTime { get; set; }
        public string Day { get; set; }
        public Account(double _money)
        {
            Money = _money;
            DateTime = DateTime.Now;
            Day = DateTime.Now.ToString("dddd");
        }
        public Account()
        {
            
        }
    }
}
