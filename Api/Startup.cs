using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Api.Middleware;
using DataAccess.Model;
using Services;
using Microsoft.Extensions.Caching.Memory;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //services.AddDbContext<NorthwindContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("SQLCONNSTR_Northwind")));
            services.AddDbContext<NorthwindContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("SQLAZURECONNSTR_Northwind")));
            services.AddTransient<NorthwindContext>();

            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<ISuppliersService, SuppliersService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation($"Application started from {env.ContentRootPath}");
            foreach (var item in Configuration.GetChildren())
            {
                logger.LogInformation($"Current configuration {item.Path} {item.Value}");
            }

            if (env.IsDevelopment())
            {
                logger.LogInformation("In Development mode");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                logger.LogInformation("In Production mode");
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ImageCachingMiddleware>("ImageCache", 5, TimeSpan.FromSeconds(10));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/Images/{id:int}", context =>
                {
                    var id = context.Request.RouteValues["id"];
                    context.Response.Redirect($"../Categories/GetImage/{id}");
                    return Task.CompletedTask;
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
