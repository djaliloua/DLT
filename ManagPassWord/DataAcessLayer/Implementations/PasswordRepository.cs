using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.MVVM.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagPassWord.DataAcessLayer.Implementations
{
    public class PasswordRepository : GenericRepository<Web>, IPasswordRepository
    {
        public async Task<Web> GetItemByUrl(string url)
        {
            return await _table.FirstOrDefaultAsync(x => x.Url == url);
        }

        public Task<int> SaveToCsv()
        {
            throw new NotImplementedException();
        }
    }
}
