namespace Exemplum.EndToEndTests.Pages
{
    using FluentAssertions;
    using Microsoft.Playwright;
    using System.Threading.Tasks;

    public class LoginPage : BasePage
    {
        public LoginPage(IPage page) : base(page)
        {
        }

        protected override string PagePath => "";

        public async Task Login()
        {
            await NavigateToLogin();
            await SetEmailAddress();
            await SetPassword();
            await ClickContinue();
            await ValidateIsLoggedIn();
        }

        public async Task NavigateToLogin()
        {
            await Page.GotoAsync(Path("authentication/login"));
        }

        public async Task SetEmailAddress()
        {
            await Page.FillAsync(Selector.Input("username"), "richard.a.forrest+st@gmail.com");
        }

        public async Task SetPassword()
        {
            await Page.FillAsync(Selector.Input("password"), "Password1?");
        }

        public async Task ClickContinue()
        {
            await Page.ClickAsync(Selector.Button("Continue"));
        }

        public async Task ValidateIsLoggedIn()
        {
            var logoutButton = await Page.WaitForSelectorAsync("#log-out",
                new PageWaitForSelectorOptions {State = WaitForSelectorState.Attached});
            logoutButton.Should().NotBeNull();
        }
    }
}