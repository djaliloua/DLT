namespace PurchaseManagement.Pattern.Abstraction
{
    public interface ILoadService<T> where T : class
    {
        CollectionView CollectionView { get; set; }
        void Initialize(IList<T> items);
        void Add(T item);
        void Remove(T item);
        void Update(T item);
        void Delete(T item);
        void Clear();
        void Refresh();
    }
}
