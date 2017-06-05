

using AutomationFramework.Base;
using AutomationFramework.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace AutomationFramework.Extensions
{
    public static class WebDriverExtensions
    {
        public static bool hasChildren = false;
        private static IWebElement directParentObj = null;
        private static Dictionary<string, object> childObjDict = null;                   
        private static int rowCounter = 0;
        private static int colCounter = 0;
       
        public static DataTable childObjDataTbl = null;
        public static DataRow dtRow = null;        
        public static bool incrementRowCounter = false;

        /// <summary>
        /// Executes javascript 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="strJavaScript"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object ExecuteJavaScript(this IWebDriver driver,string strJavaScript, params object[] args)
        {            
            try
            {
                if (args != null)
                {
                    return ((IJavaScriptExecutor)driver).ExecuteScript(strJavaScript, args);
                }
                else
                {
                    return ((IJavaScriptExecutor)driver).ExecuteScript(strJavaScript);
                }
               
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteTextToLog("Error : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Waits until the elements is found and returns the element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <param name="displayed"></param>
        /// <returns></returns>
        public static IWebElement FindElement(this IWebDriver driver, By by, uint timeoutInSeconds, bool displayed = false)
        {
            IWebElement element = null;
            var wait = new WebDriverWait(DriverContext.Driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(WebDriverTimeoutException), typeof(InvalidOperationException));
            try
            {
                element = wait.Until(drv =>
                {
                    var elem = drv.FindElement(by);
                    if (displayed && !elem.Displayed)
                        return null;
                    return elem;
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteTextToLog("WaitObjectExists Failed : " + ex.Message);                
            }
            return element;
        }

        /// <summary>
        ///Searches for multiple browser objects in a specified time limit and returns a collection of objects on the browser
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => (drv.FindElements(by).Count > 0) ? drv.FindElements(by) : null);
            }
            return driver.FindElements(by);
        }

        /// <summary>
        /// This is a dynamic delay function which waits for the element on the screen to be loaded and 
        /// displayed on the screen with in a specific time frame. If the element is found with on that 
        /// timeframe returns true else returns false.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <param name="displayed"></param>
        /// <returns></returns>
        public static bool WaitObjectExists(this IWebDriver driver, By by, uint timeoutInSeconds, bool displayed = true)
        {
            bool flag = true;            
            IWebElement element = driver.FindElement(by, timeoutInSeconds, displayed);          
            if (element == null)
            {
                flag = false;
            }
            return flag;
        }

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
        public static IWebElement GetWebObject(this IWebDriver driver, string[] objPropNames, string[] objPropValues, string elementTagName)
        {
            string ObjPropName;
            string ObjPropVlu;
            IWebElement webObj = null;
            //string elementPropVlu;

            //string propName, propVlu;
            try
            {
                IList<IWebElement> lstElement = DriverContext.Driver.FindElements(By.TagName(elementTagName));
                foreach (var item in lstElement)
                {
                    //reset the flag to true for every iteration
                    bool objectFound = false;

                    for (int i = 0; i < objPropNames.Length; i++)
                    {
                        ObjPropName = objPropNames[i];
                        ObjPropVlu = objPropValues[i];
                        //get the value of the attribute
                        //elementPropVlu = item.GetAttribute(ObjPropName.ToString());
                        var elementPropVlu = (Object)null;
                        try
                        {                            
                            elementPropVlu = driver.ExecuteJavaScript("return arguments[0]." + ObjPropName + "; ", item);
                        }
                        catch (InvalidOperationException)
                        {
                            objectFound = false;
                            break;
                        }
                        if (elementPropVlu != null)
                        {
                            if (elementPropVlu.ToString().ToLower().Trim() == ObjPropVlu.ToString().ToLower().Trim())
                            {
                                objectFound = true;                                
                            }
                            else
                            {
                                objectFound = false;
                                break;
                            }
                        }
                        else
                        {
                            objectFound = false;
                            break;
                        }

                    }
                    //if all the prop values of that object are satisfied then the match is found which 
                    //means the element we are searching for is found
                    if (objectFound)
                    {
                        webObj = item;
                        break;
                    }
                }
            }
            catch (NoSuchElementException ex)
            {
                //log the exception
                LogHelper.WriteTextToLog("Error : " + ex.Message);                               
            }
            return webObj;
        }

        /// <summary>
        /// Fetch the Parent object (not necessary the immediate parent of the child) of a child control on the screen,as described by the parentTagName  
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="childObj"></param>
        /// <param name="parentTagName"></param>
        /// <param name="SearchDepth"></param>
        /// <returns></returns>
        public static IWebElement GetParentOfChildElement(this IWebDriver driver, IWebElement childObj, string parentTagName, int SearchDepth = 10)
        {
            IWebElement parentObj = null;
            try
            {
                bool objFond = false;
                while ((!objFond) || (SearchDepth > 0))
                {                    
                    var prObjTag = driver.ExecuteJavaScript("return arguments[0].parentElement.tagName; ", childObj);

                    if (string.Compare(prObjTag.ToString().Trim().ToLower(), parentTagName.Trim().ToLower()) == 0)
                    {
                        objFond = true;                        
                        var prObj = driver.ExecuteJavaScript("return arguments[0].parentElement; ", childObj);
                        parentObj = (IWebElement)prObj;
                        break;
                    }
                    else
                    {                        
                        childObj = (IWebElement)(driver.ExecuteJavaScript("return arguments[0].parentElement; ", childObj));
                    }

                    SearchDepth--;
                }
                return parentObj;
            }
            catch (Exception ex)
            {
                LogHelper.CreateLogFile("Error : " + ex.Message);
                return null;
            }
            
        }

        public static IWebElement GetGridObject(this IWebDriver driver, string kyTextValue, string gridType = "data")
        {
            IWebElement tbl = null;
            try
            {
                string[] propNames = new string[] { "innerText" };
                string[] propVlu = new string[] { kyTextValue.ToString() };
                IWebElement tblCellObj = null;
                if (gridType.Trim().ToLower() == "data")
                {
                    tblCellObj = driver.GetWebObject(propNames, propVlu, "TD");
                }
                else
                {
                    tblCellObj = driver.GetWebObject(propNames, propVlu, "TH");
                }
                if (tblCellObj == null)
                {
                    return null;
                    //Log error
                }

                //scroll dow if the cell is invisible
                if (!tblCellObj.Displayed)
                {
                    //SCROLL : NOT YET IMPLIMENTED
                }
                tbl = driver.GetParentOfChildElement(tblCellObj, "TABLE");
            }
            catch (Exception ex)
            {

                throw;
            }
            return tbl;
        }

        public static bool MatchObjectProperty(this IWebDriver driver,IWebElement Obj, Dictionary<object, object> matchPropertiesDict)
        {
            bool objectMatched = false;
            try
            {
                foreach (var item in matchPropertiesDict)
                {
                    string propValues = item.Value.ToString();
                    string propName = item.Key.ToString();
                    var objPropValue = (object)null;
                    try
                    {
                        //check to see if the obj property values matches with any of the propValues
                        objPropValue = driver.ExecuteJavaScript("return arguments[0]." + propName + "; ", Obj);
                    }
                    catch (Exception)
                    {

                    }
                    ///if any of the propValues matches with the objPropValue                     
                    /// then match is found return true and exit the loop
                    if (objPropValue != null)
                    {
                        //Console.WriteLine("Object Prop Name" + propName + " , Object Prop Value : " + objPropValue.ToString().Trim());
                        if (propValues.Contains(","))
                        {
                            if (propValues.Contains(objPropValue.ToString().Trim()))
                            {
                                objectMatched = true;
                                break;
                            }
                        }
                        else
                        {
                            if (string.Compare(propValues.Trim(), objPropValue.ToString().Trim()) == 0)
                            {
                                objectMatched = true;
                                break;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return objectMatched;
        }


        public static DataTable CaptureDOMPropertyOfChildObjectsToDataTbl(this IWebDriver driver, IWebElement parentObj, List<string> captureChildElements, string childPropertyToCapture, Dictionary<object, object> allowedChildProperties = null)
        {
            try
            {
                /*if (parentObj == null)
                {
                    return null;
                }*/
                //Init the childObjDict Dictionary only once
                if (!hasChildren)
                {                   
                    childObjDataTbl = new DataTable();
                    directParentObj = parentObj;
                }
                hasChildren = true;
                var count = driver.ExecuteJavaScript("return arguments[0].childElementCount; ", parentObj);
                for (int i = 0; i < Convert.ToInt32(count); i++)
                {
                    var childTagName = driver.ExecuteJavaScript("return arguments[0].children[" + i + "].tagName; ", parentObj);
                    var child = driver.ExecuteJavaScript("return arguments[0].children[" + i + "]; ", parentObj);
                    var innerChildCount = driver.ExecuteJavaScript("return arguments[0].childElementCount; ", (IWebElement)child);

                    //assign the row counter
                    if (parentObj.GetAttribute("outerHTML") == directParentObj.GetAttribute("outerHTML"))
                    {
                        rowCounter = rowCounter + 1;                        
                        colCounter = 0;
                        dtRow = childObjDataTbl.NewRow();
                        incrementRowCounter = true;
                    }
                    for (int j = 0; j < captureChildElements.Count; j++)
                    {
                        //check to see if this is the child element you want to capture
                        if (captureChildElements[j].Trim().ToLower() == childTagName.ToString().Trim().ToLower())
                        {
                            IWebElement childObj = (IWebElement)child;
                            ///Restrict the capturing of children by
                            ///checking to see if the child elements property(s) matches 
                            ///to the properties in the list captureChildElementsRestrictedProperties
                            if (allowedChildProperties != null)
                            {
                                if (!driver.MatchObjectProperty(childObj, allowedChildProperties))
                                {
                                    continue;
                                }
                            }
                            //assign the column counter
                            if (parentObj.GetAttribute("outerHTML") != directParentObj.GetAttribute("outerHTML"))
                                colCounter = colCounter + 1;

                            //only add columns names once
                            if (rowCounter == 1)
                                childObjDataTbl.Columns.Add("Col" + colCounter.ToString(), typeof(object));

                            //capture the property value into the data column
                            string childObjPropertyKy = childTagName.ToString().Trim().ToUpper() + "[" + rowCounter.ToString() + ", " + colCounter.ToString() + "]";
                            var childObjPropertyVlu = (object)null;
                            if (childPropertyToCapture.Trim().ToLower() == "object")
                                childObjPropertyVlu = child;
                            else
                                childObjPropertyVlu = driver.ExecuteJavaScript("return arguments[0]." + childPropertyToCapture + "; ", child);

                            dtRow["Col" + colCounter.ToString()] = childObjPropertyVlu;                           
                        }
                    }
                    if (Convert.ToInt32(innerChildCount) > 0)
                    {
                        childObjDataTbl = driver.CaptureDOMPropertyOfChildObjectsToDataTbl((IWebElement)child, captureChildElements, childPropertyToCapture, allowedChildProperties);
                    }
                    if (incrementRowCounter)
                    {
                        childObjDataTbl.Rows.Add(dtRow);                        
                        incrementRowCounter = false;
                    }                    
                }
                return childObjDataTbl;
            }
            catch (Exception ex)
            {
                LogHelper.WriteTextToLog("Error : " + ex.Message);
                return null;

            }
        }

        public static Dictionary<string, object> CaptureDOMPropertyOfChildObjectsToDict(this IWebDriver driver, IWebElement parentObj, List<string> captureChildElements, string childPropertyToCapture, Dictionary<object, object> allowedChildProperties = null)
        {
            try
            {
                if (parentObj == null)
                {
                    return null;
                }
                //Init the childObjDict Dictionary only once
                if (!hasChildren)
                {
                    childObjDict = new Dictionary<string, object>();
                    directParentObj = parentObj;
                }
                hasChildren = true;
                var count = driver.ExecuteJavaScript("return arguments[0].childElementCount; ", parentObj);
                for (int i = 0; i < Convert.ToInt32(count); i++)
                {
                    var childTagName = driver.ExecuteJavaScript("return arguments[0].children[" + i + "].tagName; ", parentObj);
                    var child = driver.ExecuteJavaScript("return arguments[0].children[" + i + "]; ", parentObj);
                    var innerChildCount = driver.ExecuteJavaScript("return arguments[0].childElementCount; ", (IWebElement)child);

                    //assign the row counter
                    if (parentObj.GetAttribute("outerHTML") == directParentObj.GetAttribute("outerHTML"))
                    {
                        rowCounter = rowCounter + 1;
                        //Console.WriteLine();
                        //Console.WriteLine("Row : " + rowCounter.ToString());
                        colCounter = 0;
                    }
                    for (int j = 0; j < captureChildElements.Count; j++)
                    {
                        //check to see if this is the child element you want to capture
                        if (captureChildElements[j].Trim().ToLower() == childTagName.ToString().Trim().ToLower())
                        {
                            IWebElement childObj = (IWebElement)child;
                            ///Restrict the capturing of children by
                            ///checking to see if the child elements property(s) matches 
                            ///to the properties in the list captureChildElementsRestrictedProperties
                            if (allowedChildProperties != null)
                            {
                                if (!driver.MatchObjectProperty(childObj, allowedChildProperties))
                                {
                                    continue;
                                }
                            }
                            //assign the column counter
                            if (parentObj.GetAttribute("outerHTML") != directParentObj.GetAttribute("outerHTML"))
                            {
                                colCounter = colCounter + 1;
                                //Console.WriteLine("Col : [" + rowCounter.ToString() + " , " + colCounter.ToString() + "]");
                            }
                            //Write the Property of the child to the dictionary
                            string childObjPropertyKy = childTagName.ToString().Trim().ToUpper() + "[" + rowCounter.ToString() + ", " + colCounter.ToString() + "]";
                            var childObjPropertyVlu = (object)null;
                            if (childPropertyToCapture.Trim().ToLower() == "object")
                            {
                                childObjPropertyVlu = child;
                            }
                            else
                            {
                                childObjPropertyVlu = driver.ExecuteJavaScript("return arguments[0]." + childPropertyToCapture + "; ", child);
                            }

                            childObjDict.Add(childObjPropertyKy, childObjPropertyVlu);
                        }
                    }
                    if (Convert.ToInt32(innerChildCount) > 0)
                    {
                        childObjDict = driver.CaptureDOMPropertyOfChildObjectsToDict((IWebElement)child, captureChildElements, childPropertyToCapture, allowedChildProperties);
                    }
                }
                return childObjDict;
            }
            catch (Exception ex)
            {
                LogHelper.WriteTextToLog("Error : " + ex.Message);
                return null;
               
            }            
        }

        public static bool SelectItemFromListControlLookup(this IWebDriver driver, IWebElement lookupObject, List<string> kySelectFieldValues)
        {
            bool flag = false;
            Dictionary<string, object> childObjdict;
            DataTable dtChildObjects;
            List<string> captureChildElements;
            try
            {
                //childObjdict = new Dictionary<string, object>();
                dtChildObjects = new DataTable();

                ///iterate through the list controls (LI) in the lookupObject UL
                ///Note : Here the assumption is that the dictionary which is returned will have all the 
                ///elements starting with ky[1,..] will be header control
                captureChildElements = new List<string>();
                captureChildElements.Add("LI");
                //childObjdict = DriverContext.Driver.CaptureDOMPropertyOfChildObjectsToDict(lookupObject, captureChildElements,"object");
                dtChildObjects = DriverContext.Driver.CaptureDOMPropertyOfChildObjectsToDataTbl(lookupObject, captureChildElements, "object");

                ///now iterate through the childObjdict and make sure you find a match for kyFieldNames,kyFieldValues
                ///Calculte the list column index based on kyFieldNames
                ///Calculte the list row index based on  kyFieldValues
                hasChildren = false;
                rowCounter = 0;
                colCounter = 0;
                directParentObj = null;
                //List<IWebElement> matchedLstObj = WebElementExtensions.GetMatchedObjectListFromDictionary(childObjdict, kySelectFieldValues);
                List<IWebElement> matchedLstObj = dtChildObjects.MatchWebElementText(kySelectFieldValues);

                //select  the list item
                if (matchedLstObj.Count > 0)
                {
                    //Scroll the element if its not vivible on the browser
                    /*if (!matchedLstObj[0].Displayed)
                    {
                        Console.WriteLine("Scroll Please ....");
                        DriverContext.Driver.ExecuteJavaScript("arguments[0].scrollIntoView();", matchedLstObj[0].Displayed);
                    }*/

                    //bool isElementVisible = (bool)DriverContext.Driver.ExecuteJavaScript("arguments[0].is(\":visible\");", matchedLstObj[0]);
                    //var eleVisibleStatus = (bool)DriverContext.Driver.ExecuteJavaScript("arguments[0].style[\"visibility\"];", matchedLstObj[0]);
                    //bool isElementVisible = (bool)DriverContext.Driver.ExecuteJavaScript("arguments[0].style[\"visibility\"] = \"visible\";" , matchedLstObj[0]);
                    /*if ((!isElementVisible) && (!matchedLstObj[0].Displayed))
                    {
                        Console.WriteLine("Scroll Please ....");
                        DriverContext.Driver.ExecuteJavaScript("arguments[0].scrollIntoView();", matchedLstObj[0].Displayed);
                    }*/

                    DriverContext.Driver.ExecuteJavaScript("arguments[0].scrollIntoView();", matchedLstObj[0].Displayed);

                    IWebElement pr = matchedLstObj[0].FindElement(By.XPath(".."));
                    Actions actions = new Actions(driver);
                    actions.MoveToElement(matchedLstObj[0]).Perform();
                    actions.Click(matchedLstObj[0]).Perform();
                    //actions.SendKeys(Keys.Tab);
                    //actions.MoveToElement(pr).Perform();
                    //actions.Click(pr).Perform();                                      
                    flag = true;
                }
                else
                {
                    //log error in the Error log file
                    LogHelper.WriteTextToLog("Error : Unable to find the list elements and select them from the lookup");
                }
                return flag;
            }
            catch (Exception EX)
            {
                LogHelper.WriteTextToLog("Error : " + EX.Message);
                return false;
            }
            
        }

        public static IWebElement GetListItemObjectFromUnorderedList(this IWebDriver driver, IWebElement ulObject, List<string> kySelectFieldValues)
        {                        
            DataTable dtChildObjects;
            List<string> captureChildElements;
            try
            {
                //childObjdict = new Dictionary<string, object>();
                dtChildObjects = new DataTable();

                ///iterate through the list controls (LI) in the lookupObject UL
                ///Note : Here the assumption is that the dictionary which is returned will have all the 
                ///elements starting with ky[1,..] will be header control
                captureChildElements = new List<string>();
                captureChildElements.Add("LI");
                //childObjdict = DriverContext.Driver.CaptureDOMPropertyOfChildObjectsToDict(lookupObject, captureChildElements,"object");
                dtChildObjects = DriverContext.Driver.CaptureDOMPropertyOfChildObjectsToDataTbl(ulObject, captureChildElements, "object");

                ///now iterate through the childObjdict and make sure you find a match for kyFieldNames,kyFieldValues
                ///Calculte the list column index based on kyFieldNames
                ///Calculte the list row index based on  kyFieldValues
                hasChildren = false;
                rowCounter = 0;
                colCounter = 0;
                directParentObj = null;
                //List<IWebElement> matchedLstObj = WebElementExtensions.GetMatchedObjectListFromDictionary(childObjdict, kySelectFieldValues);
                List<IWebElement> matchedLstObj = dtChildObjects.MatchWebElementText(kySelectFieldValues);

                //select  the list item
                if (matchedLstObj.Count <= 0)
                {
                    //Log error saying that unable to find the matched list item on UL
                    return null;
                }
                return matchedLstObj[0];
            }
            catch (Exception EX)
            {                
                LogHelper.WriteTextToLog("Error : " + EX.Message);
                return null;
            }           
        }

        public static int GetListItemIndexFromUnorderedList(this IWebDriver driver, IWebElement ulObject, List<string> kySelectFieldValues)
        {
            DataTable dtChildObjects;
            List<string> captureChildElements;
            try
            {
                //childObjdict = new Dictionary<string, object>();
                dtChildObjects = new DataTable();

                ///iterate through the list controls (LI) in the lookupObject UL
                ///Note : Here the assumption is that the dictionary which is returned will have all the 
                ///elements starting with ky[1,..] will be header control
                captureChildElements = new List<string>();
                captureChildElements.Add("LI");
                //childObjdict = DriverContext.Driver.CaptureDOMPropertyOfChildObjectsToDict(lookupObject, captureChildElements,"object");
                dtChildObjects = DriverContext.Driver.CaptureDOMPropertyOfChildObjectsToDataTbl(ulObject, captureChildElements, "object");

                ///now iterate through the childObjdict and make sure you find a match for kyFieldNames,kyFieldValues
                ///Calculte the list column index based on kyFieldNames
                ///Calculte the list row index based on  kyFieldValues
                hasChildren = false;
                rowCounter = 0;
                colCounter = 0;
                directParentObj = null;
                //List<IWebElement> matchedLstObj = WebElementExtensions.GetMatchedObjectListFromDictionary(childObjdict, kySelectFieldValues);
               int lstIndex = dtChildObjects.GetIndexOfMatchWebElementText(kySelectFieldValues);

                //select  the list item
                if (lstIndex  < 0)
                {
                    //Log error saying that unable to find the matched list item on UL
                    return -1;
                }
                return lstIndex;
            }
            catch (Exception EX)
            {
                LogHelper.WriteTextToLog("Error : " + EX.Message);
                return -1;
            }
        }




    }
}
