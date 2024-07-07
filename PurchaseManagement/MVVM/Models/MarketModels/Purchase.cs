using System.Collections.ObjectModel;

namespace PurchaseManagement.MVVM.Models.MarketModels
{
    public class Purchase:BaseEntity
    {
        public string Title { get; set; }
        public string PurchaseDate { get; set; }
        public virtual IList<Product> Products { get; set; } = new List<Product>();
        public virtual ProductStatistics ProductStatistics { get; set; }
        public Purchase(string title, DateTime dt)
        {
            Title = title;
            PurchaseDate = dt.ToString("yyyy-MM-dd");
        }
        public void Add(Product product)
        {
            Products.Add(product);
            UpdateStatistics();
        }
        public void Remove(Product product)
        {
            Product p = Products.FirstOrDefault(p => p.Id==product.Id);  
            int index = Products.IndexOf(p);
            if(index >= 0)
            {
                Products.RemoveAt(index);
            }
            UpdateStatistics();
        }
        private double GetTotalValue(int id, string colname)
        {
            double result = 0;
            if (colname == "Price")
                result = Products.Sum(x => x.Item_Price);
            else
                result = Products.Sum(x => x.Item_Quantity);
            return result;
        }
        public void UpdateStatistics()
        {
            ProductStatistics ??= new ProductStatistics();
            ProductStatistics.Id = Id;
            ProductStatistics.PurchaseCount = Products.Count;
            ProductStatistics.TotalPrice = GetTotalValue(Id, "Price");
            ProductStatistics.TotalQuantity = GetTotalValue(Id, "Quantity");
        }
        public Purchase()
        {

        }
        public Purchase Clone() => MemberwiseClone() as Purchase;
    }
}
