using FluentAssertions;
using NUnit.Allure.Core;
using NUnit.Framework;
using PeachPayment.Pages;
using PeachPayment.TestHelpers;

namespace PeachPayment.Tests;
[NonParallelizable]
[TestFixture]
//[AllureNUnit]

class AddingNewCard : BaseSetup
{
    [Test]
    public async Task AddingNewCardTest()
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
        await Page.WaitForLoadStateAsync();
        await Page.WaitForTimeoutAsync(2000);
        await Helper.DeleteCreditCard(Page);
        await Page.WaitForLoadStateAsync();
        var qtyOfCCBefore = MyAccount.DeleteCreditCard.CountAsync();
        await MyAccount.Click(MyAccount.AddCreditCard);
        await Page.WaitForLoadStateAsync();
        await Checkout.Click(Checkout.GetCardNumber());
        await Checkout.GetCardNumber().TypeAsync(TestSettings.CreditCardNumber, new() { Delay = 100 });
        await Checkout.Click(Checkout.ExpiryDate);
        await Checkout.FillField(Checkout.ExpiryDate, TestSettings.ExpiryDate);
        await Checkout.Click(Checkout.CardHolder);
        await Checkout.FillField(Checkout.CardHolder, TestSettings.CardHolder);
        await Checkout.Click(Checkout.CVV);
        await Checkout.FillField(Checkout.CVV, TestSettings.CVV);
        await Checkout.Click(Checkout.AddNewCard);
        await Assertion.Expect(MyAccount.SavedCardSuccessMessage).ToBeVisibleAsync();
        var qtyOfCCAfter = MyAccount.DeleteCreditCard.CountAsync();
        (qtyOfCCAfter != qtyOfCCBefore).Should().BeTrue();

    }
}

