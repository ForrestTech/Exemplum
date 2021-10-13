namespace Exemplum.EndToEndTests.Steps
{
    using Pages;
    using System.Threading.Tasks;
    using TechTalk.SpecFlow;

    [Binding]
    public class LoginSteps
    {
        public LoginPage LoginPage { get; }

        public LoginSteps(LoginPage loginPage)
        {
            LoginPage = loginPage;
        }

        [Given(@"a logged out user")]
        public async Task GivenALoggedOutUser()
        {
            await LoginPage.NavigateToLogin();
        }

        [Given(@"a logged in user")]
        public async Task GivenALoggedInUser()
        {
            await LoginPage.Login();
        }

        [When(@"the user attempts to log in with valid credentials")]
        public async Task WhenTheUserAttemptsToLogInWithValidCredentials()
        {
            await LoginPage.SetEmailAddress();
            await LoginPage.SetPassword();
            await LoginPage.ClickContinue();
        }

        [Then(@"the user is logged in successfully")]
        public async Task ThenTheUserIsLoggedInSuccessfully()
        {
            await LoginPage.ValidateIsLoggedIn();
        }
    }
}