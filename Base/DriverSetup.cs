using NUnitAutomationFramework.Utility;
using NUnitAutomationFramework.WebElements;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace NUnitAutomationFramework.Base
{
    /// <summary>
    /// The `DriverSetup` class provides methods for setting up different web browsers for Selenium testing based on configuration settings.
    /// </summary>
    public class DriverSetup
    {
        public enum BrowserType
        {
            Chrome,
            Firefox,
            IE,
            Edge
        }
        private static string? Browser = ConfigHelper.GetAppSetting("Browser");

        /// <summary>
        /// The function LocalBrowserSetup sets up a local browser for web automation testing.
        /// </summary>
        /// <param name="driver">The parameter `driver` represents a web driver interface in Selenium. It is used for controlling web browsers during automated testing.</param>
        /// <returns>Returns an instance of IWebDriver with the specified browser setup.</returns>
        public static IWebDriver LocalBrowserSetup(IWebDriver driver)
        {
            switch (Browser)
            {
                case nameof(BrowserType.Chrome):
                    driver = SetChromeBrowser();
                    break;
                case nameof(BrowserType.Firefox):
                    driver = SetFirefoxBrowser();
                    break;
                case nameof(BrowserType.IE):
                    driver = SetIEBrowser();
                    break;
                case nameof(BrowserType.Edge):
                    driver = SetEdgeBrowser();
                    break;
                default:
                    throw new ActionExpection(Browser + " Browser not supported, Please check the Browser Name");
            }
            // Timeout to Fail if an element is not found using FindElement method
            AddImplicitWait(driver, timeout: 30);

            return driver;
        }

        /// <summary>
        /// The AddImplicitWait function sets the implicit wait timeout for a WebDriver instance in C#.
        /// </summary>
        /// <param name="driver">The `driver` parameter represents the interface used to interact with a web browser in Selenium.</param>
        /// <param name="timeout">The `timeout` parameter represents the amount of time, in seconds, that the WebDriver will wait for an element to be found before throwing a `NoSuchElementException`.</param>
        public static void AddImplicitWait(IWebDriver driver, double timeout = 30)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeout);
        }

        /// <summary>
        /// The function SetChromeBrowser() returns a Chrome WebDriver instance with specific desired capabilities set.
        /// </summary>
        /// <returns>Returns an instance of the ChromeDriver with Chrome-specific desired capabilities set to start the browser maximized.</returns>
        public static IWebDriver SetChromeBrowser()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized"); // Add Chrome-specific desired capabilities here
            // chromeOptions.AddArgument("--start-maximized");
            // chromeOptions.AddArgument("--start-maximized");
            // chromeOptions.AddArgument("--start-maximized");
            // chromeOptions.AddArgument("--start-maximized");
            // chromeOptions.AddArgument("--start-maximized");
            new DriverManager().SetUpDriver(new ChromeConfig());
            return new ChromeDriver(chromeOptions);
        }

        /// <summary>
        /// The function SetFirefoxBrowser() sets up and returns a Firefox WebDriver with specific desired capabilities.
        /// </summary>
        /// <returns>Returns an instance of the FirefoxDriver with the specified FirefoxOptions (including the "--start-maximized" argument).</returns>
        public static IWebDriver SetFirefoxBrowser()
        {
            var firefoxOptions = new FirefoxOptions();
            firefoxOptions.AddArgument("--start-maximized"); // Add Firefox-specific desired capabilities here
            new DriverManager().SetUpDriver(new FirefoxConfig());
            return new FirefoxDriver(firefoxOptions);
        }

        /// <summary>
        /// The function SetIEBrowser() sets up and returns an Internet Explorer WebDriver with specific desired capabilities.
        /// </summary>
        /// <returns>Returns an instance of the InternetExplorerDriver with IE-specific desired capabilities.</returns>
        public static IWebDriver SetIEBrowser()
        {
            var ieOptions = new InternetExplorerOptions();
            ieOptions.IgnoreZoomLevel = true;
            new DriverManager().SetUpDriver(new InternetExplorerConfig());
            return new InternetExplorerDriver(ieOptions);
        }

        /// <summary>
        /// The function SetEdgeBrowser() sets up and returns an Edge WebDriver with specific desired capabilities.
        /// </summary>
        /// <returns>Returns an instance of the EdgeDriver with Edge-specific desired capabilities.</returns>
        public static IWebDriver SetEdgeBrowser()
        {
            var edgeOptions = new EdgeOptions();
            new DriverManager().SetUpDriver(new EdgeConfig());
            return new EdgeDriver(edgeOptions);
        }
    }
}
