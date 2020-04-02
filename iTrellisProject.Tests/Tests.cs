using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using iTrellisProject.Models;

namespace iTrellisProject.Tests
{
    /*
     * Since I'm not really sure how to set up XUnit (or similar) in this environment, I'm just faking it
     * While it's by no means ideal, it should suffice
     * Also, can't really use AAA without a full crud, and since that's beyond the scope of this project, I'm just doing
     * a few checks that rely on the starting data
     */
    public class Tests
    {
        private const string ApiPrefix = "https://localhost:5001/api"; 
        
        public async Task run()
        {
            Console.WriteLine("Checking all-products index");
            var indexTested = await testProductsIndex();
            Console.WriteLine("---");
            Console.WriteLine("Checking single product");
            var productTested = await testSingleProduct();
            Console.WriteLine("---");
            if (indexTested && productTested)
            {
                Console.WriteLine("Checking delivery estimates");
                await testDeliveryEstimate();
            }
            else
            {
                Console.WriteLine("Skipping delivery estimate checked due to failed previous tests.");
            }
        }

        /**
         * Check that the index loads all products & spot-check values
         */
        private async Task<bool> testProductsIndex()
        {
            var response = await getResponseFromUrl("/");
            Console.WriteLine("Checking that any data is returned...");
            if (!assert(response.Length > 0)) return false;
            
            Console.WriteLine("Checking that data is valid JSON...");
            List<ProductViewModel> allProducts; 
            try
            {
                allProducts = JsonSerializer.Deserialize<List<ProductViewModel>>(response);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
                return assert(false);
            }

            Console.WriteLine("Checking that product 1 is correct...");
            return assert(allProducts.FirstOrDefault(product => product.productId == 1).productName == "fugiat exercitation adipisicing");
        }

        /**
         * Check that a single product loads 
         */
        private async Task<bool> testSingleProduct()
        {
            var response = await getResponseFromUrl("/2");
            Console.WriteLine("Checking that any data is returned...");
            if(!assert(response.Length > 0)) return false;
            
            Console.WriteLine("Checking that data is valid JSON...");
            ProductViewModel product; 
            try
            {
                product = JsonSerializer.Deserialize<ProductViewModel>(response);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
                return assert(false);
            }
            
            Console.WriteLine("Checking that product 2 is correct...");
            return assert(product.productName == "mollit cupidatat Lorem");
        }
        
        /**
         * Check that shipping estimates work
         */
        public async Task<bool> testDeliveryEstimate()
        {
            var response = await getResponseFromUrl("/3");
            Console.WriteLine("Getting default delivery date for product #3");
            var estimatedArrival = JsonSerializer.Deserialize<ProductViewModel>(response).estimatedArrival;
            
            Console.WriteLine($"Checking that result matches API estimated result ({estimatedArrival})");
            var currentDate = $"{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}";
            var estimateResponse = await getResponseFromUrl("/estimate/3/");

            return assert(estimatedArrival == estimateResponse);
        }
        
        /**
         * Simplify async web requests a bit
         */
        private async Task<string> getResponseFromUrl(string path)
        {
            var handler = new HttpClientHandler();
            
            // Since locahost doesn't have a valid SSL certificate, I'm gonna bypass validation
            // Not a good idea in real applications, BUT it works fine for my purposes here
            handler.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;
            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(ApiPrefix + path)
            };
            var response = await client.GetAsync(client.BaseAddress);
            return await response.Content.ReadAsStringAsync();
        }

        /**
         * Again, without xunit or something, I gotta make my own versions of some things here
         */
        private bool assert(bool statement)
        {
            Console.ForegroundColor = statement ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine("Assertion {0}.", statement ? "succeeded" : "failed");
            Console.ForegroundColor = ConsoleColor.White;
            return statement;
        }
    }
}