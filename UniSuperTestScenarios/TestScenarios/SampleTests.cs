using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomationFramework.Base;
using UniSuperTestScenarios.Pages.Home;
using UniSuperTestScenarios.AQATestHook;
using LightHouseTestScenarios.Pages.Examples;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Data;
using System;

namespace TestScenarios.TestScenarios
{
    /// <summary>
    /// Summary description for TestDataAccess
    /// </summary>
    [TestClass]
    public class SampleTests : HookInitialize
    {               
        [TestMethod]
        public void TestScenarios()
        {
            bool flag = false;
            //Navigate to the AngularJS page
            CurrentPage = GetPageInstance<ToDoMVCHomePage>();
            CurrentPage = CurrentPage.As<ToDoMVCHomePage>().NavigateToAngularJsPage();
            
            //Test Case 1 :  Add acouple of To do items
            CurrentPage.As<AngularJSExamplePage>().AddToDoItem("ToDoItem1");
            CurrentPage.As<AngularJSExamplePage>().AddToDoItem("ToDoItem2");


            //Test Case 2  : Edit the To do item from ToDoItem2 to ToDoItem2Edited
            List<string> editToDoList = new List<string>();
            editToDoList.Add("ToDoItem2");
            flag = CurrentPage.As<AngularJSExamplePage>().EditToDoItem(editToDoList, "ToDoItem2Edited");
            Assert.IsTrue(flag, "Failed to Edit the To do item from ToDoItem2 to ToDoItem2Edited");

            //Test Case 3: Complete a To-do by clicking inside the circle UI to 
            //the left of the To-do item ToDoItem1
            flag = false;
            editToDoList = new List<string>();
            editToDoList.Add("ToDoItem1");
            flag = CurrentPage.As<AngularJSExamplePage>().CompleteToDoItem(editToDoList,"check");
            Assert.IsTrue(flag, "Failed to Complete a To-do by clicking inside the circle UI to the left of the To-do item ToDoItem1");

            //Test Case 4 : re-activate a completed To-do by clicking inside the circle UI 
            //for the Todo item completed in test case 3 above
            flag = false;
            flag = CurrentPage.As<AngularJSExamplePage>().CompleteToDoItem(editToDoList, "uncheck");
            Assert.IsTrue(flag, "Failed to re-activate a completed To-do by clicking inside the circle UI for the Todo item completed in test case 3 above");

            //Test Case 5 : Add a third TO doItem ToDoItem3
            CurrentPage.As<AngularJSExamplePage>().AddToDoItem("ToDoItem3");

            //Test Case 6 : complete all active To-dos by clicking the down arrow at the top-left of the UI
            flag = false;
            flag = CurrentPage.As<AngularJSExamplePage>().CompleteAllToDoItems();
            Assert.IsTrue(flag, "Failed to complete all active To-dos by clicking the down arrow at the top-left of the UI");

            //Test Case 7 : Filter the visible To-dos by Completed state 
            flag = false;
            flag = CurrentPage.As<AngularJSExamplePage>().FilterToDos("completed");
            Assert.IsTrue(flag, "Failed to Filter the visible To-dos by Completed state ");

            //Test Case 8 : clear a single To-do item from the list completely by clicking the Close icon.
            //Clear ToDo Item ToDoItem3
            flag = false;
            editToDoList = new List<string>();
            editToDoList.Add("ToDoItem3");
            flag = CurrentPage.As<AngularJSExamplePage>().ClearToDoItem(editToDoList);
            Assert.IsTrue(flag, "Failed to clear a single To-do item from the list completely by clicking the Close icon.Clear ToDo Item ToDoItem3 ");

            //Test Case 9 : clear all completed To-do items from the list completely
            flag = false;
            flag = CurrentPage.As<AngularJSExamplePage>().FilterToDos("clear completed");
            Assert.IsTrue(flag, "Failed to  clear all completed To-do items from the list completely ");

        }

        public static List<string> GetMatchedObjectsList(DataTable dataTbl, List<string> kyFieldValues)
        {
            List<string> matchedListObj = new List<string>();
            int noOfMatches = 0;
            int rowCnt = 0;
            int colcnt = 0;
            try
            {

                foreach (DataRow row in dataTbl.Rows)
                {
                    rowCnt = rowCnt + 1;
                    colcnt = 0;
                    noOfMatches = 0;
                    foreach (DataColumn column in dataTbl.Columns)
                    {
                        colcnt = colcnt + 1;
                        IWebElement listObj = (IWebElement)row[column];
                        string listText = listObj.Text.ToLower().Trim();

                        for (int i = 0; i < kyFieldValues.Count; i++)
                        {
                            string searchForKyVlu = kyFieldValues[i].ToLower().Trim();
                            if (string.Compare(searchForKyVlu, listText) == 0)
                            {
                                noOfMatches = noOfMatches + 1;
                                matchedListObj.Add("Ky Search : " + searchForKyVlu + " Match Found at Row  [" + rowCnt.ToString() + ", " + colcnt.ToString() + "]");
                            }
                        }
                        if (noOfMatches == kyFieldValues.Count)
                            return matchedListObj;
                    }
                    if (noOfMatches == kyFieldValues.Count)
                        return matchedListObj;
                    else
                        matchedListObj.Clear();
                }
                return matchedListObj;
            }
            catch (Exception ex)
            {
                return matchedListObj;
            }
        }

        [TestCleanup]
        public void CleanDriver()
        {
            if (DriverContext.Driver != null)
            {
                DriverContext.Driver.Close();
            }
            
        }
    }
      
}
