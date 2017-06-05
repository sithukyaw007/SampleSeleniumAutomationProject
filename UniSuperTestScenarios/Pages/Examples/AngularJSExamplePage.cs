using AutomationFramework.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using AutomationFramework.Extensions;
using AutomationFramework.Helpers;
using System.Collections.Generic;
using System;
using OpenQA.Selenium.Interactions;

namespace LightHouseTestScenarios.Pages.Examples
{
    public class AngularJSExamplePage : BasePage
    {

        [FindsBy(How = How.Id, Using = "todo-list")]
        private IWebElement ulToDo { get; set; }

        [FindsBy(How = How.Id, Using = "new-todo")]
        private IWebElement txtToDo { get; set; }

        [FindsBy(How = How.Id, Using = "toggle-all")]
        private IWebElement btnToggleAll { get; set; }

        [FindsBy(How = How.LinkText, Using = "All")]
        private IWebElement btnFilterAll { get; set; }

        [FindsBy(How = How.LinkText, Using = "Active")]
        private IWebElement btnFilterActive { get; set; }

        [FindsBy(How = How.LinkText, Using = "Completed")]
        private IWebElement btnFilterCompleted { get; set; }

        [FindsBy(How = How.Id, Using = "clear-completed")]
        private IWebElement btnClearCompleted { get; set; }

        /// <summary>
        /// Add a to do item to the to do list
        /// </summary>
        /// <param name="toDoItem"></param>
        public void AddToDoItem(string toDoItem)
        {
            try
            {
                WebElementsHelper.KeyInObject(toDoItem, txtToDo);
                txtToDo.SendKeys(Keys.Enter);


            }
            catch (System.Exception)
            {
                //Record the known exepction to the log file if required : Feature will be implimented in the future
                throw;
            }
        }

        /// <summary>
        /// Edit the To do item by double clicking on the Existing to do item
        /// </summary>
        /// <param name="itemToEdit"></param>
        /// <param name="editVlu"></param>
        /// <returns></returns>
        public bool EditToDoItem(List<string> itemToEdit,string editVlu)
        {
            try
            {
                IWebElement toDoListObject = DriverContext.Driver.GetListItemObjectFromUnorderedList(ulToDo,itemToEdit);
                int listIndex = DriverContext.Driver.GetListItemIndexFromUnorderedList(ulToDo, itemToEdit);
                if (toDoListObject != null && listIndex != -1)
                {
                                     
                    Actions actions = new Actions(DriverContext.Driver);
                    actions.DoubleClick(toDoListObject).Perform();
                    string selector = "#todo-list > li:nth-child(" + listIndex.ToString() + ") > form > input";
                    //get the reference of the edit textbox item inside the LI object  
                    IWebElement toDOEditTextObj = DriverContext.Driver.FindElement(By.CssSelector(selector));
                    if (toDOEditTextObj != null)
                    {
                        WebElementsHelper.KeyInObject(editVlu, toDOEditTextObj, null, null, null, true);
                        toDOEditTextObj.SendKeys(Keys.Enter);
                    }
                    else
                    {
                        //Log an error sying that the required to do edit textbox was not found on the browser
                        return false;
                    }
                    return true;
                }
                else
                {
                    //Log the error 
                    return false;
                }
            }
            catch (System.Exception)
            {
                //Record the known exepction to the log file if required : Feature will be implimented in the future
                throw;
            }
        }

        /// <summary>
        /// Complete a Todo item by clicking or unclicking on the Circle checkbox
        /// </summary>
        /// <param name="itemToEdit"></param>
        /// <returns></returns>
        public bool CompleteToDoItem(List<string> itemToEdit,string action)
        {
            try
            {
                IWebElement toDoListObject = DriverContext.Driver.GetListItemObjectFromUnorderedList(ulToDo, itemToEdit);
                int listIndex = DriverContext.Driver.GetListItemIndexFromUnorderedList(ulToDo, itemToEdit);
                if (toDoListObject != null && listIndex != -1)
                {
                                       
                    Actions actions = new Actions(DriverContext.Driver);                    
                    string selector = "#todo-list > li:nth-child(" + listIndex.ToString() + ") > div > input";
                    //get the reference of the checkbox\cricle item inside the LI object 
                    IWebElement toDOChkObj = DriverContext.Driver.FindElement(By.CssSelector(selector));
                    var chkStatus = DriverContext.Driver.ExecuteJavaScript("return arguments[0].checked; ", toDOChkObj);
                    if (toDOChkObj != null)
                    {
                        switch (action.ToLower())
                        {
                            case "check":
                                if (!Convert.ToBoolean(chkStatus))//check to see if its not already checked
                                {
                                    toDOChkObj.Click();
                                }                               
                                break;
                            case "uncheck":
                                if (Convert.ToBoolean(chkStatus))//check to see if its checked
                                {
                                    toDOChkObj.Click();
                                }
                                break;
                            default:
                                break;
                        }
                       
                    }
                    else
                    {
                        //Log an error sying that the required to do checkbox was not found on the browser
                        return false;
                    }
                    return true;
                }
                else
                {
                    //Log the error 
                    return false;
                }
            }
            catch (System.Exception)
            {
                //Record the known exepction to the log file if required : Feature will be implimented in the future
                throw;
            }
        }

        /// <summary>
        /// Toggle all to do items to complete\incomplete state
        /// </summary>
        /// <returns></returns>
        public bool CompleteAllToDoItems()
        {
            btnToggleAll.Click();
            //if required check to see if all the checkboxes have the status checked
            //to make sure if the Toggle all button is doing wha its required to do
            //if something goes unexpected return false
            return true;
        }

        /// <summary>
        /// Filter the to do items by its status
        /// </summary>
        /// <param name="filterAction"></param>
        /// <returns></returns>
        public bool FilterToDos(string filterAction)
        {
            switch (filterAction.ToLower())
            {
                case "all":
                    btnFilterAll.Click();
                        break;
                case "active":
                    btnFilterActive.Click();
                    break;
                case "completed":
                    btnFilterCompleted.Click();
                    break;
                case "clear completed":
                    btnClearCompleted.Click();
                    break;                
            }
            return true;
        }

        /// <summary>
        /// Clear\delete a single to do item
        /// </summary>
        /// <param name="itemToEdit"></param>
        /// <returns></returns>
        public bool ClearToDoItem(List<string> itemToEdit)
        {
            try
            {
                IWebElement toDoListObject = DriverContext.Driver.GetListItemObjectFromUnorderedList(ulToDo, itemToEdit);
                int listIndex = DriverContext.Driver.GetListItemIndexFromUnorderedList(ulToDo, itemToEdit);
                if (toDoListObject != null && listIndex != -1)
                {

                    Actions actions = new Actions(DriverContext.Driver);                    
                    actions.MoveToElement(toDoListObject).Perform();
                    string selector = "#todo-list > li:nth-child(" + listIndex.ToString() + ") > div > button";
                    //get the reference of the delete\cross button inside the LI object 
                    IWebElement btnClearToDo = DriverContext.Driver.FindElement(By.CssSelector(selector));                    
                    if (btnClearToDo != null)
                    {
                        btnClearToDo.Click();

                    }
                    else
                    {
                        //Log an error sying that the required to do delete to do button was not found on the browser
                        return false;
                    }
                    return true;
                }
                else
                {
                    //Log the error 
                    return false;
                }
            }
            catch (System.Exception)
            {
                //Record the known exepction to the log file if required : Feature will be implimented in the future
                throw;
            }
        }



    }

}
