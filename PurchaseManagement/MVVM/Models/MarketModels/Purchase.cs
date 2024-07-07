using PurchaseManagement.Utilities;

namespace PurchaseManagement.MVVM.Models.MarketModels
{
    public class Purchase:BaseEntity
    {
        #region Properties
        public string Title { get; set; }
        public string PurchaseDate { get; set; }
        public virtual IList<Product> Products { get; set; } = new List<Product>();
        public virtual ProductStatistics ProductStatistics { get; set; }
        #endregion

        #region Constructor
        public Purchase(string title, DateTime dt)
        {
            Title = title;
            PurchaseDate = dt.ToString("yyyy-MM-dd");
        }
        public Purchase()
        {

        }
        #endregion
        public void Add(Product product)
        {
            Products.Add(product);
            PurchaseUtility.UpdateStatistics(this);
        }
        public void Remove(Product product)
        {
            Product p = Products.FirstOrDefault(p => p.Id==product.Id);  
            int index = Products.IndexOf(p);
            if(index >= 0)
            {
                Products.RemoveAt(index);
            }
            PurchaseUtility.UpdateStatistics(this);
        }
        public Purchase Clone() => MemberwiseClone() as Purchase;
    }
}
