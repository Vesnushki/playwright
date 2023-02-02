﻿using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment.Pages
{
    public class ProductPage
    {
        private readonly IPage _page;

        public ProductPage(IPage page)
        {
            _page = page;
        }
        public virtual ILocator ConfigurableProduct => _page.Locator("#maincontent").GetByText("Radiant Tee");
        public virtual ILocator ProductSize => _page.GetByRole(AriaRole.Option, new() { NameString = "XS" });
        public virtual ILocator ProductColor => _page.GetByRole(AriaRole.Option, new() { NameString = "Blue" });
        public virtual ILocator AddToCartButton => _page.GetByRole(AriaRole.Button, new() { NameString = "Add to Cart" });
        public virtual ILocator ShoppingCart => _page.GetByRole(AriaRole.Link, new() { NameString = "shopping cart" });
        public virtual ILocator SubscriptionProduct => _page.Locator("#select_6");

        public async Task Click(ILocator locator)
        {
            await locator.ClickAsync();
        }


        public async Task SelectByValue(ILocator locator, string option)
        {
            await locator.SelectOptionAsync(option);
        }
    }
}
