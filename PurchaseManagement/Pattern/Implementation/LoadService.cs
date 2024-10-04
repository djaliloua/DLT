using PurchaseManagement.Pattern.Abstraction;
using System.Data;

namespace PurchaseManagement.Pattern.Implementation
{
    public class LoadService<T>  : ILoadService<T> where T : class
    {
        private CollectionView _collectionView;
        public CollectionView CollectionView 
        { 
            get => _collectionView; 
            set => _collectionView = value; 
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
             
        }

        public void Delete(T item)
        {
            throw new NotImplementedException();
        }

        public void Initialize(IList<T> items)
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void Update(T item)
        {
            throw new NotImplementedException();
        }
    }
}
