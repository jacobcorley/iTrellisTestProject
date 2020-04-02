using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using iTrellisProject.Models;

namespace iTrellisProject.Repositories
{
    /**
     * If this data were coming from a database, a repository class wouldn't be necessary
     * However, since this is coming from a file, I'd like to keep the file-parsing logic separate from the model itself
     */
    public static class ProductRepository
    {
        private static List<Product> _products;

        public static void hydrate()
        {
            if (_products == null)
            {
                var productFilename = AppDomain.CurrentDomain.BaseDirectory + "Products.json";
                var bytes = File.ReadAllText(productFilename, Encoding.Default);
                _products = JsonSerializer.Deserialize<List<Product>>(bytes);
            }
        }

        public static List<Product> all()
        {
            return _products.OrderBy(product => product.productId).ToList();
        }
        
        public static Product find(int id)
        {
            try
            {
                return _products.First(p => p.productId == id);
            }
            catch (InvalidOperationException e)
            {
                throw new Exception($"No product found for productId {id} -- {e.Message}");
            }
        }
    }
}