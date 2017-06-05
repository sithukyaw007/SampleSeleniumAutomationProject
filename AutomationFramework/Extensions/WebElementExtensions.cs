using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using OpenQA.Selenium.Interactions;
using AutomationFramework.Base;

namespace AutomationFramework.Extensions
{
    public static class WebElementExtensions
    {       
        private static bool hasChildren = false;                
        private static DataTable childObjDataTbl = null;
        private static DataRow dtRow = null;        
        private static int rowCounter = 0;         
        private static bool incrementRowCounter = false;

       
        /*public static IWebElement FindElement(this ISearchContext context, By by, uint timeout, bool displayed = false)
        {
            var wait = new DefaultWait<ISearchContext>(context);
            wait.Timeout = TimeSpan.FromSeconds(timeout);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            return wait.Until(ctx => {
                var elem = ctx.FindElement(by);
                if (displayed && !elem.Displayed)
                    return null;

                return elem;
            });
        }*/
                   
        /// <summary>
        /// Fetch the object reference of any control on the screen
        /// Please Note : Make sure you pass unique objPropNames,objPropValues of the control,
        /// if not then the first matched control is returned
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="objPropNames"></param>
        /// <param name="objPropValues"></param>
        /// <param name="elementTagName"></param>
        /// <returns></returns>
        //public static IWebElement GetWebObject(this IWebDriver driver,string[] objPropNames, string[] objPropValues, string elementTagName)
        //{
        //    IWebElement webObj = null;
        //    //string elementPropVlu;

        //    //string propName, propVlu;
        //    try
        //    {
        //        IList<IWebElement> lstElement = DriverContext.Driver.FindElements(By.TagName(elementTagName));
        //        foreach (var item in lstElement)
        //        {
        //            //reset the flag to true for every iteration
        //            bool objectFound = true;

        //            for (int i = 0; i < objPropNames.Length; i++)
        //            {
        //                ObjPropName = objPropNames[i];
        //                ObjPropVlu = objPropValues[i];
        //                //get the value of the attribute
        //                //elementPropVlu = item.GetAttribute(ObjPropName.ToString());
        //                var elementPropVlu = (Object)null;
        //                try
        //                {
        //                   elementPropVlu = ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0]." + ObjPropName + "; ", item);
        //                }
        //                catch (InvalidOperationException ex)
        //                {
        //                    objectFound = false;
        //                    break;
        //                }
        //                if (elementPropVlu != null)
        //                {
        //                    if (elementPropVlu.ToString().ToLower().Trim() != ObjPropVlu.ToString().ToLower().Trim())
        //                    {
        //                        objectFound = false;
        //                        break;
        //                    }
        //                }                     

        //            }
        //            //if all the prop values of that object are satisfied then the match is found which 
        //            //means the element we are searching for is found
        //            if (objectFound)
        //            {
        //                webObj = item;
        //                break;
        //            }
        //        }
        //    }
        //    catch (NoSuchElementException e)
        //    {
        //        //throw;
        //        //log the exception
        //    }
        //    return webObj;
        //}

        ///// <summary>
        ///// Fetch the Parent object (not necessary the immediate parent of the child) of a child control on the screen,as described by the parentTagName  
        ///// </summary>
        ///// <param name="driver"></param>
        ///// <param name="childObj"></param>
        ///// <param name="parentTagName"></param>
        ///// <param name="SearchDepth"></param>
        ///// <returns></returns>
        //public static IWebElement GetParentOfChildElement(this IWebDriver driver, IWebElement childObj, string parentTagName,int SearchDepth=10)
        //{
        //    IWebElement parentObj = null;
        //    try
        //    {
        //        bool objFond = false;
        //        while ((!objFond) || (SearchDepth > 0))
        //        {
        //            var prObjTag = ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0].parentElement.tagName; ", childObj);

        //            if (string.Compare(prObjTag.ToString().Trim().ToLower(), parentTagName.Trim().ToLower()) == 0)
        //            {
        //                objFond = true;
        //                var prObj = ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0].parentElement; ", childObj);
        //                parentObj = (IWebElement)prObj;
        //                break;
        //            }
        //            else
        //            {
        //                childObj = (IWebElement)((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0].parentElement; ", childObj);
        //            }

        //            SearchDepth--;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return parentObj;
        //}

        //public static IWebElement GetGridObject(this IWebDriver driver, string kyTextValue,string gridType="data")
        //{
        //    IWebElement tbl = null;
        //    try
        //    {
        //        string[] propNames = new string[] {"innerText"};
        //        string[] propVlu = new string[] {kyTextValue.ToString()};
        //        IWebElement tblCellObj = null;
        //        if (gridType.Trim().ToLower() == "data")
        //        {
        //           tblCellObj = DriverContext.Driver.GetWebObject(propNames, propVlu, "TD");
        //        }
        //        else
        //        {
        //            tblCellObj = DriverContext.Driver.GetWebObject(propNames, propVlu, "TH");
        //        }
        //        if (tblCellObj == null)
        //        {
        //            return null;
        //            //Log error
        //        }

        //        //scroll dow if the cell is invisible
        //        if (!tblCellObj.Displayed)
        //        {
        //            //SCROLL : NOT YET IMPLIMENTED
        //        }
        //        tbl = DriverContext.Driver.GetParentOfChildElement(tblCellObj,"TABLE");                  
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return tbl;
        //}

        /* public static Dictionary<string, object> CaptureDOMPropertyOfChildObjectsToDict(IWebElement parentObj, string[] captureChildElements, string childPropertyToCapture, IWebDriver driver)
         {
             try
             {
                 //Init the childObjDict Dictionary only once
                 if (!hasChildren)
                 {
                     childObjDict = new Dictionary<string, object>();
                 }
                 hasChildren = true;
                 var count = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].childElementCount; ", parentObj);
                 for (int i = 0; i < Convert.ToInt32(count); i++)
                 {
                     var childTagName = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].children[" + i + "].tagName; ", parentObj);
                     var child = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].children[" + i + "]; ", parentObj);
                     var innerChildCount = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].childElementCount; ", (IWebElement)child);
                     for (int j = 0; j < captureChildElements.Length; j++)
                     {

                         //check to see if this is the child element you want to capture
                         if (captureChildElements[j].Trim().ToLower() == childTagName.ToString().Trim().ToLower())
                         {
                             //this logic is used to categorize the children inside a parent object as rows and columns
                             //every time i == 0 and j == 0 means that there is a new iteration through the new parent obje
                             if (i == 0 && j == 0)
                             {
                                 //Console.WriteLine();
                                 rowCounter = rowCounter + 1;
                                 //Console.WriteLine("Row : " + counter.ToString());
                             }
                             IWebElement childObj = (IWebElement)child;
                             //Console.WriteLine("Child Match Found at : [" + counter.ToString() + "," + i.ToString() + "]");
                             //Console.WriteLine("Tag Name : " + childTagName.ToString().Trim().ToUpper());                            
                             //Console.WriteLine("Text : "+ childObj.Text);                           
                             //Console.WriteLine();

                             //Write the Property of the child to the dictionary
                             string childObjPropertyKy = childTagName.ToString().Trim().ToUpper() + "[" + rowCounter.ToString() + ", " + i.ToString() + "]";
                             var childObjPropertyVlu = (object)null;
                             if (childPropertyToCapture.Trim().ToLower() == "object")
                             {
                                 childObjPropertyVlu = child;
                             }
                             else
                             {
                                 childObjPropertyVlu = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0]." + childPropertyToCapture + "; ", child);
                             }
                             //childObjDict.Add(childObjPropertyKy, childObjPropertyVlu);                          
                         }
                     }
                     if (Convert.ToInt32(innerChildCount) > 0)
                     {
                         childObjDict = CaptureDOMPropertyOfChildObjectsToDict((IWebElement)child, captureChildElements, childPropertyToCapture, driver);
                     }
                 }

                 IList<IWebElement> firstChildList = parentObj.FindElements(By.TagName("ul"));
             }
             catch (Exception)
             {

                 throw;
             }
             return childObjDict;
         }*/

        /// <summary>
        /// This function captures a specific DOM property of 
        /// all specified children inside a parent hierarchy by travrsing through the DOM structure.
        /// Note : Make sure the property specified should exist for all the children specified.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="parentObj"></param>
        /// <param name="captureChildElements"></param>
        /// <param name="childPropertyToCapture"></param>
        /// <returns></returns>             
        //public static Dictionary<string, object> CaptureDOMPropertyOfChildObjectsToDict(this IWebDriver driver,IWebElement parentObj, string[] captureChildElements, string childPropertyToCapture)
        //{
        //    try
        //    {
        //        //Init the childObjDict Dictionary only once
        //        if (!hasChildren)
        //        {
        //            childObjDict = new Dictionary<string, object>();
        //            directParentObj = parentObj;
        //        }
        //        hasChildren = true;
        //        var count = ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0].childElementCount; ", parentObj);
        //        for (int i = 0; i < Convert.ToInt32(count); i++)
        //        {
        //            var childTagName = ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0].children[" + i + "].tagName; ", parentObj);
        //            var child = ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0].children[" + i + "]; ", parentObj);
        //            var innerChildCount = ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0].childElementCount; ", (IWebElement)child);

        //            //assign the row counter
        //            if (parentObj.GetAttribute("outerHTML") == directParentObj.GetAttribute("outerHTML"))
        //            {
        //                rowCounter = rowCounter + 1;
        //                //Console.WriteLine();
        //                //Console.WriteLine("Row : " + rowCounter.ToString());
        //                colCounter = 0;
        //            }                   
        //            for (int j = 0; j < captureChildElements.Length; j++)
        //            {
        //                //check to see if this is the child element you want to capture
        //                if (captureChildElements[j].Trim().ToLower() == childTagName.ToString().Trim().ToLower())
        //                {
        //                    //assign the column counter
        //                    if (parentObj.GetAttribute("outerHTML") != directParentObj.GetAttribute("outerHTML"))
        //                    {
        //                        colCounter = colCounter + 1;
        //                        //Console.WriteLine("Col : [" + rowCounter.ToString() + " , " + colCounter.ToString() + "]");
        //                    }
        //                        IWebElement childObj = (IWebElement)child;                           
        //                    //Write the Property of the child to the dictionary
        //                    string childObjPropertyKy = childTagName.ToString().Trim().ToUpper() + "[" + rowCounter.ToString() + ", " + colCounter.ToString() + "]";
        //                    var childObjPropertyVlu = (object)null;
        //                    if (childPropertyToCapture.Trim().ToLower() == "object")
        //                    {
        //                        childObjPropertyVlu = child;
        //                    }
        //                    else
        //                    {
        //                        childObjPropertyVlu = ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0]." + childPropertyToCapture + "; ", child);
        //                    }
        //                    childObjDict.Add(childObjPropertyKy, childObjPropertyVlu);
        //                }
        //            }
        //            if (Convert.ToInt32(innerChildCount) > 0)
        //            {
        //                childObjDict = DriverContext.Driver.CaptureDOMPropertyOfChildObjectsToDict((IWebElement)child, captureChildElements, childPropertyToCapture);
        //            }
        //        }              
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return childObjDict;
        //}

        /// <summary>
        /// match the dom properties of the Obj with the property values specified inside the dictionary as ky,vlu pairs
        /// if any of the object property matches it returns true
        /// Note : Make sure you always provide unique DOM properties of the object inside the dictionary as input to get the best unique match
        /// otherwise there can be flase matches of objects which you might not need.
        /// </summary>
        /// <param name="Obj"></param>
        /// <param name="matchPropertiesDict"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        //public static bool MatchObjectProperty(IWebElement Obj, Dictionary<object, object>matchPropertiesDict, IWebDriver driver)
        //{
        //    bool objectMatched = false;
        //    try
        //    {
        //        foreach (var item in matchPropertiesDict)
        //        {
        //            string propValues = item.Value.ToString();
        //            string propName = item.Key.ToString();
        //            var objPropValue = (object)null;
        //            try
        //            {
        //                //check to see if the obj property values matches with any of the propValues
        //                objPropValue = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0]." + propName + "; ", Obj);                                        
        //            }
        //            catch (Exception)
        //            {

        //            }
        //            ///if any of the propValues matches with the objPropValue                     
        //            /// then match is found return true and exit the loop
        //            if (objPropValue != null)
        //            {
        //                //Console.WriteLine("Object Prop Name" + propName + " , Object Prop Value : " + objPropValue.ToString().Trim());
        //                if (propValues.Contains(","))
        //                {
        //                    if (propValues.Contains(objPropValue.ToString().Trim()))
        //                    {
        //                        objectMatched = true;
        //                        break;
        //                    }
        //                }
        //                else
        //                {
        //                    if (string.Compare(propValues.Trim(), objPropValue.ToString().Trim()) == 0)
        //                    {
        //                        objectMatched = true;
        //                        break;
        //                    }
        //                }

        //            }                    
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return objectMatched;
        //}

        //public static Dictionary<string, object> CaptureDOMPropertyOfChildObjectsToDict_Demo(IWebElement parentObj, List<string> captureChildElements, Dictionary<object, object> allowedChildProperties, string childPropertyToCapture, IWebDriver driver)
        //{
        //    try
        //    {
        //        //Init the childObjDict Dictionary only once
        //        if (!hasChildren)
        //        {
        //            childObjDict = new Dictionary<string, object>();
        //            directParentObj = parentObj;
        //        }
        //        hasChildren = true;
        //        var count = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].childElementCount; ", parentObj);
        //        for (int i = 0; i < Convert.ToInt32(count); i++)
        //        {
        //            var childTagName = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].children[" + i + "].tagName; ", parentObj);
        //            var child = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].children[" + i + "]; ", parentObj);
        //            var innerChildCount = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].childElementCount; ", (IWebElement)child);

        //            //assign the row counter
        //            if (parentObj.GetAttribute("outerHTML") == directParentObj.GetAttribute("outerHTML"))
        //            {
        //                rowCounter = rowCounter + 1;
        //                //Console.WriteLine();
        //                //Console.WriteLine("Row : " + rowCounter.ToString());
        //                colCounter = 0;
        //            }
        //            for (int j = 0; j < captureChildElements.Count; j++)
        //            {
        //                //check to see if this is the child element you want to capture
        //                if (captureChildElements[j].Trim().ToLower() == childTagName.ToString().Trim().ToLower())
        //                {
        //                    IWebElement childObj = (IWebElement)child;
        //                    ///Restrict the capturing of children by
        //                    ///checking to see if the child elements property(s) matches 
        //                    ///to the properties in the list captureChildElementsRestrictedProperties
        //                    if (allowedChildProperties != null )
        //                    {
        //                        if (!MatchObjectProperty(childObj, allowedChildProperties, driver))
        //                        {
        //                            continue;
        //                        }
        //                    }                           
        //                    //assign the column counter
        //                    if (parentObj.GetAttribute("outerHTML") != directParentObj.GetAttribute("outerHTML"))
        //                    {
        //                        colCounter = colCounter + 1;
        //                        //Console.WriteLine("Col : [" + rowCounter.ToString() + " , " + colCounter.ToString() + "]");
        //                    }                            
        //                    //Write the Property of the child to the dictionary
        //                    string childObjPropertyKy = childTagName.ToString().Trim().ToUpper() + "[" + rowCounter.ToString() + ", " + colCounter.ToString() + "]";
        //                    var childObjPropertyVlu = (object)null;
        //                    if (childPropertyToCapture.Trim().ToLower() == "object")
        //                    {
        //                        childObjPropertyVlu = child;
        //                    }
        //                    else
        //                    {
        //                        childObjPropertyVlu = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0]." + childPropertyToCapture + "; ", child);
        //                    }

        //                    childObjDict.Add(childObjPropertyKy, childObjPropertyVlu);
        //                }
        //            }
        //            if (Convert.ToInt32(innerChildCount) > 0)
        //            {
        //                childObjDict = CaptureDOMPropertyOfChildObjectsToDict_Demo((IWebElement)child, captureChildElements, allowedChildProperties, childPropertyToCapture, driver);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return childObjDict;
        //}


        //public static bool SelectItemFromListControlLookup(this IWebDriver driver, IWebElement lookupObject, string[] kySelectFieldValues)
        //{
        //    bool flag = false;
        //    Dictionary<string, object> childObjdict;
        //    try
        //    {
        //        childObjdict = new Dictionary<string, object>();

        //        ///iterate through the list controls (LI) in the lookupObject UL
        //        ///Note : Here the assumption is that the dictionary which is returned will have all the 
        //        ///elements starting with ky[1,..] will be header control                
        //        childObjdict = DriverContext.Driver.CaptureDOMPropertyOfChildObjectsToDict(lookupObject, new string[] { "LI" }, "object");

        //        ///now iterate through the childObjdict and make sure you find a match for kyFieldNames,kyFieldValues
        //        ///Calculte the list column index based on kyFieldNames
        //        ///Calculte the list row index based on  kyFieldValues
        //        hasChildren = false;
        //        rowCounter = 0;
        //        colCounter = 0;
        //        directParentObj = null;
        //        List<IWebElement> matchedLstObj = WebElementExtensions.GetMatchedObjectListFromDictionary(childObjdict, kySelectFieldValues);

        //        //select  the list item
        //        if (matchedLstObj.Count > 0)
        //        {
        //            IWebElement pr = matchedLstObj[0].FindElement(By.XPath(".."));
        //            Actions actions = new Actions(driver);
        //            actions.MoveToElement(matchedLstObj[0]).Perform();
        //            actions.Click(matchedLstObj[0]).Perform();
        //            //actions.SendKeys(Keys.Tab);
        //            //actions.MoveToElement(pr).Perform();
        //            //actions.Click(pr).Perform();                                      
        //            flag = true;
        //        }
        //        else
        //        {
        //            //log error in the Error log file
        //            Console.WriteLine("Error : Unable to find the list elements and select them from the lookup");
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return flag;
        //}

        public static List<IWebElement> GetMatchedObjectListFromDictionary(Dictionary<string, object> listDict, string[] kyFieldValues)
        {           
            List<IWebElement> matchedListObj = new List<IWebElement>();
            string matchedRowIndex = "-1";
            try
            {
                /*foreach (var listItem in listDict)
                {                   
                    IWebElement listObj = (IWebElement)listItem.Value;
                    string listText = listObj.Text.Trim().ToLower();
                    //match the list text with the kyFieldValues 
                    for (int i = 0; i < kyFieldValues.Length; i++)
                    {
                        if (listItem.Key.ToString() == "LI[11, 2]")
                        {
                            Console.WriteLine("hello");
                        }
                        string kyVlu = kyFieldValues[i].Trim().ToLower();
                        if (string.Compare(kyVlu, listText) == 0)
                        {
                            matchedListObj.Add(listObj);
                            break;
                        }
                        else
                        {
                            matchedListObj.Clear();
                        }                                              
                    }
                    if (matchedListObj.Count == kyFieldValues.Length)
                        return matchedListObj;
                }*/

                for (int i = 0; i < kyFieldValues.Length; i++)
                {
                    string kyVlu = kyFieldValues[i].Trim().ToLower();
                    foreach (var listItem in listDict)
                    {                                               
                        IWebElement listObj = (IWebElement)listItem.Value;
                        string rowIndex = listItem.Key.Substring(listItem.Key.IndexOf("[") + 1, listItem.Key.IndexOf(",") - listItem.Key.IndexOf("[") - 1);
                        string listText = listObj.Text.Trim().ToLower();
                        //match the list text with the kyFieldValues 
                        if (string.Compare(kyVlu, listText) == 0)
                        {
                            matchedRowIndex = listItem.Key.Substring(listItem.Key.IndexOf("[") + 1, listItem.Key.IndexOf(",") - listItem.Key.IndexOf("[") - 1);
                            Console.WriteLine(matchedRowIndex);
                            matchedListObj.Add(listObj);
                            break;                                                        
                        }
                        else //clear the previous added values that matched
                        {
                            if (Int32.Parse(matchedRowIndex) != Int32.Parse(rowIndex))
                            {
                                matchedListObj.Clear();
                            }
                        }
                    }
                }

                if (matchedListObj.Count != kyFieldValues.Length)
                    matchedListObj.Clear();
                return matchedListObj;
            }
            catch (Exception ex)
            {
                matchedListObj.Clear();
                return matchedListObj;
            }
            ///check to see if all the kyFieldValues have been matched if not then empty the matchedListObj
            /*if (kyFieldValues.Length != matchedListObj.Count)
            {
                matchedListObj.Clear();
            }

            return matchedListObj;*/
        }

        public static int GetListColumnIndex(Dictionary<string, object> listDict, string kyColumnName)
        {
            int colIndx = -1;            
            try
            {
                foreach (var listItem in listDict)
                {
                    IWebElement listObj = (IWebElement)listItem.Value;
                    string listText = listObj.Text.Trim().ToLower();

                    string columnName = kyColumnName.Trim().ToLower();
                    //check for the column name match
                    if (string.Compare(listText, columnName) == 0)
                    {
                        string ky = listItem.Key.Trim();
                        //extract the colindex from the key
                        colIndx = Convert.ToInt32(ky.Substring(ky.IndexOf(",") + 1, ky.IndexOf("]") - ky.IndexOf(",") - 1).Trim());
                    }
                    if (colIndx > -1)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                //if colIndx == -1 then log an error message inside the log
                throw;
            }
            return colIndx;
        } 

        //This method is still under construction
        public static DataTable CaptureDOMPropertyOfChildObjectsToDataTbl(IWebDriver driver, IWebElement parentObj, string[] captureChildElements, string childPropertyToCapture)
        {            
            try
            {
                //Init the childObjDict Dictionary only once
                if (!hasChildren)
                {
                    childObjDataTbl = new DataTable();
                }
                hasChildren = true;
                var count = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].childElementCount; ", parentObj);
                for (int i = 0; i < Convert.ToInt32(count); i++)
                {
                    if (dtRow == null)
                    {
                        dtRow = childObjDataTbl.NewRow();
                    }               
                    var childTagName = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].children[" + i + "].tagName; ", parentObj);
                    var child = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].children[" + i + "]; ", parentObj);
                    var innerChildCount = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].childElementCount; ", (IWebElement)child);
                    for (int j = 0; j < captureChildElements.Length; j++)
                    {

                        //check to see if this is the child element you want to capture
                        if (captureChildElements[j].Trim().ToLower() == childTagName.ToString().Trim().ToLower())
                        {
                            //this logic is used to categorize the children inside a parent object as rows and columns
                            //every time i == 0 and j == 0 means that there is a new iteration through the new parent obje
                            if (i == 0 && j == 0)
                            {
                                //dtRow = childObjDataTbl.NewRow();
                                rowCounter = rowCounter + 1;
                                incrementRowCounter = true;                              
                                
                            }
                            IWebElement childObj = (IWebElement)child;                            
                            //Write the Property of the child to the dictionary
                            var childObjPropertyVlu = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0]." + childPropertyToCapture + "; ", child);
                            string childObjPropertyKy = childTagName.ToString().Trim().ToUpper() + "[" + rowCounter.ToString() + ", " + i.ToString() + "]";
                            childObjDataTbl.Columns.Add(childObjPropertyKy, typeof(object));
                            dtRow[childObjPropertyKy] = childObjPropertyVlu;                            
                        }                       
                    }
                    if (Convert.ToInt32(innerChildCount) > 0)
                    {
                        childObjDataTbl = CaptureDOMPropertyOfChildObjectsToDataTbl(driver, (IWebElement)child, captureChildElements, childPropertyToCapture);
                    }                   
                    if ((i == Convert.ToInt32(count)-1) && (dtRow != null) && incrementRowCounter) 
                    {
                        childObjDataTbl.Rows.Add(dtRow);
                        dtRow = null;
                        incrementRowCounter = false;
                    }                  
                }

                //IList<IWebElement> firstChildList =  parentObj.FindElements(By.TagName("ul"));
            }
            catch (Exception)
            {

                throw;
            }
            return childObjDataTbl;
        }

      
        /// <summary>
        /// Select an item from the dropdown control
        /// </summary>
        /// <param name="dropDownObj"></param>
        /// <param name="dropDownItem"></param>
        public static void SelectFromDropDown(this IWebDriver driver, IWebElement dropDownObj, string dropDownItem)
        {
            try
            {
                SelectElement selectItem = new SelectElement(dropDownObj);
                foreach (IWebElement selectOption in selectItem.Options)
                {
                    if (selectOption.Text.ToString().ToLower().Trim() == dropDownItem.ToString().ToLower().Trim())
                    {
                        selectOption.Click();
                        break;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// compares the Property of two objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="to"></param>
        /// <param name="ignore"></param>
        /// <returns></returns>
        public static bool PublicInstancePropertyEqual<T>(T self, T to, string propName) where T : class
        {
            if (self != null && to != null)
            {
                var expType = self.GetType();
                var actType = to.GetType();
                var selfValue = expType.GetProperty(propName).GetValue(self, null);
                var toValue = actType.GetProperty(propName).GetValue(to, null);

                if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                {
                    return false;
                }
            }

            return self == to;
        }

        /// <summary>Compare the public instance properties. Uses deep comparison.</summary>
        /// <param name="self">The reference object.</param>
        /// <param name="to">The object to compare.</param>
        /// <param name="ignore">Ignore property with name.</param>
        /// <typeparam name="T">Type of objects.</typeparam>
        /// <returns><see cref="bool">True</see> if both objects are equal, else <see cref="bool">false</see>.</returns>
        public static bool PublicInstancePropertiesEqual<T>(T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                var type = self.GetType();
                var ignoreList = new List<string>(ignore);
                foreach (var pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (ignoreList.Contains(pi.Name))
                    {
                        continue;
                    }

                    var selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                    var toValue = type.GetProperty(pi.Name).GetValue(to, null);

                    if (pi.PropertyType.IsClass && !pi.PropertyType.Module.ScopeName.Equals("CommonLanguageRuntimeLibrary"))
                    {
                        // Check of "CommonLanguageRuntimeLibrary" is needed because string is also a class
                        if (PublicInstancePropertiesEqual(selfValue, toValue, ignore))
                        {
                            continue;
                        }

                        return false;
                    }

                    if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                    {
                        return false;
                    }
                }

                return true;
            }

            return self == to;
        }

    }
}
