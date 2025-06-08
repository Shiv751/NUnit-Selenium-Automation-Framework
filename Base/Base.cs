using AventStack.ExtentReports;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnitAutomationFramework.Utility;
using NUnitAutomationFramework.WebElements;
using OpenQA.Selenium;
using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace NUnitAutomationFramework.Base
{
    /// <summary>
    /// The `BaseSetup` class provides setup and teardown methods for Selenium tests using NUnit.
    /// It includes methods for initializing the ExtentReports, starting and quitting the browser,
    /// and capturing screenshots on test failure.
    /// </summary>
    [TestFixture]
    public class BaseSetup
    {
        public required ExtentReports extent;
        public required ExtentTest test;
        public ThreadLocal<IWebDriver> driver = new();
        public ThreadLocal<ExtentTest> extent_test = new();

        public static string ReportDir = ConfigHelper.GetAppSetting("ReportDir");

        /// <summary>
        /// The Setup method initializes the ExtentReports and sets up the report directory.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            try
            {
                string dir = Environment.CurrentDirectory;
                string projdir = Directory.GetParent(dir)?.Parent?.Parent?.FullName
                                 ?? throw new Exception("Failed to determine the project directory.");

                string reportPath = Path.Combine(projdir, ReportDir, "Report.html");
                var htmlReporter = new ExtentHtmlReporter(reportPath);
                AddInfoInReport(htmlReporter);
                if (ConfigHelper.GetAppSetting("DeleteOlderScreenshots") == "true")
                    DeleteScreenshotFolder();
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Error in Setup: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// The StartBrowser method initializes the test in ExtentReports and starts the browser.
        /// </summary>
        [SetUp]
        public void StartBrowser()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            extent_test.Value = test;
            SetBrowser();
        }

        /// <summary>
        /// The SetTestResults method captures the test results and takes a screenshot if the test fails.
        /// </summary>
        // [TearDown]
        // public void SetTestResults()
        // {
        //     var status = TestContext.CurrentContext.Result.Outcome.Status;
        //     if (status == TestStatus.Failed)
        //     {
        //         var stackTrace = TestContext.CurrentContext.Result.StackTrace;
        //         DateTime date = DateTime.Now;
        //         string filename = $"Screenshot_{date:yyyyMMdd_HHmmss}.png";
        //         string screenshotPath = ConfigHelper.CaptureScreenShot(driver.Value!, filename);
        //         extent_test.Value?.Fail("Status: Failed, Screenshot : ", MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
        //         extent_test.Value?.Log(Status.Fail, stackTrace);
        //     }
        //     else if (status == TestStatus.Passed)
        //     {
        //         extent_test.Value?.Log(Status.Pass, MarkupHelper.CreateLabel($"Test Case Status: {status}", ExtentColor.Green));
        //     }
        //     extent.Flush();
        //     driver.Value?.Quit();
        // }

        [TearDown]
        public void SetTestResults()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            if (status == TestStatus.Failed)
            {
                var stackTrace = TestContext.CurrentContext.Result.StackTrace;
                DateTime date = DateTime.Now;
                string filename = $"Screenshot_{date:yyyyMMdd_HHmmss}.png";

                // Get the absolute screenshot path
                string screenshotPath = ConfigHelper.CaptureScreenShot(driver.Value!, filename);

                // Convert to relative path for the report
                string relativeScreenshotPath = $"Screenshots/{filename}";

                extent_test.Value?.Fail("Status: Failed, Screenshot : ",
                    MediaEntityBuilder.CreateScreenCaptureFromPath(relativeScreenshotPath).Build());
                extent_test.Value?.Log(Status.Fail, stackTrace);
            }
            else if (status == TestStatus.Passed)
            {
                extent_test.Value?.Log(Status.Pass,
                    MarkupHelper.CreateLabel($"Test Case Status: {status}", ExtentColor.Green));
            }
            extent.Flush();
            driver.Value?.Quit();
        }

        /// <summary>
        /// The TearDown method disposes of the driver and extent_test instances.
        /// </summary>
        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Dispose();
            extent_test.Dispose();
        }

        /// <summary>
        /// The GetDriver method returns the current WebDriver instance.
        /// </summary>
        /// <returns>Returns the current WebDriver instance.</returns>
        public IWebDriver GetDriver()
        {
            return driver.Value!;
        }

        /// <summary>
        /// The SetBrowser method sets up the browser based on the configuration settings.
        /// </summary>
        private void SetBrowser()
        {
            string runEnvironment = ConfigHelper.GetAppSetting("RunEnvironment");
            if (runEnvironment != null && runEnvironment.Equals("Local", StringComparison.OrdinalIgnoreCase))
            {
                driver.Value = DriverSetup.LocalBrowserSetup(driver.Value!);
            }
            else
            {
                TestContext.Progress.WriteLine("Please check browser name and run environment value in app.config file");
            }
        }

        /// <summary>
        /// The AddInfoInReport method adds system information to the ExtentReports.
        /// </summary>
        /// <param name="htmlReporter">The ExtentHtmlReporter instance used to generate the report.</param>
        private void AddInfoInReport(ExtentHtmlReporter htmlReporter)
        {
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                extent.AddSystemInfo(key, ConfigurationManager.AppSettings[key]);
            }
        }

        private void DeleteScreenshotFolder()
        {
            string dir = Environment.CurrentDirectory;
            string projdir = Directory.GetParent(dir)?.Parent?.Parent?.FullName!;
            string screenshotPath = Path.Combine(projdir, ReportDir, "Screenshots");
            if (Directory.Exists(screenshotPath))
            {
                Directory.Delete(screenshotPath, true);
                Console.WriteLine("Screenshots deleted successfully.");
            }
        }
    }
}