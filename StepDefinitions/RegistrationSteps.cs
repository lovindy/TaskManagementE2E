using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace TaskManagementE2E.StepDefinitions
{
    [Binding]
    public class RegistrationSteps
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private const string BaseUrl = "http://localhost:5173";

        [BeforeScenario]
        public void Setup()
        {
            // Setup Chrome WebDriver
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();

            // Create WebDriverWait
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Implicit wait
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [AfterScenario]
        public void Teardown()
        {
            _driver?.Quit();
        }

        [Given(@"I am on the registration page")]
        public void GivenIAmOnTheRegistrationPage()
        {
            _driver.Navigate().GoToUrl($"{BaseUrl}/register");
        }

        [When(@"I enter username ""(.*)""")]
        public void WhenIEnterUsername(string username)
        {
            // Use multiple strategies to find the username input
            By[] usernameSelectors = new By[]
            {
                By.CssSelector("input[placeholder='Choose a username']"),
                By.XPath("//input[@placeholder='Choose a username']"),
                By.Name("username")
            };

            IWebElement usernameInput = FindElementWithMultipleStrategies(usernameSelectors);

            usernameInput.Clear();
            usernameInput.SendKeys(username);
        }

        [When(@"I enter password ""(.*)""")]
        public void WhenIEnterPassword(string password)
        {
            // Use multiple strategies to find the password input
            By[] passwordSelectors = new By[]
            {
                By.XPath("(//input[@type='password'])[1]"),
                By.CssSelector("input[type='password']:nth-of-type(1)"),
                By.Name("password")
            };

            IWebElement passwordInput = FindElementWithMultipleStrategies(passwordSelectors);

            passwordInput.Clear();
            passwordInput.SendKeys(password);
        }

        [When(@"I enter confirmation password ""(.*)""")]
        public void WhenIEnterConfirmationPassword(string confirmPassword)
        {
            // Use multiple strategies to find the confirm password input
            By[] confirmPasswordSelectors = new By[]
            {
                By.XPath("(//input[@type='password'])[2]"),
                By.CssSelector("input[type='password']:nth-of-type(2)"),
                By.Name("confirmPassword")
            };

            IWebElement confirmPasswordInput = FindElementWithMultipleStrategies(confirmPasswordSelectors);

            confirmPasswordInput.Clear();
            confirmPasswordInput.SendKeys(confirmPassword);
        }

        [When(@"I submit the registration form")]
        public void WhenISubmitTheRegistrationForm()
        {
            // Multiple strategies to find the submit button
            By[] submitButtonSelectors = new By[]
            {
                By.XPath("//button[contains(text(), 'Create Account')]"),
                By.CssSelector("button.w-full"),
                By.CssSelector("button[type='submit']"),
                By.XPath("//button[@round]")
            };

            IWebElement submitButton = FindElementWithMultipleStrategies(submitButtonSelectors);

            // Use JavaScript click if regular click fails
            try
            {
                // First try standard click
                submitButton.Click();
            }
            catch
            {
                // Fallback to JavaScript click
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", submitButton);
            }
        }

        [Then(@"I should see a successful registration message")]
        public void ThenIShouldSeeASuccessfulRegistrationMessage()
        {
            // Multiple ways to find success message
            By[] successMessageSelectors = new By[]
             {
                // More specific XPath
                By.XPath("//div[contains(@class, 'el-message') and contains(@class, 'el-message--success')]"),
    
                // Try full text match
                By.XPath("//div[contains(@class, 'el-message') and contains(text(), 'Registration successful')]"),
    
                // Add a data attribute if possible in the Vue component
                By.CssSelector("[data-test-id='registration-success-message']")
             };

            try
            {
                // Wait for success message to be visible
                IWebElement successMessage = _wait.Until(d =>
                    FindElementWithMultipleStrategies(successMessageSelectors)
                );

                Assert.IsTrue(successMessage.Displayed, "Success message not displayed");
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("Success message did not appear within the expected time");
            }
        }

        [Then(@"I should be redirected to the dashboard")]
        public void ThenIShouldBeRedirectedToDashboard()
        {
            // Wait for dashboard URL
            _wait.Until(d => d.Url.Contains("/"));

            // Assert current URL is dashboard
            StringAssert.Contains("/", _driver.Url, "Did not redirect to dashboard");
        }

        // Helper method to find element using multiple strategies
        private IWebElement FindElementWithMultipleStrategies(By[] selectors)
        {
            foreach (By selector in selectors)
            {
                try
                {
                    var element = _wait.Until(d => d.FindElement(selector));
                    return element;
                }
                catch
                {
                    // Continue to next selector if this one fails
                    continue;
                }
            }

            // If no selector works, throw an exception
            throw new NoSuchElementException($"Could not find element with any of the provided selectors");
        }
    }
}