using CommunityToolkit.Maui.Storage;
using PurchaseManagement.MVVM.Models.DTOs;
using System.Text;

namespace PurchaseManagement.Commons
{
    public class ProductTxtStrategy : IExportStrategy<ProductDto>
    {
        readonly StringBuilder sb = new StringBuilder();
        public ProductTxtStrategy()
        {
            sb.AppendLine($"Id;Day;Name;Price;Quantity;Description");
        }
        public string Export(IList<ProductDto> items)
        {
            foreach (var item in items)
            {
                sb.AppendLine($"{item.Item_Id};{item.Purchase?.PurchaseDate};{item.Item_Name};{item.Item_Price:C2};{item.Item_Quantity};{item.Item_Description}");
            }

            return sb.ToString();
        }
    }
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
    public class ExportContext<TItem> where TItem : class
    {
        private IExportStrategy<TItem> ExportStrategy;
        private readonly IFileSaver fileSaver;
        public ExportContext(IExportStrategy<TItem> _exportStrategy, IFileSaver _fileSaver)
        {
            ExportStrategy = _exportStrategy;
            fileSaver = _fileSaver;
        }
        public bool SetExportStrategy(IExportStrategy<TItem> exportStrategy)
        {
            ExportStrategy = exportStrategy;
            return true;
        }
        public bool ExportTo(string filename, IList<TItem> items)
        {
            string txt = ExportStrategy.Export(items);
            Save(txt);
            return true;
        }
        private async void Save(string txt)
        {
            CancellationToken cancellationToken = CancellationToken.None;
            using var stream = new MemoryStream(Encoding.Default.GetBytes(txt));
            var fileSaverResult = await fileSaver.SaveAsync("data.txt", stream, cancellationToken);
            fileSaverResult.EnsureSuccess();
        }
    }
}
