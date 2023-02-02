using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment.Pages
{
    public class PeachForm
    {
        private readonly IPage _page;

        public PeachForm(IPage page)
        {
            _page = page;
        }
        public virtual ILocator CardNumber => _page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").FrameLocator("iframe[name=\"card\\.number\"]").GetByPlaceholder("Card Number");
        public virtual ILocator ExpireDate => _page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").GetByRole(AriaRole.Textbox, new() { NameString = "Expiry Date" });
        public virtual ILocator CardHolder => _page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").GetByPlaceholder("Card Holder");
        public virtual ILocator CVV => _page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").FrameLocator("iframe[name=\"card\\.cvv\"]").GetByPlaceholder("CVV");
        public virtual ILocator PayNow => _page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").GetByRole(AriaRole.Button, new () { NameString = "Pay now" });
        public virtual ILocator CancelButton => _page.Locator("button .MuiButton-label");
        public virtual ILocator CancelButtonPopup => _page.Locator("[data-testid=\"dialog-cancel\"]");


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
