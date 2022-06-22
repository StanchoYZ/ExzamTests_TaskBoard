using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace TaskBoard_DesktopClientTests
{
    public class DesktopTests
    {
        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";
        private const string TaskBoard = "https://taskboard.nakov.repl.co/api";
        private const string appLocation = @"C:\work\TaskBoard.DesktopClient.exe";
        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;
        [SetUp]
        public void StartApp()
        {
            options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability("app", appLocation);
            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }
        [Test]
        public void Test_TaskBoard_Tasks_Lists()
        {
            var coonectButton = driver.FindElementByAccessibilityId("buttonConnect");
            coonectButton.Click();

            var openTable = driver.FindElementsByAccessibilityId("listViewTasks");

            foreach (var task in openTable)
            {
                if (task.Text.StartsWith("Project"))
                {
                    Assert.That(task.Text, Is.EqualTo("Project skeleton"));
                    break;
                }
            }

        }
        [Test]
        public void Test_TaskBoard_Add_NewTask_ValidData()
        {
            var coonectButton = driver.FindElementByAccessibilityId("buttonConnect");
            coonectButton.Click();

            var addButton = driver.FindElementByAccessibilityId("buttonAdd");
            addButton.Click();

            var titleTextBox = driver.FindElementByAccessibilityId("textBoxTitle");
            var uniqueTitle = "NewTaskTitle" + DateTime.Now.Ticks.ToString();
            titleTextBox.SendKeys(uniqueTitle);

            var createButton = driver.FindElementByAccessibilityId("buttonCreate");
            createButton.Click();

            var textBox = driver.FindElementByAccessibilityId("textBoxSearchText");
            textBox.SendKeys(uniqueTitle);

            var searchButton = driver.FindElementByAccessibilityId("buttonSearch");
            searchButton.Click();

            var openTable = driver.FindElementsByAccessibilityId("listViewTasks");

            foreach (var task in openTable)
            {
                if (task.Text.StartsWith("NewTask"))
                {
                    Assert.That(task.Text, Is.EqualTo(uniqueTitle));
                    break;
                }

            }
        }
    }
}