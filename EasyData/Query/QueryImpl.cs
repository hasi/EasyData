using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using EasyData.DB.Sql;
using System.Collections;
using System.Configuration;
using EasyData.DB.MySql;
using EasyData.DB.Oracle;
using System.Data.OracleClient;
using Devart.Data.MySql;

namespace EasyData.Query
{
    public class QueryImpl : IQuery
    {
        string dbType = ConfigurationSettings.AppSettings["DBType"];
        public List<SqlParameter> sqlParameters;
        public List<OracleParameter> oracleParameters;
        public List<MySqlParameter> mysqlParameters;
        public string queryString;
        public EasySession easySession;
        public CommandType commandType;
        //public List<int> positions;

        public QueryImpl(EasySession session)
        {
            easySession = session;

            if (dbType.Equals("SQL"))
            {
                sqlParameters = new List<SqlParameter>();
            }
            else if (dbType.Equals("ORACLE"))
            {
                oracleParameters = new List<OracleParameter>();
            }
            else if (dbType.Equals("MYSQL"))
            {
                mysqlParameters = new List<MySqlParameter>();
            }
        }

        public bool Update()
        {
            bool result = false;

            result = this.Execute();

            return result;
        }

        public bool Delete()
        {
            bool result = false;

            result = this.Execute();

            return result;
        }

        //public IQuery Where()
        //{
        //    return this;
        //}

        //public IQuery From(Type type)
        //{
        //    return this;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="session"></param>
        /// <param name="type"></param>
        public QueryImpl(string query, EasySession session, CommandType type)
        {
            queryString = query;
            easySession = session;
            commandType = type;

            if (dbType.Equals("SQL"))
            {
                sqlParameters = new List<SqlParameter>();
            }
            else if (dbType.Equals("ORACLE"))
            {
                oracleParameters = new List<OracleParameter>();
            }
            else if(dbType.Equals("MYSQL"))
            {
                mysqlParameters = new List<MySqlParameter>();
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        public bool Execute()
        {
            bool result = false;

            using (var command = new SqlCommand(queryString, easySession.Connection, easySession.Transaction))
            {
                command.CommandType = commandType;
                command.Parameters.AddRange(sqlParameters.ToArray());
                queryString = this.ProcessQuery(queryString);
                command.CommandText = queryString;
                
                try
                {
                    command.ExecuteNonQuery();
                    easySession.SetCommit = true;
                    result = true;
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    result = false;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected string ProcessQuery(string query)
        {
            if (dbType.Equals("SQL"))
            {
                query = query.Replace(':', '@');
            }
            else if (dbType.Equals("ORACLE") || dbType.Equals("MYSQL"))
            {
                query = query.Replace('@', ':');
            }
            
            return query;
        }

        public IQuery SetParameter(string name, object val, Type type)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType(type.ToString()), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType(type.ToString()), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType(type.ToString()), val);
                mysqlParameters.Add(mysqlParameter);
            }

            return this;
        }

        //public IQuery SetParameter(int position, object val, Type type)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType(type.ToString()), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType(type.ToString()), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType(type.ToString()), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }

        //    return this;
        //}

        public IQuery SetParameter<T>(string name, T val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType(typeof(T).ToString()), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType(typeof(T).ToString()), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType(typeof(T).ToString()), val);
                mysqlParameters.Add(mysqlParameter);
            }
            
            return this;
        }

        //public IQuery SetParameter<T>(int position, T val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType(typeof(T).ToString()), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType(typeof(T).ToString()), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType(typeof(T).ToString()), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }

        //    return this;
        //}

        public IQuery SetParameterList(string name, ICollection val, Type type)
        {
            string queryPart = string.Empty;
            string seperator = string.Empty;
            int count = 0;
            if (dbType.Equals("SQL"))
            {
                foreach (object objval in val)
                {
                    if (count != 0)
                    {
                        seperator = ", ";
                    }
                    string newName = name + count;
                    queryPart = queryPart + seperator + "@" + newName;
                    var sqlParameter = SqlParameters.CreateInputParameter("@" + newName, SqlDBTypes.GetDbType(type.ToString()), objval);
                    sqlParameters.Add(sqlParameter);
                    count++;
                }

                queryString = queryString.Replace(':', '@');
                queryString = queryString.Replace("@" + name, queryPart);
            }
            else if (dbType.Equals("ORACLE"))
            {
                foreach (object objval in val)
                {
                    if (count != 0)
                    {
                        seperator = ", ";
                    }
                    string newName = name + count;
                    queryPart = queryPart + seperator + ":" + newName;
                    var sqlParameter = OracleParameters.CreateInputParameter(":" + newName, OracleDBTypes.GetDbType(type.ToString()), objval);
                    oracleParameters.Add(sqlParameter);
                    count++;
                }

                queryString = queryString.Replace('@', ':');
                queryString = queryString.Replace(":" + name, queryPart);
            }
            else if (dbType.Equals("MYSQL"))
            {
                foreach (object objval in val)
                {
                    if (count != 0)
                    {
                        seperator = ", ";
                    }
                    string newName = name + count;
                    queryPart = queryPart + seperator + ":" + newName;
                    var sqlParameter = MySqlParameters.CreateInputParameter(":" + newName, MySqlDBTypes.GetDbType(type.ToString()), objval);
                    mysqlParameters.Add(sqlParameter);
                    count++;
                }

                queryString = queryString.Replace('@', ':');
                queryString = queryString.Replace(":" + name, queryPart);
            }

            return this;
        }

        public IQuery SetParameterList(string name, object[] val, Type type)
        {
            string queryPart = string.Empty;
            string seperator = string.Empty;
            if (dbType.Equals("SQL"))
            {
                for (int i = 0; i < val.Length; i++)
                {
                    if (i != 0)
                    {
                        seperator = ", ";
                    }
                    string newName = name + i;
                    queryPart = queryPart + seperator + "@" + newName;
                    var sqlParameter = SqlParameters.CreateInputParameter("@" + newName, SqlDBTypes.GetDbType(type.ToString()), val[i]);
                    sqlParameters.Add(sqlParameter);
                }

                queryString = queryString.Replace(':', '@');
                queryString = queryString.Replace("@" + name, queryPart);
            }
            else if (dbType.Equals("ORACLE"))
            {
                for (int i = 0; i < val.Length; i++)
                {
                    if (i != 0)
                    {
                        seperator = ", ";
                    }
                    string newName = name + i;
                    queryPart = queryPart + seperator + ":" + newName;
                    var sqlParameter = OracleParameters.CreateInputParameter(":" + newName, OracleDBTypes.GetDbType(type.ToString()), val[i]);
                    oracleParameters.Add(sqlParameter);
                }

                queryString = queryString.Replace('@', ':');
                queryString = queryString.Replace(":" + name, queryPart);
            }
            else if (dbType.Equals("MYSQL"))
            {
                for (int i = 0; i < val.Length; i++)
                {
                    if (i != 0)
                    {
                        seperator = ", ";
                    }
                    string newName = name + i;
                    queryPart = queryPart + seperator + ":" + newName;
                    var sqlParameter = MySqlParameters.CreateInputParameter(":" + newName, MySqlDBTypes.GetDbType(type.ToString()), val[i]);
                    mysqlParameters.Add(sqlParameter);
                }

                queryString = queryString.Replace('@', ':');
                queryString = queryString.Replace(":" + name, queryPart);
            }
            return this;
        }

        public IQuery SetBinary(string name, byte[] val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType("byte"), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType("byte"), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType("byte"), val);
                mysqlParameters.Add(mysqlParameter);
            }
            
            return this;
        }

        //public IQuery SetBinary(int position, byte[] val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType("byte"), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType("byte"), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType("byte"), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }

        //    return this;
        //}

        public IQuery SetBoolean(string name, bool val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType("bool"), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType("bool"), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType("bool"), val);
                mysqlParameters.Add(mysqlParameter);
            }
            
            return this;
        }

        //public IQuery SetBoolean(int position, bool val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType("bool"), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType("bool"), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType("bool"), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }
            
        //    return this;
        //}

        public IQuery SetByte(string name, byte val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType("byte"), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType("byte"), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType("byte"), val);
                mysqlParameters.Add(mysqlParameter);
            }
            
            return this;
        }

        //public IQuery SetByte(int position, byte val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType("byte"), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType("byte"), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType("byte"), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }
            
        //    return this;
        //}

        public IQuery SetCharacter(string name, char val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType("char"), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType("char"), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType("char"), val);
                mysqlParameters.Add(mysqlParameter);
            }
           
            return this;
        }

        //public IQuery SetCharacter(int position, char val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType("char"), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType("char"), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType("char"), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }
            
        //    return this;
        //}

        public IQuery SetDateTime(string name, DateTime val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType("DateTime"), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType("DateTime"), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType("DateTime"), val);
                mysqlParameters.Add(mysqlParameter);
            }
            
            return this;
        }

        //public IQuery SetDateTime(int position, DateTime val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType("DateTime"), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType("DateTime"), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType("DateTime"), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }
            
        //    return this;
        //}

        public IQuery SetDecimal(string name, decimal val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType("decimal"), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType("decimal"), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType("decimal"), val);
                mysqlParameters.Add(mysqlParameter);
            }
            
            return this;
        }

        //public IQuery SetDecimal(int position, decimal val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType("decimal"), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType("decimal"), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType("decimal"), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }
            
        //    return this;
        //}

        public IQuery SetDouble(string name, double val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType("double"), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType("double"), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType("double"), val);
                mysqlParameters.Add(mysqlParameter);
            }
            
            return this;
        }

        //public IQuery SetDouble(int position, double val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType("double"), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType("double"), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType("double"), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }

        //    return this;
        //}

        public IQuery SetEnum(string name, Enum val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType("Enum"), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType("Enum"), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType("Enum"), val);
                mysqlParameters.Add(mysqlParameter);
            }
            
            return this;
        }

        //public IQuery SetEnum(int position, Enum val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType("Enum"), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType("Enum"), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType("Enum"), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }

        //    return this;
        //}

        public IQuery SetInt16(string name, short val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType("Int16"), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType("Int16"), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType("Int16"), val);
                mysqlParameters.Add(mysqlParameter);
            }

            return this;
        }

        //public IQuery SetInt16(int position, short val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType("Int16"), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType("Int16"), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType("Int16"), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }
            
        //    return this;
        //}

        public IQuery SetInt32(string name, int val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType("Int32"), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType("Int32"), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType("Int32"), val);
                mysqlParameters.Add(mysqlParameter);
            }
            
            return this;
        }

        //public IQuery SetInt32(int position, int val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType("Int32"), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType("Int32"), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType("Int32"), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }
            
        //    return this;
        //}

        public IQuery SetInt64(string name, long val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType("Int64"), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType("Int64"), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType("Int64"), val);
                mysqlParameters.Add(mysqlParameter);
            }
            
            return this;
        }

        //public IQuery SetInt64(int position, long val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType("Int64"), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType("Int64"), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType("Int64"), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }
            
        //    return this;
        //}

        public IQuery SetString(string name, string val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType("string"), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType("string"), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType("string"), val);
                mysqlParameters.Add(mysqlParameter);
            }
            
            return this;
        }

        //public IQuery SetString(int position, string val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType("string"), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType("string"), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType("string"), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }

        //    return this;
        //}

        public IQuery SetTime(string name, DateTime val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType("DateTime"), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType("DateTime"), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType("DateTime"), val);
                mysqlParameters.Add(mysqlParameter);
            }
            
            return this;
        }

        //public IQuery SetTime(int position, DateTime val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType("DateTime"), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType("DateTime"), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType("DateTime"), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }
            
        //    return this;
        //}

        public IQuery SetTimestamp(string name, DateTime val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType("DateTime"), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType("DateTime"), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType("DateTime"), val);
                mysqlParameters.Add(mysqlParameter);
            }
            
            return this;
        }

        //public IQuery SetTimestamp(int position, DateTime val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType("DateTime"), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType("DateTime"), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType("DateTime"), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }
            
        //    return this;
        //}

        public IQuery SetGuid(string name, Guid val)
        {
            if (dbType.Equals("SQL"))
            {
                var sqlParameter = SqlParameters.CreateInputParameter("@" + name, SqlDBTypes.GetDbType("Guid"), val);
                sqlParameters.Add(sqlParameter);
            }
            else if (dbType.Equals("ORACLE"))
            {
                var oracleParameter = OracleParameters.CreateInputParameter(":" + name, OracleDBTypes.GetDbType("Guid"), val);
                oracleParameters.Add(oracleParameter);
            }
            else if (dbType.Equals("MYSQL"))
            {
                var mysqlParameter = MySqlParameters.CreateInputParameter(":" + name, MySqlDBTypes.GetDbType("Guid"), val);
                mysqlParameters.Add(mysqlParameter);
            }

            return this;
        }

        //public IQuery SetGuid(int position, Guid val)
        //{
        //    positions.Add(position);
        //    if (dbType.Equals("SQL"))
        //    {
        //        var sqlParameter = SqlParameters.CreateInputParameter("@param", SqlDBTypes.GetSQLDBType("Guid"), val);
        //        sqlParameters.Add(sqlParameter);
        //    }
        //    else if (dbType.Equals("ORACLE"))
        //    {
        //        var oracleParameter = OracleParameters.CreateInputParameter(":param", OracleDBTypes.GetOraDbType("Guid"), val);
        //        oracleParameters.Add(oracleParameter);
        //    }
        //    else if (dbType.Equals("MYSQL"))
        //    {
        //        var mysqlParameter = MySqlParameters.CreateInputParameter(":param", MySqlDBTypes.GetMySqlDbType("Guid"), val);
        //        mysqlParameters.Add(mysqlParameter);
        //    }
            
        //    return this;
        //}
    }
}
