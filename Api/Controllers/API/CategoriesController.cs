using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataAccess.ViewModels;
using Services;


namespace Api.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet(Name = nameof(GetCategories))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult<IEnumerable<CategoryViewModel>> GetCategories()
        {
            try
            {
                var categories = _categoriesService.GetCategoriesAsync();
                return Ok(categories.Result);

            }
            catch (Exception)
            {
            }

            return BadRequest();
        }

        [HttpGet("image/{id}", Name = nameof(GetImage))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetImage(int id)
        {
            try
            {
                byte[] imageArray = await _categoriesService.GetCategoriesImage(id);
                return File(imageArray, "image/jpeg");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("image/{id}", Name = nameof(UpdateImage))]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateImage(int id, IFormFile image)
        {
            if (image == null)
            {
                BadRequest();
            }

            var imageArray = new byte[] { };
            using (MemoryStream ms = new MemoryStream())
            {
                image.CopyTo(ms);
                imageArray = ms.ToArray();
            }
            await _categoriesService.SetCategoriesImage(imageArray, id);

            return CreatedAtAction(nameof(GetImage), id);
        }
    }
}
