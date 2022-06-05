using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SuppliersService
    {
        private readonly NorthwindContext context;

        public SuppliersService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Suppliers>> GetSuppliersAsync()
        {
            return await context.Suppliers.ToListAsync();
        }
    }
}
