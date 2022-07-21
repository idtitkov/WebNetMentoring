using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Identity
{
    public class NorthwindIdentityDbContext : IdentityDbContext
    {
        public NorthwindIdentityDbContext(DbContextOptions<NorthwindIdentityDbContext> options)
            : base(options)
        {
        }
    }
}
