using CommunityToolkit.Maui.Storage;
using System.Diagnostics;
using System.Text;

namespace PurchaseManagement.Commons
{
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
            try
            {
                CancellationToken cancellationToken = CancellationToken.None;
                using var stream = new MemoryStream(Encoding.Default.GetBytes(txt));
                var fileSaverResult = await fileSaver.SaveAsync("data.txt", stream, cancellationToken);
                fileSaverResult.EnsureSuccess();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
