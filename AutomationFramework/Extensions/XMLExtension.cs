using AutomationFramework.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AutomationFramework.Extensions
{
    /// <summary>
    /// This class deals with all the XML files realted functions
    /// </summary>
    public static class XMLExtension
    {

        /// <summary>
        /// Create an XMLfile and write the contents of a Datatable 
        /// to that file,returns true if the file creation is successful
        /// </summary>
        /// <param name="dtTable"></param>
        /// <param name="xmlFile"></param>
        /// <param name="includeTableHeader"></param>
        /// <returns></returns>
        public static bool DataTableToXmlFile(DataTable dtTable, string xmlFilePath,bool includeTableHeader = true)
        {
            bool flag = false;
            string[] columnNames = { "dummy" };
            try
            {
                if (includeTableHeader)
                {
                    columnNames = (from dc in dtTable.Columns.Cast<DataColumn>()
                                            select dc.ColumnName).ToArray();
                }               

                using (XmlWriter writer = XmlWriter.Create(xmlFilePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("XmlCp");
                    //write the header row of the datatable to xml file
                    if ((includeTableHeader) && (columnNames[0] != "dummy"))
                    {
                        writer.WriteStartElement("Row0");
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            string colName = columnNames[i].ToString();
                            //writer.WriteElementString("Col" + i.ToString(), colName);   // <-- These are new
                            writer.WriteStartElement("Col" + i.ToString());
                            writer.WriteAttributeString("value", colName);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }                    

                    //write the datatable rows to the xml file
                    foreach (DataRow row in dtTable.Rows)
                    {
                        writer.WriteStartElement("Row" + (dtTable.Rows.IndexOf(row) + 1).ToString());
                        foreach (DataColumn col in dtTable.Columns)
                        {
                            // writer.WriteElementString("Col" + dtTable.Columns.IndexOf(col).ToString(), row[col].ToString());   // <-- These are new
                            writer.WriteStartElement("Col" + dtTable.Columns.IndexOf(col).ToString());
                            writer.WriteAttributeString("value", row[col].ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();

                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    flag = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        private static void doIterateNode(XmlNode node,Action<XmlNode> elementVisitor)
        {
            elementVisitor(node);
            foreach (XmlNode childNode in node.ChildNodes)
            {
                doIterateNode(childNode, elementVisitor);
            }
        }

        public  static void IterateThroughAllNodes(this XmlDocument doc,Action<XmlNode> elementVisitor)
        {
            //XMLExtension obj = new XMLExtension();
            if (doc != null && elementVisitor != null)
            {
                foreach (XmlNode node in doc.ChildNodes)
                {
                    doIterateNode(node, elementVisitor);
                }
            }
        }

        /// <summary>
        /// Write an XML Node to a Dictionary Collection and return the result as a dictionary
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeDict"></param>
        /// <returns></returns>
        public static Dictionary<string, string> XmlNodeToDictionary(XmlNode node, Dictionary<string, string> nodeDict)
        {
            string dictKy = "";
            string dictVlu = "";
            try
            {
                //filter all the nodes without attributes as 
                //in our case the node which has attribute is the th one we need to capture 
                //and compare
                string attributeValue = "";
                if (node.Attributes != null && node.Attributes.Count > 0)
                {
                    attributeValue = node.Attributes["value"].Value;
                }
                else
                {
                    attributeValue = "No Attribute";
                }

                if (attributeValue != "No Attribute")
                {
                    dictKy = node.ParentNode.Name + "," + node.Name;
                    dictVlu = attributeValue.Trim().ToString();
                    nodeDict.Add(dictKy, dictVlu);
                }


            }
            catch (Exception)
            {

                throw;
            }
            return nodeDict;
        }

        /// <summary>
        /// Write the contents of an XML file nodee to a dictionary
        /// Please Note  : Only the nodes which have an attribute are recorded to the dictionary
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> XmlFileToDictionary(string xmlFilePath)
        {
            Dictionary<string, string> xmlDict = new Dictionary<string, string>();            
            try
            {               
                var doc = new XmlDocument();
                doc.Load(xmlFilePath);
                string attributeValue = "";
                XMLExtension.IterateThroughAllNodes(doc,
                    delegate (XmlNode node)
                    {
                        xmlDict = XMLExtension.XmlNodeToDictionary(node, xmlDict);
                    });
            }
            catch (Exception)
            {

                throw;
            }
            return xmlDict;
        }


        /// <summary>
        /// Gets the dictionary value for a specific key,if key is found else it returns as default value
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static TVal GetDictionaryVlueForKey<TKey, TVal>(this Dictionary<TKey, TVal> dictionary, TKey key, TVal defaultVal = default(TVal))
        {
            TVal val;
            if (dictionary.TryGetValue(key, out val))
            {
                return val;
            }
            return defaultVal;
        }

        /// <summary>
        ///Compares the difference between two dictionaries and writes the differences
        ///into a dictionary and returns the difference dictionary as a result
        /// </summary>
        /// <param name="expDict"></param>
        /// <param name="actDict"></param>
        /// <returns></returns>
        public static Dictionary<string, string> CompareDictionaries(Dictionary<string, string> expDict, Dictionary<string, string> actDict)
        {
            Dictionary<string, string> compared = new Dictionary<string, string>();

            //get the list of all keys persent in expDict but not in actDict
            foreach (var kv in expDict)
            {
                string secondValue;
                if (!actDict.TryGetValue(kv.Key, out secondValue))
                {
                    compared.Add("Key Value present in Expected but not in Actual " + kv.Key, " Value : " + kv.Value);
                }
            }


            //get the list of all keys persent in actpDict but not in expDict
            foreach (var kv in actDict)
            {
                string secondValue;
                if (!expDict.TryGetValue(kv.Key, out secondValue))
                {
                    compared.Add("Key Value present in Actual but not in Expected " + kv.Key, " Value : " + kv.Value);
                }
            }

            //difference if any between all the cmmon keys
            foreach (var kv in expDict)
            {
                string secondValue;
                if (actDict.TryGetValue(kv.Key, out secondValue))
                {
                    if (!string.Equals(kv.Value, secondValue))
                    {
                        compared.Add(kv.Key, " -> Expected Value : " + kv.Value + " ; Actual Value : " + secondValue);
                    }
                }               
            }

            return compared;
        }

        /// <summary>
        /// Writes the contents of the Grid\table control on the browser
        /// to a XML file and saves it in the C:\Automation\SeleniumProjects\Lighthouse\OutputFiles
        /// as fileName.xml and returns true if succcessful or esle returns false and logs the error
        /// in the log file
        /// </summary>
        /// <param name="tblObj"></param>
        /// <param name="fileName"></param>
        /// <param name="IsFirstRowHeader"></param>
        public static bool GridToXmlFile(IWebElement tblObj,string fileName,bool isFirstRowHeader = true)
        {
            bool flag = true;
            string xmlFilePath = System.IO.Path.GetFullPath("\\Automation\\SeleniumProjects\\Lighthouse\\OutputFiles")+"\\" + fileName;
            try
            {
                HtmlTableHelper tblHelperObj = new HtmlTableHelper();
                
                //write the grid\table on the browser to a DataTable
                DataTable dtTbl = tblHelperObj.StoreHtmlTableToDataTable(tblObj, isFirstRowHeader);

                //convert DataTable to XML File
                flag = DataTableToXmlFile(dtTbl, xmlFilePath, isFirstRowHeader);

                //Write the error to the log file if the above step fails
                if (!flag)
                {

                }
                return flag;

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Compares two XML files and displays the difference
        /// Returns true if both the files are identical or else returns false
        /// If the files are not identical then the difference is recorded to the log files
        /// </summary>
        /// <param name="expXmlFilePath"></param>
        /// <param name="actXmlFilePath"></param>
        /// <param name="logFileName"></param>
        /// <returns></returns>
        public static bool CompareXmlFiles(string expXmlFilePath, string actXmlFilePath,string logFileName)
        {
            bool flag = true;

            //read the values of the XML files to the dictionaries
            Dictionary<string, string> expDict = XmlFileToDictionary(expXmlFilePath);
            Dictionary<string, string> actDict = XmlFileToDictionary(actXmlFilePath);

            //Compare the difference
            Dictionary<string, string> diffDict = CompareDictionaries(expDict,actDict);

            //record the difference to the Log files
            if (diffDict.Count > 0 )
            {
                foreach (KeyValuePair<string, string> comparedDict in diffDict)
                {
                    //write to the log file
                    //Console.WriteLine("{0}{1}", comparedDict.Key, comparedDict.Value);
                    Console.WriteLine(comparedDict.Key +" "+ comparedDict.Value);
                }
                flag = false;
            }
            else
            {
                Console.WriteLine("Files are identical");
            }

            return flag;
        }

    }
}
