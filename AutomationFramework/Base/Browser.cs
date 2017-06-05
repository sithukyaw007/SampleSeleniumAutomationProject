using AutomationFramework.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFramework.Base
{
    public class Browser
    {
        private readonly IWebDriver _driver;

        public Browser(IWebDriver driver)
        {
            _driver = driver;
        }

        public BrowserType Type { get; set; }

        public void GotoUrl(string url)
        {
            DriverContext.Driver.Url = url;           
        }

        public void OpenElectronApp(ChromeOptions electronOption,string electronBinaryPath)
        {
            electronOption.BinaryLocation = electronBinaryPath;
            DesiredCapabilities capability = new DesiredCapabilities();
            capability.SetCapability(CapabilityType.BrowserName, "Chrome");
            capability.SetCapability("chromeOptions", electronOption);
            DriverContext.Driver = new ChromeDriver(electronOption);
            DriverContext.WebBrowser = new Browser(DriverContext.Driver);
        }

        public enum BrowserType
        {
            InternetExplorer,
            FireFox,
            Chrome,
            Electron
        }        

    }
}
