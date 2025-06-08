using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using AventStack.ExtentReports;
using EC = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace NUnitAutomationFramework.WebElements
{
    /// <summary>
    /// The `BasePage` class provides various utility methods for interacting with web elements using Selenium WebDriver.
    /// </summary>
    public class BasePage
    {
        public IWebDriver driver;
        private readonly ExtentTest test;

        public BasePage(IWebDriver driver, ExtentTest test)
        {
            this.driver = driver;
            this.test = test;
        }

        /// <summary>
        /// The WaitForPageLoad method waits for the page to fully load by checking the document ready state.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the page to load, in seconds. Defaults to 5 seconds.</param>
        public void WaitForPageLoad(int timeout = 5)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            WebDriverWait wait = new(driver, new TimeSpan(0, 0, timeout));
            wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
        }

        /// <summary>
        /// The WaitForElementToDisplay method waits for a web element to be displayed on the page.
        /// </summary>
        /// <param name="by">The locator used to find the web element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be displayed, in seconds. Defaults to 5 seconds.</param>
        /// <returns>Returns the web element if found.</returns>
        public IWebElement WaitForElementToDisplay(By by, int timeout = 5)
        {
            DefaultWait<IWebDriver> fluentwait = new(driver)
            {
                Timeout = TimeSpan.FromSeconds(timeout),
                PollingInterval = TimeSpan.FromSeconds(3)
            };
            fluentwait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            fluentwait.Message = "Element not found";

            IWebElement element = driver.FindElement(by);
            return element;
        }

        /// <summary>
        /// The WaitForElementToBeClickable method waits for a web element to be clickable.
        /// </summary>
        /// <param name="by">The locator used to find the web element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be clickable, in seconds. Defaults to 5 seconds.</param>
        /// <returns>Returns the web element if found and clickable.</returns>
        public IWebElement WaitForElementToBeClickable(By by, int timeout = 5)
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(EC.ElementToBeClickable(by));
        }

        /// <summary>
        /// The FindElement method finds a web element on the page, with retries in case of exceptions.
        /// </summary>
        /// <param name="by">The locator used to find the web element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be displayed, in seconds. Defaults to 5 seconds.</param>
        /// <returns>Returns the web element if found.</returns>
        public IWebElement FindElement(By by, int timeout = 5)
        {
            IWebElement element = null;

            try
            {
                element = WaitForElementToDisplay(by, timeout);
            }
            catch (StaleElementReferenceException e)
            {
                try
                {
                    Console.WriteLine("Stale element exception occurred, re-trying to find element");
                    WaitForPageLoad(timeout);
                    element = WaitForElementToDisplay(by, timeout);
                }
                catch (Exception e1)
                {
                    throw new ActionExpection("Exception during Find Element operation .." + e1.Message);
                }
            }
            catch (NoSuchElementException ex)
            {
                throw new ActionExpection("No Such element Exception during FindElement operation .." + ex.Message);
            }
            catch (Exception e)
            {
                throw new ActionExpection("Exception during FindElement operation .." + e.Message);
            }
            return element;
        }

        /// <summary>
        /// The ScrollToView method scrolls the page to bring the specified web element into view.
        /// </summary>
        /// <param name="element">The web element to scroll into view.</param>
        public void ScrollToView(IWebElement element)
        {
            /*
             * If this method is not working for you, use following code
             * ((JavascriptExecutor) driver).executeScript("arguments[0].scrollIntoView(true);", element);
             */
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoViewIfNeeded()", element);
        }

        /// <summary>
        /// The Click method clicks on a web element, with retries in case of exceptions.
        /// </summary>
        /// <param name="by">The locator used to find the web element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be displayed, in seconds. Defaults to 5 seconds.</param>
        public void Click(By by, int timeout = 5)
        {
            try
            {
                IWebElement element = WaitForElementToDisplay(by, timeout);
                if (element != null)
                {
                    ScrollToView(element);
                    string text = GetText(by);
                    element.Click();
                    test.Log(Status.Info, $"Clicked on '{text}'");
                }
            }
            catch (StaleElementReferenceException e)
            {
                try
                {
                    WaitForPageLoad(timeout = 15);
                    Console.WriteLine("Stale element exception occurred, re-trying to perform Click action");
                    IWebElement element = WaitForElementToDisplay(by, timeout);
                    ScrollToView(element);
                    string text = GetText(by);
                    element.Click();
                    test.Log(Status.Info, $"Clicked on '{text}'");
                }
                catch (Exception e1)
                {
                    throw new ActionExpection("Exception during click operation .." + e1.Message);
                }
            }
            catch (Exception e)
            {
                throw new ActionExpection("Exception during click operation .." + e.Message);
            }
        }

        /// <summary>
        /// The SelectDropDownByValue method selects a value from a dropdown element.
        /// </summary>
        /// <param name="by">The locator used to find the dropdown element.</param>
        /// <param name="value">The value to select from the dropdown.</param>
        /// <param name="timeout">The maximum time to wait for the element to be displayed, in seconds.</param>
        public void SelectDropDownByValue(By by, string value, int timeout)
        {
            try
            {
                SelectElement select = new(FindElement(by, timeout));
                select.SelectByValue(value);
                test.Log(Status.Info, $"Selected dropdown on '{value}'");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to select '{value}' from dropdown" + e.Message);
            }
        }

        /// <summary>
        /// The MouseOver method performs a mouse over action on a web element.
        /// </summary>
        /// <param name="by">The locator used to find the web element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be displayed, in seconds. Defaults to 5 seconds.</param>
        public void MouseOver(By by, int timeout = 5)
        {
            Actions action = new(driver);
            action.MoveToElement(FindElement(by)).Perform();
            test.Log(Status.Info, "Successfully mouseover on Mouseover button");
        }

        /// <summary>
        /// The NavigateToUrl method navigates the browser to the specified URL.
        /// </summary>
        /// <param name="URL">The URL to navigate to.</param>
        public void NavigateToUrl(string URL)
        {
            driver.Navigate().GoToUrl(URL);
        }

        /// <summary>
        /// The SendKeys method sends the specified value to a web element.
        /// </summary>
        /// <param name="by">The locator used to find the web element.</param>
        /// <param name="value">The value to send to the web element.</param>
        public void SendKeys(By by, string value)
        {
            IWebElement element = WaitForElementToDisplay(by);
            if (element != null)
            {
                element.Clear();
                element.SendKeys(value);
                test.Log(Status.Info, $"Typed '{value}' into text field");
            }
        }

        /// <summary>
        /// The GetText method retrieves the text content of a web element.
        /// </summary>
        /// <param name="by">The locator used to find the web element.</param>
        /// <returns>Returns the text content of the web element.</returns>
        public string GetText(By by)
        {
            IWebElement element = WaitForElementToDisplay(by);
            if (element != null)
            {
                return element.Text.Trim();
            }
            return null;
        }

        /// <summary>
        /// The ClickByLinkText method clicks on a link element identified by its link text.
        /// </summary>
        /// <param name="linkText">The text of the link to click.</param>
        public void ClickByLinkText(string linkText)
        {
            IWebElement element = WaitForElementToDisplay(By.LinkText(linkText));
            if (element != null)
            {
                element.Click();
                test.Log(Status.Info, $"Clicked on '{linkText}' Link");
            }
        }

        /// <summary>
        /// The SwitchToIframe method switches the browser context to the specified iframe.
        /// </summary>
        /// <param name="by">The locator used to find the iframe element.</param>
        public void SwitchToIframe(By by)
        {
            IWebElement element = WaitForElementToDisplay(by);
            driver.SwitchTo().Frame(element);
            test.Log(Status.Info, $"Switched to iframe");
        }

        /// <summary>
        /// The SwitchToDefaultContent method switches the browser context back to the default content.
        /// </summary>
        public void SwitchToDefaultContent()
        {
            driver.SwitchTo().DefaultContent();
            test.Log(Status.Info, $"Switched to default content");
        }

        /// <summary>
        /// The SwitchTab method switches the browser context to the next tab.
        /// </summary>
        /// <param name="TabNumber">The tab number to switch to. Defaults to 1.</param>
        public void SwitchTab(int TabNumber = 1)
        {
            driver.SwitchTo().Window(driver.WindowHandles[TabNumber]);
            test.Log(Status.Info, $"Switched to tab {TabNumber}");
        }

        /// <summary>
        /// The SelectDropDownByText method selects an option from a dropdown element by visible text.
        /// </summary>
        /// <param name="by">The locator used to find the dropdown element.</param>
        /// <param name="text">The visible text of the option to select.</param>
        /// <param name="timeout">The maximum time to wait for the element to be displayed, in seconds. Defaults to 5 seconds.</param>
        public void SelectDropDownByText(By by, string text, int timeout = 5)
        {
            try
            {
                SelectElement select = new(FindElement(by, timeout));
                select.SelectByText(text);
                test.Log(Status.Info, $"Selected dropdown '{text}'");
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to select text from dropdown: " + e.Message);
            }
        }

        /// <summary>
        /// The SelectDropDownByIndex method selects an option from a dropdown element by index.
        /// </summary>
        /// <param name="by">The locator used to find the dropdown element.</param>
        /// <param name="index">The index of the option to select.</param>
        /// <param name="timeout">The maximum time to wait for the element to be displayed, in seconds. Defaults to 5 seconds.</param>
        public void SelectDropDownByIndex(By by, int index, int timeout = 5)
        {
            try
            {
                SelectElement select = new(FindElement(by, timeout));
                select.SelectByIndex(index);
                test.Log(Status.Info, $"Selected dropdown at index '{index}'");
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to select index from dropdown: " + e.Message);
            }
        }

        /// <summary>
        /// The IsElementDisplayed method checks if a web element is displayed on the page.
        /// </summary>
        /// <param name="by">The locator used to find the web element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be displayed, in seconds. Defaults to 5 seconds.</param>
        /// <returns>Returns true if the element is displayed, false otherwise.</returns>
        public bool IsElementDisplayed(By by, int timeout = 5)
        {
            try
            {
                return FindElement(by, timeout).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// The IsElementEnabled method checks if a web element is enabled on the page.
        /// </summary>
        /// <param name="by">The locator used to find the web element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds. Defaults to 5 seconds.</param>
        /// <returns>Returns true if the element is enabled, false otherwise.</returns>
        public bool IsElementEnabled(By by, int timeout = 5)
        {
            try
            {
                return FindElement(by, timeout).Enabled;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// The GetElementAttribute method retrieves the value of a specified attribute of a web element.
        /// </summary>
        /// <param name="by">The locator used to find the web element.</param>
        /// <param name="attribute">The attribute whose value needs to be retrieved.</param>
        /// <param name="timeout">The maximum time to wait for the element to be displayed, in seconds. Defaults to 5 seconds.</param>
        /// <returns>Returns the value of the specified attribute.</returns>
        public string GetElementAttribute(By by, string attribute, int timeout = 5)
        {
            IWebElement element = FindElement(by, timeout);
            return element.GetAttribute(attribute);
        }

        /// <summary>
        /// The GetElementCssValue method retrieves the value of a specified CSS property of a web element.
        /// </summary>
        /// <param name="by">The locator used to find the web element.</param>
        /// <param name="property">The CSS property whose value needs to be retrieved.</param>
        /// <param name="timeout">The maximum time to wait for the element to be displayed, in seconds. Defaults to 5 seconds.</param>
        /// <returns>Returns the value of the specified CSS property.</returns>
        public string GetElementCssValue(By by, string property, int timeout = 5)
        {
            IWebElement element = FindElement(by, timeout);
            return element.GetCssValue(property);
        }

        /// <summary>
        /// The DoubleClick method performs a double-click action on a web element.
        /// </summary>
        /// <param name="by">The locator used to find the web element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be displayed, in seconds. Defaults to 5 seconds.</param>
        public void DoubleClick(By by, int timeout = 5)
        {
            Actions action = new(driver);
            IWebElement element = FindElement(by, timeout);
            string text = GetText(by);
            action.DoubleClick(element).Perform();
            test.Log(Status.Info, $"Double clicked on '{text}'");
        }

        /// <summary>
        /// The RightClick method performs a right-click action on a web element.
        /// </summary>
        /// <param name="by">The locator used to find the web element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be displayed, in seconds. Defaults to 5 seconds.</param>
        public void RightClick(By by, int timeout = 5)
        {
            Actions action = new(driver);
            IWebElement element = FindElement(by, timeout);
            string text = GetText(by);
            action.ContextClick(element).Perform();
            test.Log(Status.Info, $"Right clicked on '{text}'");
        }

        /// <summary>
        /// The DragAndDrop method performs a drag-and-drop action from one web element to another.
        /// </summary>
        /// <param name="source">The locator used to find the source web element.</param>
        /// <param name="target">The locator used to find the target web element.</param>
        /// <param name="timeout">The maximum time to wait for the elements to be displayed, in seconds. Defaults to 5 seconds.</param>
        public void DragAndDrop(By source, By target, int timeout = 5)
        {
            Actions action = new(driver);
            IWebElement sourceElement = FindElement(source, timeout);
            IWebElement targetElement = FindElement(target, timeout);
            string sourceElementText = sourceElement.Text.Trim();
            string targetElementText = targetElement.Text.Trim();
            action.DragAndDrop(sourceElement, targetElement).Perform();
            test.Log(Status.Info, $"Dragged and dropped '{sourceElementText}' to '{targetElementText}'");
        }

        /// <summary>
        /// The Sleep method pauses the execution for the specified number of seconds.
        /// </summary>
        /// <param name="seconds">The number of seconds to pause the execution.</param>
        public static void Sleep(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }

        /// <summary>
        /// The ClickByJavaScript method clicks on a web element using JavaScript.
        /// </summary>
        /// <param name="by">The locator used to find the web element.</param>
        public void ClickByJavaScript(By by)
        {
            IWebElement element = FindElement(by);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", element);
            test.Log(Status.Info, $"Clicked on element using JavaScript");
        }

        /// <summary>
        /// The AcceptAlert method accepts the currently displayed alert.
        /// </summary>
        public void AcceptAlert()
        {
            try
            {
                WebDriverWait wait = new(driver, TimeSpan.FromSeconds(30));
                wait.Until(EC.AlertIsPresent());
                IAlert alert = driver.SwitchTo().Alert();
                alert.Accept();
                test.Log(Status.Info, "Accepted the alert");
            }
            catch (NoAlertPresentException)
            {
                test.Log(Status.Warning, "No alert present to accept");
            }
        }

        /// <summary>
        /// The DismissAlert method dismisses the currently displayed alert.
        /// </summary>
        public void DismissAlert()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                alert.Dismiss();
                test.Log(Status.Info, "Dismissed the alert");
            }
            catch (NoAlertPresentException)
            {
                test.Log(Status.Warning, "No alert present to dismiss");
            }
        }

        /// <summary>
        /// The GetAlertText method retrieves the text from the currently displayed alert.
        /// </summary>
        /// <returns>Returns the text of the alert.</returns>
        public string GetAlertText()
        {
            try
            {
                WebDriverWait wait = new(driver, TimeSpan.FromSeconds(30));
                wait.Until(EC.AlertIsPresent());
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                test.Log(Status.Info, $"Alert text: {alertText}");
                return alertText;
            }
            catch (NoAlertPresentException)
            {
                test.Log(Status.Warning, "No alert present to get text from");
                return null;
            }
        }

        /// <summary>
        /// The SendKeysToAlert method sends keys to the currently displayed alert.
        /// </summary>
        /// <param name="text">The text to send to the alert.</param>
        public void SendKeysToAlert(string text)
        {
            try
            {
                WebDriverWait wait = new(driver, TimeSpan.FromSeconds(30));
                wait.Until(EC.AlertIsPresent());
                IAlert alert = driver.SwitchTo().Alert();
                alert.SendKeys(text);
                test.Log(Status.Info, $"Sent keys to alert: {text}");
            }
            catch (NoAlertPresentException)
            {
                test.Log(Status.Warning, "No alert present to send keys to");
            }
        }
    }
}
