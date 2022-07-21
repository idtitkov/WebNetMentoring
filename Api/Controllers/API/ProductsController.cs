using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DataAccess.ViewModels;
using Services;
using DataAccess.Model;
using System.Threading.Tasks;

namespace Api.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IProductsService _productsService;

        public ProductsController(IConfiguration configuration, IProductsService productsService)
        {
            _configuration = configuration;
            _productsService = productsService;
        }

        [HttpGet(Name = nameof(GetProducts))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult<IEnumerable<ProductViewModel>> GetProducts()
        {
            try
            {
                var productsCount = _configuration.GetValue<int>("ProductsCount");
                var products = _productsService.GetProductsAsync(productsCount);
                return Ok(products.Result);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}", Name = nameof(GetProduct))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult GetProduct(int id)
        {

            var products = _productsService.GetProductAsync(id);
            if (products != null)
            {
                return Ok(products.Result);
            }

            return NotFound();
        }

        [HttpPost(Name = nameof(CreateProduct))]
        [ProducesResponseType(typeof(ProductViewModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult<Products> CreateProduct([FromBody] ProductViewModel product)
        {
            var createdProduct = _productsService.AddProductAsync(product);
            if (createdProduct.Result != null)
            {
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Result.ProductId }, createdProduct.Result);
            }

            return BadRequest();
        }

        [HttpPut("{id}", Name = nameof(UpdateProduct))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductViewModel product)
        {
            try
            {
                product.ProductId = id;
                await _productsService.UpdateProductAsync(product);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("{id}", Name = nameof(DeleteProduct))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (await _productsService.DeleteProductAsync(id))
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
