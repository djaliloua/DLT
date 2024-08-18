using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.MVVM.Models.MarketModels;

namespace PurchaseManagement.Utilities
{
    public static class PurchaseUtility
    {
        public static void UpdateStatistics(PurchaseDto purchase)
        {
            purchase.ProductStatistics ??= new ProductStatisticsDto();
            purchase.ProductStatistics.Id = purchase.Id;
            purchase.ProductStatistics.PurchaseCount = purchase.Products.Count;
            purchase.ProductStatistics.TotalPrice = GetTotalValue(purchase.Id, "Price", purchase);
            purchase.ProductStatistics.TotalQuantity = GetTotalValue(purchase.Id, "Quantity", purchase);
        }
        private static double GetTotalValue(int id, string colname, PurchaseDto purchase)
        {
            double result = 0;
            if (colname == "Price")
                result = purchase.Products.Sum(x => x.Item_Price);
            else
                result = purchase.Products.Sum(x => x.Item_Quantity);
            return result;
        }
    }
}
