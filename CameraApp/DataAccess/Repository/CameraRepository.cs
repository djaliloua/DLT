using CameraApp.DataAccess.Abstraction;
using CameraApp.Models;
using CameraApp.ViewModels;
using Mapster;

namespace CameraApp.DataAccess.Repository
{
    public interface ICameraRepository: IGenericRepository<Camera>
    {
        List<CameraViewModel> GetAllAsDto();
    }
    public class CameraRepository : GenericRepository<Camera>, ICameraRepository
    {
        public List<CameraViewModel> GetAllAsDto()
        {
            return _context.Cameras.ProjectToType<CameraViewModel>().ToList();
        }
    }
}
