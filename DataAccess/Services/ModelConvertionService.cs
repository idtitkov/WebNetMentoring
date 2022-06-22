using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Model;
using DataAccess.ViewModels;

namespace DataAccess.Services
{
    public static class ModelConvertionService
    {
        public static Products ProductViewModelToProducts(ProductViewModel productViewModel)
        {
            var product = new Products()
            {
                ProductId = productViewModel.ProductId,
                ProductName = productViewModel.ProductName,
                SupplierId = productViewModel.SupplierId,
                CategoryId = productViewModel.CategoryId,
                QuantityPerUnit = productViewModel.QuantityPerUnit,
                UnitPrice = productViewModel.UnitPrice,
                Discontinued = productViewModel.Discontinued
            };
            return product;
        }

        public static ProductViewModel ProductsToProductViewModel(Products product)
        {
            var productViewModel = new ProductViewModel()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                SupplierId = product.SupplierId.Value,
                CategoryId = product.CategoryId.Value,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = product.UnitPrice.Value,
                Discontinued = product.Discontinued,
                CategoryName = product.Category?.CategoryName,
                SupplierName = product.Supplier?.CompanyName
            };
            return productViewModel;
        }

        public static CategoryViewModel CategoriesToCategoryViewModel(Categories category)
        {
            var categoryViewModel = new CategoryViewModel()
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
                Picture = category.Picture
            };
            return categoryViewModel;
        }
    }
}
