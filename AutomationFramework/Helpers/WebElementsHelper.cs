using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFramework.Helpers
{
    /// <summary>
    /// This class has all the relevant methods related to 
    /// the controls on the screen 
    /// </summary>
    public static class WebElementsHelper
    {        
        /// <summary>
        /// This method keys into a text control,and if the keyin is successful it returns true
        /// </summary>
        /// <param name="keyInVlu"></param>
        /// <param name="txtObj"></param>
        /// <param name="parentObj"></param>
        /// <param name="objPropNames"></param>
        /// <param name="objPropValues"></param>
        /// <returns></returns>
        public static bool KeyInObject(string keyInVlu, IWebElement txtObj = null, IWebElement parentObj = null, string[] objPropNames = null, string[] objPropValues = null, bool useHotKey = false)
        {
            bool flag = false;
            try
            {
                //keyin directly into the textcontrol as txtObj != null
                if (txtObj != null)
                {
                    if (useHotKey)
                    {
                        txtObj.SendKeys(Keys.Home + Keys.Shift + Keys.End);
                    }
                    txtObj.SendKeys(keyInVlu);
                    //verify if the text is keyedin properly
                }
                //get the textbox control using the params objPropNames,objPropValues  and then keyin
                else if ((parentObj != null) && (objPropNames != null) && (objPropValues != null))
                {

                }
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }





    }
}
