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

class OrderWithSubscribtionProductAndSavedCCPaymentMethod : BaseSetup
{
    [Test]
    public async Task OrderWithSubscribtionProductAndSavedCCPaymentMethodTest()
    {
        await Page.GotoAsync(TestSettings.EnvUrl);
        await LoginPage.Click(LoginPage.SignInLink);
        await LoginPage.FillField(LoginPage.EmailField, TestSettings.CustomerEmail);
        await LoginPage.FillField(LoginPage.Password, TestSettings.CustomerPassword);
        await LoginPage.Click(LoginPage.SignInButton);
        await Page.WaitForURLAsync(TestSettings.EnvUrl);
        await Page.WaitForLoadStateAsync();
        await Helper.ShoppingCartClearance(Page);
        await Page.GotoAsync(TestSettings.EnvUrl);
        await Page.WaitForURLAsync(TestSettings.EnvUrl);
        await Page.GotoAsync(TestSettings.SubscriptionProductUrl);
        await ProductPage.SelectByValue(ProductPage.SubscriptionProduct, "16");
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
        await Checkout.Check(Checkout.PayWithSavedCartPaymentMethod);
        await Checkout.Click(Checkout.PlaceOrder);
        await Page.WaitForURLAsync(TestSettings.CheckoutSuccess);
        await Assertion.Expect(Page).ToHaveURLAsync(TestSettings.CheckoutSuccess);
        await Page.GetByText("Thank you for your purchase!").WaitForAsync();
        await Page.GotoAsync(TestSettings.AdminUrl);
        await Admin.Click(Admin.UserName);
        await Admin.FillField(Admin.UserName, TestSettings.AdminUserName);
        await Admin.Click(Admin.Password);
        await Admin.FillField(Admin.Password, TestSettings.AdminPassword);
        await Admin.Click(Admin.SignIn);
        await Page.WaitForURLAsync(TestSettings.AdminDashboardUrl);
        await Admin.Click(Admin.Sales);
        await Admin.Click(Admin.Subscription);
        await Admin.Click(Admin.ViewAllLogs);
        await Admin.Click(Admin.ViewLink.First);
        await Page.WaitForLoadStateAsync();
        await Admin.Click(Admin.BillNow);
        var numbers = await Admin.TimesBilled(Page);
        Console.WriteLine(numbers);
        numbers.Should().BeEquivalentTo("2");

    }
}

