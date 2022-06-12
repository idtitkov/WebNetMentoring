using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Moq;
using Api.Controllers;
using Services;
using DataAccess.ViewModels;

namespace Api.Tests
{
    [TestFixture]
    internal class CategoriesControllerTests
    {
        private Mock<ICategoriesService> mockCategoriesService;
        private Mock<ILogger<CategoriesController>> mockLogger;

        private CategoriesController categoriesController;

        [SetUp]
        public void Setup()
        {
            mockCategoriesService = new Mock<ICategoriesService>();
            mockLogger = new Mock<ILogger<CategoriesController>>();
            categoriesController = new CategoriesController(mockCategoriesService.Object, mockLogger.Object);
        }

        [Test]
        public async Task Categories_ReturnsAViewResult_WithAListOfCategories()
        {
            // Arrange
            mockCategoriesService.Setup(cs => cs.GetCategoriesAsync()).ReturnsAsync(GetTestCategoriesAsync());

            // Act
            var result = await categoriesController.Index();

            // Assert
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            var model = (result as ViewResult).ViewData.Model;
            Assert.That(model, Is.AssignableTo<IEnumerable<CategoryViewModel>>());
            Assert.That((model as IEnumerable<CategoryViewModel>).Count(), Is.EqualTo(2));
        }

        private IEnumerable<CategoryViewModel> GetTestCategoriesAsync()
        {
            return new List<CategoryViewModel>()
            {
                new CategoryViewModel(){
                },
                new CategoryViewModel(){
                }
            };
        }
    }
}
