using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QAFrameWork.TestPack
{
    public class Scenario
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
            driver.Url = "https://www.bbc.co.uk/sport/football/scores-fixtures";
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
        public void Scenario1()
        {

            /*
            Feature: As a business user, I would like to make a record of all teams which are playing today
            Scenario: Output all team names with a match today. If there are no matches today, output a message to convey this.
            */
            string passFail;
            ExtentTest test = null;
            try
            {
                test = extent.CreateTest("Scenario 1").Info("Test Started");
                test.Log(Status.Info, "Browser launched... ");
                ////input[@class='gLFyf gsfi']




                IWebElement todayTab = driver.FindElement(By.XPath("//*[@id='sp-timeline-current-dates']/li/a/span[1]"));
                System.Threading.Thread.Sleep(4000);
                todayTab.Click();
                System.Threading.Thread.Sleep(6000);

                IList<IWebElement> MatchList = driver.FindElements(By.XPath("//div[@class= 'qa-match-block']//span[@class='sp-c-fixture__team-name-wrap']"));
                List<String> values = new List<string>();
                values = MatchList.Select(x => x.Text).ToList<string>();
                string x = values[1];

                int GameNo = MatchList.Count / 2;

                int[] GameNos = new int[GameNo];
                String NumberOfGames = GameNo.ToString();

                for (int i = 0; i < GameNos.Length; i=i+2)
                {
                
                    String Home = values[i];
                    String Away= values[i+1];
                    test.Log(Status.Skip, Home.ToString() +" VS " + Away.ToString());
               
                }


                // inText.SendKeys("L");
                // inText.SendKeys("\t");
                // btnEnter.Click();
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
        public void GoogleTestFailaa()
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