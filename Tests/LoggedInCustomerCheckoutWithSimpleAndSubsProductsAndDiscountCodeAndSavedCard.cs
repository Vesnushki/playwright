using FluentAssertions;
using NUnit.Framework;
using PeachPayment.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeachPayment.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]

class LCCheckoutWithSimpleAndSubsProductsAndDCAndSavedCardPM : BaseSetup
{
    [Test]
    public async Task LoggedInCustomerCheckoutWithSimpleAndSubsProductsAndDiscountCodeAndSavedCard()//need to doublecheck
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
        await Page.GotoAsync(TestSettings.SubscriptionProductUrl);
        await ProductPage.SelectByValue(ProductPage.SubscriptionProduct, "16");
        await ProductPage.Click(ProductPage.AddToCartButton);
        await ProductPage.Click(ProductPage.ShoppingCart.First);
        await Page.WaitForLoadStateAsync();
        await Assertion.Expect(Page).ToHaveURLAsync(TestSettings.CheckoutCartUrl);
        await Page.WaitForLoadStateAsync();
        await ShoppingCart.ProceedToCheckout.WaitForAsync();
        await ShoppingCart.Click(ShoppingCart.ProceedToCheckout);
        await Page.WaitForURLAsync(TestSettings.CheckoutShippingUrl);
        await ShippingPage.Check(ShippingPage.ShippingMethod);
        await ShippingPage.Click(ShippingPage.NextButton);
        await Page.WaitForURLAsync(TestSettings.CheckoutPaymentUrl);
        var orderTotal = await Checkout.OrderTotal.TextContentAsync();
        Console.WriteLine(orderTotal);
        await Checkout.ApplyDiscountCodeLink.ClickAsync();
        await Checkout.FillField(Checkout.DiscountField, "START");
        await Checkout.Click(Checkout.DiscountButton);
        await Page.WaitForTimeoutAsync(1000);
        var orderTotalAfterDiscount = await Checkout.OrderTotal.TextContentAsync();
        Console.WriteLine(orderTotalAfterDiscount);
        await Checkout.Check(Checkout.PayWithSavedCartPaymentMethod);
        await Checkout.Click(Checkout.PlaceOrder);
        await Page.WaitForURLAsync(TestSettings.CheckoutSuccess);
        await Assertion.Expect(Page).ToHaveURLAsync(TestSettings.CheckoutSuccess);
        await Page.GetByText(TestSettings.OrderSuccessMessage).WaitForAsync();
        var orderNumber = await Page.TextContentAsync(SuccessPage.OrderNumber);
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
        var capturedAmount = await Admin.CapturedAmount2.TextContentAsync();
        Console.WriteLine(capturedAmount);
        capturedAmount.Should().NotContain(orderTotal);

    }
}
