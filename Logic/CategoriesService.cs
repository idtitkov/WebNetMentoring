using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccess.Model;
using DataAccess.Services;
using DataAccess.ViewModels;

namespace Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly NorthwindContext context;
        private readonly int stubLength = 78;

        public CategoriesService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()
        {
            var categories = await context.Categories.ToListAsync();
            return categories.Select(c => ModelConvertionService.CategoriesToCategoryViewModel(c));
        }

        public async Task<byte[]> GetCategoriesImage(int id)
        {
            Categories category;
            try
            {
                category = await context.Categories.Where(c => c.CategoryId == id).SingleAsync();
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException(nameof(id), $"Cannot find category with id {id}");
            }

            return category.Picture.Skip(stubLength).ToArray();
        }

        public async Task SetCategoriesImage(byte[] image, int id)
        {
            Categories category;
            try
            {
                category = await context.Categories.Where(c => c.CategoryId == id).SingleAsync();
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException(nameof(id), $"Cannot find category with id {id}");
            }

            var stub = new byte[stubLength];
            Array.ForEach(stub, v => v = 0);
            image = stub.Concat(image).ToArray();
            category.Picture = image;

            await context.SaveChangesAsync();
        }
    }
}
