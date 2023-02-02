using FluentAssertions;
using NUnit.Framework;
using PeachPayment.Pages;
using PeachPayment.TestHelpers;

namespace PeachPayment.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
class OrderStatusWhenUserCloseRedirectPage : BaseSetup
{
    [Test]
    public async Task OrderStatusWhenUserCloseRedirectPageTest()
    {
        await Page.GotoAsync(TestSettings.EnvUrl);
        await LoginPage.Click(LoginPage.SignInLink);
        await LoginPage.FillField(LoginPage.EmailField, TestSettings.CustomerEmail);
        await LoginPage.FillField(LoginPage.Password, TestSettings.CustomerPassword);
        await LoginPage.Click(LoginPage.SignInButton);
        await Page.WaitForURLAsync(TestSettings.EnvUrl);
        await Page.WaitForLoadStateAsync();
        await Helper.ShoppingCartClearance(Page);
        await Page.GotoAsync(TestSettings.SimpleFirstProductUrl);
        await Page.WaitForURLAsync(TestSettings.SimpleFirstProductUrl);
        await ProductPage.Click(ProductPage.AddToCartButton);
        await ProductPage.Click(ProductPage.ShoppingCart.First);
        await Assertion.Expect(Page).ToHaveURLAsync(TestSettings.CheckoutCartUrl);
        await Page.WaitForLoadStateAsync();
        await ShoppingCart.ProceedToCheckout.WaitForAsync();
        await ShoppingCart.Click(ShoppingCart.ProceedToCheckout);
        await Page.WaitForURLAsync(TestSettings.CheckoutShippingUrl);
        await ShippingPage.Check(ShippingPage.ShippingMethod);
        await ShippingPage.Click(ShippingPage.NextButton);
        await Page.WaitForURLAsync(TestSettings.CheckoutPaymentUrl);
        await Checkout.Click(Checkout.PayWithCardRedirectMethod);
        await Checkout.Click(Checkout.ContinueButton);
        await Page.WaitForURLAsync(TestSettings.CheckoutRedirect);
        await PeachForm.Click(PeachForm.CardNumber);
        await PeachForm.CardNumber.TypeAsync(TestSettings.CreditCardNumber, new() { Delay = 100 });
        await PeachForm.Click(PeachForm.ExpireDate);
        await PeachForm.FillField(PeachForm.ExpireDate, TestSettings.ExpiryDate);
        await PeachForm.Click(PeachForm.CardHolder);
        await PeachForm.FillField(PeachForm.CardHolder, TestSettings.CardHolder);
        await Page.Mouse.WheelAsync(0, 100);
        await PeachForm.Click(PeachForm.CVV);
        await PeachForm.FillField(PeachForm.CVV, TestSettings.CVV);
        await PeachForm.Click(PeachForm.PayNow);
        await Page.WaitForTimeoutAsync(1000);
        await Page.CloseAsync();
        var Context2 = await Browser.NewContextAsync();
        var Context2Page = await Context2.NewPageAsync();
        await Context2Page.GotoAsync(TestSettings.AdminUrl);
        var Admin2 = new Admin(Context2Page);
        await Admin2.Click(Admin2.UserName);
        await Admin2.FillField(Admin2.UserName, TestSettings.AdminUserName);
        await Admin2.Click(Admin2.Password);
        await Admin2.FillField(Admin2.Password, TestSettings.AdminPassword);
        await Admin2.Click(Admin2.SignIn);
        await Context2Page.WaitForURLAsync(TestSettings.AdminDashboardUrl);
        await Admin2.Click(Admin2.Sales);
        await Admin2.Click(Admin2.Orders);
        await Context2Page.WaitForLoadStateAsync();
        await Admin2.WaitViewLinkLoaded(Context2Page);
        await Admin2.OpenFirstViewLink(Context2Page);
        var status = await Admin2.OrderStatus(Context2Page);
        Console.WriteLine(status);
        status.Should().BeEquivalentTo(TestSettings.OrderStatus);

    }
}
