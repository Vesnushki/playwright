using Microsoft.Playwright;
using Microsoft.VisualBasic;
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
        public virtual ILocator Subscription => _page.GetByRole(AriaRole.Link, new() { NameString = "Subscriptions" });
        public virtual ILocator ViewAllLogs => _page.GetByText("View All Logs");
        public virtual ILocator InvoicesMenu => _page.GetByRole(AriaRole.Tab, new() { NameString = "Invoices" });
        public virtual ILocator CreditMemosMenu => _page.GetByRole(AriaRole.Tab, new() { NameString = "Credit Memos" });
        public virtual ILocator CreditMemo => _page.GetByRole(AriaRole.Button, new() { NameString = "Credit Memo" });
        public virtual ILocator ViewLink => _page.GetByRole(AriaRole.Link, new() { NameString = "View" });
        public virtual ILocator QtyInputOnCreditMemoPageFirst => _page.Locator(".col-qty input").First;
        public virtual ILocator QtyInputOnCreditMemoPageLast => _page.Locator(".col-qty input").Last;
        public virtual ILocator UpdateQtyInput => _page.GetByTitle("Update Qty's");
        public virtual ILocator RefundButton => _page.GetByRole(AriaRole.Button, new() { NameString = "Refund" }).Nth(1);
        public virtual ILocator Records => _page.Locator("#sales_order_view_tabs_order_creditmemos_content > .admin__data-grid-outer-wrap > .admin__data-grid-header > div:nth-child(2) > .col-xs-10 > .row > .col-xs-3");





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

        public async Task<string?> OrderTitle(IPage page)
        {
            var orderTitle = await page.TextContentAsync(".admin__page-section-item-title span");
            return orderTitle;
        }
        public async Task<string?> OrderStatus(IPage page)
        {
            var orderStatus = await page.TextContentAsync("#order_status"); ;
            return orderStatus;
        }
        public async Task<string?> CreditMemoPageTitle(IPage page)
        {
            var pageTitle = await page.TextContentAsync(".page-title"); ;
            return pageTitle;
        }

        public async Task<string?> Record(IPage page)
        {
            var records = await page.TextContentAsync("#sales_order_view_tabs_order_creditmemos_content > .admin__data-grid-outer-wrap > .admin__data-grid-header > div:nth-child(2) > .col-xs-10 > .row > .col-xs-3>.admin__control-support-text");
            return records;
        }




    }
}
