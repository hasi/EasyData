using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.OracleClient;
using System.Data;

namespace EasyData.DB.Oracle
{
    /// <summary>
    /// 
    /// </summary>
    public class OracleParameters
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParameterName"></param>
        /// <param name="DataType"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        internal static OracleParameter CreateInputParameter(string ParameterName,
            OracleType DataType, object Value)
        {
            return CreateInputParameter(ParameterName, DataType, Value, ParameterDirection.Input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParameterName"></param>
        /// <param name="DataType"></param>
        /// <returns></returns>
        internal static OracleParameter CreateOutputParameter(string ParameterName,
            OracleType DataType)
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
        internal static OracleParameter CreateOutputParameter(string ParameterName,
            OracleType DataType, ParameterDirection Direction)
        {

            OracleParameter param = new OracleParameter(ParameterName, DataType);
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
        internal static OracleParameter CreateInputParameter(string ParameterName,
            OracleType DataType, object Value, ParameterDirection Direction)
        {

            OracleParameter param = new OracleParameter(ParameterName, DataType);
            param.Direction = Direction;

            //CStr only for strings
            if (DataType == OracleType.VarChar || DataType == OracleType.LongVarChar)
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
            else if (DataType == OracleType.Char)
            {
                if (Value != null)
                {
                    if (Value.Equals(true))
                    {
                        Value = 'Y';
                    }
                    else if(Value.Equals(false))
                    {
                        Value = 'N';
                    }

                    Char chrTempCharVal = (Char)Value;

                    if (chrTempCharVal == ' ')
                        param.Value = null;
                    else
                        param.Value = chrTempCharVal;
                }
            }
            else if (DataType == OracleType.Number)
            {
                if (Value != null)
                {
                    if (Value.Equals(true))
                    {
                        Value = 1;
                    }
                    else if (Value.Equals(false))
                    {
                        Value = 0;
                    }

                    int chrTempCharVal = (int)Value;
                    param.Value = chrTempCharVal;
                }
            }
            else
                param.Value = Value;


            return param;
        }
    }
}
