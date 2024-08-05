using ManagPassWord.MVVM.Models;

namespace ManagPassWord.DataAcessLayer.Abstractions
{
    public interface IPasswordRepository: IGenericRepository<Web>
    {
        Task<int> SaveToCsv();
        Task<Web> GetItemByUrl(string url);
        Task<IList<WebDto>> GetItemsAsDtos();
    }
}
