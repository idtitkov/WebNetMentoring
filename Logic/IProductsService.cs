using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Model;
using DataAccess.ViewModels;

namespace Services
{
    public interface IProductsService
    {
        Task<Products> AddProductAsync(Products product);
        Task<Products> AddProductAsync(ProductViewModel productViewModel);
        Task<IEnumerable<ProductViewModel>> GetProductsAsync();
        Task<IEnumerable<ProductViewModel>> GetProductsAsync(int quantity);
        Task<ProductViewModel> GetProductAsync(int id);
        Task UpdateProductAsync(Products product);
        Task UpdateProductAsync(ProductViewModel productViewModel);
        Task<bool> DeleteProductAsync(int id);
    }
}
