using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment.Pages
{
    internal class AdminLogin
    {
        private readonly IPage _page;

        public AdminLogin(IPage page)
        {
            _page = page;
        }
        public virtual ILocator UserName => _page.GetByPlaceholder("user name");
        public virtual ILocator Password => _page.GetByPlaceholder("password");
        public virtual ILocator SignIn => _page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" });

        public async Task Click(ILocator locator)
        {
            await locator.ClickAsync();
        }


        public async Task FillField(ILocator locator, string val)
        {
            await locator.FillAsync(val);
        }
    }
}
