namespace PurchaseManagement.Commons
{
    public interface IExportStrategy<TItem>
    {
        string Export(IList<TItem> items);
    }
}
