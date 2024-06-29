using PurchaseManagement.MVVM.Models.DTOs;
using System.Text;

namespace PurchaseManagement.Commons.ExportFileStrategies
{
    public class AccountTxtStrategy : IExportStrategy<AccountDTO>
    {
        readonly StringBuilder sb = new StringBuilder();
        public AccountTxtStrategy()
        {
            sb.AppendLine($"Id;Day;Money");
        }
        public string Export(IList<AccountDTO> items)
        {
            foreach (var item in items)
            {
                sb.AppendLine($"{item.Id};{item.Day};{item.Money:C2}");
            }

            return sb.ToString();
        }
    }
}
