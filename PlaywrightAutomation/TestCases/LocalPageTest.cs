using NUnit.Framework;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using static PlaywrightAutomation.Base.Locators;

namespace PlaywrightAutomation.TestCases
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class LocalPageTest : PageTest
    {
        [Test, MaxTime(300000), TestCase(TestName = "Test Online Shopping"), Category("testsuite")]
        public async Task onlineShopping()
        {
            await using var Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 300
            });
            var Context = await Browser.NewContextAsync();

            var Page = await Context.NewPageAsync();

            await Page.GotoAsync("https://www.saucedemo.com/");

            await Page.Locator("[data-test=\"login-credentials\"]").ClickAsync();

            await Page.Locator("[data-test=\"username\"]").ClickAsync();

            await Page.Locator("[data-test=\"username\"]").FillAsync("standard_user");

            await Page.Locator("[data-test=\"password\"]").ClickAsync();

            await Page.Locator("[data-test=\"password\"]").FillAsync("secret_sauce");

            await Page.Locator("[data-test=\"login-button\"]").ClickAsync();

            await Page.Locator("[data-test=\"add-to-cart-test\\.allthethings\\(\\)-t-shirt-\\(red\\)\"]").ClickAsync();

            await Page.Locator("[data-test=\"shopping-cart-link\"]").ClickAsync();

            await Page.Locator("[data-test=\"checkout\"]").ClickAsync();

            await Page.Locator("[data-test=\"firstName\"]").ClickAsync();

            await Page.Locator("[data-test=\"firstName\"]").FillAsync("Fred");

            await Page.Locator("[data-test=\"firstName\"]").PressAsync("Tab");

            await Page.Locator("[data-test=\"lastName\"]").FillAsync("Friendly");

            await Page.Locator("[data-test=\"lastName\"]").PressAsync("Tab");

            await Page.Locator("[data-test=\"postalCode\"]").FillAsync("77002");

            await Page.Locator("[data-test=\"continue\"]").ClickAsync();

            await Page.Locator("[data-test=\"finish\"]").ClickAsync();

            await Page.Locator("[data-test=\"checkout-complete-container\"]").ClickAsync(new LocatorClickOptions
            {
                Button = MouseButton.Right,
            });

            // Assert
            bool successMsg = await Page.Locator("[data-test=\"complete-header\"]").IsEnabledAsync();
            Assert.IsTrue(successMsg);
        }

        [Test, MaxTime(300000), TestCase(TestName = "DuckDuckGo _ PageTest"), Category("testsuite")]
        public async Task DuckDuckGo()
        {
            await using var Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 300
            });
            var Context = await Browser.NewContextAsync();

            var Page = await Context.NewPageAsync();
            await Page.GotoAsync("https://duckduckgo.com/");
            // Search for Playwright
            await Page.Locator("id=searchbox_input").FillAsync("playwright");
            // Press Enter
            await Page.Locator("id=searchbox_input").PressAsync("Enter");
            //await page.WaitForURLAsync("https://duckduckgo.com/?va=j&t=hc&q=Playwright&ia=web");

            // Click Playwright 
            await Page.Locator("text=playwright").ClickAsync();
        }
    }
}