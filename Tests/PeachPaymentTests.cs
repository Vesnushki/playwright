using FluentAssertions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PeachPayment.Pages;
using PlaywrightTests.Pages;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PeachPayment.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]

class Tests : PageTest
{
    [Test]
    public async Task LoggedInCustomerOrderDetailsWithSimpleProduct()
    {
        var page = await Context.NewPageAsync();
        var LoginPage = new CustomerLogin(page);
        var ProductPage = new ProductPage(page);
        var ShoppingCart = new ShoppingCart(page);
        var ShippingPage = new ShippingPage(page);
        var Checkout = new Checkout(page);
        var PeachForm = new PeachForm(page);
        var SuccessPage = new SuccessOrderPage(page);
        var AdminLogin = new AdminLogin(page);

        await page.GotoAsync(TestSettings.EnvUrl);
        await LoginPage.Click(LoginPage.SignInLink);
        await LoginPage.FillField(LoginPage.EmailField,TestSettings.CustomerEmail);
        await LoginPage.FillField(LoginPage.Password, TestSettings.CustomerPassword);
        await LoginPage.Click(LoginPage.SignInButton);
        await page.WaitForURLAsync(TestSettings.EnvUrl);
        await ProductPage.Click(ProductPage.Product);
        await page.WaitForURLAsync(TestSettings.ProductUrl);
        await ProductPage.Click(ProductPage.ProductSize);
        await ProductPage.Click(ProductPage.ProductColor);
        await ProductPage.Click(ProductPage.AddToCartButton);
        await ProductPage.Click(ProductPage.ShoppingCart);
        await page.WaitForURLAsync(TestSettings.CheckoutCartUrl);
        await ShoppingCart.Click(ShoppingCart.ProceedToCheckout);
        await page.WaitForURLAsync(TestSettings.CheckoutShippingUrl);
        await ShippingPage.Check(ShippingPage.ShippingMethod);
        await ShippingPage.Click(ShippingPage.NextButton);
        await page.WaitForURLAsync(TestSettings.CheckoutPaymentUrl);
        await Checkout.Click(Checkout.PayWithCardRedirectMethod);
        await Checkout.Click(Checkout.ContinueButton);
        await page.WaitForURLAsync(TestSettings.CheckoutRedirect);
        await PeachForm.Click(PeachForm.CardNumber);
        await PeachForm.FillField(PeachForm.CardNumber, TestSettings.CreditCardNumber);
        await PeachForm.Click(PeachForm.ExpireDate);
        await PeachForm.FillField(PeachForm.ExpireDate, TestSettings.ExpiryDate);
        await PeachForm.Click(PeachForm.CardHolder);
        await PeachForm.FillField(PeachForm.CardHolder, TestSettings.CardHolder);
        await page.Mouse.WheelAsync(0, 100);
        await PeachForm.Click(PeachForm.CVV);
        await PeachForm.FillField(PeachForm.CVV, TestSettings.CVV);
        await PeachForm.Click(PeachForm.CardNumber);
        await PeachForm.Click(PeachForm.PayNow);
        await page.WaitForURLAsync("https://magento.x2y.dev/checkout/onepage/success/");
        await Expect(page).ToHaveURLAsync("https://magento.x2y.dev/checkout/onepage/success/");
        await page.GetByText("Thank you for your purchase!").WaitForAsync();
        var orderNumber = await page.TextContentAsync(SuccessPage.locator);
        await page.GotoAsync(TestSettings.AdminUrl);
        await AdminLogin.Click(AdminLogin.UserName);
        await AdminLogin.FillField(AdminLogin.UserName,TestSettings.AdminUserName);
        await AdminLogin.Click(AdminLogin.Password);
        await AdminLogin.FillField(AdminLogin.Password,TestSettings.AdminPassword);
        await AdminLogin.Click(AdminLogin.SignIn);  

        //await Page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" }).ClickAsync();

        //await Page.WaitForURLAsync("https://magento.x2y.dev/admin/admin/dashboard/");

        //await Page.GetByRole(AriaRole.Link, new() { NameString = " Sales" }).ClickAsync();

        //await Page.GetByRole(AriaRole.Link, new() { NameString = "Orders" }).ClickAsync();

        //await Page.WaitForLoadStateAsync();

        //await Page.WaitForSelectorAsync("tbody tr:nth-child(1) a");

        //await Page.Locator("tbody tr:nth-child(1) a").First.ClickAsync();

        //var orderTitle = await Page.TextContentAsync(".admin__page-section-item-title span");

        //orderTitle.Should().Contain(SuccessPage.TextContent(););

        //var status = await Page.TextContentAsync("#order_status");

        //Console.WriteLine(status);

        //status.Should().BeEquivalentTo("Processing");

    }
}




