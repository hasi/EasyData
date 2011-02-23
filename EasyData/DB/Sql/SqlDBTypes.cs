using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace EasyData.DB.Sql
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlDBTypes
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataType"></param>
        /// <returns></returns>
        internal static SqlDbType GetDbType(string DataType)
        {
            if (DataType.Length > 6)
            {
                if (DataType.Substring(0, 6) == "System")
                {
                    DataType = DataType.Substring(7);
                }
            }
            
            SqlDbType result = SqlDbType.NVarChar;

            if (DataType == "int" || DataType == "Int" || DataType == "Int32")
            {
                result = SqlDbType.Int;
            }
            else if (DataType == "short" || DataType == "Int16")
            {
                result = SqlDbType.SmallInt;
            }
            else if (DataType == "long" || DataType == "Int64")
            {
                result = SqlDbType.BigInt;
            }
            else if (DataType == "nvarchar" || DataType == "varchar" || DataType == "String")
            {
                result = SqlDbType.NVarChar;
            }
            else if (DataType == "ntext" || DataType == "text" || DataType == "Text")
            {
                result = SqlDbType.NText;
            }
            else if (DataType == "datetime" || DataType == "DateTime")
            {
                result = SqlDbType.DateTime;
            }
            else if (DataType == "binary" || DataType == "Byte[]")
            {
                result = SqlDbType.Binary;
            }
            else if (DataType == "bit" || DataType == "Boolean")
            {
                result = SqlDbType.Bit;
            }
            else if (DataType == "decimal" || DataType == "Decimal")
            {
                result = SqlDbType.Decimal;
            }
            else if (DataType == "nchar" || DataType == "char" || DataType == "Char")
            {
                result = SqlDbType.NChar;
            }
            else if (DataType == "Guid")
            {
                result = SqlDbType.UniqueIdentifier;
            }
            else if (DataType == "xml" || DataType == "Xml" || DataType == "XML")
            {
                result = SqlDbType.Xml;
            }

            return result;
        }
    }
}
