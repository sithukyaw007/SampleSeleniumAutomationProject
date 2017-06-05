using AutomationFramework.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFramework.DataAccess
{
    public static class DataHandlerExtensions
    {

        /// <summary>
        /// Assign the Stored procedure parameters and return a command Object 
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="SProcName"></param>
        /// <param name="paramDict"></param>
        /// <returns></returns>
        public static SqlCommand AssignSprocParameters(this SqlConnection sqlConnection, string SProcName, Dictionary<string, string> paramDict)
        {
            SqlCommand cmd = null;
            try
            {
                if (sqlConnection == null || ((sqlConnection != null && (sqlConnection.State == ConnectionState.Closed ||
                    sqlConnection.State == ConnectionState.Broken))))
                    sqlConnection.Open();

                cmd = new SqlCommand(SProcName, sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlCommandBuilder.DeriveParameters(cmd);
                //setting up the Parameter Direction and Type
                for (int i = 0; i < cmd.Parameters.Count; i++)
                {
                    //if the parameter direction is not input
                    if (
                        (cmd.Parameters[i].Direction == ParameterDirection.Output) |
                        (cmd.Parameters[i].Direction == ParameterDirection.InputOutput) |
                        (cmd.Parameters[i].Direction == ParameterDirection.ReturnValue)
                       )
                    {
                        if (
                            (cmd.Parameters[i].DbType == DbType.Int16) |
                            (cmd.Parameters[i].DbType == DbType.Int32) |
                            (cmd.Parameters[i].DbType == DbType.Int64) |
                            (cmd.Parameters[i].DbType == DbType.UInt16) |
                            (cmd.Parameters[i].DbType == DbType.UInt32) |
                            (cmd.Parameters[i].DbType == DbType.UInt64) |
                            (cmd.Parameters[i].DbType == DbType.Single) |
                            (cmd.Parameters[i].DbType == DbType.Double) |
                            (cmd.Parameters[i].DbType == DbType.Currency) |
                            (cmd.Parameters[i].DbType == DbType.Decimal) |
                            (cmd.Parameters[i].DbType == DbType.Byte) |
                            (cmd.Parameters[i].DbType == DbType.SByte)
                           )
                        {
                            cmd.Parameters[i].Value = 0;

                        }
                        else
                        {
                            cmd.Parameters[i].Value = null;
                        }
                    }
                    else
                    {
                        foreach (var ky in paramDict)
                        {
                            if (cmd.Parameters[i].ParameterName.ToString().ToLower().Trim() == ky.Key.ToLower().Trim())
                            {
                                cmd.Parameters[i].Value = ky.Value;
                                break;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteTextToLog("Error : " + ex.Message);
                return null;
            }
            finally
            {
                //sqlConnection.CloseDBConnection();
            }
            return cmd;
        }

        /// <summary>
        ///This function returns a dictionary object which stores the values of a record inside a database table
        ///Please Note : All the table columns having the " " values are not added to the dictionary
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="paramDict"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionaryFromDB(this SqlConnection sqlConnection, Dictionary<string, string> paramDict)
        {
            Dictionary<string, string> dbDict;
            SqlCommand cmd;
            string vlu, ky;
            try
            {
                if (sqlConnection == null || ((sqlConnection != null && (sqlConnection.State == ConnectionState.Closed ||
                    sqlConnection.State == ConnectionState.Broken))))
                    sqlConnection.Open();

                dbDict = new Dictionary<string, string>();

                //execute the sproc and get the values from the test database
                cmd = sqlConnection.AssignSprocParameters("Get_Dict_From_Table", paramDict);
                SqlDataReader reader = cmd.ExecuteReader();

                //iterate through the reader and write the values inside it to the Dictionary object
                while (reader.Read())
                {
                    vlu = reader["val"].ToString().ToLower().Trim();
                    ky = reader["ky"].ToString().ToLower().Trim();

                    //eliminate the " " characted while adding it to the dictionary
                    if (vlu.Trim() != "")
                    {
                        dbDict.Add(ky, vlu);
                    }
                }
                return dbDict;

            }
            catch (Exception ex)
            {

                LogHelper.WriteTextToLog("Error : " + ex.Message);
                return null;
            }
            finally
            {
                //sqlConnection.CloseDBConnection();
            }
        }

        /// <summary>
        /// Verifies whether arecord exists inside the test db table for the testCaseRef passed 
        /// if exists fetches the GUID related to  it or else returns as no result string
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="tblName"></param>
        /// <param name="testCaseRef"></param>
        /// <returns></returns>
        public static string VarifyAndGetGuid(this SqlConnection sqlConnection, string tblName, string testCaseRef)
        {
            string guid = "no result";
            Dictionary<string, string> paramDict;
            SqlCommand cmd;
            try
            {
                paramDict = new Dictionary<string, string>();
                paramDict.Add("@table_name", tblName.Trim());
                paramDict.Add("@tst_ref", testCaseRef.Trim());
                paramDict.Add("@guid", "");

                if (sqlConnection == null || ((sqlConnection != null && (sqlConnection.State == ConnectionState.Closed ||
                    sqlConnection.State == ConnectionState.Broken))))
                    sqlConnection.Open();

                cmd = sqlConnection.AssignSprocParameters("validate_params", paramDict);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //if the record for that testCaseRef is not found log a error and return no result string
                    if ((reader["errcode"].ToString().Trim() == "1") | (reader["errcode"].ToString().Trim() == "2"))
                    {
                        //log an error

                    }
                    else
                    {
                        guid = reader["GID"].ToString().Trim();
                    }
                }
                return guid;

            }
            catch (Exception ex)
            {
                LogHelper.WriteTextToLog("Error : " + ex.Message);
                return "";
            }
            finally
            {
                //sqlConnection.CloseDBConnection();                
            }

        }


    }
}
