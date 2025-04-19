using PurchaseManagement.MVVM.Models.ViewModel;
using System.Text;

namespace PurchaseManagement.Commons.ExportFileStrategies
{
    public class ProductTxtStrategy : IExportStrategy<ProductViewModel>
    {
        readonly StringBuilder sb = new StringBuilder();
        public ProductTxtStrategy()
        {
            sb.AppendLine($"Id;Day;Name;Price;Quantity;Description");
        }
        public string Export(IList<ProductViewModel> items)
        {
            foreach (var item in items)
            {
                sb.AppendLine($"{item.Id};{item.Purchase?.PurchaseDate};{item.Item_Name};{item.Item_Price:C2};{item.Item_Quantity};{item.Item_Description}");
            }

            return sb.ToString();
        }
    }
}
