using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PeachPayment.Pages;
using PlaywrightTests.Pages;
using FluentAssertions;
using PlaywrightTests.Tests;
using Bogus.DataSets;

namespace PeachPayment.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]

class Tests : BaseSetup
{
    [Test]
    [Retry(2)]
    public async Task LoggedInCustomerOrderDetailsWithSimpleProduct()
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
        var orderTitle = await Admin.OrderTitle(Page);
        orderTitle.Should().Contain(orderNumber);
        var status = await Admin.OrderStatus(Page);
        Console.WriteLine(status);
        status.Should().BeEquivalentTo(TestSettings.OrderStatus);

    }

    [Test]
    [Retry(2)]
    public async Task OrderStatusWhenUserCloseRedirectPage()
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

    [Test]
    [Retry(2)]
    public async Task CreditMemos()
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
        await ProductPage.Click(ProductPage.AddToCartButton);
        await Page.GotoAsync(TestSettings.SimpleSecondProductUrl);
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
        await Page.GetByText("Thank you for your purchase!").WaitForAsync();
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
        await Admin.Click(Admin.InvoicesMenu);
        await Admin.Click(Admin.ViewLink);
        await Admin.Click(Admin.CreditMemo);
        var title = await Admin.CreditMemoPageTitle(Page);
        title.Should().Contain("New Memo");
        await Admin.Click(Admin.QtyInputOnCreditMemoPageFirst);
        await Admin.QtyInputOnCreditMemoPageFirst.ClearAsync();
        await Admin.QtyInputOnCreditMemoPageFirst.BlurAsync();
        await Admin.FillField(Admin.QtyInputOnCreditMemoPageFirst, "0");
        await Admin.Click(Admin.UpdateQtyInput);
        await Admin.Click(Admin.RefundButton);
        await Page.WaitForLoadStateAsync();
        await Admin.Click(Admin.CreditMemosMenu);
        await Page.WaitForLoadStateAsync();
        await Page.WaitForTimeoutAsync(5000);
        var recordsText = await Admin.Record(Page);
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
        await Page.WaitForLoadStateAsync();
        await Admin.Click(Admin.CreditMemosMenu);
        await Page.WaitForLoadStateAsync();
        await Page.WaitForTimeoutAsync(5000);
        var recordsText2 = await Admin.Record(Page);
        recordsText2.Should().Contain("2 records found");
    }

    [Test]
    [Retry(2)]
    public async Task OrderWithSubscriptionsForLoggedInCustomer()
    {
        await Page.GotoAsync(TestSettings.EnvUrl);
        await LoginPage.Click(LoginPage.SignInLink);
        await LoginPage.FillField(LoginPage.EmailField, TestSettings.CustomerEmail);
        await LoginPage.FillField(LoginPage.Password, TestSettings.CustomerPassword);
        await LoginPage.Click(LoginPage.SignInButton);
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
        await Page.WaitForLoadStateAsync();
        await ShippingPage.Check(ShippingPage.ShippingMethod);
        await ShippingPage.Click(ShippingPage.NextButton);
        await Page.WaitForURLAsync(TestSettings.CheckoutPaymentUrl);
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

    [Test]
    [Retry(2)]
    public async Task OrderWithSubscriptionsForGuest()
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

    [Test]
    [Retry(2)]
    public async Task OrderWithSubscribtionProductAndSavedCCPaymentMethod()
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

    [Test]
    [Retry(2)]
    public async Task OrderWithSimpleProductAndSavedCCPaymentMethod()
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
        await Page.GotoAsync(TestSettings.SimpleFirstProductUrl);
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
        await Page.WaitForLoadStateAsync();
        await Checkout.Check(Checkout.PayWithSavedCartPaymentMethod);//need to add test attribute
        await Checkout.Click(Checkout.PlaceOrder);
        await Page.WaitForURLAsync(TestSettings.CheckoutSuccess);
        await Assertion.Expect(Page).ToHaveURLAsync(TestSettings.CheckoutSuccess);
        await Page.GetByText("Thank you for your purchase!").WaitForAsync();

    }
    [Test]
    [Retry(2)]
    public async Task AddingNewCard()
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
        await Page.WaitForTimeoutAsync(1000);
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

    [Test]
    [Retry(2)]
    public async Task RemovalOfSavedCreditCards()
    {
        var Page = await Context.NewPageAsync();
        var LoginPage = new CustomerLogin(Page);
        var Header = new MagentoHeader(Page);
        var MyAccount = new MyAccount(Page);
        var Helper = new Helper();
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
        var qtyOfCCBeforeRemove = MyAccount.DeleteCreditCard.CountAsync();
        await Page.WaitForLoadStateAsync();
        await MyAccount.Click(MyAccount.DeleteCreditCard.First);

    }

    [Test]
    [Retry(2)]
    public async Task GuestCheckoutWithSimpleProduct()
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

    [Test]
    [Retry(2)]
    public async Task LoggedInCustomerOrderWithSimpleProductAndDiscountCode()
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
        var orderTitle = await Admin.OrderTitle(Page);
        orderTitle.Should().Contain(orderNumber);
        var capturedAmount = await Admin.CapturedAmount.TextContentAsync();
        Console.WriteLine(capturedAmount);
        capturedAmount.Should().NotContain(orderTotal);

    }

    [Test]
    [Retry(2)]
    public async Task LoggedInCustomerCheckoutWithSimpleAndSubscriptionProductsAndDiscountCodeEmbeddedPM()
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
        await Assertion.Expect(Page).ToHaveURLAsync(TestSettings.CheckoutCartUrl);
        await Page.WaitForLoadStateAsync();
        await ShoppingCart.ProceedToCheckout.WaitForAsync();
        await ShoppingCart.Click(ShoppingCart.ProceedToCheckout);
        await Page.WaitForURLAsync(TestSettings.CheckoutShippingUrl);
        await ShippingPage.Check(ShippingPage.ShippingMethod);
        await ShippingPage.Click(ShippingPage.NextButton);
        await Page.WaitForURLAsync(TestSettings.CheckoutPaymentUrl);
        var orderTotal = await Checkout.OrderTotal.TextContentAsync();
        await Checkout.ApplyDiscountCodeLink.ClickAsync();
        await Checkout.FillField(Checkout.DiscountField, "START");
        await Checkout.Click(Checkout.DiscountButton);
        await Page.WaitForTimeoutAsync(1000);
        var orderTotalAfterDiscount = await Checkout.OrderTotal.TextContentAsync();
        Console.WriteLine(orderTotalAfterDiscount);
        await Checkout.Check(Checkout.PayAndSaveNewCartMethod);
        await Checkout.Click(Checkout.GetCardNumber());
        await Checkout.GetCardNumber().TypeAsync(TestSettings.CreditCardNumber, new() { Delay = 100 });
        await Checkout.Click(Checkout.ExpiryDate);
        await Checkout.FillField(Checkout.ExpiryDate, TestSettings.ExpiryDate);
        await Checkout.Click(Checkout.CardHolder);
        await Checkout.FillField(Checkout.CardHolder, Faker.Name.FullName());
        await Checkout.Click(Checkout.CVV);
        await Checkout.FillField(Checkout.CVV, TestSettings.CVV);
        await Checkout.Click(Checkout.PayNowButton);
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
    [Test]
    [Retry(2)]
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

    [Test]
    [Retry(2)]
    public async Task GuestOrderWithSimpleProductAndDiscountCodeAndCardRedirect()
    {
        await Page.GotoAsync(TestSettings.EnvUrl);
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
        Console.WriteLine(orderNumber);
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
    [Test]
    [Retry(2)]
    public async Task GuestOrderWithSimpleAndSubscribtionProductDiscountCodeAndPayAndSaveNewCard()
    {
        await Page.GotoAsync(TestSettings.EnvUrl);
        await Page.WaitForLoadStateAsync();
        await Helper.ShoppingCartClearance(Page);
        await Page.GotoAsync(TestSettings.SimpleFirstProductUrl);
        await Page.WaitForURLAsync(TestSettings.SimpleFirstProductUrl);
        await ProductPage.Click(ProductPage.AddToCartButton);
        await Page.GotoAsync(TestSettings.SubscriptionProductUrl);
        await ProductPage.SelectByValue(ProductPage.SubscriptionProduct, "16");
        await ProductPage.Click(ProductPage.AddToCartButton);
        await ProductPage.Click(ProductPage.ShoppingCart.First);
        await Page.WaitForURLAsync(TestSettings.CheckoutCartUrl);
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
        var orderTotal = await Checkout.OrderTotal.TextContentAsync();
        Console.WriteLine("OrderTotal" + orderTotal);
        await Checkout.ApplyDiscountCodeLink.ClickAsync();
        await Checkout.FillField(Checkout.DiscountField, "START");
        await Checkout.Click(Checkout.DiscountButton);
        await Page.WaitForTimeoutAsync(2000);
        var orderTotalAfterDiscount = await Checkout.OrderTotal.TextContentAsync();
        Console.WriteLine("After discount" + orderTotalAfterDiscount);
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
        await Page.WaitForURLAsync(TestSettings.CheckoutSuccess);
        await Assertion.Expect(Page).ToHaveURLAsync(TestSettings.CheckoutSuccess);
        await Page.GetByText(TestSettings.OrderSuccessMessage).WaitForAsync();
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

    [Test]
    [Retry(2)]
    public async Task LoggedInUserReInitiatesCheckoutAfterCancel()
    {
        await Page.GotoAsync(TestSettings.EnvUrl);
        await LoginPage.Click(LoginPage.SignInLink);
        await LoginPage.FillField(LoginPage.EmailField, TestSettings.CustomerEmail);
        await LoginPage.FillField(LoginPage.Password, TestSettings.CustomerPassword);
        await LoginPage.Click(LoginPage.SignInButton);
        await Page.WaitForLoadStateAsync();
        await Assertion.Expect(Page).ToHaveURLAsync(TestSettings.EnvUrl);
        await Helper.ShoppingCartClearance(Page);
        await Page.GotoAsync(TestSettings.SimpleFirstProductUrl);
        await Page.WaitForURLAsync(TestSettings.SimpleFirstProductUrl);
        await ProductPage.Click(ProductPage.AddToCartButton);
        await ProductPage.Click(ProductPage.ShoppingCart.First);
        await Page.WaitForURLAsync(TestSettings.CheckoutCartUrl);
        await Assertion.Expect(Page).ToHaveURLAsync(TestSettings.CheckoutCartUrl);
        await Page.WaitForLoadStateAsync();
        await ShoppingCart.ProceedToCheckout.WaitForAsync();
        await ShoppingCart.Click(ShoppingCart.ProceedToCheckout);
        await ShippingPage.Check(ShippingPage.ShippingMethod);
        await ShippingPage.Click(ShippingPage.NextButton);
        await Page.WaitForURLAsync(TestSettings.CheckoutPaymentUrl);
        await Checkout.Click(Checkout.PayWithCardRedirectMethod);
        await Checkout.Click(Checkout.ContinueButton);
        await Page.WaitForURLAsync(TestSettings.CheckoutRedirect);
        await PeachForm.Click(PeachForm.CancelButton);
        await PeachForm.Click(PeachForm.CancelButtonPopup);
        await Page.WaitForURLAsync(TestSettings.ShoppingCartURL);
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
        await Page.WaitForURLAsync(TestSettings.CheckoutSuccess);
        await Assertion.Expect(Page).ToHaveURLAsync(TestSettings.CheckoutSuccess);
        await Page.GetByText(TestSettings.OrderSuccessMessage).WaitForAsync();

    }
}





