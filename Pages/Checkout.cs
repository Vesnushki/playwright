using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment.Pages
{
    internal class Checkout
    {
        private readonly IPage _page;

        public Checkout(IPage page)
        {
            _page = page;
        }
        public virtual ILocator PayWithCardRedirectMethod => _page.GetByLabel("Pay with Card (Redirect)");
        public virtual ILocator ContinueButton => _page.GetByRole(AriaRole.Button, new() { NameString = "Continue to payment" });



        public async Task Click(ILocator locator)
        {
            await locator.ClickAsync();
        }
    }
}
