using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Devart.Data.MySql;
using System.Data;

namespace EasyData.DB.MySql
{
    /// <summary>
    /// 
    /// </summary>
    public class MySqlParameters
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParameterName"></param>
        /// <param name="DataType"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        internal static MySqlParameter CreateInputParameter(string ParameterName,
            MySqlType DataType, object Value)
        {
            return CreateInputParameter(ParameterName, DataType, Value, ParameterDirection.Input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParameterName"></param>
        /// <param name="DataType"></param>
        /// <returns></returns>
        internal static MySqlParameter CreateOutputParameter(string ParameterName,
            MySqlType DataType)
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
        internal static MySqlParameter CreateOutputParameter(string ParameterName,
            MySqlType DataType, ParameterDirection Direction)
        {

            MySqlParameter param = new MySqlParameter(ParameterName, DataType);
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
        internal static MySqlParameter CreateInputParameter(string ParameterName,
            MySqlType DataType, object Value, ParameterDirection Direction)
        {

            MySqlParameter param = new MySqlParameter(ParameterName, DataType);
            param.Direction = Direction;

            //CStr only for strings
            if (DataType == MySqlType.VarChar || DataType == MySqlType.Text)
            {
                if (Value != null)
                {
                    string strTempStringVal = (string)Value;

                    if (strTempStringVal.Trim() == "")
                        param.MySqlValue = null;
                    else
                        param.MySqlValue = strTempStringVal.Trim();
                }
            }
            else if (DataType == MySqlType.Char)
            {
                if (Value != null)
                {
                    Char chrTempCharVal = (Char)Value;

                    if (chrTempCharVal == ' ')
                        param.MySqlValue = null;
                    else
                        param.MySqlValue = chrTempCharVal;
                }
            }
            else
                param.MySqlValue = Value;


            return param;
        }
    }
}
