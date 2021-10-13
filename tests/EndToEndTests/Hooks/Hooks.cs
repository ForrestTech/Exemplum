namespace Exemplum.EndToEndTests.Hooks
{
    using AventStack.ExtentReports;
    using AventStack.ExtentReports.Reporter;
    using BoDi;
    using PlaywrightSharp;
    using PlaywrightSharp.Chromium;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using TechTalk.SpecFlow;

    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _container;
        private readonly ScenarioContext _scenarioContext;
        private static DirectoryInfo _resultsRoot;
        private static readonly ExtentReports Report = new();
        private ExtentTest _test;
        private ExtentTest _currentTest;


        public Hooks(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _container = container;
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void GlobalSetup()
        {
            _resultsRoot = Directory.CreateDirectory("Results");
            Report.AttachReporter(new ExtentHtmlReporter(_resultsRoot.FullName,
                AventStack.ExtentReports.Reporter.Configuration.ViewStyle.SPA));
        }

        [AfterTestRun]
        public static void GlobalTeardown()
        {
            Report.Flush();
        }

        [BeforeScenario]
        public async Task NavigateTo()
        {
            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new LaunchOptions { Headless = false });
            var browserContext = await browser.NewContextAsync(new BrowserContextOptions { BypassCSP = true });
            var page = await browserContext.NewPageAsync();
            await page.GoToAsync(Url.Root);
            await page.SetViewportSizeAsync(2560, 1284);

            _container.RegisterInstanceAs(playwright);
            _container.RegisterInstanceAs(browser);
            _container.RegisterInstanceAs(browserContext);
            _container.RegisterInstanceAs(page);

            var currentTest = _scenarioContext.ScenarioInfo.Title;
            _test = Report.CreateTest(currentTest);
            _currentTest = _test.CreateNode(currentTest);
        }


        [AfterScenario]
        public async Task AfterScenario()
        {
            var page = _container.Resolve<IPage>();
            await page.CloseAsync();
            var context = _container.Resolve<IChromiumBrowserContext>();
            await context.DisposeAsync();
            var browser = _container.Resolve<IChromiumBrowser>();
            await browser.DisposeAsync();
            var playwright = _container.Resolve<IPlaywright>();
            playwright.Dispose();

            if (_scenarioContext.TestError == null)
            {
                return;
            }

            byte[] screenshot = await TakeScreenShot();

            ReportTest(Convert.ToBase64String(screenshot));
        }

        private async Task<byte[]> TakeScreenShot()
        {
            var dir = _resultsRoot.CreateSubdirectory("Screenshots");
            var screenshot = await _container.Resolve<IPage>()
                .ScreenshotAsync(Path.Join(dir.FullName, $"{_scenarioContext.ScenarioInfo.Title}.jpg"));
            return screenshot;
        }

        private void ReportTest(string screenshot)
        {
            if (_scenarioContext.TestError is not null)
            {
                _currentTest
                    .Fail(
                        $"{_scenarioContext.TestError.Message}. Stack trace:{Environment.NewLine}{_scenarioContext.TestError.StackTrace}")
                    .AddScreenCaptureFromBase64String(screenshot);

                return;
            }

            _currentTest.Pass("");
        }
    }
}