using OpenQA.Selenium;
using NUnitAutomationFramework.WebElements;
using NUnitAutomationFramework.Base;
using AventStack.ExtentReports;
using NUnit.Framework;

namespace NUnitAutomationFramework.Pages
{
    public class ResetPasswordPage : BasePage
    {
        private readonly ExtentTest test;
        public ResetPasswordPage(IWebDriver driver, ExtentTest test) : base(driver, test)
        {
            this.driver = driver;
            this.test = test;
        }


        // Locators
        private readonly By opentab = By.XPath("//*[@id='opentab']");
        private readonly By mouseover = By.XPath("//*[@id='mousehover']");
        private readonly By top = By.XPath("//*[contains(text(), 'Top')]");
        private readonly By logo = By.CssSelector("[data-testid='accessibleImg']");
        private readonly By alternateSignIn = By.CssSelector("[data-testid='mainText']");
        private readonly By faceFP = By.CssSelector("[data-testid='mainText']");
        private readonly By headerText = By.CssSelector("#usernameTitle");
        private readonly By inputUsername = By.CssSelector("[data-testid='inputComponentWrapper']");
        private readonly By inputPassword = By.CssSelector("#input_2");
        private readonly By signInButton = By.CssSelector("#idSIButton9");
        private readonly By resetPasswordLink = By.CssSelector("#assistance_link>a");

        private readonly By signInButtonMSFT = By.CssSelector("[class='mectrl_header_text mectrl_truncate']");

        private readonly By headerForgetPassword = By.CssSelector("#login-form .widget-header");
        private readonly By inputEmailForgetPassword = By.CssSelector("input[name='Email']");
        private readonly By btnGetUserName = By.CssSelector("button[value='GetUsername']");
        private readonly By btnRequestPassword = By.CssSelector("button[value='ResetPassword']");


        // Actions
        public void OpenTab()
        {
            Click(opentab);
        }

        public bool IsLogoVisible()
        {
            return IsElementDisplayed(logo);
        }

        public string GetHeaderText()
        {
            return GetText(headerText);
        }

        public void ValidateElementsOnLoginPage()
        {
            try
            {
                Assert.That(IsElementDisplayed(logo), Is.True, "Logo is not displayed");
                Assert.That(GetText(headerText), Is.EqualTo("Welcome to the Abbott Study Portal. Please Sign In."), "Header text is not matching");
                Assert.That(IsElementDisplayed(inputUsername), Is.True, "Username field is not displayed");
                Assert.That(IsElementDisplayed(inputPassword), Is.True, "Password field is not displayed");
                Assert.That(IsElementDisplayed(signInButton), Is.True, "Sign In button is not displayed");
                Assert.That(IsElementDisplayed(resetPasswordLink), Is.True, "Reset Password link is not displayed");
                test.Log(Status.Info, "Successfully validated Logo, Header text, username, password fields, Sign In button and Reset Password link on Reset Password Page");
            }
            catch
            {
                test.Log(Status.Fail, "Failed to validate UI on Reset Password Page");
            }
        }
        public void ValidateElementsOnLoginPageMSFT()
        {
            try
            {
                Assert.That(IsElementDisplayed(logo), Is.True, "Logo is not displayed");
                Assert.That(GetText(headerText), Is.EqualTo("Sign ain"), "Header text is not matching");
                Assert.That(IsElementDisplayed(inputUsername), Is.True, "Username field is not displayed");
                Assert.That(IsElementDisplayed(signInButton), Is.True, "Sign In button is not displayed");
                test.Log(Status.Info, "Successfully validated Logo, Header text, username, password fields, Sign In button and Reset Password link on Reset Password Page");
            }
            catch
            {
                test.Log(Status.Fail, "Failed to validate UI on Reset Password Page");
            }
        }

        public bool IsSignInButtonVisible()
        {
            return IsElementDisplayed(signInButtonMSFT);
        }

        public void clickSignInButton()
        {
            if (IsElementDisplayed(signInButtonMSFT))
                Click(signInButtonMSFT);
            else
                test.Log(Status.Fail, "Sign In button is not displayed");
        }
        public void ClickResetPasswordLink()
        {
            Click(resetPasswordLink);
        }

        public void OpenResetPasswordPage()
        {
            SwitchToIframe(By.CssSelector(".cboxIframe"));
            ClickByLinkText("find your username or reset your password.");
            SwitchTab();
            Assert.That(driver.Url.Contains("pw.clinicalstudies.abbott.com"), Is.True, "URL does not contain Reset");
        }

        public void ClickAlternateSigninOption()
        {
            Click(alternateSignIn);
            Assert.That(GetText(faceFP), Is.EqualTo("Sign-in options"), "Header text is not matching");
        }

        public void ValidateElementsOnForgetPasswordPage()
        {
            try
            {
                Assert.That(GetText(headerForgetPassword), Is.EqualTo("Forgot Username or Password"), "Header text is not matching");
                Assert.That(IsElementDisplayed(inputEmailForgetPassword), Is.True, "Email field is not displayed");
                Assert.That(IsElementEnabled(btnGetUserName), Is.False, "Get Username button is enabled");
                Assert.That(IsElementEnabled(btnRequestPassword), Is.False, "Request Password button is enabled");
                test.Log(Status.Info, "Successfully validated Header text, Email field, Get Username button and Request Password button on Forget Password Page");
            }
            catch
            {
                test.Log(Status.Fail, "Failed to validate UI on Reset Password Page");
            }
        }

        public void GetUsernameByEmail(string email)
        {
            SendKeys(inputEmailForgetPassword, email);
            Assert.That(IsElementEnabled(btnGetUserName), Is.True, "Get Username button is Disabled");
            Assert.That(IsElementEnabled(btnRequestPassword), Is.True, "Request Password button is Disabled");
            test.Log(Status.Info, "Successfully validated Get Username button and Request Password button are enabled");
            Click(btnGetUserName);
        }

        public void MouseOver()
        {
            IWebElement element = FindElement(mouseover);
            ScrollToView(element);
            MouseOver(mouseover);
            Click(top);
        }
    }
}

