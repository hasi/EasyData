using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data.SqlClient;
using Devart.Data.MySql;

namespace EasyData.DB
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataReader<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        internal static object GetDBValue(T dataReader, string columnName)
        {
            Type type = typeof(T);
            object objValue = null;

            if (type == typeof(OracleDataReader))
            {
                OracleDataReader oraDataReader = (OracleDataReader)Convert.ChangeType(dataReader, typeof(OracleDataReader));
                objValue = oraDataReader.GetValue(oraDataReader.GetOrdinal(columnName));
            }
            else if (type == typeof(SqlDataReader))
            {
                SqlDataReader sqlDataReader = (SqlDataReader)Convert.ChangeType(dataReader, typeof(SqlDataReader));
                objValue = sqlDataReader.GetValue(sqlDataReader.GetOrdinal(columnName));
            }
            else if (type == typeof(MySqlDataReader))
            {
                MySqlDataReader mysqlDataReader = (MySqlDataReader)Convert.ChangeType(dataReader, typeof(MySqlDataReader));
                objValue = mysqlDataReader.GetValue(mysqlDataReader.GetOrdinal(columnName));
            }
             
            return objValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="strColumnName"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static object GetDBValue(T dataReader, string strColumnName, int index)
        {
            Type type = typeof(T);
            object objValue = null;

            if (type == typeof(OracleDataReader))
            {
                OracleDataReader oraDataReader = (OracleDataReader)Convert.ChangeType(dataReader, typeof(OracleDataReader));

                if (oraDataReader.IsDBNull(index))
                {
                    //objValue = null;
                }
                else
                {
                    objValue = oraDataReader.GetValue(oraDataReader.GetOrdinal(strColumnName));
                }
            }
            else if (type == typeof(SqlDataReader))
            {
                SqlDataReader sqlDataReader = (SqlDataReader)Convert.ChangeType(dataReader, typeof(SqlDataReader));

                if (sqlDataReader.IsDBNull(index))
                {
                    //objValue = null;
                }
                else
                {
                    objValue = sqlDataReader.GetValue(sqlDataReader.GetOrdinal(strColumnName));
                }
            }
            else if (type == typeof(MySqlDataReader))
            {
                MySqlDataReader mysqlDataReader = (MySqlDataReader)Convert.ChangeType(dataReader, typeof(MySqlDataReader));

                if (mysqlDataReader.IsDBNull(index))
                {
                    //objValue = null;
                }
                else
                {
                    objValue = mysqlDataReader.GetValue(mysqlDataReader.GetOrdinal(strColumnName));
                }
            }
            return objValue;
        }
    }
}
