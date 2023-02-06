using Bogus.DataSets;
using Microsoft.Playwright;
using PeachPayment.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Faker.Finance.Credit;

namespace PeachPayment.TestHelpers
{
    public class Helper
    {

        public async Task ShoppingCartClearance(IPage page)
        {
            await page.WaitForLoadStateAsync();
            await page.GotoAsync(TestSettings.ShoppingCartURL);
            var list = await page.Locator("[title='Remove item'].action-delete").AllAsync();
            if (list != null)
            {
                for (int index = list.Count() - 1; index >= 0; index--)
                {
                    await page.WaitForLoadStateAsync();
                    await list[index].ClickAsync();
                    await page.WaitForLoadStateAsync();
                    await page.WaitForTimeoutAsync(3000);

                }

            }
            return;
        }
        public async Task DeleteCreditCard(IPage page)
        {
            var list = await new MyAccount(page).DeleteCreditCard.AllAsync();
            var list2 = await new MyAccount(page).DeleteCreditCardButton.AllAsync();
            if (list != null)
            {
                for (int index = list.Count() - 1; index >= 0; index--)
                {
                    await page.WaitForLoadStateAsync();
                    await list[index].ClickAsync();
                    await page.WaitForLoadStateAsync();
                    await page.WaitForTimeoutAsync(1000);
                    await list2[index].ClickAsync();
                    await page.WaitForLoadStateAsync();
                }
            }

        }
    }
}
