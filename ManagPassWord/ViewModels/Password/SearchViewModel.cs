using ManagPassWord.Data_AcessLayer;
using ManagPassWord.Models;

namespace ManagPassWord.ViewModels
{
    public class SearchViewModel:BaseViewModel
    {
        private readonly IRepository<User> _db;
        private int _count;
        public int Count
        {
            get => _count;
            set => UpdateObservable(ref _count, value);
        }
        public SearchViewModel(IRepository<User> db)
        {
            //_db = db;
            //load();
        }
        private async void load()
        {
            Count = await _db.CountItemAsync();
        }
    }
}
