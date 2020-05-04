using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquityConsole
{
    class ResetTool
    {
        IWebDriver driver;

        static void Main(string[] args)
        {
            //Console.WriteLine("Test done");
            //Console.ReadLine();
            var r = new ResetTool();
            r.invokeBrowser();
            r.test();
            r.closeBrowser();
           
        }

        void invokeBrowser()
        {
            driver = new ChromeDriver();
        }

        void test()
        {
            driver.Url = "http://www.google.com";
        }

        void closeBrowser()
        {
            driver.Close();
        }
    }
}
