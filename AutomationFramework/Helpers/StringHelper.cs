using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFramework.Helpers
{
    public class StringHelper
    {

        /// <summary>
        /// Perform a wild Card comparision of two strings
        /// The accepted wild cards are *  which ignores a multiple characters whre its found
        /// and ? which ignores a single character whre its found
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool WildCardcompare(string str1, string str2)
        {
            bool flag = true;
            return flag;
        }

        /// <summary>
        /// Writes the contents of the delimited string to an string Array
        /// </summary>
        /// <param name="delimitedString"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        public static string[] WriteDelimitedStringToStringArray(string delimitedString,string delimeter=",")
        {
            string[] strArr = null;
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return strArr;
        }

        /// <summary>
        /// Writes the contents of the delimited string to an string List
        /// </summary>
        /// <param name="delimitedString"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        public static List<string> WriteDelimitedStringToStringList(string delimitedString, string delimeter = ",")
        {
            List<string> strList = null;
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return strList;
        }
    }
}
