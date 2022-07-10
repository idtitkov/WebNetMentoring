using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DataAccess.ViewModels;
using Services;

namespace Api.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IProductsService productsService;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IConfiguration configuration, IProductsService productsService, ILogger<ProductsController> logger)
        {
            this.configuration = configuration;
            this.productsService = productsService;
            this.logger = logger;
            this.logger.LogInformation("ProductsController started");
        }

        [HttpGet(Name = nameof(Index))]
        public async Task<IActionResult> Index()
        {
            var productsCount = configuration.GetValue<int>("ProductsCount");
            var products = await productsService.GetProductsAsync(productsCount);

            return View(products);
        }

        [HttpPost(Name = nameof(Add))]
        public async Task<IActionResult> Add(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                await productsService.AddProductAsync(product);
            }

            return RedirectToAction("Index");
        }

        [HttpPost(Name = nameof(Edit))]
        public IActionResult Edit(ProductViewModel product)
        {
            return View(product);
        }

        [HttpPost(Name = nameof(Update))]
        public async Task<IActionResult> Update(ProductViewModel product)
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

            return RedirectToAction("Index");
        }
    }
}
