using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PeachPayment.Pages;
using PlaywrightTests.Pages;
using FluentAssertions;
using PlaywrightTests.Tests;
using Microsoft.Playwright;
using System.Text.RegularExpressions;
using System.Security.Principal;

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
        await page.GotoAsync(TestSettings.SimpleFisrtProductUrl);
        await page.WaitForURLAsync(TestSettings.SimpleFisrtProductUrl);
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
        await PeachForm.CardNumber.TypeAsync(TestSettings.CreditCardNumber, new() { Delay = 100 });
        await PeachForm.Click(PeachForm.ExpireDate);
        await PeachForm.FillField(PeachForm.ExpireDate, TestSettings.ExpiryDate);
        await PeachForm.Click(PeachForm.CardHolder);
        await PeachForm.FillField(PeachForm.CardHolder, TestSettings.CardHolder);
        await page.Mouse.WheelAsync(0, 100);
        await PeachForm.Click(PeachForm.CVV);
        await PeachForm.FillField(PeachForm.CVV, TestSettings.CVV);
        await PeachForm.Click(PeachForm.PayNow);
        await page.WaitForURLAsync(TestSettings.CheckoutSuccess);
        await Assertion.Expect(page).ToHaveURLAsync(TestSettings.CheckoutSuccess);
        await page.GetByText(TestSettings.OrderSuccessMessage).WaitForAsync();
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
        var orderTitle = await Admin.OrderTitle(page);
        orderTitle.Should().Contain(orderNumber);
        var status = await Admin.OrderStatus(page);
        Console.WriteLine(status);
        status.Should().BeEquivalentTo(TestSettings.OrderStatus);

    }

    [Test]
    public async Task OrderStatusWhenUserCloseRedirectPage()
    {
        var page = await Context.NewPageAsync();
        var LoginPage = new CustomerLogin(page);
        var ProductPage = new ProductPage(page);
        var ShoppingCart = new ShoppingCart(page);
        var ShippingPage = new ShippingPage(page);
        var Checkout = new Checkout(page);
        var PeachForm = new PeachForm(page);
        
        await page.GotoAsync(TestSettings.EnvUrl);
        await LoginPage.Click(LoginPage.SignInLink);
        await LoginPage.FillField(LoginPage.EmailField, TestSettings.CustomerEmail);
        await LoginPage.FillField(LoginPage.Password, TestSettings.CustomerPassword);
        await LoginPage.Click(LoginPage.SignInButton);
        await page.WaitForURLAsync(TestSettings.EnvUrl);
        await page.GotoAsync(TestSettings.SimpleFisrtProductUrl);
        await page.WaitForURLAsync(TestSettings.SimpleFisrtProductUrl);
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
        await PeachForm.CardNumber.TypeAsync(TestSettings.CreditCardNumber, new() { Delay = 100 });
        await PeachForm.Click(PeachForm.ExpireDate);
        await PeachForm.FillField(PeachForm.ExpireDate, TestSettings.ExpiryDate);
        await PeachForm.Click(PeachForm.CardHolder);
        await PeachForm.FillField(PeachForm.CardHolder, TestSettings.CardHolder);
        await page.Mouse.WheelAsync(0, 100);
        await PeachForm.Click(PeachForm.CVV);
        await PeachForm.FillField(PeachForm.CVV, TestSettings.CVV);
        await PeachForm.Click(PeachForm.PayNow);
        await page.WaitForTimeoutAsync(1000);
        await page.CloseAsync();
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
        await Admin.Click(Admin.InvoicesMenu);
        await Admin.Click(Admin.ViewLink);
        await Admin.Click(Admin.CreditMemo);
        var title = await Admin.CreditMemoPageTitle(page);
        title.Should().Contain("New Memo");
        await Admin.Click(Admin.QtyInputOnCreditMemoPageFirst);
        await Admin.QtyInputOnCreditMemoPageFirst.ClearAsync();
        await Admin.QtyInputOnCreditMemoPageFirst.BlurAsync();
        await Admin.FillField(Admin.QtyInputOnCreditMemoPageFirst, "0");
        await Admin.Click(Admin.UpdateQtyInput);
        await Admin.Click(Admin.RefundButton);
        await page.WaitForLoadStateAsync();
        await Admin.Click(Admin.CreditMemosMenu);
        await page.WaitForLoadStateAsync();
        await page.WaitForTimeoutAsync(5000);
        var recordsText = await Admin.Record(page);
        recordsText.Should().Contain("1 records found");
        await Admin.Click(Admin.InvoicesMenu);
        await Admin.Click(Admin.ViewLink);
        await Admin.Click(Admin.CreditMemo);
        await Admin.Click(Admin.QtyInputOnCreditMemoPageLast);
        await Admin.QtyInputOnCreditMemoPageLast.ClearAsync();
        await Admin.QtyInputOnCreditMemoPageLast.BlurAsync();
        await Admin.FillField(Admin.QtyInputOnCreditMemoPageLast, "0");
        await Admin.Click(Admin.UpdateQtyInput);
        await Admin.Click(Admin.RefundButton);
        await page.WaitForLoadStateAsync();
        await Admin.Click(Admin.CreditMemosMenu);
        await page.WaitForLoadStateAsync();
        await page.WaitForTimeoutAsync(5000);
        var recordsText2 = await Admin.Record(page);
        recordsText2.Should().Contain("2 records found");
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
        var Assertion = new PlaywrightTest();
        var Header = new MagentoHeader(page);
        var MyAccount = new MyAccount(page);
        var Admin = new Admin(page);

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
        await Checkout.Click(Checkout.GetCardNumber());
        await Checkout.GetCardNumber().TypeAsync(TestSettings.CreditCardNumber, new() { Delay = 100 });
        await Checkout.Click(Checkout.ExpiryDate);
        await Checkout.FillField(Checkout.ExpiryDate, TestSettings.ExpiryDate);
        await Checkout.Click(Checkout.CardHolder);
        await Checkout.FillField(Checkout.CardHolder, TestSettings.CardHolder);
        await Checkout.Click(Checkout.CVV);
        await Checkout.FillField(Checkout.CVV, TestSettings.CVV);
        await Checkout.Click(Checkout.PayNowButton);
        await page.WaitForURLAsync(TestSettings.CheckoutSuccess);
        await Assertion.Expect(page).ToHaveURLAsync(TestSettings.CheckoutSuccess);
        await page.GetByText("Thank you for your purchase!").WaitForAsync();
        await Header.Click(Header.Menu);
        await Header.Click(Header.MyAccount);
        await MyAccount.Click(MyAccount.StoredPaymentMethods);
        //await Assertion.Expect(MyAccount.CardNumber).ToContainTextAsync(TestSettings.CreditCardNumber.Substring(TestSettings.CreditCardNumber.Length - 4));
        //var list = await MyAccount.CardNumber.AllTextContentsAsync();
        //var newList = list.Select(s => s.Replace("ending", "")).ToList();
        //Console.WriteLine(TestSettings.CreditCardNumber.Substring(TestSettings.CreditCardNumber.Length - 4));
        //newList.Contains(TestSettings.CreditCardNumber.Substring(TestSettings.CreditCardNumber.Length - 4)).Should().BeTrue();
        await page.GotoAsync(TestSettings.AdminUrl);
        await Admin.Click(Admin.UserName);
        await Admin.FillField(Admin.UserName, TestSettings.AdminUserName);
        await Admin.Click(Admin.Password);
        await Admin.FillField(Admin.Password, TestSettings.AdminPassword);
        await Admin.Click(Admin.SignIn);
        await page.WaitForURLAsync(TestSettings.AdminDashboardUrl);
        await Admin.Click(Admin.Sales);
        await Admin.Click(Admin.Subscription);
        await Admin.Click(Admin.ViewAllLogs);


    }
}





