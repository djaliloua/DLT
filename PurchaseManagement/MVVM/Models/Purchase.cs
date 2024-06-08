using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace PurchaseManagement.MVVM.Models
{
    public class Purchase
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Purchase_Date { get; set; }
        public ICollection<Product> Products { get; }
        public Purchase(string title, DateTime dt)
        {
            Title = title;
            Purchase_Date = dt.ToString("yyyy-MM-dd");
        }
        public Purchase()
        {

        }
    }
}
