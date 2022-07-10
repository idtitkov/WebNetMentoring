using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccess.Model;
using DataAccess.Services;
using DataAccess.ViewModels;

namespace Services
{
    public class ProductsService : IProductsService
    {
        private readonly NorthwindContext context;

        public ProductsService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<ProductViewModel>> GetProductsAsync()
        {
            var products = await context.Products.ToListAsync();
            products.ForEach(p =>
                {
                    p.Category = context.Categories.Where(c => c.CategoryId == p.CategoryId).Single();
                    p.Supplier = context.Suppliers.Where(s => s.SupplierId == p.SupplierId).Single();
                });

            return products.Select(p => ModelConvertionService.ProductsToProductViewModel(p)).Reverse();
        }

        public async Task<IEnumerable<ProductViewModel>> GetProductsAsync(int quantity)
        {
            if (quantity == 0)
            {
                return await GetProductsAsync();
            }
            else if (quantity > 0)
            {
                var products = await GetProductsAsync();
                return products.Take(quantity);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(quantity));
            }
        }

        public async Task<Products> AddProductAsync(Products product)
        {
            var createdProduct = await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return createdProduct.Entity;
        }

        public async Task<Products> AddProductAsync(ProductViewModel productViewModel)
        {
            var product = ModelConvertionService.ProductViewModelToProducts(productViewModel);
            var createdProduct = await AddProductAsync(product);
            return createdProduct;
        }

        public async Task UpdateProductAsync(Products product)
        {
            context.Entry(await context.Products.FirstOrDefaultAsync(p => p.ProductId == product.ProductId)).CurrentValues.SetValues(product);
            await context.SaveChangesAsync();

        }

        public async Task UpdateProductAsync(ProductViewModel productViewModel)
        {
            var product = ModelConvertionService.ProductViewModelToProducts(productViewModel);
            await UpdateProductAsync(product);
        }

        public async Task<ProductViewModel> GetProductAsync(int id)
        {
            var product = await context.Products.FindAsync(id);

            product.Category = context.Categories.Where(c => c.CategoryId == product.CategoryId).Single();
            product.Supplier = context.Suppliers.Where(s => s.SupplierId == product.SupplierId).Single();

            return ModelConvertionService.ProductsToProductViewModel(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product != null)
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
