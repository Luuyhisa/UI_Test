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
            System.Threading.Thread.Sleep(3000);
        }
        [Test]
        public void EviVar()
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
            // Feature: As a business user, I would like to make a record of all teams which are playing today
            // Scenario: Output all team names with a match today. If there are no matches today, output a message to convey this.
            string passFail;
            ExtentTest test = null;
            try
            {
                test = extent.CreateTest("Scenario 1").Info("Test Started");
                test.Log(Status.Info, "Browser launched... ");
                IWebElement todayTab = driver.FindElement(By.XPath("//*[@id='sp-timeline-current-dates']/li/a/span[1]"));
                System.Threading.Thread.Sleep(4000);
                todayTab.Click();
                System.Threading.Thread.Sleep(6000);
                IList<IWebElement> MatchList = driver.FindElements(By.XPath("//div[@class= 'qa-match-block']//span[@class='sp-c-fixture__team-name-wrap']"));
                List<String> values = new List<string>();
                values = MatchList.Select(x => x.Text).ToList<string>();
                string x = values[1];
                int GameNo = MatchList.Count;
                int[] GameNos = new int[GameNo];
                String NumberOfGames = GameNo.ToString();
                if (GameNos.Length > 0)
                {
                    for (int intCounter = 0; intCounter < GameNos.Length; intCounter = intCounter + 2)
                    {
                        String Home = values[intCounter];
                        String Away = values[intCounter + 1];
                        test.Log(Status.Skip, Home.ToString() + " VS " + Away.ToString());
                    }
                }
                else
                {
                    test.Log(Status.Warning, " No Games today  ");
                }
                System.Threading.Thread.Sleep(2000);
                string screenshot = ScreenGrab(driver, "Screenshot");
                test.AddScreenCaptureFromPath(screenshot);
                passFail = null;
                test.Log(Status.Pass, "Task Completed... ");

            }
            catch (Exception e)
            {
                passFail = e.ToString();
                test.Log(Status.Fail, "Task Failed... ");

                throw;
            }
            finally
            {
                if (driver != null) { driver.Quit(); }
                test.Log(Status.Pass, "Driver closed... ");

            }
            Assert.IsNull(passFail, "Failed to complete the Test");
        }
        [Test]
        public void Scenario2()
        {
            //Feature: As a sports user, I would like to read about all articles related to sports
            //Scenario: Use the search option to find all articles related to ‘sports’. Output the first heading and the last heading returned on the page
            string passFail;
            ExtentTest test = null;
            try
            {
                test = extent.CreateTest("Scenario 2").Info("Test Started");
                test.Log(Status.Info, "Browser launched... ");
                IWebElement inputSearch = driver.FindElement(By.XPath("//input[@id='orb-search-q']"));
                IWebElement btnSearch = driver.FindElement(By.XPath("//button[@id='orb-search-button']"));
                inputSearch.Clear();
                inputSearch.SendKeys("sports");
                System.Threading.Thread.Sleep(2000);
                test.Log(Status.Pass, "Text Entered... ");

                btnSearch.Click();
                System.Threading.Thread.Sleep(6000);
                test.Log(Status.Pass, "Search complete... ");
                string screenshot = ScreenGrab(driver, "Screenshot");
                test.AddScreenCaptureFromPath(screenshot);
                test.Log(Status.Pass, "Navigated to the result Page...");
                System.Threading.Thread.Sleep(2000);
                IList<IWebElement> listOfArticle = driver.FindElements(By.XPath("//div[@class= 'ssrcss-lr3ueg-Promo ett16tt0']//p[@class='ssrcss-12fmgoa-PromoHeadline e1f5wbog6']"));
                List<String> values = new List<string>();
                values = listOfArticle.Select(x => x.Text).ToList<string>();
                int ArticleNo = listOfArticle.Count;
                int[] ArticleList = new int[ArticleNo];

                if (ArticleList.Length > 0)
                {
                    for (int intCounter = 0; intCounter <= ArticleList.Length; intCounter = intCounter + ArticleList.Length)
                    {
                        if (intCounter == 0)
                        {
                            String ArticleName = values[intCounter];
                            test.Log(Status.Info, " first article heading : " + ArticleName.ToString());
                        }
                        else
                        {
                            intCounter = intCounter - 1;
                            String ArticleName = values[intCounter];
                            test.Log(Status.Info, " last article heading : " + ArticleName.ToString());
                        }
                    }
                }
                else
                {
                    test.Log(Status.Warning, " No Articles under 'Sports'...  ");
                }

                test.Log(Status.Pass, "Task Completed... ");

                passFail = null;
            }
            catch (Exception e)
            {
                passFail = e.ToString();
                test.Log(Status.Fail, "Task Failed... ");

                throw;
            }
            finally
            {
                if (driver != null) { driver.Quit(); }
                test.Log(Status.Pass, "Driver closed... ");
            }
            Assert.IsNull(passFail, "Failed to complete the Test");
        }

        [Test]
        public void Scenario3()
        {
            // Feature: As a QA, I would like to verify all negative scenarios for login
            //Scenario: Select ‘Sign in’, and enter as many negative scenarios as possible.Verify that a error message is displayed and the text that it contains is as expected

            string passFail;
            ExtentTest test = null;
            try
            {
                test = extent.CreateTest("Scenario 3").Info("Test Started");
                test.Log(Status.Info, "Browser launched... ");
                //click SignIn
                IWebElement btnSignIn = driver.FindElement(By.XPath("//span[@id='idcta-username']"));
                btnSignIn.Click();




                int negativeScenarios;

                for (int i = 0; i < 3; i++)
                {
                    negativeScenarios = +i;


                    switch (negativeScenarios)
                    {
                        case 1:
                            System.Threading.Thread.Sleep(6000);
                            ///-----------------------------------------------------------------------------------------------------------------------------
                            IWebElement inputUser = driver.FindElement(By.XPath("//input[@id='user-identifier-input']"));
                            inputUser.Clear();
                            inputUser.SendKeys("");
                            System.Threading.Thread.Sleep(2000);

                            IWebElement inputPassword = driver.FindElement(By.XPath("//input[@id='password-input']"));
                            inputPassword.Clear();
                            inputPassword.SendKeys("");
                            System.Threading.Thread.Sleep(2000);

                            IWebElement btnLogIn = driver.FindElement(By.XPath("//button[@id='submit-button']"));
                            btnLogIn.Click();
                            System.Threading.Thread.Sleep(6000);
                            IWebElement errorUsername = driver.FindElement(By.XPath(" (//p[@class='form-message__text'])[1]"));
                            IWebElement errorPassword = driver.FindElement(By.XPath(" (//p[@class='form-message__text'])[2]"));

                            if ((errorUsername.Text == "Something's missing. Please check and try again.") && (errorPassword.Text == "Something's missing. Please check and try again."))
                            {
                                test.Log(Status.Pass, "Error : " + errorUsername.Text);
                                test.Log(Status.Pass, "Error : " + errorPassword.Text);

                            }
                            else
                            {
                                test.Log(Status.Fail, "Incorrect Error : " + errorUsername.Text);
                                test.Log(Status.Fail, "Incorect Error : " + errorPassword.Text);
                            }

                            break;
                        case 2:
                            System.Threading.Thread.Sleep(6000);
                            ///-----------------------------------------------------------------------------------------------------------------------------
                            inputUser = driver.FindElement(By.XPath("//input[@id='user-identifier-input']"));
                            inputUser.Clear();
                            inputUser.SendKeys("212@1");
                            System.Threading.Thread.Sleep(2000);

                            inputPassword = driver.FindElement(By.XPath("//input[@id='password-input']"));
                            inputPassword.Clear();
                            inputPassword.SendKeys("@@");
                            System.Threading.Thread.Sleep(2000);

                            btnLogIn = driver.FindElement(By.XPath("//button[@id='submit-button']"));
                            btnLogIn.Click();
                            System.Threading.Thread.Sleep(6000);
                            errorUsername = driver.FindElement(By.XPath(" (//p[@class='form-message__text'])[1]"));
                            errorPassword = driver.FindElement(By.XPath(" (//p[@class='form-message__text'])[2]"));
                            string expectedError = "Something's missing. Please check and try again.";
                            if ((errorUsername.Text == expectedError) && (errorPassword.Text == expectedError))
                            {
                                test.Log(Status.Pass, "Error : " + errorUsername.Text);
                                test.Log(Status.Pass, "Error : " + errorPassword.Text);

                            }
                            else
                            {
                                test.Log(Status.Fail, "Incorrect Error : " + errorUsername.Text);
                                test.Log(Status.Fail, "Incorect Error : " + errorPassword.Text);
                            }

                            break;



                    }
                }







                test.Log(Status.Pass, "Task Completed... ");

                passFail = null;
            }
            catch (Exception e)
            {
                passFail = e.ToString();
                test.Log(Status.Fail, "Task Failed... ");

                throw;
            }
            finally
            {
                if (driver != null) { driver.Quit(); }
                test.Log(Status.Pass, "Driver closed... ");
            }
            Assert.IsNull(passFail, "Failed to complete the Test");



        }


        [TearDown]
        public void BrowserTearDown()
        {
            driver.Quit();
        }
        [OneTimeTearDown]
        public void Flush()
        {

            extent.Flush();
        }




    }
}