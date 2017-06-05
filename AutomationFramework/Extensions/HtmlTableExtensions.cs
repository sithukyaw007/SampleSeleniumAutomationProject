using HtmlAgilityPack;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFramework.Extensions
{
    public static class HtmlTableExtensions
    {
        //    //private static readonly ILog Log = LogManager.GetLogger(typeof(HtmlTableExtensions));

        //    /// <summary>
        //    ///     based on an idea from http://stackoverflow.com/questions/655603/html-agility-pack-parsing-tables
        //    /// </summary>
        //    /// <param name="tableBy"></param>
        //    /// <param name="driver"></param>
        //    /// <returns></returns>
        //    public static HtmlTableData GetTableData(this By tableBy, IWebDriver driver)
        //    {
        //        try
        //        {
        //            var doc = tableBy.GetTableHtmlAsDoc(driver);
        //            var columns = doc.GetHtmlColumnNames();
        //            return doc.GetHtmlTableCellData(columns);
        //        }
        //        catch (Exception e)
        //        {
        //            //Log.Warn(String.Format("unable to get table data from {0} using driver {1} ", tableBy, driver), e);
        //            return null;
        //        }
        //    }

        //    /// <summary>
        //    ///     Take an HtmlTableData object and convert it into an untyped data table,
        //    ///     assume that the row key is the sole primary key for the table,
        //    ///     and the key in each of the rows is the column header
        //    ///     Hopefully this will make more sense when its written!
        //    ///     Expecting overloads for swichting column and headers,
        //    ///     multiple primary keys, non standard format html tables etc
        //    /// </summary>
        //    /// <param name="htmlTableData"></param>
        //    /// <param name="primaryKey"></param>
        //    /// <param name="tableName"></param>
        //    /// <returns></returns>
        //    public static DataTable ConvertHtmlTableDataToDataTable(this HtmlTableData htmlTableData,
        //        string primaryKey = null, string tableName = null)
        //    {
        //        if (htmlTableData == null) return null;
        //        var table = new DataTable(tableName);

        //        foreach (var colName in htmlTableData.Values.First().Keys)
        //        {
        //            table.Columns.Add(new DataColumn(colName, typeof(string)));
        //        }
        //        table.SetPrimaryKey(new[] { primaryKey });
        //        foreach (var values in htmlTableData
        //            .Select(row => row.Value.Values.ToArray<object>()))
        //        {
        //            table.Rows.Add(values);
        //        }

        //        return table;
        //    }


        //    private static HtmlTableData GetHtmlTableCellData(this HtmlDocument doc, IReadOnlyList<string> columns)
        //    {
        //        var data = new HtmlTableData();
        //        foreach (
        //            var rowData in doc.DocumentNode.SelectNodes(XmlExpressions.AllDescendants + HtmlAttributes.TableRow)
        //                .Skip(1)
        //                .Select(row => row.SelectNodes(HtmlAttributes.TableCell)
        //                    .Select(n => WebUtility.HtmlDecode(n.InnerText)).ToList()))
        //        {
        //            data[rowData.First()] = new Dictionary<string, string>();
        //            for (var i = 0; i < columns.Count; i++)
        //            {
        //                data[rowData.First()].Add(columns[i], rowData[i]);
        //            }
        //        }
        //        return data;
        //    }

        //    private static List<string> GetHtmlColumnNames(this HtmlDocument doc)
        //    {
        //        var columns =
        //            doc.DocumentNode.SelectNodes(XmlExpressions.AllDescendants + HtmlAttributes.TableRow)
        //                .First()
        //                .SelectNodes(XmlExpressions.AllDescendants + HtmlAttributes.TableHeader)
        //                .Select(n => WebUtility.HtmlDecode(n.InnerText).Trim())
        //                .ToList();
        //        return columns;
        //    }

        //    private static HtmlDocument GetTableHtmlAsDoc(this By tableBy, IWebDriver driver)
        //    {
        //        var webTable = driver.FindElement(tableBy);
        //        var doc = new HtmlDocument();
        //        doc.LoadHtml(webTable.GetAttribute(HtmlAttributes.InnerHtml));
        //        return doc;
        //    }

        //    public static void SetPrimaryKey(this DataTable table, string[] primaryKeyColumns)
        //    {
        //        int size = primaryKeyColumns.Length;
        //        var keyColumns = new DataColumn[size];
        //        for (int i = 0; i < size; i++)
        //        {
        //            keyColumns[i] = table.Columns[primaryKeyColumns[i]];
        //        }
        //        table.PrimaryKey = keyColumns;
        //    }
        //}

        //public class HtmlTableData : Dictionary<string, Dictionary<string, string>>
        //{

        //}

        ///// <summary>
        ///// config class holding common Html Attributes and tag names etc
        ///// </summary>
        //public static class HtmlAttributes
        //{
        //    public const string InnerHtml = "innerHTML";
        //    public const string TableRow = "tr";
        //    public const string TableHeader = "th";
        //    public const string TableCell = "th|td";
        //    public const string Class = "class";

        /*
         * 
         *         private static List<TableDataCollection> _tblDataCollection;

        /// <summary>
        /// Fetches all the special objects in the tabl cell like hyperlinks,div tag elements,button controls,
        /// textbox etc
        /// </summary>
        /// <param name="columnObj"></param>
        /// <returns></returns>
        private static ColumnSpclObjects GetCellControls(IWebElement columnObj)
        {
            ColumnSpclObjects colControl = null;

            //check to see if the columnObj has tags like input\hyperlink

            //check to see if the ColumnObj has div tag elements which inturn has  span tag
            //(in our structure this is how a button control is embeded inside a table column)
            if (columnObj.FindElements(By.TagName("div")).Count > 0)
            {
                colControl = new ColumnSpclObjects
                {
                    ElementCollection = columnObj.FindElements(By.TagName("div")),
                    ControlType = "div"
                };
            }

            return colControl;
        }

        /// <summary>
        /// Reads the HTML table column values and stores them into a List
        /// and returns that List
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="_tblDataCollection"></param>
        /// <param name="lstColumData"></param>
        /// <returns></returns>
        public List<TableDataCollection> ReadTableColumns(int rowIndex,List<TableDataCollection> _tblDataCollection, IList<IWebElement> lstColumData)
        {
            int colIndex = 1;
            foreach (IWebElement colVlu in lstColumData)
            {
                _tblDataCollection.Add(new TableDataCollection
                {
                    RowNumber = rowIndex,
                    ColNumber = colIndex,
                    ColumnName = colVlu.Text.Trim()!= "" ?
                                 colVlu.Text.Trim() : colIndex.ToString().Trim(),
                    ColumnValue = colVlu.Text.Trim(),
                    //TblColumnControls = GetCellControls(colVlu)
                });
                //move to the next column 
                colIndex++;
                //Console.WriteLine("Row[" + rowIndex.ToString() + "]Col[" + colIndex + "]");
            }
            return _tblDataCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        public static List<TableDataCollection> RadTable(IWebElement table)
        {
            //Init the table
            _tblDataCollection = new List<TableDataCollection>();

            //get all the rows of the table
            var rows = table.FindElements(By.TagName("tr"));

            //Get all the columns from the tbl
            var cols = table.FindElements(By.TagName("td"));

            //create a row index
            int rowIndex = 1;

            //LogHelper.CreateLogFile();


            foreach (var row in rows)
            {
                int colIndex = 0;

                var colData = row.FindElements(By.TagName("td"));

                //Store data only if it has value in the row,in this case the row is a header row
                if (colData.Count != 0)
                {
                    foreach (var colVlu in colData)
                    {
                        _tblDataCollection.Add(new TableDataCollection
                        {
                            RowNumber = rowIndex,
                            ColumnName = cols[colIndex].Text.Trim() != "" ?
                                         cols[colIndex].Text.Trim() : colIndex.ToString().Trim(),
                            ColumnValue = colVlu.Text.Trim(),
                            TblColumnControls = GetCellControls(colVlu)
                        });
                        //move to the next column
                        colIndex++;
                    }
                    //move to the next row
                    rowIndex++;
                }
            }
            return _tblDataCollection;
        }

        /// <summary>
        /// Reads the contents of a HTML table control on the browser and saves it to a DataTable
        /// </summary>
        /// <param name="tblObj"></param>
        /// <returns></returns>
        public List<TableDataCollection> StoreHtmlTableToList_Old(IWebElement tblObj)
        {
            DataTable dataTbl = new DataTable();
            int rowIndex = 1; ;
            try
            {
                IList<IWebElement> tblHead, tblRow, tblData;
                _tblDataCollection = new List<TableDataCollection>();
                
                //Iterate through the table row Object on the browser                
                tblRow = tblObj.FindElements(By.TagName("tr"));
                foreach (IWebElement tRow in tblRow)
                {                   
                    try
                    {
                        //read the contents of the <th> tag of the table if it exists
                        tblHead = tRow.FindElements(By.TagName("th"));
                        if (tblHead.Count > 0)
                        {
                            ReadTableColumns(rowIndex, _tblDataCollection, tblHead);
                        }                       

                        //read the contents of the <td> tag
                        tblData = tRow.FindElements(By.TagName("td"));
                        if (tblData.Count > 0)
                        {
                            ReadTableColumns(rowIndex, _tblDataCollection, tblData);
                        }                        
                    }
                    catch (NoSuchElementException e)
                    {
                        throw;
                    }
                    rowIndex++;
                }                    

            }
            catch (Exception)
            {
                throw;
            }
            return _tblDataCollection;
        }

     
        /// <summary>
        /// Perform click action on the cell
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="refColumnName"></param>
        /// <param name="refColumnValue"></param>
        /// <param name="controlToOperate"></param>
        public static void PerformActionOnCell(string columnIndex, string refColumnName, string refColumnValue, string controlToOperate = null)
        {
            foreach (int rowNumber in GetDynamicRowNumber(refColumnName, refColumnValue))
            {
                var cell = (from e in _tblDataCollection
                            where e.ColumnName == columnIndex.Trim() && e.RowNumber == rowNumber
                            select e.TblColumnControls).SingleOrDefault();

                //Need to operate on those controls
                if (controlToOperate != null && cell != null)
                {
                    //Since based on the control type, the retriving of text changes
                    //created this kind of control                   

                    if (cell.ControlType == "div")
                    {
                        var returnedControl = (from c in cell.ElementCollection
                                               where c.GetAttribute("value") == controlToOperate.Trim()
                                               select c).SingleOrDefault();

                        //ToDo: Currenly only click is supported, future is not taken care here
                        returnedControl?.Click();
                    }

                }
                else
                {
                    cell.ElementCollection?.First().Click();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        private static IEnumerable GetDynamicRowNumber(string columnName, string columnValue)
        {
            //dynamic row
            foreach (var table in _tblDataCollection)
            {
                if (table.ColumnName.Trim() == columnName.Trim() && table.ColumnValue.Trim() == columnValue.Trim())
                    yield return table.RowNumber;
            }
        }

        /// <summary>
        /// Returns true if the table object has <thead><tr><th></th></tr></thead> structure
        /// whih means it has a proper header row
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool TableHasHeaderTag(IWebElement table)
        {
            bool hasHaderTag = true;
            try
            {
                IWebElement headerRow = table.FindElement(By.TagName("thead"));
                IWebElement tblHead = headerRow.FindElement(By.TagName("th"));
            }
            catch (NoSuchElementException e)
            {
                hasHaderTag = false;
            }
            return hasHaderTag;
        }

          /// <summary>
    /// 
    /// </summary>
    public class TableDataCollection
    {
        public int RowNumber { get; set; }
        public int ColNumber { get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
        public ColumnSpclObjects TblColumnControls { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ColumnSpclObjects
    {
        public IEnumerable<IWebElement> ElementCollection { get; set; }
        public string ControlType { get; set; }
    }
          
        */
    }

}