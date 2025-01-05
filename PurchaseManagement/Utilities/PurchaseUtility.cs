using PurchaseManagement.MVVM.Models.ViewModel;

namespace PurchaseManagement.Utilities
{
    public static class PurchaseUtility
    {
        public static void UpdateStatistics(PurchaseViewModel purchase)
        {
            purchase.ProductStatistics ??= new ProductStatisticsDto();
            purchase.ProductStatistics.Id = purchase.Id;
            purchase.ProductStatistics.PurchaseCount = purchase.Products.Count;
            purchase.ProductStatistics.TotalPrice = GetTotalValue(purchase.Id, "Price", purchase);
            purchase.ProductStatistics.TotalQuantity = GetTotalValue(purchase.Id, "Quantity", purchase);
        }
        private static double GetTotalValue(int id, string colname, PurchaseViewModel purchase)
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
