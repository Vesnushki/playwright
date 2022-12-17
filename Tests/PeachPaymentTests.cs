using FluentAssertions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PeachPayment.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]

class Tests : PageTest
{
    [Test]
    public async Task Main()
    {
        await Page.GotoAsync("https://magento.x2y.dev/");

        await Page.GetByRole(AriaRole.Link, new() { NameString = "Sign In" }).ClickAsync();
      //  await Page.WaitForURLAsync("https://magento.x2y.dev/customer/account/login/referer/aHR0cHM6Ly9tYWdlbnRvLngyeS5kZXYv/");

        await Page.GetByRole(AriaRole.Textbox, new() { NameString = "Email*" }).ClickAsync();

        await Page.GetByRole(AriaRole.Textbox, new() { NameString = "Email*" }).FillAsync("roni_cost@example.com");

        await Page.GetByRole(AriaRole.Textbox, new() { NameString = "Password*" }).ClickAsync();

        await Page.GetByRole(AriaRole.Textbox, new() { NameString = "Password*" }).FillAsync("roni_cost3@example.com");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Sign In" }).ClickAsync();
        await Page.WaitForURLAsync("https://magento.x2y.dev/");

        await Page.Locator("#maincontent").GetByText("Radiant Tee").ClickAsync();
        await Page.WaitForURLAsync("https://magento.x2y.dev/radiant-tee.html");

        await Page.GetByRole(AriaRole.Option, new() { NameString = "XS" }).ClickAsync();

        await Page.GetByRole(AriaRole.Option, new() { NameString = "Blue" }).ClickAsync();

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Add to Cart" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { NameString = "shopping cart" }).ClickAsync();
        await Page.WaitForURLAsync("https://magento.x2y.dev/checkout/cart/");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Proceed to Checkout" }).ClickAsync();
        await Page.WaitForURLAsync("https://magento.x2y.dev/checkout/#shipping");

        await Page.GetByRole(AriaRole.Radio, new() { NameString = "Table Rate Best Way" }).CheckAsync();

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Next" }).ClickAsync();
        await Page.WaitForURLAsync("https://magento.x2y.dev/checkout/#payment");

        await Page.GetByLabel("Pay with Card (Redirect)").CheckAsync();

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Continue to payment" }).ClickAsync();

        await Page.WaitForURLAsync("https://testsecure.peachpayments.com/checkout");

        await Page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").FrameLocator("iframe[name=\"card\\.number\"]").GetByPlaceholder("Card Number").ClickAsync();

        await Page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").FrameLocator("iframe[name=\"card\\.number\"]").GetByPlaceholder("Card Number").FillAsync("5105105105105100");

        await Page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").GetByRole(AriaRole.Textbox, new() { NameString = "Expiry Date" }).ClickAsync();

        await Page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").GetByRole(AriaRole.Textbox, new() { NameString = "Expiry Date" }).FillAsync("12 / 28");

        await Page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").GetByPlaceholder("Card Holder").ClickAsync();

        await Page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").GetByPlaceholder("Card Holder").FillAsync("Test");

        await Page.Mouse.WheelAsync(0, 100);

        await Page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").FrameLocator("iframe[name=\"card\\.cvv\"]").GetByPlaceholder("CVV").ClickAsync();

        await Page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").FrameLocator("iframe[name=\"card\\.cvv\"]").GetByPlaceholder("CVV").FillAsync("123");

        await Page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").FrameLocator("iframe[name=\"card\\.number\"]").GetByPlaceholder("Card Number").ClickAsync();

        await Page.FrameLocator("internal:attr=[data-testid=\"iframe\"]").GetByRole(AriaRole.Button, new() { NameString = "Pay now" }).ClickAsync();

        await Page.WaitForURLAsync("https://magento.x2y.dev/checkout/onepage/success/");

        await Expect(Page).ToHaveURLAsync("https://magento.x2y.dev/checkout/onepage/success/");

        await Page.GetByText("Thank you for your purchase!").WaitForAsync();

        var orderNumber = await Page.TextContentAsync(".order-number strong");

        await Page.GotoAsync("https://magento.x2y.dev/admin");

        await Page.GetByPlaceholder("user name").ClickAsync();

        await Page.GetByPlaceholder("user name").FillAsync("magento");

        await Page.GetByPlaceholder("password").ClickAsync();

        await Page.GetByPlaceholder("password").FillAsync("Password1");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" }).ClickAsync();

        await Page.WaitForURLAsync("https://magento.x2y.dev/admin/admin/dashboard/");

        await Page.GetByRole(AriaRole.Link, new() { NameString = " Sales" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { NameString = "Orders" }).ClickAsync();

        await Page.WaitForLoadStateAsync();

        await Page.WaitForSelectorAsync("tbody tr:nth-child(1) a");

        await Page.Locator("tbody tr:nth-child(1) a").First.ClickAsync();

        var orderTitle = await Page.TextContentAsync(".admin__page-section-item-title span");

        orderTitle.Should().Contain(orderNumber);

        var status = await Page.TextContentAsync("#order_status");

        Console.WriteLine(status);

        status.Should().BeEquivalentTo("Processing");

    }
}




