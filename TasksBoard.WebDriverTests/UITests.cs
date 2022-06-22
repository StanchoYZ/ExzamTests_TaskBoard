using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace TasksBoard.WebDriverTests
{
    public class UITests
    {
        private const string url = "https://taskboard-2.stanislavzlatan.repl.co";
        private WebDriver driver;

        [SetUp]
        public void OpenBrowser()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

        }
        [TearDown]
        public void CloseBrowser()
        {
            this.driver.Quit();


        }

        [Test]
        public void Test_GetAllTasks_FirstTasksName()
        {
            //Arrange
            driver.Navigate().GoToUrl(url);
            var taskLink = driver.FindElement(By.LinkText("Task Board"));
            //Act
            taskLink.Click();
            var title = driver.FindElement(By.XPath("//div[3]/table[1]/tbody/tr[1]/th")).Text;
            var description = driver.FindElement(By.XPath("//div[3]/table[1]/tbody/tr[1]/td")).Text;
            //Assert
            Assert.That(title, Is.EqualTo("Title"));
            Assert.That(description, Is.EqualTo("Project skeleton"));
        }
        [Test]
        public void Test_SearchValidKeyWorld()
        {
            //Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();
            //Act
            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("home");
            driver.FindElement(By.Id("search")).Click();

            //Assert
            var titleName = driver.FindElement(By.CssSelector("tr.title > td")).Text;
            var title = driver.FindElement(By.CssSelector("tr.title > th")).Text;
            Assert.That(title, Is.EqualTo("Title"));
            Assert.That(titleName, Is.EqualTo("Home page"));

        }
        [Test]
        public void Test_SearchInvalidKeyWorld()
        {
            //Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();
            //Act
            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("mising1234567");
            driver.FindElement(By.Id("search")).Click();

            //Assert
            var resultLable = driver.FindElement(By.Id("searchResult")).Text;
            Assert.That(resultLable, Is.EqualTo("No tasks found."));

        }
        [Test]
        public void Test_CreateTaskInvalidData()
        {
            //Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();
            //Act
            var creatButton = driver.FindElement(By.Id("create"));
            creatButton.Click();
            var errmsg = driver.FindElement(By.CssSelector("div.err")).Text;

            //Assert

            Assert.That(errmsg, Is.EqualTo("Error: Title cannot be empty!"));

        }
        [Test]
        public void Test_CreateTaskValidData()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            var titleSend = "Alabala" + DateTime.Now.Ticks;
            var descriptionSend = "Alabala" + DateTime.Now.Ticks;

            // Act
            driver.FindElement(By.Id("title")).SendKeys(titleSend);
            driver.FindElement(By.Id("description")).SendKeys(descriptionSend);

            var buttonCreate = driver.FindElement(By.Id("create"));
            buttonCreate.Click();


            // Assert
            var taskTitle = driver.FindElements(By.XPath("/html/body/main/div/div[1]/table/tbody/tr[1]/td")).Last().Text;
            var taskDescription = driver.FindElements(By.XPath("/html/body/main/div/div[1]/table/tbody/tr[2]/td/div")).Last().Text;

            Assert.That(taskTitle, Is.EqualTo(titleSend));
            Assert.That(taskDescription, Is.EqualTo(descriptionSend));

        }
    }
}