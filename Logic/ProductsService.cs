using DataAccess.Model;
using DataAccess.Services;
using DataAccess.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductsService
    {
        private readonly NorthwindContext context;

        public ProductsService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Products>> GetProductsAsync()
        {
            var products = await context.Products.ToListAsync();
            products.ForEach(p =>
            {
                p.Category = context.Categories.Where(c => c.CategoryId == p.CategoryId).Single();
                p.Supplier = context.Suppliers.Where(s => s.SupplierId == p.SupplierId).Single();
            });
            products.Reverse();

            return products;
        }

        public async Task<IEnumerable<Products>> GetProductsAsync(int quantity)
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

        public async Task AddProductAsync(Products product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }

        public async Task AddProductAsync(ProductViewModel productViewModel)
        {
            var product = ModelConvertionService.ProductViewModelToProducts(productViewModel);
            await AddProductAsync(product);
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
    }
}
