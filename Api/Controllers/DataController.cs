using Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using DataAccess.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Controllers
{
    public class DataController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly CategoriesService categoriesService;
        private readonly ProductsService productsService;
        private readonly SuppliersService suppliersService;
        private readonly ILogger<DataController> logger;

        public DataController(IConfiguration configuration, CategoriesService categoriesService, ProductsService productsService, SuppliersService suppliersService, ILogger<DataController> logger)
        {
            this.configuration = configuration;
            this.categoriesService = categoriesService;
            this.productsService = productsService;
            this.suppliersService = suppliersService;
            this.logger = logger;
            this.logger.LogInformation("DataController started");
        }

        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            var categories = await categoriesService.GetCategoriesAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Products()
        {
            var productsCount = configuration.GetValue<int>("ProductsCount");
            var products = await productsService.GetProductsAsync(productsCount);
            var categories = await categoriesService.GetCategoriesAsync();
            var suppliers = await suppliersService.GetSuppliersAsync();
            return View((Products: products, Categories: categories, Suppliers: suppliers));
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                await productsService.AddProductAsync(product);
            }

            return RedirectToAction("Products");
        }

        [HttpPost]
        public IActionResult EditProduct(ProductViewModel product)
        {
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                await productsService.UpdateProductAsync(product);
            }
            else
            {
                var modelErrors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(modelErrors.First());
            }

            return RedirectToAction("Products");
        }
    }
}
