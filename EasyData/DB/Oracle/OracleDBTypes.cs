using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.OracleClient;

namespace EasyData.DB.Oracle
{
    /// <summary>
    /// 
    /// </summary>
    public class OracleDBTypes
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataType"></param>
        /// <returns></returns>
        internal static OracleType GetDbType(string DataType)
        {
            if (DataType.Length > 6)
            {
                if (DataType.Substring(0, 6) == "System")
                {
                    DataType = DataType.Substring(7);
                }
            }
            OracleType result = OracleType.VarChar;

            if (DataType == "int" || DataType == "Int" || DataType == "Int32")
            {
                result = OracleType.Number;
            }
            else if (DataType == "varchar" || DataType == "String")
            {
                result = OracleType.VarChar;
            }
            else if (DataType == "text" || DataType == "Text")
            {
                result = OracleType.VarChar;
            }
            else if (DataType == "datetime" || DataType == "DateTime")
            {
                result = OracleType.DateTime;
            }
            else if (DataType == "binary" || DataType == "Byte[]")
            {
                result = OracleType.Blob;
            }
            else if (DataType == "bit" || DataType == "Boolean")
            {
                result = OracleType.Char;
            }
            else if (DataType == "decimal" || DataType == "Decimal")
            {
                result = OracleType.Number;
            }
            else if (DataType == "char" || DataType == "Char")
            {
                result = OracleType.Char;
            }
            else if (DataType == "Guid")
            {
                result = OracleType.VarChar;
            }

            return result;
        }
    }
}
