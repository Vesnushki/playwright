using FluentAssertions;
using NUnit.Framework;
using PeachPayment.Pages;
using PeachPayment.TestHelpers;

namespace PeachPayment.Tests;

[NonParallelizable]
[TestFixture]
class CreditMemos : BaseSetup
{

    [Test]
    public async Task CreditMemosTest()
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
}

