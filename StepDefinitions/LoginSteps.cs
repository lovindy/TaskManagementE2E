using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System;

namespace TaskManagementE2E.StepDefinitions
{
    [Binding]
    public class LoginSteps
    {
        private IWebDriver _driver;

        [Given(@"I am on the login page")]
        public void GivenIAmOnTheLoginPage()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("http://localhost:5173/login");
        }

        [When(@"I enter ""(.*)"" as username and ""(.*)"" as password")]
        public void WhenIEnterCredentials(string username, string password)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Alternative to ExpectedConditions
            var usernameElement = wait.Until(d =>
            {
                var element = d.FindElement(By.CssSelector("input[placeholder='Username']"));
                return element.Displayed ? element : null;
            });

            usernameElement.Clear();
            usernameElement.SendKeys(username);

            var passwordElement = wait.Until(d =>
            {
                var element = d.FindElement(By.CssSelector("input[placeholder='Password']"));
                return element.Displayed ? element : null;
            });

            passwordElement.Clear();
            passwordElement.SendKeys(password);
        }

        [When(@"I click the login button")]
        public void WhenIClickTheLoginButton()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Alternative to ElementToBeClickable
            var loginButton = wait.Until(d =>
            {
                var element = d.FindElement(By.CssSelector("button[type='button'].el-button--primary"));
                return element.Enabled ? element : null;
            });

            loginButton.Click();
        }

        [Then(@"I should see the user dashboard after login")]
        public void ThenIShouldSeeTheUserDashboard()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Wait for URL to change (assuming dashboard is at root '/')
            wait.Until(_driver => _driver.Url.EndsWith("/"));

            Assert.IsTrue(_driver.Url.EndsWith("/"), "Did not navigate to dashboard.");

            // Pause for 3 seconds to observe the dashboard
            System.Threading.Thread.Sleep(3000);

            //_driver.Quit();
        }
    }
}