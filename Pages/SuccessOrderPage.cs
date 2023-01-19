﻿using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment.Pages
{
    internal class SuccessOrderPage
    {
        private readonly IPage _page;

        public SuccessOrderPage(IPage page)
        {
            _page = page;
        }
        public virtual string OrderNumber => ".order-number strong";
        public virtual string OrderNumberGuest => ".checkout-success p span:nth-child(1)";
    }
}
