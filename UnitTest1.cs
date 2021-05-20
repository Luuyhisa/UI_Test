using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace QAFrameWork
{
    public class Tests
    {
        IWebDriver driver;




        [OneTimeSetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Url = "https://www.google.com/";
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
        [Test]
        public void GoogleTest()
        {
            //driver.Url = "https://www.google.com/";
            ////input[@class='gLFyf gsfi']
            IWebElement inText = driver.FindElement(By.XPath("//input[@class='gLFyf gsfi']"));
            IWebElement btnEnter = driver.FindElement(By.XPath("//div[@class='FPdoLc lJ9FBc']//input[@class='gNO89b']"));
            inText.SendKeys("Lungisa MKhasakhasa");
            inText.SendKeys("\t");
            btnEnter.Click();
            //div[@class='lJ9FBc']//input[@class='gNO89b']

            Assert.Pass();
        }

        [OneTimeTearDown]
        public void Close()
        {
           // driver.Close();
        }
    }
}