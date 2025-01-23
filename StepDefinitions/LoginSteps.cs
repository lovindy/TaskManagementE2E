using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace MyApp.E2ETests.Steps
{
    [Binding]
    public class LoginSteps
    {
        private IWebDriver driver;

        [Given(@"I am on the login page")]
        public void GivenIAmOnTheLoginPage()
        {
            // Initialize ChromeDriver (you can use another browser as well)
            driver = new ChromeDriver();

            // Navigate to the local login page (port 5173)
            driver.Navigate().GoToUrl("http://localhost:5173/login");
        }

        [When(@"I enter ""(.*)"" as username and ""(.*)"" as password")]
        public void WhenIEnterCredentials(string username, string password)
        {
            // Find the username and password fields and enter the test credentials
            driver.FindElement(By.CssSelector("input[placeholder='Enter username']")).SendKeys(username);
            driver.FindElement(By.CssSelector("input[placeholder='Enter password']")).SendKeys(password);
        }

        [When(@"I click the login button")]
        public void WhenIClickTheLoginButton()
        {
            // Find and click the login button using its class selector
            driver.FindElement(By.CssSelector("button.el-button--primary")).Click();
        }

        [Then(@"I should see the user dashboard")]
        public void ThenIShouldSeeTheUserDashboard()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Wait for URL to change (assuming dashboard is at root '/')
            wait.Until(driver => driver.Url.EndsWith("/"));

            Assert.IsTrue(driver.Url.EndsWith("/"), "Did not navigate to dashboard.");

            driver.Quit();
        }
    }
}
