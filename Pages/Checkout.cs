﻿using Microsoft.Playwright;
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
        public virtual ILocator PayAndSaveNewCartMethod => _page.GetByLabel("Pay and save New Card");
        public virtual ILocator ContinueButton => _page.GetByRole(AriaRole.Button, new() { NameString = "Continue to payment" });
        public virtual ILocator CardNumber => _page.FrameLocator("iframe").FrameLocator("iframe[name=\"card\\.number\"]").GetByPlaceholder("Card Number");
        public virtual ILocator CardHolder => _page.FrameLocator("iframe").GetByPlaceholder("Card Holder");
        public virtual ILocator ExpiryDate => _page.FrameLocator("iframe").GetByRole(AriaRole.Textbox, new() { NameString = "Expiry Date" });
        public virtual ILocator CVV => _page.FrameLocator("iframe").FrameLocator("iframe[name=\"card\\.cvv\"]").GetByPlaceholder("CVV");
        public virtual ILocator PayNowButton => _page.FrameLocator("iframe").GetByRole(AriaRole.Button, new() { NameString = "Pay now" });



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
