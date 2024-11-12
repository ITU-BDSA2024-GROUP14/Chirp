using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Playwright;
using NUnit.Framework;

namespace YourNamespace.Tests
{
    public class UITests : PageTest
    {
        private WebApplicationFactory<Program> _factory;
        private IPlaywright _playwright;
        private IBrowser _browser;
        private string _baseAddress;

        [OneTimeSetUp] // Runs once for the test class
        public async Task OneTimeSetUp()
        {
            // Initialize the WebApplicationFactory
            _factory = new WebApplicationFactory<Program>();

            // Start the test server and get the base URL
            var client = _factory.CreateClient();
            _baseAddress = client.BaseAddress.ToString(); // Accessible by Playwright

            // Initialize Playwright and launch the browser
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        }

        [OneTimeTearDown] // Runs once after all tests in the class
        public async Task OneTimeTearDown()
        {
            // Close Playwright resources
            await _browser.CloseAsync();
            _playwright.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task MyTest()
        {
            await Page.GotoAsync(_baseAddress);
            await Expect(Page.Locator("h1")).ToContainTextAsync("Chirp!");
        }
    }
}