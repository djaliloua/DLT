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
        
        public Purchase Clone() => MemberwiseClone() as Purchase;
    }
}
