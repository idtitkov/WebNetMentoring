using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using DataAccess.Model;

namespace Api.ConsoleApp
{
    public class Program
    {
        public IConfigurationRoot Configuration { get; set; }

        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var apiSection = config["Api"];

            var client = new HttpClient();

            await ShowProducts(client, apiSection);
            Console.WriteLine();
            await ShowCategories(client, apiSection);

            Console.ReadLine();
        }

        private static async Task ShowProducts(HttpClient client, string baseUrl)
        {
            var responce = await client.GetAsync(baseUrl + "/products");
            if (responce.IsSuccessStatusCode)
            {
                var content = await responce.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Products>>(content);

                Console.WriteLine("Products:");
                foreach (var product in products)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Id: {product.ProductId}");
                    Console.WriteLine($"Name: {product.ProductName}");
                    Console.WriteLine($"Discontinued: {product.Discontinued}");
                    Console.WriteLine($"Unit Price: {product.UnitPrice}");
                    Console.WriteLine($"Quantity Per Unit: {product.QuantityPerUnit}");
                }
            }
        }

        private static async Task ShowCategories(HttpClient client, string baseUrl)
        {
            var responce = await client.GetAsync(baseUrl + "/categories");
            if (responce.IsSuccessStatusCode)
            {
                var content = await responce.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<Categories>>(content);

                Console.WriteLine("Categories:");
                foreach (var category in categories)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Id  : {category.CategoryId}");
                    Console.WriteLine($"Name: {category.CategoryName}");
                    Console.WriteLine($"Desc: {category.Description}");
                }

            }
        }
    }
}
