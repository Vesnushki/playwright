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
        public virtual ILocator EmailField => _page.Locator("#customer-email");
        public virtual ILocator FirstName => _page.Locator("[name=firstname]");
        public virtual ILocator LastName => _page.Locator("[name=lastname]");
        public virtual ILocator StreetAddress => _page.GetByLabel("Street Address: Line 1");
        public virtual ILocator State => _page.Locator("[name=region_id]");
        public virtual ILocator City => _page.Locator("[name=city]");
        public virtual ILocator PostCode => _page.Locator("[name=postcode]");
        public virtual ILocator PhoneNumber => _page.Locator("[name=telephone]");
        

        public async Task Check(ILocator locator)
        {
            await locator.CheckAsync();
        }

        public async Task Click(ILocator locator)
        {
            await locator.ClickAsync();
        }

        public async Task FillField(ILocator locator, string val)
        {
            await locator.FillAsync(val);
        }

        public async Task SelectByValue(ILocator locator, string option)
        {
            await locator.SelectOptionAsync(option);
        }
    }
}
