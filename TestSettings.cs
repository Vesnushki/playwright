using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment
{
    internal class TestSettings
    {
        public static string EnvUrl { get; set; } = "https://magento.x2y.dev/";
        public static string AdminUrl { get; set; } = "https://magento.x2y.dev/admin";
        public static string ConfigurableProductUrl { get; set; } = "https://magento.x2y.dev/radiant-tee.html";
        public static string SimpleFisrtProductUrl { get; set; } = "https://magento.x2y.dev/push-it-messenger-bag.html";
        public static string SimpleSecondProductUrl { get; set; } = "https://magento.x2y.dev/dash-digital-watch.html";
        public static string SubscriptionProductUrl { get; set; } = "https://magento.x2y.dev/compete-track-tote.html";
        public static string CheckoutCartUrl { get; set; } = "https://magento.x2y.dev/checkout/cart/";
        public static string CheckoutShippingUrl { get; set; } = "https://magento.x2y.dev/checkout/#shipping";
        public static string CheckoutPaymentUrl { get; set; } = "https://magento.x2y.dev/checkout/#payment";
        public static string CheckoutRedirect { get; set; } = "https://testsecure.peachpayments.com/checkout";
        public static string CheckoutSuccess { get; set; } = "https://magento.x2y.dev/checkout/onepage/success/";
        public static string AdminDashboardUrl { get; set; } = "https://magento.x2y.dev/admin/admin/dashboard/";
        public static string CreditCardNumber { get; set; } = "5105105105105100";
        public static string ExpiryDate { get; set; } = "12 / 28";
        public static string CardHolder { get; set; } = "Testov Test";
        public static string CVV { get; set; } = "123";
        public static string CustomerEmail { get; set; } = "roni_cost@example.com";
        public static string CustomerPassword { get; set; } = "roni_cost3@example.com";
        public static string AdminUserName { get; set; } = "magento";
        public static string AdminPassword { get; set; } = "Password1";




    }
    
}
