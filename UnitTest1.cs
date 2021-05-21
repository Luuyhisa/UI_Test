using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;

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
            extent.AttachReporter(htmlReporter);
        }
        [SetUp]
        public void BrowserSetUp()
        {
            driver = new ChromeDriver();
            driver.Url = "https://www.google.com/";
            driver.Manage().Window.Maximize();
        }
        [Test]
        public void EviromentVar()
        {
            extent.AddSystemInfo("Operating System", "Win 10");
            extent.AddSystemInfo("Host Name", "Computer Name");
            extent.AddSystemInfo("Browser", "Google Chrome");
        }


        public static string ScreenGrab(IWebDriver driver, string ScreenShotName)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            Screenshot screenshot = ts.GetScreenshot();
            string path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string uptobinPath = path.Substring(0, path.LastIndexOf("bin")) + "Report\\Screenshots\\" + ScreenShotName + ".png";
            string localPath = new Uri(uptobinPath).LocalPath;
            screenshot.SaveAsFile(localPath, ScreenshotImageFormat.Png);
            return localPath;
        }

        [Test]
        public void GoogleTest()
        {
            string passFail;
            ExtentTest test = null;
            try
            {
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
                test.Log(Status.Pass, "Navigated to the next Page...");
                //screeenshot
                string screenshot = ScreenGrab(driver, "Screenshot");
                test.AddScreenCaptureFromPath(screenshot);
                passFail = null;
            }
            catch (Exception e)
            {
                passFail = e.ToString();
                throw;
            }
            finally
            {
                if (driver != null) { driver.Quit(); }
            }
            Assert.IsNull(passFail, "Failed to complete the Test");
        }

        [Test]
        public void GoogleTestFail()
        {
            ExtentTest test = null;
            try
            {
                test = extent.CreateTest("GoogleTestFail").Info("Test Started...");
                test.Log(Status.Info, "Browser launched... ");
                IWebElement inText = driver.FindElement(By.XPath("//input[@class='111']"));
                IWebElement btnEnter = driver.FindElement(By.XPath("//div[@class='FPdoLc lJ9FBc']//input[@class='gNO89b']"));
                inText.SendKeys("Lungisa MKhasakhasa");
                inText.SendKeys("\t");
                btnEnter.Click();
                test.Log(Status.Pass, "Navigated to the next Page...");
                Assert.Pass();
            }
            catch (Exception e)
            {
                test.Log(Status.Fail, e.ToString());
                throw;
            }
            finally
            {
                if (driver != null) { driver.Quit(); }
            }
        }

        [TearDown]
        public void BrowserTearDown()
        {
            //driver.Close();
        }
        [OneTimeTearDown]
        public void Flush()
        {

            extent.Flush();
        }
    }
}