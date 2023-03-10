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

class GuestCheckoutWithSimpleProduct : BaseSetup
{
    [Test]
    public async Task GuestCheckoutWithSimpleProductTest()
    {
        await Page.GotoAsync(TestSettings.EnvUrl);
        await Page.WaitForURLAsync(TestSettings.EnvUrl);
        await Page.WaitForLoadStateAsync();
        await Helper.ShoppingCartClearance(Page);
        await Page.GotoAsync(TestSettings.SimpleFirstProductUrl);
        await ProductPage.Click(ProductPage.AddToCartButton);
        await ProductPage.Click(ProductPage.ShoppingCart.First);
        await Assertion.Expect(Page).ToHaveURLAsync(TestSettings.CheckoutCartUrl);
        await Page.WaitForLoadStateAsync();
        await ShoppingCart.ProceedToCheckout.WaitForAsync();
        await ShoppingCart.Click(ShoppingCart.ProceedToCheckout);
        await Page.WaitForURLAsync(TestSettings.CheckoutShippingUrl);
        await ShippingPage.Click(ShippingPage.EmailField);
        await ShippingPage.FillField(ShippingPage.EmailField, TestSettings.GuestEmail);
        await ShippingPage.Click(ShippingPage.FirstName);
        await ShippingPage.FillField(ShippingPage.FirstName, TestSettings.GuestFirstName);
        await ShippingPage.Click(ShippingPage.LastName);
        await ShippingPage.FillField(ShippingPage.LastName, TestSettings.GuestLastName);
        await ShippingPage.Click(ShippingPage.StreetAddress);
        await ShippingPage.FillField(ShippingPage.StreetAddress, TestSettings.GuestStreetAddress);
        await ShippingPage.SelectByValue(ShippingPage.State, "1");
        await ShippingPage.Click(ShippingPage.City);
        await ShippingPage.FillField(ShippingPage.City, TestSettings.GuestCity);
        await ShippingPage.Click(ShippingPage.PostCode);
        await ShippingPage.FillField(ShippingPage.PostCode, TestSettings.GuestZipCode);
        await ShippingPage.Click(ShippingPage.PhoneNumber);
        await ShippingPage.FillField(ShippingPage.PhoneNumber, TestSettings.GuestPhoneNumber);
        await ShippingPage.Check(ShippingPage.ShippingMethod);
        await ShippingPage.Click(ShippingPage.NextButton);
        await Page.WaitForURLAsync(TestSettings.CheckoutPaymentUrl);
        await Checkout.Click(Checkout.PayWithCardRedirectMethod);
        await Checkout.Click(Checkout.ContinueButton);
        await Page.WaitForURLAsync(TestSettings.CheckoutRedirect);
        await PeachForm.Click(PeachForm.CardNumber);
        await PeachForm.FillField(PeachForm.CardNumber, TestSettings.CreditCardNumber);
        await PeachForm.Click(PeachForm.ExpireDate);
        await PeachForm.FillField(PeachForm.ExpireDate, TestSettings.ExpiryDate);
        await PeachForm.Click(PeachForm.CardHolder);
        await PeachForm.FillField(PeachForm.CardHolder, TestSettings.CardHolder);
        await Page.Mouse.WheelAsync(0, 100);
        await PeachForm.Click(PeachForm.CVV);
        await PeachForm.FillField(PeachForm.CVV, TestSettings.CVV);
        await PeachForm.Click(PeachForm.CardNumber);
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
        var status = await Admin.OrderStatus(Page);
        Console.WriteLine(status);
        status.Should().BeEquivalentTo(TestSettings.OrderStatus);
    }
}
