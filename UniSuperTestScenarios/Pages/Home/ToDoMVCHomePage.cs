using AutomationFramework.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using AutomationFramework.Extensions;
using LightHouseTestScenarios.Pages.Examples;

namespace UniSuperTestScenarios.Pages.Home
{
    public class ToDoMVCHomePage: BasePage
    {
        [FindsBy(How = How.LinkText, Using = "AngularJS")]
        private IWebElement lnkAngularJS { get; set; }        

        public bool ClickOnMvcToDoExamplesLink(string linkName)
        {
            bool flag;
            flag = false;
            switch (linkName.ToLower())
            {
                case "angularjs":
                    lnkAngularJS.Click();
                    DriverContext.Driver.WaitObjectExists(By.Id("new-todo"), 10, true);
                    flag = true;
                    break;
                default:
                    break;

            }
            return flag;
        }

        public AngularJSExamplePage NavigateToAngularJsPage()
        {
            bool flag = ClickOnMvcToDoExamplesLink("AngularJS");
            if (!flag)
            {
                //log the error and take a screenshot
                return null;
            }
            return GetPageInstance<AngularJSExamplePage>();
        }
    }
}
