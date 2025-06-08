using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V102.IndexedDB;

namespace NUnitAutomationFramework.Utility
{
    /// <summary>
    /// The `ConfigHelper` class provides utility methods for configuration settings and capturing screenshots.
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// The CaptureScreenShot method captures a screenshot of the current browser window and saves it to a specified location.
        /// </summary>
        /// <param name="driver">The WebDriver instance used to take the screenshot.</param>
        /// <param name="screenShotName">The name of the screenshot file.</param>
        /// <returns>Returns the path of the saved screenshot file.</returns>
        // public static string CaptureScreenShot(IWebDriver driver, string screenShotName)
        // {
        //     ITakesScreenshot scr = (ITakesScreenshot)driver;
        //     Screenshot screenshot = scr.GetScreenshot();
        //     // string dir = Environment.CurrentDirectory;
        //     // string projdir = Directory.GetParent(dir)?.Parent?.Parent?.FullName!;
        //     string screenshotPath = Path.Combine(GetAppSetting("ReportDir"), "Screenshots", screenShotName);
        //     Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath)!);
        //     screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
        //     Console.WriteLine("Screenshot saved at: " + screenshotPath);
        //     return screenshotPath;
        // }
        public static string CaptureScreenShot(IWebDriver driver, string screenShotName)
        {
            try
            {
                ITakesScreenshot scr = (ITakesScreenshot)driver;
                Screenshot screenshot = scr.GetScreenshot();

                string baseDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName
                    ?? throw new DirectoryNotFoundException("Could not determine project root directory");

                string screenshotPath = Path.Combine(baseDirectory, GetAppSetting("ReportDir"), "Screenshots", screenShotName);
                string screenshotDirectory = Path.GetDirectoryName(screenshotPath)
                    ?? throw new DirectoryNotFoundException("Could not determine screenshot directory");

                Directory.CreateDirectory(screenshotDirectory);
                screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
                Console.WriteLine($"Screenshot saved at: {screenshotPath}");

                return screenshotPath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to capture screenshot: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// The GetAppSetting method retrieves the value of a specified key from the application configuration settings.
        /// </summary>
        /// <param name="key">The key of the configuration setting.</param>
        /// <returns>Returns the value of the specified configuration setting.</returns>
        public static string GetAppSetting(string key)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key) == false)
            {
                throw new Exception(key + " not found in App.config");
            }
            else
            {
                return ConfigurationManager.AppSettings[key]!;
            }
        }
    }
}