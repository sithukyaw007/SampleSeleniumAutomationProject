using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AutomationFramework.Base
{
    public abstract class BasePage:Base
    {
        public BasePage()
        {
            PageFactory.InitElements(DriverContext.Driver, this);            
        }
    }
}
