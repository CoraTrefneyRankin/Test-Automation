using NUnit.Framework;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using System;
using System.IO;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static NUnit.Framework.Constraints.Tolerance;
using NUnit.Framework.Internal;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics.Metrics;
using System.Windows.Controls;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;

namespace PlaywrightAutomation
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class BaseWebTestPlaywright : PageTest
    {
        // Reporting variables
        protected ExtentReports extent;
        public ExtentTest CurrentTest;
        // Get Project and ProjectEnvironment from Nunit console parameters
        public static string Project = TestContext.Parameters["Project"];
        public static string ProjectEnvironment = TestContext.Parameters["ProjectEnvironment"];
        /// <summary>
        /// Report Name passed from Jenkins
        /// </summary>
        /// <summary>
        /// Jenkins job Id, stripped from ReportName passed from Jenkins
        /// </summary>
        //public static string SourceJobId = new JenkinsApiHelper().GetJobId(ReportName);
        /// <summary>
        /// RecipientEmails email ids passed from Jenkins
        /// </summary>
        public static string RecipientEmails = TestContext.Parameters["RecipientEmails"];

        // Environment variables
        public static string ReportName = TestContext.Parameters["ReportName"];
        public static string SourceJobId = GetJobId(ReportName);
        public static string CurrentMachineName = Environment.MachineName;
        public string reportFilePath = TestContext.CurrentContext.TestDirectory + "\\";
        //string reportFileName = "PlaywrightAutomation_" + DateTime.Now.ToString("yyyy'-'MM'-'dd'-'HH'-'mm'-'ss") + ".html";

        private static Random random = new Random();
        public static string screenshotName;

        [OneTimeSetUp]
        public void SetupFixture()
        {
            // Reporting
            var reporter = new ExtentHtmlReporter(reportFilePath);
            extent = new ExtentReports();
            extent.AttachReporter(reporter);                        
        }

        [OneTimeTearDown]
        public void TeardownFixture()
        {
            // Flush the report
            extent.Flush();

            // Rename the report file
            if (ReportName is null)
            {
                ReportName = "TestCases_" + CurrentMachineName + "_" + DateTime.Now.ToString("yyyy'-'MM'-'dd'_'HH'-'mm'-'ss") + ".html";
            }
            System.IO.File.Move(reportFilePath + "index.html", reportFilePath + ReportName);
        }

        [SetUp]
        public void Setup()
        {
            // Reporting - create the test
            CurrentTest = extent.CreateTest(NUnit.Framework.TestContext.CurrentContext.Test.Name);  
        }

        public async Task Screenshot()
        {
            screenshotName = reportFilePath + NUnit.Framework.TestContext.CurrentContext.Test.Name + DateTime.Now.ToString("yyyy'-'MM'-'dd'-'HH'-'mm'-'ss") + ".png";
            await Page.ScreenshotAsync(new()
            {
                Path = screenshotName,
                FullPage = true,
            });
            CurrentTest.AddScreenCaptureFromPath(screenshotName);
        }

        // Get the path to our Data folder
        public static string getDataFolder()
        {
            try 
            {
                string dataFolder = AppDomain.CurrentDomain.BaseDirectory;
                if (dataFolder.Contains(@"bin\Debug\net6.0-windows"))
                {
                    dataFolder = dataFolder.Replace(@"bin\Debug\net6.0-windows", "");
                }

                if (dataFolder.Contains(@"packages\NUnit.ConsoleRunner.3.16.3\tools\agents"))
                {
                    dataFolder = dataFolder.Replace(@"packages\NUnit.ConsoleRunner.3.16.3\tools\agents", "");
                }

                if (dataFolder.Contains(@"\\"))
                {
                    dataFolder = dataFolder.Replace(@"\\", @"\");
                }

                if (dataFolder.Contains(@"automation\workspace"))
                {
                    dataFolder = dataFolder.Replace(@"PlaywrightAutomation\", @"PlaywrightAutomation\Automation\Data");
                }
                else
                {
                    dataFolder = dataFolder.Replace(@"PlaywrightAutomation", @"Automation\Data");
                }

                dataFolder = dataFolder.Replace("net6.0", "");

                return dataFolder;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static (string, string, string, string, string, string) ourDates()
        {
            DateTime today = DateTime.Now;
            DateTime tomorrow = today.AddDays(1);
            DateTime yesterday = today.AddDays(-1);
            DateTime dayBeforeYesterday = today.AddDays(-2);
            string todayString = today.ToString("MM'/'dd'/'yyyy");
            string tomorrowString = tomorrow.ToString("MM'/'dd'/'yyyy");
            string yesterdayString = yesterday.ToString("MM'/'dd'/'yyyy");
            string dayBeforeYesterdayString = dayBeforeYesterday.ToString("MM'-'dd'-'yyyy");
            string nowMmddyyyhhmmss = today.ToString("MM'/'dd'/'yyyy HH':'mm':'ss");
            string nowYyymmddmmss = today.ToString("yyyy'/'MM'/'dd HH':'mm':'ss");
            string ourDate = today.ToString("yyyy'-'MM'-'dd");
            string nowYyyymmddhhmmss = today.ToString("yyyyMMddHHmmss");
            string screenshotPath = "Screenshot" + nowYyymmddmmss + ".png";
            string ourRandomString = RandomString(12);
            Random rnd = new Random();
            int ourInteger = rnd.Next(100, 999);
            string ourTitle = " _ " + ourRandomString + ourInteger;
            return (ourDate, ourTitle, yesterdayString, todayString, tomorrowString, dayBeforeYesterdayString);
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReportName"></param>
        /// <returns>Returns valid integer(x) > 0 - for valid jenkins report name ;
        /// else
        /// '-100L(local)' - if running on local machine ;
        /// '-99' - Error 
        /// ''</returns>
        public static string GetJobId(string ReportName)
        {
            #region WebReferences
            //https://stackoverflow.com/questions/45472604/get-jenkins-job-build-id-from-queue-id
            //if you are admin then try below
            // https://riptutorial.com/jenkins/example/29046/how-to-get-infromation-about-jenkins-instance
            #endregion
            string BuildNo = "-99E"; // default value/Error
            try
            {
                if (string.IsNullOrEmpty(ReportName))
                    return "-100L"; //Run on LocalMachine and therefore 'L'

                var ReportNameArray = ReportName.Split('_');
                //var Env = ReportNameArray[ReportNameArray.Length - 1];
                BuildNo = ReportNameArray[ReportNameArray.Length - 2];
                return BuildNo;
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Invalid ReportName passed - '{ReportName}' --- <<{e.Message}>>");
            }
            return BuildNo;
        }
    }
}