using AutomationFramework.Config;
using AutomationFramework.Helpers;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using static AutomationFramework.Base.Browser;

namespace AutomationFramework.Base
{
    public abstract class TestInitializeHooks:Base
    {
        public readonly BrowserType Browser;

        /*public TestInitializeHooks(BrowserType _browser)
        {
            Browser = _browser;
        } */       

        /// <summary>
        /// Open a new instance of the browser
        /// </summary>
        /// <param name="browserType"></param>
        private static void OpenBrowser(BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.InternetExplorer:
                    DriverContext.Driver = new InternetExplorerDriver();                    
                    DriverContext.WebBrowser = new Browser(DriverContext.Driver);
                    break;
                case BrowserType.FireFox:
                    DriverContext.Driver = new FirefoxDriver();                    
                    DriverContext.WebBrowser = new Browser(DriverContext.Driver);
                    break;
                case BrowserType.Chrome:
                    ChromeOptions chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments("test-type");
                    chromeOptions.AddArguments("start-maximized");
                    DriverContext.Driver = new ChromeDriver(chromeOptions);                    
                    DriverContext.WebBrowser = new Browser(DriverContext.Driver);                    
                    break;                
                default:
                    break;

            }
        }

        public void InitSettings()
        {
            //Set all the framework settings
            ConfigReader.SetFrameworkSettings();            

            //open the Browser
            OpenBrowser(Settings.BrowserType);

            //Open the application URL
            DriverContext.WebBrowser.GotoUrl(Settings.ApplicationUrl);
            //DriverContext.Driver.Manage().Window.Maximize();
        }
    }
   
}
