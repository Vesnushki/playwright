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

        public virtual ILocator SignInLink => _page.GetByRole(AriaRole.Link, new () { NameString = "Sign In" });
    

        public async void ClickSignIn()
        {
            await SignInLink.ClickAsync();
        }

                     
    }
}
