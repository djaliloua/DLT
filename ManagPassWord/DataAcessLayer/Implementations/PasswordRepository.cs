using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.MVVM.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ManagPassWord.DataAcessLayer.Implementations
{
    public class PasswordRepository : GenericRepository<Web>, IPasswordRepository
    {
        public async Task<Web> GetItemByUrl(string url)
        {
            return await _table.FirstOrDefaultAsync(x => x.Url == url);
        }

        public async Task<List<WebDto>> GetItemsAsDtos()
        {
            return await _table.ProjectToType<WebDto>().ToListAsync();
        }

        public Task<int> SaveToCsv()
        {
            throw new NotImplementedException();
        }
    }
}
