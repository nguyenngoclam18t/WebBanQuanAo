using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTL_LTWeb.Models;
using BTL_LTWeb.Services;

namespace BTL_LTWeb.Controllers
{
    public class CartController : Controller
    {
        private CartService cartService = new CartService();

        public ActionResult Cart()
        {
            List<CartDetail> cart = cartService.GetCart();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            return View(cart);
        }
        public ActionResult AddToCart(int productId, int quantity, string selectedColor, string selectedSize)
        {
            cartService.AddToCart(productId, quantity, selectedColor, selectedSize);
            ViewBag.TongSoLuong = TongSoLuong();
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public ActionResult UpdateCart(int productId, int quantity)
        {
            cartService.UpdateCart(productId, quantity);
            ViewBag.TongSoLuong = TongSoLuong();
            return RedirectToAction("Cart");
        }
        public ActionResult RemoveFromCart(int productId)
        {
            cartService.RemoveFromCart(productId);
            ViewBag.TongSoLuong = TongSoLuong();
            return RedirectToAction("Cart");
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<CartDetail> lstGioHang = Session["GioHang"] as List<CartDetail>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Sum(sp => sp.Quantity);
            }
            Session["Tong so luong"] = tsl;
            return tsl;
        }
        private double TongThanhTien()
        {
            double ttt = 0;
            List<CartDetail> lstGioHang = Session["GioHang"] as List<CartDetail>;
            if (lstGioHang != null)
            {
                ttt += lstGioHang.Sum(sp => sp.Total);
            }
            return ttt;
        }
        [HttpPost]
        public ActionResult ClearCart()
        {
            List<CartDetail> cart = cartService.GetCart();
            cart.Clear();
            ViewBag.TongSoLuong = 0;
            ViewBag.TongThanhTien = 0;
            return RedirectToAction("Cart");
        }
    }
}
