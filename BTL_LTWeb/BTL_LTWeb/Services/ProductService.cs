using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BTL_LTWeb.Models;

namespace BTL_LTWeb.Services
{
    public class ProductService : BaseService
    {
        public static int PAGE_SIZE = 6;
        public List<Product> GetProducts()
        {
            return db.Products.ToList();
        }
        public List<Product> GetProductsByPrice(int? min, int? max, string name)
        {
            return db.Products
                 .Where(p => string.IsNullOrEmpty(name) ? p.product_name.Contains("") : p.product_name.Contains(name))
                .Where(p => p.price >= min && p.price < max)
                .OrderBy(p => p.product_id)
                .ToList();
        }

        public List<Product> GetProductsByPage(int? pageNum, string name)
        {

            int recordsToSkip = (int)(pageNum - 1) * PAGE_SIZE;
            return db.Products
             .Where(p => string.IsNullOrEmpty(name) ? p.product_name.Contains("") : p.product_name.Contains(name))
            .OrderBy(p => p.product_id)  
            .Skip(recordsToSkip)        
            .Take(PAGE_SIZE)            
            .ToList();
        }

        public List<Product> GetProductsByPageAndPrice(int? pageNum, int? min, int? max, string name)
        {
            int recordsToSkip = (int)(pageNum - 1) * PAGE_SIZE;
            return db.Products
              .Where(p => string.IsNullOrEmpty(name) ? p.product_name.Contains("") : p.product_name.Contains(name))
             .Where(p => p.price >= min && p.price < max)
            .OrderBy(p => p.product_id)
            .Skip(recordsToSkip)
            .Take(PAGE_SIZE)
            .ToList();
        }

        public List<Product> GetProductsByName(string name)
        {
            return db.Products
            .Where(p => p.product_name.Contains(name))
            .OrderBy(p => p.product_id)
            .ToList();
        }

        public List<Product> GetProductsByPageAndName(int? pageNum, string name)
        {

            int recordsToSkip = (int)(pageNum - 1) * PAGE_SIZE;

            return db.Products
             .Where(p => p.product_name.Contains(name))
            .OrderBy(p => p.product_id)
            .Skip(recordsToSkip)
            .Take(PAGE_SIZE)
            .ToList();
        }

        public List<Product> GetProductByCategoryId(int? categoryId)
        {
            List<Product> products = db.Products
            .Where(p => p.Categories.Any(c => c.category_id == categoryId))
            .ToList();

            return products;
        }
        public Product GetProductById(int productId)
        {
            var product = db.Products.Find(productId);
            return product;
        }
    }
}