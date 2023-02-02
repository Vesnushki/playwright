using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment.Pages
{
    public class ShoppingCart
    {
        private readonly IPage _page;

        public ShoppingCart(IPage page)
        {
            _page = page;
        }
        public virtual ILocator ProceedToCheckout => _page.Locator("button[data-role=\"proceed-to-checkout\"]");
        

        public async Task Click(ILocator locator)
        {
            await locator.ClickAsync();
        }
    }
}
