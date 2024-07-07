using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.Models;

namespace ManagPassWord.DataAcessLayer.Implementations
{
    public class PasswordRepository : GenericRepository<User>, IPasswordRepository
    {
        public Task<int> SaveToCsv()
        {
            throw new NotImplementedException();
        }
    }
}
