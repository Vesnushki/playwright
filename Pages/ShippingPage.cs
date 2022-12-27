using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment.Pages
{
    internal class ShippingPage
    {
        private readonly IPage _page;

        public ShippingPage(IPage page)
        {
            _page = page;
        }
        public virtual ILocator ShippingMethod => _page.GetByRole(AriaRole.Radio, new() { NameString = "Free Shipping" });
        public virtual ILocator NextButton => _page.GetByRole(AriaRole.Button, new() { NameString = "Next" });
        

        public async Task Check(ILocator locator)
        {
            await locator.CheckAsync();
        }

        public async Task Click(ILocator locator)
        {
            await locator.ClickAsync();
        }
    }
}
