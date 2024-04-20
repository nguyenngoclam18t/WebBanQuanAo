using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTL_LTWeb.Models;
using BTL_LTWeb.Services;
using Stripe;

namespace BTL_LTWeb.Controllers
{
    public class CheckoutController : Controller
    {
        private CheckoutService checkoutService = new CheckoutService();
        // GET: Checkout
        public ActionResult Index()
        {
            List<CartDetail> carts = checkoutService.getCartDetailsBySession();
            return View(carts);
        }


        public ActionResult Submit(string stripeToken, string stripeEmail)
        {
            Account currentAccount = Session["account"] as Account;
            StripeConfiguration.SetApiKey("sk_test_51KATlDIqI2QEiuWA7qeqyeDF31nnHYH87OrSTsE2PZFG0ucx3M9csQZCYHaUuDdgiZHfRqMPkf47qeTwsRGeKFqX00GQIoEXDY");

            //List<Cart> carts = checkoutService.getCartByAccountName(currentAccount.account_name);
            List<CartDetail> carts = checkoutService.getCartDetailsBySession();

            decimal vndToUsdExchangeRate = 22000; // Change based on the actual exchange rate
            decimal vndTotal = (decimal)carts.Select(c => c.Total).Sum();

            decimal usdTotal = vndTotal / vndToUsdExchangeRate;

            // Ensure that the amount is at least 50 cents in USD
            int amountInCent = Math.Max((int)(usdTotal * 100), 50);


            var options = new StripeChargeCreateOptions
            {
                Amount = amountInCent, // Số tiền thanh toán (cent), ví dụ: $10
                Currency = "usd",
                Description = $"Khách hàng thanh toán",
                SourceTokenOrExistingSourceId = stripeToken,
            };

            var service = new StripeChargeService();
            StripeCharge charge = service.Create(options);

            checkoutService.changeCartToInvoiceDetails(currentAccount.account_name);

            Session["GioHang"] = new List<CartDetail>();

            return RedirectToAction("GetInvoices", "Invoice");
        }
    }
}