using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Api.Filters;
using Services;

namespace Api.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly ILogger<CategoriesController> logger;

        public CategoriesController(ICategoriesService categoriesService, ILogger<CategoriesController> logger)
        {
            this.categoriesService = categoriesService;
            this.logger = logger;
            this.logger.LogInformation("CategoriesController started");
        }

        [HttpGet(Name = nameof(Index))]
        [LogActionFilter(logParameters: false)]
        public async Task<IActionResult> Index()
        {
            var categories = await categoriesService.GetCategoriesAsync();
            return View(categories);
        }

        [HttpGet(Name = nameof(Image))]
        [LogActionFilter(logParameters: true)]
        public IActionResult Image(int id)
        {
            return View(id);
        }

        [HttpGet(Name = nameof(GetImage))]
        public async Task<IActionResult> GetImage(int id)
        {
            byte[] imageArray = await categoriesService.GetCategoriesImage(id);
            var imageStream = new MemoryStream(imageArray);
            var fileStream = new FileStreamResult(imageStream, "image/jpeg");
            return fileStream;
        }

        [HttpPost(Name = nameof(SetImage))]
        public async Task<IActionResult> SetImage(IFormFile image, int id)
        {
            if (!IsImage(image))
            {
                throw new Exception("Use only valid images");
            }

            var imageArray = new byte[] { };
            using (MemoryStream ms = new MemoryStream())
            {
                image.CopyTo(ms);
                imageArray = ms.ToArray();
            }
            await categoriesService.SetCategoriesImage(imageArray, id);

            return RedirectToAction("Image", new { id });
        }

        private bool IsImage(IFormFile postedFile)
        {
            var ImageMinimumBytes = 512;

            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return false;
                }
                //------------------------------------------
                //check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (postedFile.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                postedFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
