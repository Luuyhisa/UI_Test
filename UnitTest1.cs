using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;


namespace QAFrameWork
{
    public class Tests
    {
        IWebDriver driver;
        ExtentTest test = null;

        ExtentReports extent = null;

        [OneTimeSetUp]
        public void Setup()
        {




            extent = new ExtentReports();

            string startupPath = System.IO.Directory.GetDirectoryRoot("Report");
            var htmlReporter = new ExtentHtmlReporter(@"C:\\QAFrameWork\Report\QAReport.html");
            // var htmlReporter = new ExtentHtmlReporter("C:\\QAFrameWork\\Report\\QAReport.html");
            extent.AttachReporter(htmlReporter);



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
            ExtentTest test = null;
            test = extent.CreateTest("GoogleTest").Info("Test Started");
            test.Log(Status.Info, "Browser launched... ");
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
            extent.Flush();
        }
    }
}