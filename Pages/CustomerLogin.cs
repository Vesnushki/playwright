using System.Threading.Tasks;
using Microsoft.Playwright;

namespace PlaywrightTests.Pages
{
    internal class CustomerLogin
    {
        private readonly IPage _page;

        public CustomerLogin(IPage page)
        {
            _page = page;
        }

        public virtual ILocator SignInLink => _page.GetByRole(AriaRole.Link, new() { NameString = "Sign In" });
        public virtual ILocator EmailField => _page.GetByRole(AriaRole.Textbox, new() { NameString = "Email*" });
        public virtual ILocator Password => _page.GetByRole(AriaRole.Textbox, new() { NameString = "Password*" });
        public virtual ILocator SignInButton => _page.GetByRole(AriaRole.Button, new() { NameString = "Sign In" });
       


        public async Task Click(ILocator locator)
        {
             await locator.ClickAsync();
        }
        public async Task FillField(ILocator locator, string fieldValue)
        {
            await locator.FillAsync(fieldValue);
        }

    }
}
