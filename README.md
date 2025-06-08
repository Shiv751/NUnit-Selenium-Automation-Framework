# NUnit Selenium Automation Framework

This project is a robust, extensible automation framework built with C#, Selenium WebDriver, and NUnit. It is designed for automated UI testing of web applications, supporting features like configuration management, reporting, and test data handling.

## Features

- **Selenium WebDriver Integration**: Automate browser actions for end-to-end testing.
- **NUnit Test Framework**: Structure and execute tests with NUnit.
- **Page Object Model**: Maintainable and reusable page classes (see [`Pages/`](Pages/)).
- **JSON-based Configuration & Test Data**: Manage settings and test data in JSON files ([`Properties.json`](Properties.json), [`Constants/Testdata.json`](Constants/Testdata.json)).
- **ExtentReports Integration**: Generate rich HTML reports for test runs.
- **Screenshot Capture**: Automatically capture screenshots on test failures.
- **Thread-safe Execution**: Supports parallel test execution.

## Project Structure

```
NUnitAutomationFramework/
├── App.config
├── NUnitAutomationFramework.csproj
├── Properties.json
├── Base/
│   ├── Base.cs
│   ├── BasePage.cs
│   └── DriverSetup.cs
├── Constants/
│   ├── BaseConstant.cs
│   └── Testdata.json
├── Pages/
│   └── ResetPasswordPage.cs
├── Reports/
│   └── index.html
├── TestSuites/
│   └── ResetPasswordTest.cs
├── Utility/
│   ├── ConfigHelper.cs
│   └── Parser.cs
└── ...
```

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- ChromeDriver/EdgeDriver/GeckoDriver (as required by your tests)
- Visual Studio or VS Code

### Setup

1. **Clone the repository**

   ```sh
   git clone <repository-url>
   cd NUnitAutomationFramework
   ```

2. **Restore NuGet packages**

   ```sh
   dotnet restore
   ```

3. **Configure settings**

   - Update [`Properties.json`](Properties.json) and [`App.config`](App.config) as needed for your environment.

4. **Build the project**
   ```sh
   dotnet build
   ```

### Running Tests

Run all tests using the .NET CLI:

```sh
dotnet test
```

Test results and ExtentReports will be generated in the `Reports/` directory.

## Key Components

- **Base Classes**: Common setup, teardown, and utility methods ([`Base/Base.cs`](Base/Base.cs), [`Base/BasePage.cs`](Base/BasePage.cs))
- **Page Objects**: Encapsulate UI interactions ([`Pages/ResetPasswordPage.cs`](Pages/ResetPasswordPage.cs))
- **Test Suites**: NUnit test classes ([`TestSuites/ResetPasswordTest.cs`](TestSuites/ResetPasswordTest.cs))
- **Utilities**: Helpers for config, parsing, and reporting ([`Utility/ConfigHelper.cs`](Utility/ConfigHelper.cs), [`Utility/Parser.cs`](Utility/Parser.cs))

## Customization

- Add new page objects in the [`Pages/`](Pages/) directory.
- Add new test cases in the [`TestSuites/`](TestSuites/) directory.
- Update configuration and test data in the [`Constants/`](Constants/) and root directories.

## Reporting

After test execution, open the HTML report at [`Reports/index.html`](Reports/index.html) for a detailed summary.

## License

This project is licensed under the MIT License.

---

**Happy Testing!**
