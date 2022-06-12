using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Model;

namespace Services
{
    public interface ISuppliersService
    {
        Task<IEnumerable<Suppliers>> GetSuppliersAsync();
    }
}
