using NUnit.Framework;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static PlaywrightAutomation.Base.Locators;
using PlaywrightAutomation.DataModel;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.VisualBasic;

namespace PlaywrightAutomation.TestCases
{
    [TestFixture]
    public class Local
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test, MaxTime(300000), TestCase(TestName = "DuckDuckGo"), Category("testsuite")]
        public async Task DuckDuckGo()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                Devtools = true,
                SlowMo = 2000
            });

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://duckduckgo.com/");
            // Search for Playwright
            await page.Locator("id=searchbox_input").FillAsync("playwright");
            // Press Enter
            await page.Locator("id=searchbox_input").PressAsync("Enter");
            //await page.WaitForURLAsync("https://duckduckgo.com/?va=j&t=hc&q=Playwright&ia=web");

            // Click Playwright 
            await page.Locator("text=playwright").ClickAsync();
            await page.WaitForURLAsync("https://www.Playwright.com/");

            // Assert
            string title = await page.TitleAsync();
            Assert.IsTrue(title.Contains("Playwright"));
        }

        [Test, MaxTime(300000), TestCase(TestName = "Google"), Category("testsuite")]
        public async Task Google()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                Devtools = true,
                SlowMo = 2000
            });

            //await using var browser = await playwright.Webkit.LaunchAsync(launchOptions);

            //var contextOptions = playwright.Devices["iPhone 11 Pro"]..ToBrowserContextOptions();
            //contextOptions.Locale = "en-US";
            //contextOptions.Geolocation = new Geolocation { Longitude = 12.492507m, Latitude = 41.889938m };
            //contextOptions.Permissions = new[] { ContextPermission.Geolocation };

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://google.com");

            // Click Search
            await page.Locator("[aria-label=\"Search\"]").ClickAsync();
            // Fill Search
            await page.Locator("[aria-label=\"Search\"]").FillAsync("playwright");
            // Press Enter
            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.Locator("[aria-label=\"Search\"]").PressAsync("Enter");
            } /*, new PageWaitForNavigationOptions
            {
                UrlString = "https://www.google.com/search?q=Playwright&source=hp&ei=W6ofY4uQH-ne0PEP0Z6q0A4&iflsig=AJiK0e8AAAAAYx-4a50qaHE0MUGREWOu6q3H0n6VpJjy&ved=0ahUKEwjLqcvpnpD6AhVpLzQIHVGPCuoQ4dUDCAk&uact=5&oq=Playwright&gs_lcp=Cgdnd3Mtd2l6EAMyEQguEIAEELEDEIMBEMcBENEDMgsIABCABBCxAxCDATILCAAQgAQQsQMQgwEyCwguEIAEEMcBEK8BMggIABCABBCxAzILCC4QgAQQxwEQrwEyBQgAEIAEMgUIABCABDILCAAQgAQQsQMQgwEyCAgAEIAEELEDOg4IABCPARDqAhCMAxDlAjoOCC4QjwEQ6gIQjAMQ5QI6CAgAELEDEIMBOggILhCxAxCDAToOCC4QgAQQsQMQxwEQ0QM6CwguEIAEEMcBENEDOgsILhCABBCxAxCDAToLCC4QgAQQsQMQ1AI6CAgAELEDEMkDOgUIABCSAzoHCAAQgAQQCjoNCC4QgAQQxwEQrwEQClCNG1iiKWC6LGgBcAB4AIABeIgBmQeSAQM1LjSYAQCgAQGwAQo&sclient=gws-wiz"
            }*/);

            // Assert
            string title = await page.TitleAsync();
            Assert.IsTrue(title.Contains("playwright"));
        }

        [Test, MaxTime(300000), TestCase(TestName = "Google Maps"), Category("testsuite")]
        public async Task GoogleMaps()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://maps.google.com/");
            // Click "Search Google Maps"
            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.Locator("[aria-label=\"Search Google Maps\"]").ClickAsync();
            } /*, new PageWaitForNavigationOptions
            {
                UrlString = "https://www.google.com/maps/@37.6,-95.665,4z"
            }*/);
            // Send text Houston
            await page.Locator("[aria-label=\"Search Google Maps\"]").FillAsync("houston, tx");
            // Press Enter
            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.Locator("[aria-label=\"Search Google Maps\"]").PressAsync("Enter");
            } /*, new PageWaitForNavigationOptions
            {
                UrlString = "https://www.google.com/maps/place/Houston,+TX/@29.8168824,-95.6814841,10z/data=!3m1!4b1!4m5!3m4!1s0x8640b8b4488d8501:0xca0d02def365053b!8m2!3d29.7604267!4d-95.3698028"
            }*/);

            // Assert
            bool header = await page.Locator("h1 >> text=Houston").IsEnabledAsync();
            Assert.IsTrue(header);
        }

    }
}