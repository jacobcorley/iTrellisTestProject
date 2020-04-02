using System;
using System.Linq;
using System.Text.Json;
using iTrellisProject.Models;
using iTrellisProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iTrellisProject.Controllers
{
    /**
     * Putting the RESTful calls in a separate controller
     */
    [Route("api")]
    public class ProductsApiController : Controller
    {
        /**
         * Get all products as JSON
         */
        [HttpGet]
        [Route("")]
        public string GetProducts()
        {
            return JsonSerializer.Serialize(ProductRepository.all().Select(product => new ProductViewModel(product)).ToList());
        }
        
        /**
         * Get a single product as JSON
         */
        [HttpGet]
        [Route("{id}")]
        public string GetProduct(int id)
        {
            return JsonSerializer.Serialize(new ProductViewModel(ProductRepository.find(id)));
        }
        
        /**
         * Get an estimated delivery date for a product, given product id and starting date (yyyyMMdd)
         * No timestamp => start at the current time
         */
        [HttpGet]
        [Route("estimate/{productId}/{startDate?}")]
        public string EstimateArrival(string productId, string startDate = "")
        {
            var product = ProductRepository.find(int.Parse(productId));
            var shippingDate = startDate == ""
                ? DateTime.Now
                : DateTime.Parse(startDate);
            return product.getArrivalDate(shippingDate).ToLongDateString();
        }
    }
}