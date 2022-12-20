using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment
{
    internal class TestSettings
    {
        public static string EnvUrl { get; set; } = "https://magento.x2y.dev/";
        public static string ProductUrl { get; set; } = "https://magento.x2y.dev/radiant-tee.html";
        public static string CheckoutCartUrl { get; set; } = "https://magento.x2y.dev/checkout/cart/";
        public static string CheckoutShippingUrl { get; set; } = "https://magento.x2y.dev/checkout/#shipping";
        public static string CheckoutPaymentUrl { get; set; } = "https://magento.x2y.dev/checkout/#payment";
        public static string CheckoutRedirect { get; set; } = "https://testsecure.peachpayments.com/checkout";
        public static string CheckoutSuccess { get; set; } = "https://magento.x2y.dev/checkout/onepage/success";


    }
    
}
