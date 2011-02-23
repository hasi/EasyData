using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Devart.Data.MySql;

namespace EasyData.DB.MySql
{
    /// <summary>
    /// 
    /// </summary>
    public class MySqlDBTypes
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataType"></param>
        /// <returns></returns>
        internal static MySqlType GetDbType(string DataType)
        {
            if (DataType.Length > 6)
            {
                if (DataType.Substring(0, 6) == "System")
                {
                    DataType = DataType.Substring(7);
                }
            }
            MySqlType result = MySqlType.VarChar;

            if (DataType == "int" || DataType == "Int" || DataType == "Int32")
            {
                result = MySqlType.Int;
            }
            if (DataType == "Int16")
            {
                result = MySqlType.SmallInt;
            }
            if (DataType == "Int64")
            {
                result = MySqlType.BigInt;
            }
            else if (DataType == "varchar" || DataType == "String")
            {
                result = MySqlType.VarChar;
            }
            else if (DataType == "text" || DataType == "Text")
            {
                result = MySqlType.Text;
            }
            else if (DataType == "datetime" || DataType == "DateTime")
            {
                result = MySqlType.DateTime;
            }
            else if (DataType == "binary" || DataType == "Byte[]")
            {
                result = MySqlType.Binary;
            }
            else if (DataType == "bit" || DataType == "Boolean" || DataType == "Boolean")
            {
                result = MySqlType.Bit;
            }
            else if (DataType == "decimal" || DataType == "Decimal")
            {
                result = MySqlType.Decimal;
            }
            else if (DataType == "float" || DataType == "Float")
            {
                result = MySqlType.Float;
            }
            else if (DataType == "double" || DataType == "Double")
            {
                result = MySqlType.Double;
            }
            else if (DataType == "char" || DataType == "Char")
            {
                result = MySqlType.Char;
            }
            else if (DataType == "Guid")
            {
                result = MySqlType.Guid;
            }
            
            return result;
        }
    }
}
