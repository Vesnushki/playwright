﻿using FluentAssertions;
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

class OrderWithSubscriptionsForGuest : BaseSetup
{
    [Test]
    public async Task OrderWithSubscriptionsForGuestTest()
    {
        await Page.GotoAsync(TestSettings.EnvUrl);
        await Page.WaitForURLAsync(TestSettings.EnvUrl);
        await Page.WaitForLoadStateAsync();
        await Helper.ShoppingCartClearance(Page);
        await Page.GotoAsync(TestSettings.SubscriptionProductUrl);
        await ProductPage.SelectByValue(ProductPage.SubscriptionProduct, "16");
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
        await Checkout.Click(Checkout.PayAndSaveNewCartMethod);
        await Page.WaitForLoadStateAsync();
        await Checkout.Click(Checkout.GetCardNumber());
        await Checkout.GetCardNumber().TypeAsync(TestSettings.CreditCardNumber, new() { Delay = 100 });
        await Checkout.Click(Checkout.ExpiryDate);
        await Checkout.FillField(Checkout.ExpiryDate, TestSettings.ExpiryDate);
        await Checkout.Click(Checkout.CardHolder);
        await Checkout.FillField(Checkout.CardHolder, TestSettings.CardHolder);
        await Checkout.Click(Checkout.CVV);
        await Checkout.FillField(Checkout.CVV, TestSettings.CVV);
        await Checkout.Click(Checkout.PayNowButton);
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
        await Admin.Click(Admin.BillNow);
        var numbers = await Admin.TimesBilled(Page);
        Console.WriteLine(numbers);
        numbers.Should().BeEquivalentTo("2");

    }
}
