using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment.Pages
{
    internal class ShoppingCart
    {
        private readonly IPage _page;

        public ShoppingCart(IPage page)
        {
            _page = page;
        }
        public virtual ILocator ProceedToCheckout => _page.GetByRole(AriaRole.Button, new() { NameString = "Proceed to Checkout" });
        

        public async void Click(ILocator locator)
        {
            await locator.ClickAsync();
        }
    }
}
