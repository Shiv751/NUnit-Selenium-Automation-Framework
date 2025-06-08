using NUnit.Framework;
using NUnitAutomationFramework.Base;
using NUnitAutomationFramework.Pages;
using NUnitAutomationFramework.Utility;
using OpenQA.Selenium;

namespace NUnitAutomationFramework.TestSuites
{

    // have to make it dynamic 
    // true = Run in Parallel
    // false = Run Sequentially
    [Parallelizable(false ? ParallelScope.Children : ParallelScope.None)]
    public class ResetPasswordTest : BaseSetup
    {
        ResetPasswordPage? page;

        // This method runs before each test
        [SetUp]
        public void SetUp()
        {
            page = new(GetDriver(), extent_test.Value!);
            string CustomURL = "https://www.microsoft.com/en-in/"; // Custom URL as per need
            string AppURL = Parser.GetProperty(ConfigHelper.GetAppSetting("URL"));
            string FinalURL = string.IsNullOrEmpty(AppURL) ? CustomURL : AppURL;
            GetDriver().Navigate().GoToUrl(FinalURL);
            page.clickSignInButton();
        }

        [Test, Category("ResetPasswordTest")]
        public void TC_01_ValidateResetPassword_UI()
        {
            string user = TestData.List("Registered_User");
            extent_test?.Value?.Info("Testdata is : " + user);

            page?.ValidateElementsOnLoginPageMSFT();
        }

        // [Test, Category("ResetPasswordTest")]
        // public void TC_02_AlternateSignInOptions()
        // {
        //     // To get User from List file
        //     string user = TestData.List("Default_Users");
        //     extent_test?.Value?.Info("user is : " + user);

        //     // page.ClickResetPasswordLink();
        //     page.ClickAlternateSigninOption();
        //     extent_test?.Value?.Pass("TC_02_OpenFAQ Testcase is passed");
        // }

        // [Test, Category("ResetPasswordTest")]
        // public void TC_03_ResetPassword()
        // {
        //     // To get User from List file
        //     string user = TestData.List("Registered_User");
        //     extent_test?.Value?.Info("user is : " + user);

        //     page.ClickResetPasswordLink();
        //     page.OpenResetPasswordPage();
        //     page.ValidateElementsOnForgetPasswordPage();
        //     page.GetUsernameByEmail(TestData.GetTestData("resetPassword.email"));
        //     extent_test?.Value?.Pass("TC_03_ResetPassword Testcase is passed");
        // }
    }
}
