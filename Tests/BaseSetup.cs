using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PeachPayment.Pages;
using PeachPayment.Tests;
using PlaywrightTests.Pages;

namespace PlaywrightTests.Tests
{
    public class BaseSetup
    {
        protected IBrowser Browser;
        protected IBrowserContext Context;
        protected IPlaywright Playwright;
        protected IPage Page;
        protected CustomerLogin LoginPage;
        protected ProductPage ProductPage;
        protected ShoppingCart ShoppingCart;
        protected ShippingPage ShippingPage;
        protected Checkout Checkout;
        protected PeachForm PeachForm;
        protected SuccessOrderPage SuccessPage;
        protected PlaywrightTest Assertion;
        protected Admin Admin;
        protected Helper Helper;
        protected MyAccount MyAccount;
        protected MagentoHeader Header;


       [SetUp]
        public async Task Setup()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            { Headless = false, Channel = "chrome" });
            Context = await Browser.NewContextAsync();
            Page = await Context.NewPageAsync();
            Initialization();
            await Context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }
         public void Initialization()
        {
            LoginPage = new CustomerLogin(Page);
            ProductPage = new ProductPage(Page);
            ShoppingCart = new ShoppingCart(Page);
            ShippingPage = new ShippingPage(Page);
            Checkout = new Checkout(Page);
            PeachForm = new PeachForm(Page);
            SuccessPage = new SuccessOrderPage(Page);
            Assertion = new PlaywrightTest();
            Admin = new Admin(Page);
            Helper = new Helper();
            MyAccount = new MyAccount(Page);
            Header = new MagentoHeader(Page);
        }

        [TearDown]
        public async Task Teardown()
        {
            await Context.Tracing.StopAsync(new TracingStopOptions
            {
                Path = $"screenshots_{Guid.NewGuid()}.zip",

            });
            await Browser.CloseAsync();
        }
    }
}