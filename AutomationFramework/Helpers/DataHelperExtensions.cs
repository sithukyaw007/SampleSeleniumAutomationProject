using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFramework.Helpers
{
    public static class DataHelperExtensions
    {
        /// <summary>
        /// Open a new database connection and returns the SqlConnection object
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static SqlConnection OpenDBConnection(this SqlConnection sqlConnection, string connectionString)
        {
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                return sqlConnection;
            }
            catch (Exception e)
            {
                LogHelper.WriteTextToLog("Error opening the DB COnnection : " + e.Message);
                return null;
            }           
        }

        /// <summary>
        /// close the existing DB connection
        /// </summary>
        /// <param name="sqlConnection"></param>
        public static void CloseDBConnection(this SqlConnection sqlConnection)
        {
            try
            {
                sqlConnection.Close();
            }
            catch (Exception e)
            {

                LogHelper.WriteTextToLog("Error closing the DB COnnection : " + e.Message);               
            }
        }

        public static DataTable ExecuteQuery(this SqlConnection sqlConnection,string queryString)
        {
            DataSet dataSet;
            try
            {
                if (sqlConnection == null || ((sqlConnection != null && (sqlConnection.State == ConnectionState.Closed ||
                    sqlConnection.State == ConnectionState.Broken))))
                    sqlConnection.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(queryString, sqlConnection);
                dataAdapter.SelectCommand.CommandType = CommandType.Text;
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "table");
                return dataSet.Tables["table"];
            }
            catch (Exception e)
            {
                dataSet = null;
                LogHelper.WriteTextToLog("Error executing the SQL Query : " + e.Message);
                sqlConnection.Close();
                return null;
            }
            finally
            {
                sqlConnection.Close();
                dataSet = null;
            }            
        }


        /// <summary>
        /// Assign the first row of the datatable as its header  
        /// </summary>
        /// <param name="dtTab"></param>
        /// <returns></returns>
        public static DataTable AssignDataTableHeader(this DataTable dtTab)
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
                return dtTab;
            }
            catch (Exception ex)
            {
                LogHelper.WriteTextToLog("Error : " + ex.Message);
                return null;                
            }           
        }

        /// <summary>
        /// Matches all the list values in the kyFieldValues to the columns in the 
        /// Data Table row by row until the complete match is found in a single row
        /// If no match is found an empty list is returned.
        /// </summary>
        /// <param name="dataTbl"></param>
        /// <param name="kyFieldValues"></param>
        /// <returns></returns>
        public static List<IWebElement> MatchWebElementText(this DataTable dataTbl, List<string> kyFieldValues)
        {
            List<IWebElement> matchedListObj = new List<IWebElement>();
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
                                //matchedListObj.Add("Ky Search : " + searchForKyVlu + " Match Found at Row  [" + rowCnt.ToString() + ", " + colcnt.ToString() + "]");
                                matchedListObj.Add(listObj);
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
                matchedListObj.Clear();
                LogHelper.WriteTextToLog("Error : " + ex.Message);
                return matchedListObj;
            }
        }

        public static int GetIndexOfMatchWebElementText(this DataTable dataTbl, List<string> kyFieldValues)
        {
            //List<IWebElement> matchedListObj = new List<IWebElement>();
            int indx = -1;
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
                                //matchedListObj.Add("Ky Search : " + searchForKyVlu + " Match Found at Row  [" + rowCnt.ToString() + ", " + colcnt.ToString() + "]");
                                //matchedListObj.Add(listObj);
                                indx = rowCnt;
                            }
                        }
                        if (noOfMatches == kyFieldValues.Count)
                            return indx;
                            //return matchedListObj;
                    }
                    if (noOfMatches == kyFieldValues.Count)
                        return indx;                   
                }
                return indx;
            }
            catch (Exception ex)
            {                
                LogHelper.WriteTextToLog("Error : " + ex.Message);
                return indx;
            }
        }


    }
}
