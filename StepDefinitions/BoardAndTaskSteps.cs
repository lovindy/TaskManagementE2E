using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace TaskManagementE2E.StepDefinitions
{
    [Binding]
    public class BoardAndTaskSteps
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

        [Given(@"I am logged in")]
        public void GivenIAmLoggedIn()
        {
            // Navigate to login page
            _driver.Navigate().GoToUrl($"{BaseUrl}/login");

            // Find and fill username
            By[] usernameSelectors = new By[]
            {
                By.CssSelector("input[placeholder='Username']"),
                By.XPath("//input[@type='text']"),
                By.Name("username")
            };
            IWebElement usernameInput = FindElementWithMultipleStrategies(usernameSelectors);
            usernameInput.Clear();
            usernameInput.SendKeys("username-001");

            // Find and fill password
            By[] passwordSelectors = new By[]
            {
                By.XPath("//input[@type='password']"),
                By.CssSelector("input[type='password']"),
                By.Name("password")
            };
            IWebElement passwordInput = FindElementWithMultipleStrategies(passwordSelectors);
            passwordInput.Clear();
            passwordInput.SendKeys("password123");

            // Submit login
            By[] loginButtonSelectors = new By[]
            {
                By.XPath("//button[contains(text(), 'Login')]"),
                By.CssSelector("button[type='submit']"),
                By.CssSelector("button.login-button")
            };
            IWebElement loginButton = FindElementWithMultipleStrategies(loginButtonSelectors);
            loginButton.Click();

            // Wait for dashboard or main page to load
            _wait.Until(d => d.Url.Contains("/") || d.FindElements(By.CssSelector(".dashboard")).Count > 0);
        }

        [When(@"I navigate to the ""(.*)"" board")]
        public void WhenINavigateToTheBoard(string boardTitle)
        {
            // Find and click on the specific board in the sidebar
            By[] boardSelectors = new By[]
            {
                By.XPath($"//span[contains(text(), '{boardTitle}')]"),
                By.CssSelector($"[data-board-title='{boardTitle}']"),
                By.XPath("//div[contains(@class, 'board-item') and contains(., '" + boardTitle + "')]")
            };

            IWebElement boardLink = FindElementWithMultipleStrategies(boardSelectors);
            boardLink.Click();

            // Wait for board page to load
            _wait.Until(d =>
                d.Url.Contains("/boards/") &&
                d.FindElements(By.CssSelector(".board-container")).Count > 0
            );
        }

        [When(@"I click to add a new task to the ""(.*)"" list")]
        public void WhenIClickToAddANewTaskToTheList(string listName)
        {
            // Multiple strategies to find the 'Add Task' button for the specific list
            By[] addTaskButtonSelectors = new By[]
            {
                By.XPath($"//div[contains(text(), '{listName}')]//following-sibling::button[contains(@class, 'add-task')]"),
                By.CssSelector($"[data-list-name='{listName}'] .add-task-button"),
                By.XPath($"//div[contains(@class, 'list-header') and contains(., '{listName}')]//button")
            };

            IWebElement addTaskButton = FindElementWithMultipleStrategies(addTaskButtonSelectors);
            addTaskButton.Click();

            // Wait for task creation modal to open
            _wait.Until(d =>
                d.FindElements(By.CssSelector(".task-creation-modal")).Count > 0
            );
        }

        [When(@"I enter task title ""(.*)""")]
        public void WhenIEnterTaskTitle(string taskTitle)
        {
            // Multiple strategies to find task title input
            By[] taskTitleSelectors = new By[]
            {
                By.CssSelector("input[placeholder='Enter task title']"),
                By.XPath("//input[@name='taskTitle']"),
                By.Id("taskTitleInput")
            };

            IWebElement taskTitleInput = FindElementWithMultipleStrategies(taskTitleSelectors);
            taskTitleInput.Clear();
            taskTitleInput.SendKeys(taskTitle);
        }

        [When(@"I enter task description ""(.*)""")]
        public void WhenIEnterTaskDescription(string taskDescription)
        {
            // Multiple strategies to find task description textarea
            By[] taskDescriptionSelectors = new By[]
            {
                By.CssSelector("textarea[placeholder='Enter task description']"),
                By.XPath("//textarea[@name='taskDescription']"),
                By.Id("taskDescriptionInput")
            };

            IWebElement taskDescriptionInput = FindElementWithMultipleStrategies(taskDescriptionSelectors);
            taskDescriptionInput.Clear();
            taskDescriptionInput.SendKeys(taskDescription);
        }

        [When(@"I submit the task creation form")]
        public void WhenISubmitTheTaskCreationForm()
        {
            // Multiple strategies to find task creation submit button
            By[] submitButtonSelectors = new By[]
            {
                By.XPath("//button[contains(text(), 'Create Task')]"),
                By.CssSelector("button[type='submit'].create-task-button"),
                By.Id("createTaskSubmit")
            };

            IWebElement submitButton = FindElementWithMultipleStrategies(submitButtonSelectors);

            // Use JavaScript click if regular click fails
            try
            {
                submitButton.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", submitButton);
            }
        }

        [Then(@"I should see the new task ""(.*)"" in the ""(.*)"" list")]
        public void ThenIShouldSeeTheNewTaskInTheList(string taskTitle, string listName)
        {
            // Multiple strategies to find the task in the specific list
            By[] taskSelectors = new By[]
            {
                By.XPath($"//div[contains(@class, 'list-name') and contains(text(), '{listName}')]//ancestor::div[contains(@class, 'list-container')]//div[contains(text(), '{taskTitle}')]"),
                By.CssSelector($"[data-list-name='{listName}'] .task-item:contains('{taskTitle}')"),
                By.XPath($"//div[@data-task-title='{taskTitle}' and ancestor::div[contains(text(), '{listName}')]]")
            };

            try
            {
                IWebElement newTask = _wait.Until(d =>
                    FindElementWithMultipleStrategies(taskSelectors)
                );

                Assert.IsTrue(newTask.Displayed, $"Task '{taskTitle}' not found in '{listName}' list");
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail($"Task '{taskTitle}' did not appear in '{listName}' list within the expected time");
            }
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