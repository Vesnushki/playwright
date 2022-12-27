using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PeachPayment.Pages;
using PlaywrightTests.Pages;
using FluentAssertions;
using PlaywrightTests.Tests;
using Microsoft.Playwright;

namespace PeachPayment.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]

class Tests : BaseSetup
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
        var Assertion = new PlaywrightTest();
        var Admin = new Admin(page);

        await page.GotoAsync(TestSettings.EnvUrl);
        await LoginPage.Click(LoginPage.SignInLink);
        await LoginPage.FillField(LoginPage.EmailField, TestSettings.CustomerEmail);
        await LoginPage.FillField(LoginPage.Password, TestSettings.CustomerPassword);
        await LoginPage.Click(LoginPage.SignInButton);
        await page.WaitForURLAsync(TestSettings.EnvUrl);
        await ProductPage.Click(ProductPage.ConfigurableProduct);
        await page.WaitForURLAsync(TestSettings.ConfigurableProductUrl);
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
        await page.WaitForURLAsync(TestSettings.CheckoutSuccess);
        await Assertion.Expect(page).ToHaveURLAsync(TestSettings.CheckoutSuccess);
        await page.GetByText("Thank you for your purchase!").WaitForAsync();
        var orderNumber = await page.TextContentAsync(SuccessPage.locator);
        await page.GotoAsync(TestSettings.AdminUrl);
        await Admin.Click(Admin.UserName);
        await Admin.FillField(Admin.UserName, TestSettings.AdminUserName);
        await Admin.Click(Admin.Password);
        await Admin.FillField(Admin.Password, TestSettings.AdminPassword);
        await Admin.Click(Admin.SignIn);
        await page.WaitForURLAsync(TestSettings.AdminDashboardUrl);
        await Admin.Click(Admin.Sales);
        await Admin.Click(Admin.Orders);
        await page.WaitForLoadStateAsync();
        await Admin.WaitViewLinkLoaded(page);
        await Admin.OpenFirstViewLink(page);
        var orderTitle = await page.TextContentAsync(".admin__page-section-item-title span");
        orderTitle.Should().Contain(orderNumber);
        var status = await page.TextContentAsync("#order_status");
        Console.WriteLine(status);
        status.Should().BeEquivalentTo("Processing");

    }

    [Test]
    public async Task CreditMemos()
    {
        var page = await Context.NewPageAsync();
        var LoginPage = new CustomerLogin(page);
        var ProductPage = new ProductPage(page);
        var ShoppingCart = new ShoppingCart(page);
        var ShippingPage = new ShippingPage(page);
        var Checkout = new Checkout(page);
        var PeachForm = new PeachForm(page);
        var SuccessPage = new SuccessOrderPage(page);
        var Admin = new Admin(page);
        var Assertion = new PlaywrightTest();

        await page.GotoAsync(TestSettings.EnvUrl);
        await LoginPage.Click(LoginPage.SignInLink);
        await LoginPage.FillField(LoginPage.EmailField, TestSettings.CustomerEmail);
        await LoginPage.FillField(LoginPage.Password, TestSettings.CustomerPassword);
        await LoginPage.Click(LoginPage.SignInButton);
        await page.WaitForURLAsync(TestSettings.EnvUrl);
        await page.GotoAsync(TestSettings.SimpleFisrtProductUrl);
        await ProductPage.Click(ProductPage.AddToCartButton);
        await page.GotoAsync(TestSettings.SimpleSecondProductUrl);
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
        await page.WaitForURLAsync(TestSettings.CheckoutSuccess);
        await Assertion.Expect(page).ToHaveURLAsync(TestSettings.CheckoutSuccess);
        await page.GetByText("Thank you for your purchase!").WaitForAsync();
        var orderNumber = await page.TextContentAsync(SuccessPage.locator);
        await page.GotoAsync(TestSettings.AdminUrl);
        await Admin.Click(Admin.UserName);
        await Admin.FillField(Admin.UserName, TestSettings.AdminUserName);
        await Admin.Click(Admin.Password);
        await Admin.FillField(Admin.Password, TestSettings.AdminPassword);
        await Admin.Click(Admin.SignIn);
        await page.WaitForURLAsync(TestSettings.AdminDashboardUrl);
        await Admin.Click(Admin.Sales);
        await Admin.Click(Admin.Orders);
        await page.WaitForLoadStateAsync();
        await Admin.WaitViewLinkLoaded(page);
        await Admin.OpenFirstViewLink(page);
        //var orderTitle = await page.TextContentAsync(".admin__page-section-item-title span");
        //orderTitle.Should().Contain(orderNumber);
        //var status = await page.TextContentAsync("#order_status");
        //Console.WriteLine(status);
        //status.Should().BeEquivalentTo("Processing");

    }
    [Test]
    public async Task OrderWithSubscriptionsForLoggedInCustomer()
    {
        var page = await Context.NewPageAsync();
        var LoginPage = new CustomerLogin(page);
        var ProductPage = new ProductPage(page);
        var ShoppingCart = new ShoppingCart(page);
        var ShippingPage = new ShippingPage(page);
        var Checkout = new Checkout(page);
        var PeachForm = new PeachForm(page);
        var SuccessPage = new SuccessOrderPage(page);
        var Admin = new Admin(page);
        var Assertion = new PlaywrightTest();

        await page.GotoAsync(TestSettings.EnvUrl);
        await LoginPage.Click(LoginPage.SignInLink);
        await LoginPage.FillField(LoginPage.EmailField, TestSettings.CustomerEmail);
        await LoginPage.FillField(LoginPage.Password, TestSettings.CustomerPassword);
        await LoginPage.Click(LoginPage.SignInButton);
        await page.WaitForURLAsync(TestSettings.EnvUrl);
        await page.GotoAsync(TestSettings.SubscriptionProductUrl);
        await ProductPage.SelectByValue(ProductPage.SubscriptionProduct, "15");
        await ProductPage.Click(ProductPage.AddToCartButton);
        await ProductPage.Click(ProductPage.ShoppingCart);
        await page.WaitForURLAsync(TestSettings.CheckoutCartUrl);
        await ShoppingCart.Click(ShoppingCart.ProceedToCheckout);
        await page.WaitForURLAsync(TestSettings.CheckoutShippingUrl);
        await ShippingPage.Check(ShippingPage.ShippingMethod);
        await ShippingPage.Click(ShippingPage.NextButton);
        await page.WaitForURLAsync(TestSettings.CheckoutPaymentUrl);
        await Checkout.Click(Checkout.PayAndSaveNewCartMethod);
        await Checkout.Click(Checkout.CardNumber);
        object value = await page.TypeAsync(Checkout.CardNumber.ToString,TestSettings.CreditCardNumber);
        await Checkout.Click(Checkout.ExpiryDate);
        await Checkout.FillField(Checkout.ExpiryDate, TestSettings.ExpiryDate);
        await Checkout.Click(Checkout.CardHolder);
        await Checkout.FillField(Checkout.CardHolder, TestSettings.CardHolder);
        await Checkout.Click(Checkout.CVV);
        await Checkout.FillField(Checkout.CVV, TestSettings.CVV);
        await Checkout.Click(Checkout.PayNowButton);



        //await Checkout.Click(Checkout.ContinueButton);
        //await page.WaitForURLAsync(TestSettings.CheckoutRedirect);
        //await PeachForm.Click(PeachForm.CardNumber);
        //await PeachForm.FillField(PeachForm.CardNumber, TestSettings.CreditCardNumber);
        //await PeachForm.Click(PeachForm.ExpireDate);
        //await PeachForm.FillField(PeachForm.ExpireDate, TestSettings.ExpiryDate);
        //await PeachForm.Click(PeachForm.CardHolder);
        //await PeachForm.FillField(PeachForm.CardHolder, TestSettings.CardHolder);
        //await page.Mouse.WheelAsync(0, 100);
        //await PeachForm.Click(PeachForm.CVV);
        //await PeachForm.FillField(PeachForm.CVV, TestSettings.CVV);
        //await PeachForm.Click(PeachForm.CardNumber);
        //await PeachForm.Click(PeachForm.PayNow);
        //await page.WaitForURLAsync(TestSettings.CheckoutSuccess);
        //await Assertion.Expect(page).ToHaveURLAsync(TestSettings.CheckoutSuccess);
        //await page.GetByText("Thank you for your purchase!").WaitForAsync();
        //var orderNumber = await page.TextContentAsync(SuccessPage.locator);
        //await page.GotoAsync(TestSettings.AdminUrl);
        //await Admin.Click(Admin.UserName);
        //await Admin.FillField(Admin.UserName, TestSettings.AdminUserName);
        //await Admin.Click(Admin.Password);
        //await Admin.FillField(Admin.Password, TestSettings.AdminPassword);
        //await Admin.Click(Admin.SignIn);
        //await page.WaitForURLAsync(TestSettings.AdminDashboardUrl);
        //await Admin.Click(Admin.Sales);
        //await Admin.Click(Admin.Orders);
        //await page.WaitForLoadStateAsync();
        //await Admin.WaitViewLinkLoaded(page);
        //await Admin.OpenFirstViewLink(page);
        ////var orderTitle = await page.TextContentAsync(".admin__page-section-item-title span");
        ////orderTitle.Should().Contain(orderNumber);
        ////var status = await page.TextContentAsync("#order_status");
        ////Console.WriteLine(status);
        ////status.Should().BeEquivalentTo("Processing");


    }
}





