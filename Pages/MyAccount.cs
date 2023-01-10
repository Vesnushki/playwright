using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment.Pages
{
    internal class MyAccount
    {
        private readonly IPage _page;

        public MyAccount(IPage page)
        {
            _page = page;
        }

        public virtual ILocator StoredPaymentMethods => _page.GetByText("Stored Payment Methods");
        public virtual ILocator CardNumber => _page.Locator("td.col.card-number");
        public virtual ILocator DeleteCreditCard => _page.GetByText("Delete");


        public async Task Click(ILocator locator)
        {
            await locator.ClickAsync();
        }
    }
}
