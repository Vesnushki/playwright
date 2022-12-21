using Microsoft.Playwright;
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
        public virtual string locator  => ".order-number strong";

    }
}
