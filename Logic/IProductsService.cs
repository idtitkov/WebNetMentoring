using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Model;
using DataAccess.ViewModels;

namespace Services
{
    public interface IProductsService
    {
        Task AddProductAsync(Products product);
        Task AddProductAsync(ProductViewModel productViewModel);
        Task<IEnumerable<ProductViewModel>> GetProductsAsync();
        Task<IEnumerable<ProductViewModel>> GetProductsAsync(int quantity);
        Task UpdateProductAsync(Products product);
        Task UpdateProductAsync(ProductViewModel productViewModel);
    }
}
