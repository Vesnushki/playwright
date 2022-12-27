using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment.Pages
{
    internal class Admin
    {
        private readonly IPage _page;

        public Admin(IPage page)
        {
            _page = page;
        }
        public virtual ILocator UserName => _page.GetByPlaceholder("user name");
        public virtual ILocator Password => _page.GetByPlaceholder("password");
        public virtual ILocator SignIn => _page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" });
        public virtual ILocator Sales => _page.GetByRole(AriaRole.Link, new() { NameString = " Sales" });
        public virtual ILocator Orders => _page.GetByRole(AriaRole.Link, new() { NameString = "Orders" });


        public async Task Click(ILocator locator)
        {
            await locator.ClickAsync();
        }


        public async Task FillField(ILocator locator, string val)
        {
            await locator.FillAsync(val);
        }

        public async Task WaitViewLinkLoaded(IPage page)
        {
            await page.WaitForSelectorAsync("tbody tr:nth-child(1) a");
        }

        public async Task OpenFirstViewLink(IPage page)
        {
            await page.Locator("tbody tr:nth-child(1) a").First.ClickAsync();
        }

        public async Task OrderTitle(IPage page)
        {
           await page.TextContentAsync(".admin__page-section-item-title span");

        }

    }
}
