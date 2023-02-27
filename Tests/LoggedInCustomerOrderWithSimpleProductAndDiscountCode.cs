using FluentAssertions;
using NUnit.Framework;
using PeachPayment.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment.Tests;

[NonParallelizable]
[TestFixture]

class LoggedInCustomerOrderWithSimpleProductAndDiscountCode : BaseSetup
{
    [Test]
    public async Task LoggedInCustomerOrderWithSimpleProductAndDiscountCodeTest()
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
        var orderTotal = await Checkout.OrderTotal.TextContentAsync();
        Console.WriteLine("OrderTotal" + orderTotal);
        await Checkout.ApplyDiscountCodeLink.ClickAsync();
        await Checkout.FillField(Checkout.DiscountField, "START");
        await Checkout.Click(Checkout.DiscountButton);
        await Page.WaitForTimeoutAsync(2000);
        var orderTotalAfterDiscount = await Checkout.OrderTotal.TextContentAsync();
        Console.WriteLine("After discount" + orderTotalAfterDiscount);
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
        await Page.WaitForURLAsync(TestSettings.CheckoutSuccess);
        await Assertion.Expect(Page).ToHaveURLAsync(TestSettings.CheckoutSuccess);
        await Page.GetByText(TestSettings.OrderSuccessMessage).WaitForAsync();
        var orderNumber = await Page.TextContentAsync(SuccessPage.OrderNumberGuest);
        await Page.GotoAsync(TestSettings.AdminUrl);
        await Admin.Click(Admin.UserName);
        await Admin.FillField(Admin.UserName, TestSettings.AdminUserName);
        await Admin.Click(Admin.Password);
        await Admin.FillField(Admin.Password, TestSettings.AdminPassword);
        await Admin.Click(Admin.SignIn);
        await Page.WaitForURLAsync(TestSettings.AdminDashboardUrl);
        await Admin.Click(Admin.Sales);
        await Admin.Click(Admin.Orders);
        await Page.WaitForLoadStateAsync();
        await Admin.WaitViewLinkLoaded(Page);
        await Admin.OpenFirstViewLink(Page);
        var orderTitle = await Admin.OrderTitle(Page);
        orderTitle.Should().Contain(orderNumber);
        var capturedAmount = await Admin.CapturedAmount.TextContentAsync();
        Console.WriteLine(capturedAmount);
        capturedAmount.Should().NotContain(orderTotal);

    }
}
