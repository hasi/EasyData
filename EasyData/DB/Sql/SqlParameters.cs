using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;

namespace EasyData.DB.Sql
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlParameters
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParameterName"></param>
        /// <param name="DataType"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        internal static SqlParameter CreateInputParameter(string ParameterName,
            SqlDbType DataType, object Value)
        {
            return CreateInputParameter(ParameterName, DataType, Value, ParameterDirection.Input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParameterName"></param>
        /// <param name="DataType"></param>
        /// <returns></returns>
        internal static SqlParameter CreateOutputParameter(string ParameterName,
            SqlDbType DataType)
        {
            return CreateOutputParameter(ParameterName, DataType, ParameterDirection.Output);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParameterName"></param>
        /// <param name="DataType"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        internal static SqlParameter CreateOutputParameter(string ParameterName,
            SqlDbType DataType, ParameterDirection Direction)
        {

            SqlParameter param = new SqlParameter(ParameterName, DataType);
            param.Direction = Direction;

            return param;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParameterName"></param>
        /// <param name="DataType"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        internal static SqlParameter CreateInputParameter(string ParameterName,
            SqlDbType DataType, object Value, ParameterDirection Direction)
        {

            SqlParameter param = new SqlParameter(ParameterName, DataType);
            param.Direction = Direction;

            //CStr only for strings
            if (DataType == SqlDbType.VarChar || DataType == SqlDbType.Text || DataType == SqlDbType.NVarChar || DataType == SqlDbType.NText)
            {
                if (Value != null)
                {
                    string strTempStringVal = (string)Value;

                    if (strTempStringVal.Trim() == "")
                        param.Value = null;
                    else
                        param.Value = strTempStringVal.Trim();
                }
            }
            else if (DataType == SqlDbType.Char || DataType == SqlDbType.NChar)
            {
                if (Value != null)
                {
                    Char chrTempCharVal = (Char)Value;

                    if (chrTempCharVal == ' ')
                        param.Value = null;
                    else
                        param.Value = chrTempCharVal;
                }
            }
            else
                param.Value = Value;

            return param;
        }
    }
}
