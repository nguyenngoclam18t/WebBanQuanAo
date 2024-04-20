using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTL_LTWeb.Models;

namespace BTL_LTWeb.Services
{
    public class CartService : BaseService
    {
        public List<CartDetail> GetCart()
        {
            List<CartDetail> cart = HttpContext.Current.Session["GioHang"] as List<CartDetail>;

            if (cart == null)
            {
                cart = new List<CartDetail>();
                HttpContext.Current.Session["GioHang"] = cart;
            }
            return cart;
        }
        public void AddToCart(int productId, int quantity, string selectedColor, string selectedSize)
        {
            if (string.IsNullOrEmpty(selectedColor) || string.IsNullOrEmpty(selectedSize))
            {
                return;
            }
            else
            {
                List<CartDetail> cart = GetCart();

                CartDetail existingItem = cart.Find(item => item.ID == productId && item.Color == selectedColor && item.Size == selectedSize);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    Product product = db.Products.Find(productId);

                    if (product != null)
                    {
                        CartDetail newItem = new CartDetail(productId, selectedColor, selectedSize)
                        {
                            ID = productId,
                            Picture = product.image_url,
                            ProductName = product.product_name,
                            Price = double.Parse(product.price.ToString()),
                            Quantity = quantity
                        };

                        cart.Add(newItem);
                    }
                }
                HttpContext.Current.Session["GioHang"] = cart;
            }
        }
        public void UpdateCart(int productId, int quantity)
        {
            List<CartDetail> cart = GetCart();
            CartDetail existingItem = cart.Find(item => item.ID == productId);

            if (existingItem != null)
            {
                existingItem.Quantity = quantity;
                HttpContext.Current.Session["GioHang"] = cart;
            }
        }
        public void RemoveFromCart(int productId)
        {
            List<CartDetail> cart = GetCart();
            CartDetail itemToRemove = cart.Find(item => item.ID == productId);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Current.Session["GioHang"] = cart;
            }
        }
    }
}