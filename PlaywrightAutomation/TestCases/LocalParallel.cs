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

namespace PlaywrightAutomation.TestCases
{
    [TestFixture]
    public class LocalParallel
    {
        [TestFixture("Chrome")]
        [TestFixture("Edge")]
        [Parallelizable(ParallelScope.Children)]
        internal class ParallelTest : PlaywrightTest
        {
            private IBrowser? browser;
            private IBrowserContext Context;
            private string? browsertype;

            public ParallelTest(string browserstype)
            {
                browsertype = browserstype;
            }

            [SetUp]
            public async Task Setup()
            {
                //Assert.AreEqual("Chrome", browsertype);
                browser = browsertype switch
                {
                    "Chrome" => await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                    {
                        Headless = false,
                        Timeout = 30000,
                        Channel = "chrome"
                    }),
                    "Edge" => await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                    {
                        Headless = false,
                        Timeout = 30000,
                        Channel = "msedge"
                    }),
                    _ => await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                    {
                        Headless = false,
                        Timeout = 30000,
                        Channel = "chrome"
                    }),
                };
            }

            [TearDown]
            public async Task Teardown()
            {
                if (browser != null)
                {
                    Console.WriteLine("brower is not null");
                    await browser.CloseAsync();
                }

                if (Context != null)
                {
                    Console.WriteLine("context is not null");
                    await Context.CloseAsync();
                }
            }

            [Test]
            [TestCase(TestName = "LocalParallel1")]
            [Category("PlaywrightPOCLibraryParallel")]
            public async Task Page1Test()
            {
                //Assert.Pass();
                Context = await browser.NewContextAsync();
                var fpage = await Context.NewPageAsync();

                await fpage.GotoAsync("https://www.google.com");
                //await fpage.ScreenshotAsync(new PageScreenshotOptions
                //{
                //    Path = "c:\\temp\\screenshot1.png"
                //});
                //await fpage.CloseAsync();
                //await fpage.PauseAsync();
            }

            [Test]
            [TestCase(TestName = "LocalParallel2")]
            [Category("PlaywrightPOCLibraryParallel")]
            public async Task Page2Test()
            {
                //Assert.Pass();
                Context = await browser.NewContextAsync();
                var spage = await Context.NewPageAsync();
                await spage.GotoAsync("https://playwright.dev/dotnet");
                //await Expect(spage).ToHaveURLAsync("**/dotnet");
                //await spage.ScreenshotAsync(new PageScreenshotOptions
                //{
                //    Path = "c:\\temp\\screenshot2.png"
                //});
                //await spage.CloseAsync();
                //await spage.PauseAsync();
            }

            [Test]
            [TestCase(TestName = "LocalParallel3")]
            [Category("PlaywrightPOCLibraryParallel")]
            public async Task Page3Test()
            {
                //Assert.Pass();
                Context = await browser.NewContextAsync();
                var spage = await Context.NewPageAsync();
                //await spage.GotoAsync(
                //    "https://docs.nunit.org/articles/nunit/technical-notes/usage/Framework-Parallel-Test-Execution.html");
                await spage.GotoAsync("https://playwright.dev");
                //await Expect(spage).ToHaveURLAsync("**/dotnet");
                //await spage.ScreenshotAsync(new PageScreenshotOptions
                //{
                //    Path = "c:\\temp\\screenshot3.png"
                //});
                //await spage.CloseAsync();
                //await spage.PauseAsync();
            }
        }
    }
}