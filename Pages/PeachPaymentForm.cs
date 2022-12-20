using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment.Pages
{
    internal class PeachPaymentForm
    {
        private readonly IPage _page;

        public PeachPaymentForm(IPage page)
        {
            _page = page;
        }
        public virtual ILocator CreditCardNumber => _page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").FrameLocator("iframe[name=\"card\\.number\"]").GetByPlaceholder("Card Number");
        public virtual ILocator ExpiryDate => _page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").GetByRole(AriaRole.Textbox, new() { NameString = "Expiry Date" });
        public virtual ILocator CardHolder => _page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").GetByPlaceholder("Card Holder");
        public virtual ILocator CardCVV => _page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").FrameLocator("iframe[name=\"card\\.cvv\"]").GetByPlaceholder("CVV");
        public virtual ILocator CardNumber => _page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").FrameLocator("iframe[name=\"card\\.number\"]").GetByPlaceholder("Card Number");
        public virtual ILocator PayNow => _page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").GetByRole(AriaRole.Button, new() { NameString = "Pay now" });

    }
}
