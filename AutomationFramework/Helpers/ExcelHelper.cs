
using Excel;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace AutomationFramework.Helpers
{
    public class ExcelHelper
    {

        private static List<DataCollection> _dataCol = new List<DataCollection>();
       
        /// <summary>
        /// Read the Excel sheet Value into a DataTable
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private static DataTable ExcelToDataTable(string fileName,string sheetName)
        {
            //open the file and return as stream
            FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
            //CreateOpenXMLReader via ExcelReaderFactory
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);//.xlsx
            //Set the First Row in the excel as the cloumn name
            excelReader.IsFirstRowAsColumnNames = true;
            //Return as data set
            DataSet result = excelReader.AsDataSet();
            //get all the tables
            DataTableCollection table = result.Tables;
            //Store in the DataTable
            DataTable resultTable = table[sheetName];
            //return
            return resultTable;
        }

        /// <summary>
        /// Populate the excel data into a Data table collection
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sheetName"></param>
        public static void PopulateIntoCollection(string fileName, string sheetName)
        {
            DataTable table = ExcelToDataTable(fileName, sheetName);

            //iterate through the rows and columns of the table
            for (int row = 1; row <= table.Rows.Count; row++)
            {
                for (int col = 0; col < table.Columns.Count; col++)
                {
                    DataCollection dtTable = new DataCollection()
                    {
                        rowNumber = row,
                        colName = table.Columns[col].ColumnName,
                        colValue = table.Rows[row - 1][col].ToString()
                    };
                    _dataCol.Add(dtTable);
                }
            }
        }


        /// <summary>
        /// Read column data from the DataTable based on either a KeyColumnName and KyColumnVlu combo or with a KyRowNo
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="kyColumnName"></param>
        /// <param name="kyColumnVlu"></param>
        /// <param name="kyRowNo"></param>
        /// <returns></returns>
        public static string ReadColumnData(string columnName, string kyColumnName = "", string kyColumnVlu = "", int kyRowNo = 0)
        {
            try
            {
                //selecting a specific column value from the dataTable collection bases on kyColumnName and kyColumnVlu
                if ((kyColumnName != "") && (kyColumnVlu != "") && (kyRowNo == 0))
                {                    
                    string data = (from colData in _dataCol
                                   where colData.colName.Trim().ToLower() == columnName.Trim().ToLower()
                                   && colData.colName.Trim().ToLower() == kyColumnName.Trim().ToLower()
                                   && colData.colValue.Trim().ToLower() == kyColumnVlu.Trim().ToLower()
                                   select colData.colValue).SingleOrDefault();
                    return data.ToString();
                }
                else //selecting a specific column value from the dataTable collection bases on kyRowNo
                {
                    
                    string data = (from colData in _dataCol
                                   where colData.colName == columnName                                  
                                   && colData.rowNumber == kyRowNo
                                   select colData.colValue).SingleOrDefault();
                    return data.ToString();
                }
                                
            }
            catch (System.Exception)
            {

                return null;
            }
        }

    }

    public class DataCollection
    {
        public int rowNumber { get; set; }
        public string colName { get; set; }
        public string  colValue { get; set; }
        //public string kyColVlu { get; set; }
    }

}
