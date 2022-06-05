using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class CategoriesService
    {
        private readonly NorthwindContext context;

        public CategoriesService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Categories>> GetCategoriesAsync()
        {
            return await context.Categories.ToListAsync();
        }
    }
}
