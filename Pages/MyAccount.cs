using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment.Pages
{
    public class MyAccount
    {
        private readonly IPage _page;

        public MyAccount(IPage page)
        {
            _page = page;
        }

        public virtual ILocator StoredPaymentMethods => _page.GetByText("Stored Payment Methods");
        public virtual ILocator CardNumber => _page.Locator("td.col.card-number");
        public virtual ILocator DeleteCreditCard => _page.Locator(".action.delete span");
        public virtual ILocator DeleteCreditCardButton => _page.Locator(".modal-footer button:nth-child(2)");
        public virtual ILocator AddCreditCard => _page.Locator(".action.primary.new-payment-card");
        public virtual ILocator SavedCardSuccessMessage => _page.GetByText("New card successfully added!");
       // public virtual ILocator SavedCardSuccessMessage => _page.Locator(".message-success div");
     

        public async Task Click(ILocator locator)
        {
            await locator.ClickAsync();
        }
    }
}
