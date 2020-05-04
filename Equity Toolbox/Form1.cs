using System;
using System.Windows.Forms;
using System.Threading;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.ComponentModel;

namespace Equity_Toolbox
{
    public partial class Form1 : Form
    {
        //Global Variables
        IWebDriver driver;
        string currentYear = DateTime.Now.Year.ToString();

        //Initialize Form
        public Form1()
        {
            InitializeComponent();

        }

        //Form Load
        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxPassword.Text = "Equity" + currentYear + "!";
            textBoxBrittniPassword.Text = "Equity" + currentYear + "!";
        }

        public void buttonGo_Click(object sender, EventArgs e)
        {
            setup();
            resetPass();
            closeDriver();
            
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            driver.Quit();
        }

        public void setup()
        {
            buttonGo.Enabled = false;
            labelStatus.Text = "Status - Getting browser ready in background";
            labelStatus.Refresh();
            progressBarStatus.Value = 10;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("headless"); //hide chrome window
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true; //hide cmd prompt window for webdriver
            driver = new ChromeDriver(service, options);
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Window.Minimize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);

            driver.Url = "https://www.equityelevate.com";
        }

        public void resetPass()
        {
            //Login
            labelStatus.Text = "Status - Logging in to Equity Elevate";
            labelStatus.Refresh();
            driver.FindElement(By.Id("Email")).SendKeys("brittnibasso@gmail.com");
            driver.FindElement(By.Id("Password")).SendKeys(textBoxBrittniPassword.Text);
            
            //Select admin user and navigate to roster
            driver.FindElement(By.ClassName("button-title-h")).Click();
            driver.FindElement(By.ClassName("button-title-h")).Click();
            driver.Url = "https://equityelevate.com/home/mybusinessroster";

            //Get name of agent and search for it
            labelStatus.Text = "Status - Searching for agent..";
            labelStatus.Refresh();
            driver.FindElement(By.Name("searchname")).SendKeys(textBoxName.Text);
            driver.FindElement(By.Id("searchsubmit")).Click();
            Thread.Sleep(500);

            //Count number of results. If more than 1, stop reset process
            labelStatus.Text = "Status - Checking if there's more than 1 result";
            labelStatus.Refresh();
            Thread.Sleep(750);
            IList<IWebElement> agents = driver.FindElements(By.XPath("//*[@id=\"exitingform\"]/div/div[2]"));
            if (agents.Count > 3) 
            {
                labelStatus.Text = "Status - More than 1 agent with similar name, proceed manually";
                labelStatus.Refresh();
            } //Do nothing if greater than 1. Empty {}'s
            else
            {
                //Select user from list div[1]/div[2] for first user in list or div[2]/div[2] for second user in list
                labelStatus.Text = "Status - Agent found, starting password reset process";
                labelStatus.Refresh();
                driver.FindElement(By.XPath("//*[@id=\"exitingform\"]/div[1]/div[2]/a")).Click();
                Thread.Sleep(500);

                //Change password button
                driver.FindElement(By.XPath("//*[@id=\"personal-info-form\"]/div[3]/div/div/div/button")).Click();

                //Enter password, confirm and save
                driver.FindElement(By.Id("Password1")).SendKeys(textBoxPassword.Text);
                Thread.Sleep(500);
                driver.FindElement(By.Id("Password2")).SendKeys(textBoxPassword.Text);
                driver.FindElement(By.Id("chgPasswordSubmit")).Click();
                Thread.Sleep(500);
                driver.FindElement(By.XPath("/html/body/div[2]/div/div[3]/button[1]")).Click();
                labelStatus.Text = "Status - Password successfully changed";
                labelStatus.Refresh();
                Thread.Sleep(500);

            }
        }

        public void closeDriver()
        {
            labelStatus.Text = "Status - Closing connection to browser";
            labelStatus.Refresh();
            driver.Quit();
            Thread.Sleep(1000);
            labelStatus.Text = "Status - Done";
            labelStatus.Refresh();
            buttonGo.Enabled = true;
            progressBarStatus.Value = 0;

        }

        // On worker thread so do our thing!
        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Your background task goes here
            setup();
            resetPass();
            closeDriver();
            {
                // Report progress to 'UI' thread
                //backgroundWorker1.ReportProgress();
                
            }
        }
        // Back on the 'UI' thread so we can update the progress bar
        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // The progress percentage is a property of e
            progressBarStatus.Value = e.ProgressPercentage;
        }


    }
    /*    
     *                //var r = new toolbox();
     *    public class toolbox
        {
            IWebDriver driver;


            public void invokeBrowser()
            {
                ChromeOptions options = new ChromeOptions();
                //options.AddArgument("--headless"); //hide chrome window
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = false; //hide cmd prompt window for webdriver
                driver = new ChromeDriver(service,options);
                driver.Manage().Cookies.DeleteAllCookies();
                driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);

                driver.Url = "https://www.equityelevate.com";

            }

            public void resetPass()
            {
                //Declarations
                Form1 f = new Form1();
                string currentYear = DateTime.Now.Year.ToString();
                f.label1.Text = currentYear;
            }

            public void closeBrowser()
            {
                driver.Quit();
            }
        } */



}
