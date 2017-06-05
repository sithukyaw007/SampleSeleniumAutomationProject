using AutomationFramework.Base;
using AutomationFramework.Extensions;
using HtmlAgilityPack;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutomationFramework.Helpers
{

    /// <summary>
    /// 
    /// </summary>
    public class HtmlTableHelper
    {
        /// <summary>
        /// /// Returns the nearest table object inside the parentObj 
        /// whose column value matches the string matchingColumnVlu
        /// </summary>
        /// <param name="parentObj"></param>
        /// <param name="matchingColumnVlu"></param>
        /// <param name="IsLogError"></param>
        /// <param name="WaitTime"></param>
        /// <returns></returns>
        public IWebElement FetchTableObject(IWebElement parentObj, string matchingColumnVlu, bool IsLogError, int WaitTime)
        {
            bool matchFound = false;
            IWebElement tbl = null;
            //get the list of all tables inside the parent object
            IList<IWebElement> tblList = parentObj.FindElements(By.TagName("table"));
            int tblCount = 0; ;
            //now match the unique string matchingColumnVlu with the table column values 
            //and fetch the table where the match is found
            foreach (IWebElement table in tblList)
            {
                if (matchFound)
                {
                    break;
                }
                tblCount = tblCount + 1;
                //Console.WriteLine("Table : " + tblCount.ToString());
                int rowCount = 0;
                //get the list of alll table rows
                IList<IWebElement> trList = table.FindElements(By.TagName("tr"));

                //iterte through each row
                foreach (IWebElement tblRow in trList)
                {
                    if (matchFound)
                    {
                        break;
                    }
                    int thCount = 0;

                    rowCount = rowCount + 1;
                    //Console.WriteLine("row : " + rowCount.ToString());
                    //get the list of all table  th or td elements
                    try
                    {
                        //look for all the child th tags inside the tr
                        IList<IWebElement> thList = tblRow.FindElements(By.TagName("th"));
                        foreach (IWebElement tblHead in thList)
                        {
                            if (matchFound)
                            {
                                break;
                            }
                            thCount = thCount + 1;
                            if (matchingColumnVlu.Trim() == tblHead.Text.ToString().Trim())
                            {
                                //Console.WriteLine("Th : " + thCount.ToString() + " match found...");
                                tbl = table;
                                matchFound = true;
                                break;
                            }
                        }
                    }
                    catch (NoSuchElementException e)
                    {
                        throw;
                    }

                    //look for all the child td tags inside the tr
                    //int tdCount = 0;
                    IList<IWebElement> tdList = tblRow.FindElements(By.TagName("td"));
                    foreach (IWebElement tblData in tdList)
                    {
                        if (matchFound)
                        {
                            break;
                        }

                        //check to see if the td has a nested table control 
                        //if yes folllow  all the above steps again

                        if (matchingColumnVlu.Trim() == tblData.Text.ToString().Trim())
                        {
                            //Console.WriteLine("Tr : " + thCount.ToString() + " match found...");
                            tbl = table;
                            matchFound = true;
                            break;
                        }
                    }
                }
            }

            return tbl;
        }

        /// <summary>
        /// Executes a javascript and returns the list of all table rows inside the table 
        /// </summary>
        /// <param name="tbl"></param>
        /// <returns></returns>
        public static IEnumerable GetTableRowCollection(IWebElement tbl)
        {
            IEnumerable tblRows = null;
            try
            {
                var tr = ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0].rows; ", tbl);
                tblRows = (IEnumerable)tr;
            }
            catch (Exception)
            {

                throw;
            }
           
            return tblRows;
             
        }

        /// <summary>
        /// Executes a javascript and returns the list of all table columns inside the table row 
        /// </summary>
        /// <param name="tblRow"></param>
        /// <returns></returns>
        public static IEnumerable GetTableColumnCollection(IWebElement tblRow)
        {
            IEnumerable tblCols = null;
            try
            {
                var tc = ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0].cells; ", tblRow);
                tblCols = (IEnumerable)tc;
            }
            catch (Exception)
            {

                throw;
            }

            return tblCols;

        }

        /*
         * **************************************************************************************************************************************************
         * These functions are all related to performing on table rows and columns
         * **************************************************************************************************************************************************
        */

        /// <summary>
        /// Get the column index inside the table based on the columnName parameter
        /// </summary>
        /// <param name="tbl"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static int GetTableColumnIndex(IWebElement tbl, string columnName)
        {
            int colIndx = -1;
            try
            {
                IEnumerable tblRows = GetTableRowCollection(tbl);
                foreach (IWebElement tr in tblRows)
                {
                    int cellCount = 0;
                    // Iterate through each cell of the table row
                    IEnumerable tblCols = GetTableColumnCollection(tr);
                    foreach (IWebElement td in (IEnumerable)tblCols)
                    {                        
                        string cellTxt = td.Text.ToString().Trim().ToLower();
                        if (string.Compare(cellTxt, columnName.Trim().ToLower()) == 0 )
                        {
                            colIndx = cellCount;
                            break;
                        }
                        cellCount++;
                    }
                    if (colIndx > -1)
                    {
                        break;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            return colIndx;
        }

        /// <summary>
        /// Get the row index inside the table based on the keyColumnNames and keycolumnValues parameters
        /// </summary>
        /// <param name="tbl"></param>
        /// <param name="keyColumnNames"></param>
        /// <param name="keycolumnValues"></param>
        /// <returns></returns>
        public static int GetTableRowIndex(string[] keyColumnNames, string[] keycolumnValues,IWebElement tbl = null)
        {
            int rowIndx = -1;
            bool match = true;
            IWebElement dataTbl, headerTbl;
            try
            {
                //In case of split tables where we have header in one table and data in another table controls
                dataTbl = DriverContext.Driver.GetGridObject(keycolumnValues[0].ToString(),"data");
                headerTbl = DriverContext.Driver.GetGridObject(keyColumnNames[0].ToString(), "header");

                if (dataTbl == null || headerTbl == null)
                {
                    return rowIndx;
                }

                var rCount = ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0].rows.length; ", dataTbl);
                int rowCount = Convert.ToInt32(rCount);
                int colIndx = -1;
                for (int i = 0; i < rowCount; i++)
                {
                    match = true;
                    for (int j = 0; j < keyColumnNames.Length; j++)
                    {    
                        //could not find the name of the column in the table columns in such case exit the scenario               
                        colIndx = HtmlTableHelper.GetTableColumnIndex(headerTbl, keyColumnNames.GetValue(j).ToString());
                        if (colIndx == -1)
                        {
                            break;
                        }

                        //column count of the current row < colIndex in such case ignore that  row
                        var currentRowColCount = ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0].rows["+i+"].cells.length; ", dataTbl);
                        if (Convert.ToInt32(currentRowColCount) < colIndx)
                        {
                            continue;
                        }

                        //get the cell text
                        var cellTxt = ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0].rows[" + i + "].cells["+colIndx+"].textContent; ", dataTbl);

                        match = match && (cellTxt.ToString().Trim().ToLower() == keycolumnValues.GetValue(j).ToString().Trim().ToLower());
                       
                    }
                    if (colIndx == -1)
                    {
                        break;
                    }
                    if (match)
                    {
                        rowIndx = i;
                        break;
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return rowIndx;
        }

        /*
        * **************************************************************************************************************************************************
        * These functions are for storing Table Data
        * **************************************************************************************************************************************************
       */

        /// <summary>
        /// Assign the first row of the datatable as its header  
        /// </summary>
        /// <param name="dtTab"></param>
        /// <returns></returns>
       /* public DataTable AssignDataTableHeader(DataTable dtTab)
        {
            int emptyColHeaderIndx = 0;
            try
            {
                //make the first row of the datatable as the header row and then delete the first row
                foreach (DataColumn column in dtTab.Columns)
                {
                    string cName = dtTab.Rows[0][column.ColumnName].ToString();
                    if (!dtTab.Columns.Contains(cName) && cName.Trim() != "")
                    {
                        column.ColumnName = cName;
                    }
                    else//if the header value ie empty then assign a custom header with a counter attached to that custom string
                    {
                        emptyColHeaderIndx++;
                        column.ColumnName = "EmptyHeader" + emptyColHeaderIndx.ToString(); ;

                    }

                }
                dtTab.Rows[0].Delete();
            }
            catch (Exception)
            {

                throw;
            }
            return dtTab;
        }*/


        /// <summary>
        /// Stores the Table\Grid values on the browser to a DataTable 
        /// </summary>
        /// <param name="tblObj"></param>
        /// <param name="IsFirstRowHeader"></param>
        /// <returns></returns>
        public DataTable StoreHtmlTableToDataTable(IWebElement tblObj, bool isFirstRowHeader = true)
        {
            DataTable dataTbl = new DataTable();
            int rowIndex = 0;

            try
            {
                //_tblDataCollection = new List<TableDataCollection>();

                var tblRows = ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0].rows; ", tblObj);

                if (tblRows != null)
                {
                    //Iterate through each row of the table
                    foreach (IWebElement tr in (IEnumerable)tblRows)
                    {
                        int colIndx = 0;
                        DataRow dtRow = dataTbl.NewRow();
                        // Iterate through each cell of the table row
                        var tblCols = ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("return arguments[0].cells; ", tr);
                        foreach (IWebElement td in (IEnumerable)tblCols)
                        {
                            //add the header row of the table as  the datatable column hader row
                            if (rowIndex == 0)
                            {
                                dataTbl.Columns.Add("Col" + colIndx.ToString(), typeof(string));
                            }

                            dtRow["Col" + colIndx.ToString()] = td.Text;

                            //loop through any child or nested table structures if you want using the same approach

                            //Write Table to List : This part is not done yet                           
                            colIndx++;
                        }
                        dataTbl.Rows.Add(dtRow);
                        rowIndex++;
                    }

                }
                //if first row is the header row then assign it as a header of the datatable
                if (isFirstRowHeader)
                {
                    dataTbl = dataTbl.AssignDataTableHeader();
                }

                return dataTbl;
            }
            catch (Exception ex)
            {
                LogHelper.WriteTextToLog("Error : " + ex.Message);
                return null;                
            }                  
        }

    }
}
