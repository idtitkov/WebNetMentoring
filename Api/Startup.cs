using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Api.Middleware;
using DataAccess.Identity;
using DataAccess.Model;
using Services;

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
            services.AddDbContext<NorthwindContext>(
                options => options.UseSqlServer(
            //Configuration.GetConnectionString("SQLAZURECONNSTR_Northwind")));
            Configuration.GetConnectionString("SQLCONNSTR_Northwind")));

            services.AddDbContext<NorthwindIdentityDbContext>(
                options => options.UseSqlServer(
            //Configuration.GetConnectionString("SQLAZURECONNSTR_NorthwindIdentity")));
            Configuration.GetConnectionString("SQLCONNSTR_NorthwindIdentity")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<NorthwindIdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options =>
                {
                    Configuration.Bind("AzureAd", options);
                    options.CookieSchemeName = IdentityConstants.ExternalScheme;
                });

            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<ISuppliersService, SuppliersService>();

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddRazorPages();
            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddJavaScriptBundle("/js/bundle.js", "lib/**/*.js")
                    .UseContentRoot();

                pipeline.MinifyCssFiles();
                pipeline.MinifyJsFiles();
            });

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider srp, ILogger<Startup> logger)
        {
            logger.LogInformation($"Application started from {env.ContentRootPath}");
            foreach (var item in Configuration.GetChildren())
            {
                logger.LogInformation($"Current configuration {item.Path} {item.Value}");
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind API v1")
            );

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

            app.UseWebOptimizer();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ImageCachingMiddleware>("ImageCache", 10, TimeSpan.FromSeconds(60));

            CreateAdministratorRole(srp);

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

                endpoints.MapRazorPages();
            });
        }

        private void CreateAdministratorRole(IServiceProvider srp)
        {
            var roleManager = srp.GetRequiredService<RoleManager<IdentityRole>>();
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                roleManager.CreateAsync(new IdentityRole("Administrator")).Wait();
            }
        }
    }
}
