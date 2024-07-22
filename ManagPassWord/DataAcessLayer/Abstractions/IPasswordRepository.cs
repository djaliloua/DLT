using ManagPassWord.MVVM.Models;

namespace ManagPassWord.DataAcessLayer.Abstractions
{
    public interface IPasswordRepository: IGenericRepository<Web>
    {
        Task<int> SaveToCsv();
    }
}
