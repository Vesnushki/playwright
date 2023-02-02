using NUnit.Framework;
using PeachPayment.Pages;
using PeachPayment.TestHelpers;

namespace PeachPayment.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]

class RemovalOfSavedCreditCards : BaseSetup
{
    [Test]
    public async Task RemovalOfSavedCreditCardsTest()
    {
        await Page.GotoAsync(TestSettings.EnvUrl);
        await LoginPage.Click(LoginPage.SignInLink);
        await LoginPage.FillField(LoginPage.EmailField, TestSettings.CustomerEmail);
        await LoginPage.FillField(LoginPage.Password, TestSettings.CustomerPassword);
        await LoginPage.Click(LoginPage.SignInButton);
        await Page.WaitForURLAsync(TestSettings.EnvUrl);
        await Page.WaitForLoadStateAsync();
        await Helper.ShoppingCartClearance(Page);
        await Header.Click(Header.Menu);
        await Header.Click(Header.MyAccount);
        await MyAccount.Click(MyAccount.StoredPaymentMethods);
        var qtyOfCCBeforeRemove = MyAccount.DeleteCreditCard.CountAsync();
        await Page.WaitForLoadStateAsync();
        await MyAccount.Click(MyAccount.DeleteCreditCard.First);

    }
}
