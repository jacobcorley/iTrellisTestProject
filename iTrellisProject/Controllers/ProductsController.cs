using System.Linq;
using iTrellisProject.Models;
using iTrellisProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iTrellisProject.Controllers
{
    [Route("")]
    [Route("products")]
    public class ProductsController : Controller
    {
        /**
         * View all products
         */
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.title = "Products";
            ViewBag.products = ProductRepository.all().Select(product => new ProductViewModel(product)).ToList();
            
            return View("Index");
        }
        
        /**
         * View a single product
         */
        [Route("{id}")]
        public IActionResult Show(int id)
        {
            var product = ProductRepository.find(id);
            ViewBag.title = product.productName;
            ViewBag.product = new ProductViewModel(product);
            
            return View("Show");
        }
    }
}