using DataAccess.Model;
using DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Services
{
    public static class ModelConvertionService
    {
        public static Products ProductViewModelToProducts(ProductViewModel productViewModel)
        {
            var product = new Products()
            {
                ProductId = (int)productViewModel.ProductId,
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
                SupplierId = (int)product.SupplierId,
                CategoryId = (int)product.CategoryId,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = (decimal)product.UnitPrice,
                Discontinued = product.Discontinued
            };
            return productViewModel;
        }
    }
}
