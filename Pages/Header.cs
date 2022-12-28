using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment.Pages
{
    internal class MagentoHeader
    {
        private readonly IPage _page;

        public MagentoHeader(IPage page)
        {
            _page = page;
        }

        public virtual ILocator Menu => _page.GetByRole(AriaRole.Listitem).Filter(new () { HasTextString = "Change My Account My Wish List Sign Out" }).Locator("button");
        public virtual ILocator MyAccount => _page.GetByRole(AriaRole.Link, new() { NameString = "My Account" });




        public async Task Click(ILocator locator)
        {
            await locator.ClickAsync();
        }
      
    }
}
