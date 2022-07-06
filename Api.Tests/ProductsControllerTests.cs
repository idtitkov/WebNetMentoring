using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Moq;
using Api.Controllers;
using DataAccess.ViewModels;
using Services;

namespace Api.Tests
{
    [TestFixture]
    internal class ProductsControllerTests
    {
        private Mock<IProductsService> mockProductsService;
        private Mock<ILogger<ProductsController>> mockLogger;

        private IConfiguration mockConfiguration;

        private ProductsController productsController;

        [SetUp]
        public void Setup()
        {
            mockProductsService = new Mock<IProductsService>();
            mockLogger = new Mock<ILogger<ProductsController>>();
            mockConfiguration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string> {
                {"ProductsCount", 0.ToString() }
            }).Build();
            productsController = new ProductsController(mockConfiguration, mockProductsService.Object, mockLogger.Object);

        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task Products_ReturnsAViewResult_WithAListOfProducts(int productsCount)
        {
            // Arrange
            mockProductsService.Setup(ps => ps.GetProductsAsync(productsCount)).ReturnsAsync(GetTestProductsAsync(productsCount));
            mockConfiguration["ProductsCount"] = productsCount.ToString();

            // Act
            var result = await productsController.Index();

            // Assert
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            var model = (result as ViewResult).ViewData.Model;
            Assert.That(model, Is.AssignableTo<IEnumerable<ProductViewModel>>());
            Assert.That((model as IEnumerable<ProductViewModel>).Count(), Is.EqualTo(productsCount));
        }

        [Test]
        public async Task Products_AddsProduct_WithValidModel()
        {
            // Arrange
            var model = new ProductViewModel();

            // Act
            var result = await productsController.Add(model);

            // Assert
            mockProductsService.Verify(ps => ps.AddProductAsync(model));
        }

        [Test]
        public async Task Products_NotAddsProduct_WithInvalidModel()
        {
            // Arrange
            var model = new ProductViewModel();

            // Act
            productsController.ViewData.ModelState.AddModelError("UnitPrice", "Invalid price");
            var result = await productsController.Add(model);

            // Assert
            mockProductsService.Verify(ps => ps.AddProductAsync(It.IsAny<ProductViewModel>()), Times.Never());
        }

        [Test]
        public async Task Products_UpdateProduct_WithValidModel()
        {
            // Arrange
            var model = new ProductViewModel();

            // Act
            var result = await productsController.Update(model);

            // Assert
            mockProductsService.Verify(ps => ps.UpdateProductAsync(model));
        }

        [Test]
        public void Products_NotUpdateProduct_WithInvalidModel()
        {
            // Arrange
            var model = new ProductViewModel();

            // Act
            productsController.ViewData.ModelState.AddModelError("UnitPrice", "Invalid price");
            Task Update() => productsController.Update(model);

            // Assert
            Assert.That(Update, Throws.TypeOf<ArgumentException>());
            mockProductsService.Verify(ps => ps.UpdateProductAsync(model), Times.Never());
        }

        private IEnumerable<ProductViewModel> GetTestProductsAsync(int? count)
        {
            var products = new List<ProductViewModel>()
            {
                new ProductViewModel(){
                },
                new ProductViewModel(){
                },
                new ProductViewModel(){
                }
            };
            return products.Take(count ?? products.Count);
        }
    }
}
