using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.MVVM.Models;

namespace ManagPassWord.DataAcessLayer.Implementations
{
    public class PasswordRepository : GenericRepository<Web>, IPasswordRepository
    {
        public Task<int> SaveToCsv()
        {
            throw new NotImplementedException();
        }
    }
}
