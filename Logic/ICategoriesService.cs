using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Model;
using DataAccess.ViewModels;

namespace Services
{
    public interface ICategoriesService
    {
        Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync();
        Task<byte[]> GetCategoriesImage(int id);
        Task SetCategoriesImage(byte[] image, int id);
    }
}
