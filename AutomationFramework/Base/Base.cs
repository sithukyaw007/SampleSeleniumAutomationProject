

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AutomationFramework.Base
{
    public class Base
    {
        private IWebDriver _driver { get; set; }
         
        public BasePage CurrentPage { get; set; }


        /// <summary>
        ///Init the Page and return its instance 
        /// </summary>
        /// <typeparam name="TPage"></typeparam>
        /// <returns></returns>
        protected TPage GetPageInstance<TPage>() where TPage : BasePage, new()
        {
            TPage pageInstance = new TPage()
            {
                _driver = DriverContext.Driver
            };
            PageFactory.InitElements(DriverContext.Driver, pageInstance);
            return pageInstance;
        }
        
        /// <summary>
        /// Get the Page Reference
        /// </summary>
        /// <typeparam name="TPage"></typeparam>
        /// <returns></returns>
        public TPage As<TPage>() where TPage : BasePage
        {
            return (TPage)this;
        }

    }
}
