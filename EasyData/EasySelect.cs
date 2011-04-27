using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using EasyData.Attributes;
using System.Reflection;
using EasyData.DB;
using System.Collections;
using System.Data.OracleClient;
using Devart.Data.MySql;
using EasyData.DB.Sql;
using EasyData.DB.Oracle;
using EasyData.DB.MySql;
using EasyData.Core;


namespace EasyData
{
    /// <summary>
    /// Provide the read functionality for the EasyData
    /// </summary>
    /// <typeparam name="T">Type of the entity class</typeparam>
    public class EasySelect<T>
    {
        string projPath = ConfigurationSettings.AppSettings[Constants.PROJECT_PATH];
        string assemblyPath = ConfigurationSettings.AppSettings[Constants.ASSEMBLY_PATH];
        string dbType = ConfigurationSettings.AppSettings[Constants.DB_TYPE];

        static string pKeyColumn;
        static string fKeyColumn;

        static string aKeyColumn1;
        static string aKeyColumn2;

        //static object pKeyValue;
        //static object propertyValue;

        static object propertyValueByInstance;

        static string propertyTypeByType;
        static string propertyTypeByInstance;

        static string propertyColumnByInstance;
        static string propertyColumnByType;

        static EasyLoad easyLoadGlobal;

        /// <summary>
        /// Finds the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        public List<T> FindAll(Type type, string where, EasySession easySession, EasyLoad easyLoad)
        {
            easyLoadGlobal = easyLoad;
            List<T> returnList = null;

            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SqlFindAll(type, where, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleFindAll(type, where, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlFindAll(type, where, easySession);
            }

            return returnList;
        }

        /// <summary>
        /// Finds the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="id">The id.</param>
        /// <param name="easySession">The easy session.</param>
        /// <param name="easyLoad">The easy load.</param>
        /// <returns></returns>
        public T Find(Type type, object id, EasySession easySession, EasyLoad easyLoad)
        {
            easyLoadGlobal = easyLoad;
            object retObject = Activator.CreateInstance(typeof(T));
            T returnObject = (T)Convert.ChangeType(retObject, typeof(T));

            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SqlFind(type, id, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleFind(type, id, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlFind(type, id, easySession);
            }

            return returnObject;
        }

        /// <summary>
        /// Finds the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="easySession">The easy session.</param>
        /// <param name="easyLoad">The easy load.</param>
        /// <returns></returns>
        public T Find(T instance, EasySession easySession, EasyLoad easyLoad)
        {
            easyLoadGlobal = easyLoad;
            object retObject = Activator.CreateInstance(typeof(T));
            T returnObject = (T)Convert.ChangeType(retObject, typeof(T));

            this.SetPrimaryKeyValue(instance);
            Type type = instance.GetType();

            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SqlFind(type, propertyValueByInstance, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleFind(type, propertyValueByInstance, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlFind(type, propertyValueByInstance, easySession);
            }

            return returnObject;
        }

        /// <summary>
        /// Finds the by property.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="property">The property.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        public List<T> FindByProperty(T instance, string property, EasySession easySession, EasyLoad easyLoad)
        {
            easyLoadGlobal = easyLoad;
            List<T> returnList = null;

            SetPropertyValue(instance, property);
            Type type = instance.GetType();
            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SqlFindProperty(type, property, propertyValueByInstance, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleFindProperty(type, property, propertyValueByInstance, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlFindProperty(type, property, propertyValueByInstance, easySession);
            }

            return returnList;
        }

        /// <summary>
        /// Finds the by property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        public List<T> FindByProperty(Type type, string property, object value, EasySession easySession, EasyLoad easyLoad)
        {
            easyLoadGlobal = easyLoad;
            List<T> returnList = null;

            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SqlFindProperty(type, property, value, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleFindProperty(type, property, value, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlFindProperty(type, property, value, easySession);
            }

            return returnList;
        }

        /// <summary>
        /// Counts the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        public int Count(Type type, EasySession easySession)
        {
            int count = 0;

            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SqlCount(type, null, null, null, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleCount(type, null, null, null, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlCount(type, null, null, null, easySession);
            }

            return count;
        }

        /// <summary>
        /// Counts the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="where">The where.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        public int Count(Type type, string where, EasySession easySession)
        {
            int count = 0;

            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SqlCount(type, where, null, null, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleCount(type, where, null, null, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlCount(type, where, null, null, easySession);
            }

            return count;
        }

        /// <summary>
        /// Counts the by property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        public int CountByProperty(Type type, string property, object value, EasySession easySession)
        {
            int count = 0;

            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SqlCount(type, null, value, property, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleCount(type, null, value, property, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlCount(type, null, value, property, easySession);
            }

            return count;
        }

        /// <summary>
        /// Counts the by property.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="property">The property.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        public int CountByProperty(T instance, string property, EasySession easySession)
        {
            int count = 0;
            SetPropertyValue(instance, property);
            Type type = instance.GetType();

            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SqlCount(type, null, propertyValueByInstance, property, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleCount(type, null, propertyValueByInstance, property, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlCount(type, null, propertyValueByInstance, property, easySession);
            }

            return count;
        }

        /// <summary>
        /// Existses the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        public bool Exists(Type type, EasySession easySession)
        {
            bool isExists = false;

            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SqlExists(type, null, null, null, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleExists(type, null, null, null, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlExists(type, null, null, null, easySession);
            }

            return isExists;
        }

        /// <summary>
        /// Existses the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="where">The where.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        public bool Exists(Type type, string where, EasySession easySession)
        {
            bool isExists = false;

            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SqlExists(type, where, null, null, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleExists(type, where, null, null, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlExists(type, where, null, null, easySession);
            }

            return isExists;
        }

        /// <summary>
        /// Existses the by property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        public bool ExistsByProperty(Type type, string property, object value, EasySession easySession)
        {
            bool isExists = false;

            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SqlExists(type, null, value, property, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleExists(type, null, value, property, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlExists(type, null, value, property, easySession);
            }

            return isExists;
        }

        /// <summary>
        /// Existses the by property.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="property">The property.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        public bool ExistsByProperty(T instance, string property, EasySession easySession)
        {
            bool isExists = false;
            SetPropertyValue(instance, property);
            Type type = instance.GetType();

            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SqlExists(type, null, propertyValueByInstance, property, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleExists(type, null, propertyValueByInstance, property, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlExists(type, null, propertyValueByInstance, property, easySession);
            }

            return isExists;
        }

        /// <summary>
        /// SQLs the exists.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="where">The where.</param>
        /// <param name="value">The value.</param>
        /// <param name="property">The property.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected bool SqlExists(Type type, string where, object value, string property, EasySession easySession)
        {
            string query = string.Empty;
            object count;
            bool isExists = false;

            using (var command = this.CountQueryBuilder(type, where, value, property, query, easySession))
            {
                try
                {
                    count = command.ExecuteScalar();
                    easySession.SetCommit = true;
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }

            int returnCount = Convert.ToInt32(count);
            if (returnCount == 0)
            {
                isExists = false;
            }
            else
            {
                isExists = true;
            }

            return isExists;
        }

        /// <summary>
        /// SQLs the exists.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="property">The property.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected bool OracleExists(Type type, string where, object value, string property, EasySession easySession)
        {
            string query = string.Empty;
            object count;
            bool isExists = false;

            using (var command = this.OracleCountQueryBuilder(type, where, value, property, query, easySession))
            {
                try
                {
                    count = command.ExecuteScalar();
                    easySession.SetCommit = true;
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }

            int returnCount = Convert.ToInt32(count);
            if (returnCount == 0)
            {
                isExists = false;
            }
            else
            {
                isExists = true;
            }

            return isExists;
        }

        /// <summary>
        /// SQLs the exists.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="property">The property.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected bool MySqlExists(Type type, string where, object value, string property, EasySession easySession)
        {
            string query = string.Empty;
            object count;
            bool isExists = false;

            using (var command = this.MySqlCountQueryBuilder(type, where, value, property, query, easySession))
            {
                try
                {
                    count = command.ExecuteScalar();
                    easySession.SetCommit = true;
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }

            int returnCount = Convert.ToInt32(count);
            if (returnCount == 0)
            {
                isExists = false;
            }
            else
            {
                isExists = true;
            }

            return isExists;
        }

        /// <summary>
        /// SQLs the count.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="property">The property.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected int SqlCount(Type type, string where, object value, string property, EasySession easySession)
        {
            string query = string.Empty;
            object count;

            using (var command = this.CountQueryBuilder(type, where, value, property, query, easySession))
            {
                try
                {
                    count = command.ExecuteScalar();
                    easySession.SetCommit = true;
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }

            int returnCount = Convert.ToInt32(count);
            return returnCount;
        }

        /// <summary>
        /// SQLs the count.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="property">The property.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected int OracleCount(Type type, string where, object value, string property, EasySession easySession)
        {
            string query = string.Empty;
            object count;

            using (var command = this.OracleCountQueryBuilder(type, where, value, property, query, easySession))
            {
                try
                {
                    count = command.ExecuteScalar();
                    easySession.SetCommit = true;
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }

            int returnCount = Convert.ToInt32(count);
            return returnCount;
        }

        /// <summary>
        /// SQLs the count.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="property">The property.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected int MySqlCount(Type type, string where, object value, string property, EasySession easySession)
        {
            string query = string.Empty;
            object count;

            using (var command = this.MySqlCountQueryBuilder(type, where, value, property, query, easySession))
            {
                try
                {
                    count = command.ExecuteScalar();
                    easySession.SetCommit = true;
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }

            int returnCount = Convert.ToInt32(count);
            return returnCount;
        }

        /// <summary>
        /// Counts the query builder.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="property">The property.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected SqlCommand CountQueryBuilder(Type type, string where, object value, string property, string query, EasySession easySession)
        {
            SqlCommand command = new SqlCommand(query, easySession.Connection, easySession.Transaction);
            command.CommandType = CommandType.Text;

            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(type, typeof(EasyDataAttribute));
            StringBuilder sbQuery = new StringBuilder();
            
            sbQuery.Append("SELECT COUNT(*) FROM ");
            if (easyAttr.Schema != null)
            {
                sbQuery.Append(easyAttr.Schema);
                sbQuery.Append(".");
            }
            if (easyAttr.Table != null)
            {
                sbQuery.Append(easyAttr.Table);
            }
            else
            {
                sbQuery.Append(type.Name);
            }

            StringBuilder sbWhere = new StringBuilder();
            if (type != null && property != null)
            {
                GetPropertyColumn(type, property);

                command.Parameters.Add(SqlParameters.CreateInputParameter("@" + propertyColumnByType + "_", SqlDBTypes.GetDbType(propertyTypeByType), value));

                sbWhere.Append(" WHERE ");
                if (easyAttr.Table != null)
                {
                    sbWhere.Append(easyAttr.Table);
                }
                else
                {
                    sbWhere.Append(type.Name);
                }
                sbWhere.Append(".");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append(" = ");
                sbWhere.Append("@");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append("_");
            }
            else if (type != null && property == null && value != null)
            {
                GetPrimaryKeyColumn(type);

                command.Parameters.Add(SqlParameters.CreateInputParameter("@" + propertyColumnByType + "_", SqlDBTypes.GetDbType(propertyTypeByType), value));

                sbWhere.Append(" WHERE ");
                if (easyAttr.Table != null)
                {
                    sbWhere.Append(easyAttr.Table);
                }
                else
                {
                    sbWhere.Append(type.Name);
                }
                sbWhere.Append(".");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append(" = ");
                sbWhere.Append("@");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append("_");
            }
            else if (where != null)
            {
                sbWhere.Append(" WHERE ");
                sbWhere.Append(where);
            }
            sbQuery.Append(sbWhere.ToString());

            command.CommandText = sbQuery.ToString();
            return command;
        }

        /// <summary>
        /// Counts the query builder.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="property">The property.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected OracleCommand OracleCountQueryBuilder(Type type, string where, object value, string property, string query, EasySession easySession)
        {
            OracleCommand command = new OracleCommand(query, easySession.OConnection, easySession.OTransaction);
            command.CommandType = CommandType.Text;

            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(type, typeof(EasyDataAttribute));
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT COUNT(*) FROM ");
            if (easyAttr.Schema != null)
            {
                sbQuery.Append(easyAttr.Schema);
                sbQuery.Append(".");
            }
            if (easyAttr.Table != null)
            {
                sbQuery.Append(easyAttr.Table);
            }
            else
            {
                sbQuery.Append(type.Name);
            }

            StringBuilder sbWhere = new StringBuilder();
            if (type != null && property != null)
            {
                GetPropertyColumn(type, property);

                command.Parameters.Add(OracleParameters.CreateInputParameter(":" + propertyColumnByType + "_", OracleDBTypes.GetDbType(propertyTypeByType), value));

                sbWhere.Append(" WHERE ");
                if (easyAttr.Table != null)
                {
                    sbWhere.Append(easyAttr.Table);
                }
                else
                {
                    sbWhere.Append(type.Name);
                }
                sbWhere.Append(".");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append(" = ");
                sbWhere.Append(":");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append("_");
            }
            else if (type != null && property == null && value != null)
            {
                GetPrimaryKeyColumn(type);

                command.Parameters.Add(OracleParameters.CreateInputParameter(":" + propertyColumnByType + "_", OracleDBTypes.GetDbType(propertyTypeByType), value));

                sbWhere.Append(" WHERE ");
                if (easyAttr.Table != null)
                {
                    sbWhere.Append(easyAttr.Table);
                }
                else
                {
                    sbWhere.Append(type.Name);
                }
                sbWhere.Append(".");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append(" = ");
                sbWhere.Append(":");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append("_");
            }
            else if (where != null)
            {
                sbWhere.Append(" WHERE ");
                sbWhere.Append(where);
            }
            sbQuery.Append(sbWhere.ToString());

            command.CommandText = sbQuery.ToString();
            return command;
        }

        /// <summary>
        /// Counts the query builder.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="property">The property.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected MySqlCommand MySqlCountQueryBuilder(Type type, string where, object value, string property, string query, EasySession easySession)
        {
            MySqlCommand command = new MySqlCommand(query, easySession.MConnection, easySession.MTransaction);
            command.CommandType = CommandType.Text;

            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(type, typeof(EasyDataAttribute));
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT COUNT(*) FROM ");
            if (easyAttr.Schema != null)
            {
                sbQuery.Append(easyAttr.Schema);
                sbQuery.Append(".");
            }
            if (easyAttr.Table != null)
            {
                sbQuery.Append(easyAttr.Table);
            }
            else
            {
                sbQuery.Append(type.Name);
            }

            StringBuilder sbWhere = new StringBuilder();
            if (type != null && property != null)
            {
                GetPropertyColumn(type, property);

                command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + propertyColumnByType + "_", MySqlDBTypes.GetDbType(propertyTypeByType), value));

                sbWhere.Append(" WHERE ");
                if (easyAttr.Table != null)
                {
                    sbWhere.Append(easyAttr.Table);
                }
                else
                {
                    sbWhere.Append(type.Name);
                }
                sbWhere.Append(".");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append(" = ");
                sbWhere.Append(":");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append("_");
            }
            else if (type != null && property == null && value != null)
            {
                GetPrimaryKeyColumn(type);

                command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + propertyColumnByType + "_", MySqlDBTypes.GetDbType(propertyTypeByType), value));

                sbWhere.Append(" WHERE ");
                if (easyAttr.Table != null)
                {
                    sbWhere.Append(easyAttr.Table);
                }
                else
                {
                    sbWhere.Append(type.Name);
                }
                sbWhere.Append(".");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append(" = '");
                sbWhere.Append(":");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append("_");
            }
            else if (where != null)
            {
                sbWhere.Append(" WHERE ");
                sbWhere.Append(where);
            }
            sbQuery.Append(sbWhere.ToString());

            command.CommandText = sbQuery.ToString();
            return command;
        }

        /// <summary>
        /// SQLs the find property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected List<T> SqlFindProperty(Type type, string property, object value, EasySession easySession)
        {
            string query = string.Empty;
            List<T> returnResult = new List<T>();

            using (var command = this.SelectQueryBuilder(type, null, property, value, query, easySession))
            {
                try
                {
                    using (SqlDataReader resultDataReader = command.ExecuteReader())
                    {
                        while (true)
                        {
                            object resultObject = Activator.CreateInstance(typeof(T));
                            resultObject = CreateInstance(resultDataReader, type);
                            if (resultObject == null)
                            {
                                break;
                            }
                            else
                            {
                                returnResult.Add((T)Convert.ChangeType(resultObject, typeof(T)));
                            }
                        }
                        resultDataReader.Close();
                        easySession.SetCommit = true;
                    }
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }
            return returnResult;
        }

        /// <summary>
        /// SQLs the find property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected List<T> OracleFindProperty(Type type, string property, object value, EasySession easySession)
        {
            string query = string.Empty;
            List<T> returnResult = new List<T>();

            using (var command = this.OracleSelectQueryBuilder(type, null, property, value, query, easySession))
            {
                try
                {
                    using (OracleDataReader resultDataReader = command.ExecuteReader())
                    {
                        while (true)
                        {
                            object resultObject = Activator.CreateInstance(typeof(T));
                            resultObject = OracleCreateInstance(resultDataReader, type);
                            if (resultObject == null)
                            {
                                break;
                            }
                            else
                            {
                                returnResult.Add((T)Convert.ChangeType(resultObject, typeof(T)));
                            }
                        }
                        resultDataReader.Close();
                        easySession.SetCommit = true;
                    }
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }
            return returnResult;
        }

        /// <summary>
        /// SQLs the find property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected List<T> MySqlFindProperty(Type type, string property, object value, EasySession easySession)
        {
            string query = string.Empty;
            List<T> returnResult = new List<T>();

            using (var command = this.MySqlSelectQueryBuilder(type, null, property, value, query, easySession))
            {
                try
                {
                    using (MySqlDataReader resultDataReader = command.ExecuteReader())
                    {
                        while (true)
                        {
                            object resultObject = Activator.CreateInstance(typeof(T));
                            resultObject = MySqlCreateInstance(resultDataReader, type);
                            if (resultObject == null)
                            {
                                break;
                            }
                            else
                            {
                                returnResult.Add((T)Convert.ChangeType(resultObject, typeof(T)));
                            }
                        }
                        resultDataReader.Close();
                        easySession.SetCommit = true;
                    }
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }
            return returnResult;
        }

        /// <summary>
        /// SQLs the find.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="id">The id.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected T SqlFind(Type type, object id, EasySession easySession)
        {
            string query = string.Empty;
            object resultObject = Activator.CreateInstance(typeof(T));

            using (var command = this.SelectQueryBuilder(type, null, null, id, query, easySession))
            {
                try
                {
                    using (SqlDataReader resultDataReader = command.ExecuteReader())
                    {
                        resultObject = CreateInstance(resultDataReader, type);
                        resultDataReader.Close();
                        easySession.SetCommit = true;
                    }
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }
            return (T)Convert.ChangeType(resultObject, typeof(T));
        }

        /// <summary>
        /// SQLs the find.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="id">The id.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected T OracleFind(Type type, object id, EasySession easySession)
        {
            string query = string.Empty;
            object resultObject = Activator.CreateInstance(typeof(T));

            using (var command = this.OracleSelectQueryBuilder(type, null, null, id, query, easySession))
            {
                try
                {
                    using (OracleDataReader resultDataReader = command.ExecuteReader())
                    {
                        resultObject = OracleCreateInstance(resultDataReader, type);
                        resultDataReader.Close();
                        easySession.SetCommit = true;
                    }
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }
            return (T)Convert.ChangeType(resultObject, typeof(T));
        }

        /// <summary>
        /// SQLs the find.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="id">The id.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected T MySqlFind(Type type, object id, EasySession easySession)
        {
            string query = string.Empty;
            object resultObject = Activator.CreateInstance(typeof(T));

            using (var command = this.MySqlSelectQueryBuilder(type, null, null, id, query, easySession))
            {
                try
                {
                    using (MySqlDataReader resultDataReader = command.ExecuteReader())
                    {
                        resultObject = MySqlCreateInstance(resultDataReader, type);
                        resultDataReader.Close();
                        easySession.SetCommit = true;
                    }
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }
            return (T)Convert.ChangeType(resultObject, typeof(T));
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected List<T> SqlFindAll(Type type, string where, EasySession easySession)
        {
            string query = string.Empty;
            List<T> returnResult = new List<T>();
            
            using (var command = this.SelectQueryBuilder(type, where, null, null, query, easySession))
            {
                try
                {
                    using (SqlDataReader resultDataReader = command.ExecuteReader())
                    {
                        while (true)
                        {
                            object resultObject = Activator.CreateInstance(typeof(T));
                            resultObject = CreateInstance(resultDataReader, type);
                            if (resultObject == null)
                            {
                                break;
                            }
                            else
                            {
                                returnResult.Add((T)Convert.ChangeType(resultObject, typeof(T)));
                            }
                        }
                        resultDataReader.Close();
                        easySession.SetCommit = true;
                    }
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }
            return returnResult;
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected List<T> OracleFindAll(Type type, string where, EasySession easySession)
        {
            string query = string.Empty;
            List<T> returnResult = new List<T>();

            using (var command = this.OracleSelectQueryBuilder(type, where, null, null, query, easySession))
            {
                try
                {
                    using (OracleDataReader resultDataReader = command.ExecuteReader())
                    {
                        while (true)
                        {
                            object resultObject = Activator.CreateInstance(typeof(T));
                            resultObject = OracleCreateInstance(resultDataReader, type);
                            if (resultObject == null)
                            {
                                break;
                            }
                            else
                            {
                                returnResult.Add((T)Convert.ChangeType(resultObject, typeof(T)));
                            }
                        }
                        resultDataReader.Close();
                        easySession.SetCommit = true;
                    }
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }
            return returnResult;
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        protected List<T> MySqlFindAll(Type type, string where, EasySession easySession)
        {
            string query = string.Empty;
            List<T> returnResult = new List<T>();

            using (var command = this.MySqlSelectQueryBuilder(type, where, null, null, query, easySession))
            {
                try
                {
                    using (MySqlDataReader resultDataReader = command.ExecuteReader())
                    {
                        while (true)
                        {
                            object resultObject = Activator.CreateInstance(typeof(T));
                            resultObject = MySqlCreateInstance(resultDataReader, type);
                            if (resultObject == null)
                            {
                                break;
                            }
                            else
                            {
                                returnResult.Add((T)Convert.ChangeType(resultObject, typeof(T)));
                            }
                        }
                        resultDataReader.Close();
                        easySession.SetCommit = true;
                    }
                }
                catch (Exception ex)
                {
                    easySession.SetRollback = true;
                    throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
                }
            }
            return returnResult;
        }

        /// <summary>
        /// Creates the result instance.
        /// </summary>
        /// <param name="resultDataReader">The result data reader.</param>
        /// <param name="type">The type.</param>
        /// <returns>T type instance</returns>
        protected T CreateInstance(SqlDataReader resultDataReader, Type type)
        {
            object resultObject = Activator.CreateInstance(typeof(T));

            if (resultDataReader.Read())
            {
                EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(type, typeof(EasyDataAttribute));
                int intFieldCount = resultDataReader.FieldCount;

                if (intFieldCount != 0)
                {
                    PropertyInfo[] classProperties = type.GetProperties();
                    foreach (PropertyInfo classProperty in classProperties)
                    {
                        Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                        for (int i = 0; i < customAttr.Length; i++)
                        {
                            string columnName = string.Empty;
                            Type attrType = customAttr[i].GetType();
                            switch (attrType.Name)
                            {
                                case "PrimaryKeyAttribute":
                                    PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!priKeyAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }

                                    if (priKeyAttr.Column != null)
                                    {
                                        if (easyAttr.Table != null)
                                        {
                                            columnName = easyAttr.Table + "_" + priKeyAttr.Column;
                                        }
                                        else
                                        {
                                            columnName = type.Name + "_" + priKeyAttr.Column;
                                        }
                                    }
                                    else
                                    {
                                        if (easyAttr.Table != null)
                                        {
                                            columnName = easyAttr.Table + "_" + classProperty.Name;
                                        }
                                        else
                                        {
                                            columnName = type.Name + "_" + classProperty.Name;
                                        }
                                    }

                                    for (int l = 0; l < intFieldCount; l++)
                                    {
                                        if (columnName == resultDataReader.GetName(l))
                                        {
                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                            else { PropertyForMainCls.SetValue(resultObject, DataReader<SqlDataReader>.GetDBValue(resultDataReader, columnName), null); }

                                            break;
                                        }
                                    }
                                    break;

                                case "PropertyAttribute":
                                    PropertyAttribute propAttr = (PropertyAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!propAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }

                                    if (propAttr.Column != null)
                                    {
                                        if (easyAttr.Table != null)
                                        {
                                            columnName = easyAttr.Table + "_" + propAttr.Column;
                                        }
                                        else
                                        {
                                            columnName = type.Name + "_" + propAttr.Column;
                                        }
                                    }
                                    else
                                    {
                                        if (easyAttr.Table != null)
                                        {
                                            columnName = easyAttr.Table + "_" + classProperty.Name;
                                        }
                                        else
                                        {
                                            columnName = type.Name + "_" + classProperty.Name;
                                        }
                                    }

                                    for (int l = 0; l < intFieldCount; l++)
                                    {
                                        if (columnName == resultDataReader.GetName(l))
                                        {
                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                            else { PropertyForMainCls.SetValue(resultObject, DataReader<SqlDataReader>.GetDBValue(resultDataReader, columnName), null); }

                                            break;
                                        }
                                    }
                                    break;

                                case "BelongsToAttribute":
                                    BelongsToAttribute belongAttr = (BelongsToAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!belongAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }
                                    else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                                    {
                                        break;
                                    }

                                    Type foreignClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                                    PropertyInfo[] foreignClassProperties = foreignClassPropType.GetProperties();
                                    EasyDataAttribute easySubAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignClassPropType, typeof(EasyDataAttribute));

                                    foreach (PropertyInfo foreignClassProperty in foreignClassProperties)
                                    {
                                        Attribute[] foreignCustomAttr = Attribute.GetCustomAttributes(foreignClassProperty);
                                        for (int j = 0; j < foreignCustomAttr.Length; j++)
                                        {
                                            Type foreignAttrType = foreignCustomAttr[j].GetType();
                                            switch (foreignAttrType.Name)
                                            {
                                                case "PrimaryKeyAttribute":

                                                    PrimaryKeyAttribute priSubKeyAttr = (PrimaryKeyAttribute)foreignCustomAttr[i];
                                                    if (priSubKeyAttr.Column != null)
                                                    {
                                                        if (easySubAttr.Table != null)
                                                        {
                                                            columnName = easySubAttr.Table + "_" + priSubKeyAttr.Column;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignClassPropType.Name + "_" + priSubKeyAttr.Column;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (easySubAttr.Table != null)
                                                        {
                                                            columnName = easySubAttr.Table + "_" + foreignClassProperty.Name;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignClassPropType.Name + "_" + foreignClassProperty.Name;
                                                        }
                                                    }

                                                    for (int l = 0; l < intFieldCount; l++)
                                                    {
                                                        if (columnName == resultDataReader.GetName(l))
                                                        {
                                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                                            object SubClass = PropertyForMainCls.GetValue(resultObject, null);
                                                            System.Reflection.PropertyInfo PropertyForSubCls = foreignClassPropType.GetProperty(foreignClassProperty.Name);
                                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                            else { PropertyForSubCls.SetValue(SubClass, DataReader<SqlDataReader>.GetDBValue(resultDataReader, columnName), null); }
                                                            break;
                                                        }
                                                    }
                                                    break;

                                                case "PropertyAttribute":
                                                    PropertyAttribute propSubAttr = (PropertyAttribute)foreignCustomAttr[i];
                                                    if (propSubAttr.Column != null)
                                                    {
                                                        if (easySubAttr.Table != null)
                                                        {
                                                            columnName = easySubAttr.Table + "_" + propSubAttr.Column;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignClassPropType.Name + "_" + propSubAttr.Column;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (easySubAttr.Table != null)
                                                        {
                                                            columnName = easySubAttr.Table + "_" + foreignClassProperty.Name;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignClassPropType.Name + "_" + foreignClassProperty.Name;
                                                        }
                                                    }

                                                    for (int l = 0; l < intFieldCount; l++)
                                                    {
                                                        if (columnName == resultDataReader.GetName(l))
                                                        {
                                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                                            object SubClass = PropertyForMainCls.GetValue(resultObject, null);
                                                            System.Reflection.PropertyInfo PropertyForSubCls = foreignClassPropType.GetProperty(foreignClassProperty.Name);
                                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                            else { PropertyForSubCls.SetValue(SubClass, DataReader<SqlDataReader>.GetDBValue(resultDataReader, columnName), null); }
                                                            break;
                                                        }
                                                    }
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                    break;

                                case "OneToOneAttribute":
                                    OneToOneAttribute oneToOneAttr = (OneToOneAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!oneToOneAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }
                                    else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                                    {
                                        break;
                                    }

                                    Type foreignOneToOneClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                                    PropertyInfo[] foreignOneToOneClassProperties = foreignOneToOneClassPropType.GetProperties();
                                    EasyDataAttribute easyOneToOneAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignOneToOneClassPropType, typeof(EasyDataAttribute));

                                    foreach (PropertyInfo foreignOneToOneClassProperty in foreignOneToOneClassProperties)
                                    {
                                        Attribute[] foreignOneToOneCustomAttr = Attribute.GetCustomAttributes(foreignOneToOneClassProperty);
                                        for (int j = 0; j < foreignOneToOneCustomAttr.Length; j++)
                                        {
                                            Type foreignOneToOneAttrType = foreignOneToOneCustomAttr[j].GetType();
                                            switch (foreignOneToOneAttrType.Name)
                                            {
                                                case "PrimaryKeyAttribute":

                                                    PrimaryKeyAttribute priOneToOneKeyAttr = (PrimaryKeyAttribute)foreignOneToOneCustomAttr[i];
                                                    if (priOneToOneKeyAttr.Column != null)
                                                    {
                                                        if (easyOneToOneAttr.Table != null)
                                                        {
                                                            columnName = easyOneToOneAttr.Table + "_" + priOneToOneKeyAttr.Column;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignOneToOneClassPropType.Name + "_" + priOneToOneKeyAttr.Column;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (easyOneToOneAttr.Table != null)
                                                        {
                                                            columnName = easyOneToOneAttr.Table + "_" + foreignOneToOneClassProperty.Name;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignOneToOneClassPropType.Name + "_" + foreignOneToOneClassProperty.Name;
                                                        }
                                                    }

                                                    for (int l = 0; l < intFieldCount; l++)
                                                    {
                                                        if (columnName == resultDataReader.GetName(l))
                                                        {
                                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                                            object SubClass = PropertyForMainCls.GetValue(resultObject, null);
                                                            System.Reflection.PropertyInfo PropertyForSubCls = foreignOneToOneClassPropType.GetProperty(foreignOneToOneClassProperty.Name);
                                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                            else { PropertyForSubCls.SetValue(SubClass, DataReader<SqlDataReader>.GetDBValue(resultDataReader, columnName), null); }
                                                            break;
                                                        }
                                                    }
                                                    break;

                                                case "PropertyAttribute":
                                                    PropertyAttribute propOneToOneAttr = (PropertyAttribute)foreignOneToOneCustomAttr[i];
                                                    if (propOneToOneAttr.Column != null)
                                                    {
                                                        if (easyOneToOneAttr.Table != null)
                                                        {
                                                            columnName = easyOneToOneAttr.Table + "_" + propOneToOneAttr.Column;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignOneToOneClassPropType.Name + "_" + propOneToOneAttr.Column;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (easyOneToOneAttr.Table != null)
                                                        {
                                                            columnName = easyOneToOneAttr.Table + "_" + foreignOneToOneClassProperty.Name;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignOneToOneClassPropType.Name + "_" + foreignOneToOneClassProperty.Name;
                                                        }
                                                    }

                                                    for (int l = 0; l < intFieldCount; l++)
                                                    {
                                                        if (columnName == resultDataReader.GetName(l))
                                                        {
                                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                                            object SubClass = PropertyForMainCls.GetValue(resultObject, null);
                                                            System.Reflection.PropertyInfo PropertyForSubCls = foreignOneToOneClassPropType.GetProperty(foreignOneToOneClassProperty.Name);
                                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                            else { PropertyForSubCls.SetValue(SubClass, DataReader<SqlDataReader>.GetDBValue(resultDataReader, columnName), null); }
                                                            break;
                                                        }
                                                    }
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                    break;

                                case "HasAndBelongsToManyAttribute":
                                    HasAndBelongsToManyAttribute HasAndBelongsToManyAttr = (HasAndBelongsToManyAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!HasAndBelongsToManyAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }
                                    else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                                    {
                                        break;
                                    }

                                    IList listManyToMany = new List<object>();
                                    string tableManyToMany;
                                    if (easyAttr.Table != null)
                                    {
                                        tableManyToMany = easyAttr.Table;
                                    }
                                    else
                                    {
                                        tableManyToMany = type.Name;
                                    }

                                    this.GetPrimaryKeyColumn(type);
                                    pKeyColumn = propertyColumnByType;
                                    if (HasAndBelongsToManyAttr.ColumnKey != null)
                                    {
                                        aKeyColumn1 = HasAndBelongsToManyAttr.ColumnKey;
                                    }
                                    else
                                    {
                                        aKeyColumn1 = pKeyColumn;
                                    }

                                    string columnManyToMany = tableManyToMany + "_" + pKeyColumn;
                                    object pKeyValManyToMany = DataReader<SqlDataReader>.GetDBValue(resultDataReader, columnManyToMany);

                                    this.GetPrimaryKeyColumn(HasAndBelongsToManyAttr.MapType);
                                    fKeyColumn = propertyColumnByType;
                                    if (HasAndBelongsToManyAttr.ColumnRef != null)
                                    {
                                        aKeyColumn2 = HasAndBelongsToManyAttr.ColumnRef;
                                    }
                                    else
                                    {
                                        aKeyColumn2 = fKeyColumn;
                                    }

                                    EasyDataAttribute easyAttrManyToMany = (EasyDataAttribute)Attribute.GetCustomAttribute(HasAndBelongsToManyAttr.MapType, typeof(EasyDataAttribute));
                                    Type foreignHasAndBelongsClassPropType = HasAndBelongsToManyAttr.MapType;
                                    PropertyInfo[] foreignHasAndBelongsClassProperties = foreignHasAndBelongsClassPropType.GetProperties();
                                    StringBuilder sbQuery = new StringBuilder();
                                    StringBuilder sbSelect = new StringBuilder();
                                    StringBuilder sbJoin = new StringBuilder();
                                    string seperator = string.Empty;
                                    sbQuery.Append("SELECT ");

                                    foreach (PropertyInfo foreignHasAndBelongsClassProperty in foreignHasAndBelongsClassProperties)
                                    {
                                        Attribute[] foreignHasAndBelongsCustomAttr = Attribute.GetCustomAttributes(foreignHasAndBelongsClassProperty);
                                        for (int j = 0; j < foreignHasAndBelongsCustomAttr.Length; j++)
                                        {
                                            Type foreignHasAndBelongsAttrType = foreignHasAndBelongsCustomAttr[j].GetType();
                                            switch (foreignHasAndBelongsAttrType.Name)
                                            {
                                                case "PrimaryKeyAttribute":
                                                    PrimaryKeyAttribute primaryKeyAttr = (PrimaryKeyAttribute)foreignHasAndBelongsCustomAttr[j];
                                                    if (primaryKeyAttr.Column != null)
                                                    {
                                                        columnName = primaryKeyAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasAndBelongsClassProperty.Name;
                                                    }

                                                    if (sbSelect.Length != 0)
                                                    {
                                                        seperator = ", ";
                                                    }
                                                    sbSelect.Append(seperator);
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append(".");
                                                    sbSelect.Append(columnName);
                                                    sbSelect.Append(" AS ");
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append("_");
                                                    sbSelect.Append(columnName);
                                                    break;
                                                case "PropertyAttribute":
                                                    PropertyAttribute fPropAttr = (PropertyAttribute)foreignHasAndBelongsCustomAttr[j];
                                                    if (fPropAttr.Column != null)
                                                    {
                                                        columnName = fPropAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasAndBelongsClassProperty.Name;
                                                    }

                                                    if (sbSelect.Length != 0)
                                                    {
                                                        seperator = ", ";
                                                    }
                                                    sbSelect.Append(seperator);
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append(".");
                                                    sbSelect.Append(columnName);
                                                    sbSelect.Append(" AS ");
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append("_");
                                                    sbSelect.Append(columnName);
                                                    break;
                                                case "BelongsToAttribute":
                                                    BelongsToAttribute fBelongsToAttr = (BelongsToAttribute)foreignHasAndBelongsCustomAttr[j];
                                                    if (fBelongsToAttr.Column != null)
                                                    {
                                                        columnName = fBelongsToAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasAndBelongsClassProperty.Name;
                                                    }

                                                    if (sbSelect.Length != 0)
                                                    {
                                                        seperator = ", ";
                                                    }
                                                    sbSelect.Append(seperator);
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append(".");
                                                    sbSelect.Append(columnName);
                                                    sbSelect.Append(" AS ");
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append("_");
                                                    sbSelect.Append(columnName);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }

                                    sbQuery.Append(sbSelect.ToString());
                                    sbQuery.Append(" FROM ");
                                    if (easyAttr.Schema != null)
                                    {
                                        sbQuery.Append(easyAttr.Schema);
                                        sbQuery.Append(".");
                                    }
                                    string primaryTable = string.Empty;
                                    if (easyAttr.Table != null)
                                    {
                                        sbQuery.Append(easyAttr.Table);
                                        primaryTable = easyAttr.Table;
                                    }
                                    else
                                    {
                                        sbQuery.Append(type.Name);
                                        primaryTable = type.Name;
                                    }

                                    sbJoin.Append(" INNER JOIN ");
                                    if (HasAndBelongsToManyAttr.Schema != null)
                                    {
                                        sbJoin.Append(HasAndBelongsToManyAttr.Schema);
                                        sbJoin.Append(".");
                                    }
                                    sbJoin.Append(HasAndBelongsToManyAttr.Table);
                                    sbJoin.Append(" ON ");
                                    sbJoin.Append(primaryTable);
                                    sbJoin.Append(".");
                                    sbJoin.Append(pKeyColumn);
                                    sbJoin.Append(" = ");
                                    sbJoin.Append(HasAndBelongsToManyAttr.Table);
                                    sbJoin.Append(".");
                                    sbJoin.Append(aKeyColumn1);

                                    sbJoin.Append(" INNER JOIN ");
                                    if (easyAttrManyToMany.Schema != null)
                                    {
                                        sbJoin.Append(easyAttrManyToMany.Schema);
                                        sbJoin.Append(".");
                                    }
                                    string secondaryTable = string.Empty;
                                    if (easyAttrManyToMany.Table != null)
                                    {
                                        sbJoin.Append(easyAttrManyToMany.Table);
                                        secondaryTable = easyAttrManyToMany.Table;
                                    }
                                    else
                                    {
                                        sbQuery.Append(HasAndBelongsToManyAttr.MapType.Name);
                                        secondaryTable = HasAndBelongsToManyAttr.MapType.Name;
                                    }
                                    sbJoin.Append(" ON ");
                                    sbJoin.Append(HasAndBelongsToManyAttr.Table);
                                    sbJoin.Append(".");
                                    sbJoin.Append(aKeyColumn2);
                                    sbJoin.Append(" = ");
                                    sbJoin.Append(secondaryTable);
                                    sbJoin.Append(".");
                                    sbJoin.Append(fKeyColumn);

                                    sbQuery.Append(sbJoin.ToString());
                                    sbQuery.Append(" WHERE ");
                                    sbQuery.Append(primaryTable);
                                    sbQuery.Append(".");
                                    sbQuery.Append(pKeyColumn);
                                    sbQuery.Append(" = ");
                                    sbQuery.Append(pKeyValManyToMany);
                                    if (HasAndBelongsToManyAttr.Where != null)
                                    {
                                        sbQuery.Append(" AND ");
                                        sbQuery.Append(HasAndBelongsToManyAttr.Where);
                                    }
                                    if (HasAndBelongsToManyAttr.OrderBy != null)
                                    {
                                        sbQuery.Append(" ORDER BY ");
                                        sbQuery.Append(HasAndBelongsToManyAttr.OrderBy);
                                    }
                                    using (EasySession newSession = new EasySession())
                                    {
                                        using (var subCommand = new SqlCommand(sbQuery.ToString(), newSession.Connection, newSession.Transaction))
                                        {
                                            try
                                            {
                                                using (SqlDataReader subResultDataReader = subCommand.ExecuteReader())
                                                {
                                                    while (true)
                                                    {
                                                        object subResultObject = Activator.CreateInstance(HasAndBelongsToManyAttr.MapType);
                                                        if (subResultDataReader.Read())
                                                        {
                                                            int fieldCount = subResultDataReader.FieldCount;
                                                            if (fieldCount != 0)
                                                            {
                                                                Type foreignSubHasAndBelongsClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, HasAndBelongsToManyAttr.MapType.FullName);
                                                                PropertyInfo[] foreignSubHasAndBelongsClassProperties = foreignSubHasAndBelongsClassPropType.GetProperties();
                                                                EasyDataAttribute easyAttrSubHasAndBelongs = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignSubHasAndBelongsClassPropType, typeof(EasyDataAttribute));

                                                                foreach (PropertyInfo foreignSubHasAndBelongsClassProperty in foreignSubHasAndBelongsClassProperties)
                                                                {
                                                                    Attribute[] foreignSubHasAndBelongsCustomAttr = Attribute.GetCustomAttributes(foreignSubHasAndBelongsClassProperty);
                                                                    for (int j = 0; j < foreignSubHasAndBelongsCustomAttr.Length; j++)
                                                                    {
                                                                        Type foreignSubHasAndBelongsAttrType = foreignSubHasAndBelongsCustomAttr[j].GetType();
                                                                        switch (foreignSubHasAndBelongsAttrType.Name)
                                                                        {
                                                                            case "PrimaryKeyAttribute":

                                                                                PrimaryKeyAttribute priSubKeyAttr = (PrimaryKeyAttribute)foreignSubHasAndBelongsCustomAttr[i];
                                                                                if (priSubKeyAttr.Column != null)
                                                                                {
                                                                                    if (easyAttrSubHasAndBelongs.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasAndBelongs.Table + "_" + priSubKeyAttr.Column;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasAndBelongsClassPropType.Name + "_" + priSubKeyAttr.Column;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (easyAttrSubHasAndBelongs.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasAndBelongs.Table + "_" + foreignSubHasAndBelongsClassProperty.Name;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasAndBelongsClassPropType.Name + "_" + foreignSubHasAndBelongsClassProperty.Name;
                                                                                    }
                                                                                }

                                                                                for (int l = 0; l < intFieldCount; l++)
                                                                                {
                                                                                    if (columnName == subResultDataReader.GetName(l))
                                                                                    {
                                                                                        PropertyInfo PropertyForMainCls = HasAndBelongsToManyAttr.MapType.GetProperty(foreignSubHasAndBelongsClassProperty.Name);
                                                                                        if (subResultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                                                        else { PropertyForMainCls.SetValue(subResultObject, DataReader<SqlDataReader>.GetDBValue(subResultDataReader, columnName), null); }
                                                                                        break;
                                                                                    }
                                                                                }
                                                                                break;

                                                                            case "PropertyAttribute":
                                                                                PropertyAttribute propSubAttr = (PropertyAttribute)foreignSubHasAndBelongsCustomAttr[i];
                                                                                if (propSubAttr.Column != null)
                                                                                {
                                                                                    if (easyAttrSubHasAndBelongs.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasAndBelongs.Table + "_" + propSubAttr.Column;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasAndBelongsClassPropType.Name + "_" + propSubAttr.Column;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (easyAttrSubHasAndBelongs.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasAndBelongs.Table + "_" + foreignSubHasAndBelongsClassProperty.Name;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasAndBelongsClassPropType.Name + "_" + foreignSubHasAndBelongsClassProperty.Name;
                                                                                    }
                                                                                }

                                                                                for (int l = 0; l < intFieldCount; l++)
                                                                                {
                                                                                    if (columnName == subResultDataReader.GetName(l))
                                                                                    {
                                                                                        PropertyInfo PropertyForMainCls = HasAndBelongsToManyAttr.MapType.GetProperty(foreignSubHasAndBelongsClassProperty.Name);
                                                                                        if (subResultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                                                        else { PropertyForMainCls.SetValue(subResultObject, DataReader<SqlDataReader>.GetDBValue(subResultDataReader, columnName), null); }
                                                                                        break;
                                                                                    }
                                                                                }
                                                                                break;

                                                                            default:
                                                                                break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            subResultObject = null;
                                                        }

                                                        if (subResultObject == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            listManyToMany.Add(subResultObject);
                                                        }
                                                    }
                                                    subResultDataReader.Close();
                                                }
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }

                                    PropertyInfo HasAndBelongsProperty = type.GetProperty(classProperty.Name);
                                    HasAndBelongsProperty.SetValue(resultObject, listManyToMany, null);
                                    
                                    break;
                                case "HasManyAttribute":
                                    HasManyAttribute HasManyAttr = (HasManyAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!HasManyAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }
                                    else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                                    {
                                        break;
                                    }

                                    IList list = new List<object>();
                                    this.GetPrimaryKeyColumn(type);
                                    pKeyColumn = propertyColumnByType;
                                    string column = easyAttr.Table + "_" + pKeyColumn;
                                    object pKeyVal = DataReader<SqlDataReader>.GetDBValue(resultDataReader, column);

                                    if (HasManyAttr.ColumnKey != null)
                                    {
                                        fKeyColumn = HasManyAttr.ColumnKey;
                                    }
                                    else
                                    {
                                        fKeyColumn = classProperty.Name;
                                    }

                                    EasyDataAttribute easyAttrHasMany = (EasyDataAttribute)Attribute.GetCustomAttribute(HasManyAttr.MapType, typeof(EasyDataAttribute));
                                    Type foreignHasManyClassPropType = HasManyAttr.MapType;
                                    PropertyInfo[] foreignHasManyClassProperties = foreignHasManyClassPropType.GetProperties();
                                    StringBuilder sbHasManyQuery = new StringBuilder();
                                    StringBuilder sbHasManySelect = new StringBuilder();
                                    string seperatorHasMany = string.Empty;
                                    sbHasManyQuery.Append("SELECT ");

                                    foreach (PropertyInfo foreignHasManyClassProperty in foreignHasManyClassProperties)
                                    {
                                        Attribute[] foreignHasManyCustomAttr = Attribute.GetCustomAttributes(foreignHasManyClassProperty);
                                        for (int j = 0; j < foreignHasManyCustomAttr.Length; j++)
                                        {
                                            Type foreignHasManyAttrType = foreignHasManyCustomAttr[j].GetType();
                                            switch (foreignHasManyAttrType.Name)
                                            {
                                                case "PrimaryKeyAttribute":
                                                    PrimaryKeyAttribute primaryKeyAttr = (PrimaryKeyAttribute)foreignHasManyCustomAttr[j];
                                                    if (primaryKeyAttr.Column != null)
                                                    {
                                                        columnName = primaryKeyAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasManyClassProperty.Name;
                                                    }

                                                    if (sbHasManySelect.Length != 0)
                                                    {
                                                        seperatorHasMany = ", ";
                                                    }
                                                    sbHasManySelect.Append(seperatorHasMany);
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append(".");
                                                    sbHasManySelect.Append(columnName);
                                                    sbHasManySelect.Append(" AS ");
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append("_");
                                                    sbHasManySelect.Append(columnName);
                                                    break;
                                                case "PropertyAttribute":
                                                    PropertyAttribute fPropAttr = (PropertyAttribute)foreignHasManyCustomAttr[j];
                                                    if (fPropAttr.Column != null)
                                                    {
                                                        columnName = fPropAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasManyClassProperty.Name;
                                                    }

                                                    if (sbHasManySelect.Length != 0)
                                                    {
                                                        seperatorHasMany = ", ";
                                                    }
                                                    sbHasManySelect.Append(seperatorHasMany);
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append(".");
                                                    sbHasManySelect.Append(columnName);
                                                    sbHasManySelect.Append(" AS ");
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append("_");
                                                    sbHasManySelect.Append(columnName);
                                                    break;
                                                case "BelongsToAttribute":
                                                    BelongsToAttribute fBelongsToAttr = (BelongsToAttribute)foreignHasManyCustomAttr[j];
                                                    if (fBelongsToAttr.Column != null)
                                                    {
                                                        columnName = fBelongsToAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasManyClassProperty.Name;
                                                    }

                                                    if (sbHasManySelect.Length != 0)
                                                    {
                                                        seperatorHasMany = ", ";
                                                    }
                                                    sbHasManySelect.Append(seperatorHasMany);
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append(".");
                                                    sbHasManySelect.Append(columnName);
                                                    sbHasManySelect.Append(" AS ");
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append("_");
                                                    sbHasManySelect.Append(columnName);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }

                                    sbHasManyQuery.Append(sbHasManySelect.ToString());
                                    sbHasManyQuery.Append(" FROM ");
                                    if (easyAttrHasMany.Schema != null)
                                    {
                                        sbHasManyQuery.Append(easyAttrHasMany.Schema);
                                        sbHasManyQuery.Append(".");
                                    }
                                    sbHasManyQuery.Append(easyAttrHasMany.Table);
                                    sbHasManyQuery.Append(" WHERE ");
                                    sbHasManyQuery.Append(easyAttrHasMany.Table);
                                    sbHasManyQuery.Append(".");
                                    sbHasManyQuery.Append(fKeyColumn);
                                    sbHasManyQuery.Append(" = ");
                                    sbHasManyQuery.Append(pKeyVal);
                                    if (HasManyAttr.Where != null)
                                    {
                                        sbHasManyQuery.Append(" AND ");
                                        sbHasManyQuery.Append(HasManyAttr.Where);
                                    }
                                    if (HasManyAttr.OrderBy != null)
                                    {
                                        sbHasManyQuery.Append(" ORDER BY ");
                                        sbHasManyQuery.Append(HasManyAttr.OrderBy);
                                    }
                                    using (EasySession newSession = new EasySession())
                                    {
                                        using (var subCommand = new SqlCommand(sbHasManyQuery.ToString(), newSession.Connection, newSession.Transaction))
                                        {
                                            try
                                            {
                                                using (SqlDataReader subResultDataReader = subCommand.ExecuteReader())
                                                {
                                                    while (true)
                                                    {
                                                        object subResultObject = Activator.CreateInstance(HasManyAttr.MapType);
                                                        if (subResultDataReader.Read())
                                                        {
                                                            int fieldCount = subResultDataReader.FieldCount;
                                                            if (fieldCount != 0)
                                                            {
                                                                Type foreignSubHasManyClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, HasManyAttr.MapType.FullName);
                                                                PropertyInfo[] foreignSubHasManyClassProperties = foreignSubHasManyClassPropType.GetProperties();
                                                                EasyDataAttribute easyAttrSubHasMany = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignSubHasManyClassPropType, typeof(EasyDataAttribute));

                                                                foreach (PropertyInfo foreignSubHasManyClassProperty in foreignSubHasManyClassProperties)
                                                                {
                                                                    Attribute[] foreignSubHasManyCustomAttr = Attribute.GetCustomAttributes(foreignSubHasManyClassProperty);
                                                                    for (int j = 0; j < foreignSubHasManyCustomAttr.Length; j++)
                                                                    {
                                                                        Type foreignSubHasManyAttrType = foreignSubHasManyCustomAttr[j].GetType();
                                                                        switch (foreignSubHasManyAttrType.Name)
                                                                        {
                                                                            case "PrimaryKeyAttribute":

                                                                                PrimaryKeyAttribute priSubKeyAttr = (PrimaryKeyAttribute)foreignSubHasManyCustomAttr[i];
                                                                                if (priSubKeyAttr.Column != null)
                                                                                {
                                                                                    if (easyAttrSubHasMany.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasMany.Table + "_" + priSubKeyAttr.Column;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasManyClassPropType.Name + "_" + priSubKeyAttr.Column;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (easyAttrSubHasMany.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasMany.Table + "_" + foreignSubHasManyClassProperty.Name;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasManyClassPropType.Name + "_" + foreignSubHasManyClassProperty.Name;
                                                                                    }
                                                                                }

                                                                                for (int l = 0; l < intFieldCount; l++)
                                                                                {
                                                                                    if (columnName == subResultDataReader.GetName(l))
                                                                                    {
                                                                                        PropertyInfo PropertyForMainCls = HasManyAttr.MapType.GetProperty(foreignSubHasManyClassProperty.Name);
                                                                                        if (subResultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                                                        else { PropertyForMainCls.SetValue(subResultObject, DataReader<SqlDataReader>.GetDBValue(subResultDataReader, columnName), null); }
                                                                                        break;
                                                                                    }
                                                                                }
                                                                                break;

                                                                            case "PropertyAttribute":
                                                                                PropertyAttribute propSubAttr = (PropertyAttribute)foreignSubHasManyCustomAttr[i];
                                                                                if (propSubAttr.Column != null)
                                                                                {
                                                                                    if (easyAttrSubHasMany.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasMany.Table + "_" + propSubAttr.Column;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasManyClassPropType.Name + "_" + propSubAttr.Column;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (easyAttrSubHasMany.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasMany.Table + "_" + foreignSubHasManyClassProperty.Name;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasManyClassPropType.Name + "_" + foreignSubHasManyClassProperty.Name;
                                                                                    }
                                                                                }

                                                                                for (int l = 0; l < intFieldCount; l++)
                                                                                {
                                                                                    if (columnName == subResultDataReader.GetName(l))
                                                                                    {
                                                                                        PropertyInfo PropertyForMainCls = HasManyAttr.MapType.GetProperty(foreignSubHasManyClassProperty.Name);
                                                                                        if (subResultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                                                        else { PropertyForMainCls.SetValue(subResultObject, DataReader<SqlDataReader>.GetDBValue(subResultDataReader, columnName), null); }
                                                                                        break;
                                                                                    }
                                                                                }
                                                                                break;

                                                                            default:
                                                                                break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            subResultObject = null;
                                                        }

                                                        if (subResultObject == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            list.Add(subResultObject);
                                                        }
                                                    }
                                                    subResultDataReader.Close();
                                                }
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }

                                    PropertyInfo HasManyProperty = type.GetProperty(classProperty.Name);
                                    HasManyProperty.SetValue(resultObject, list, null);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                resultObject = null;
            }

            T returnObject = (T)Convert.ChangeType(resultObject, typeof(T));
            return returnObject;
        }

        /// <summary>
        /// Creates the result instance.
        /// </summary>
        /// <param name="resultDataReader">The result data reader.</param>
        /// <param name="type">The type.</param>
        /// <returns>T type instance</returns>
        protected T OracleCreateInstance(OracleDataReader resultDataReader, Type type)
        {
            object resultObject = Activator.CreateInstance(typeof(T));

            if (resultDataReader.Read())
            {
                EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(type, typeof(EasyDataAttribute));
                int intFieldCount = resultDataReader.FieldCount;

                if (intFieldCount != 0)
                {
                    PropertyInfo[] classProperties = type.GetProperties();
                    foreach (PropertyInfo classProperty in classProperties)
                    {
                        Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                        for (int i = 0; i < customAttr.Length; i++)
                        {
                            string columnName = string.Empty;
                            Type attrType = customAttr[i].GetType();
                            switch (attrType.Name)
                            {
                                case "PrimaryKeyAttribute":
                                    PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!priKeyAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }

                                    if (priKeyAttr.Column != null)
                                    {
                                        if (easyAttr.Table != null)
                                        {
                                            columnName = easyAttr.Table + "_" + priKeyAttr.Column;
                                        }
                                        else
                                        {
                                            columnName = type.Name + "_" + priKeyAttr.Column;
                                        }
                                    }
                                    else
                                    {
                                        if (easyAttr.Table != null)
                                        {
                                            columnName = easyAttr.Table + "_" + classProperty.Name;
                                        }
                                        else
                                        {
                                            columnName = type.Name + "_" + classProperty.Name;
                                        }
                                    }

                                    for (int l = 0; l < intFieldCount; l++)
                                    {
                                        if (columnName == resultDataReader.GetName(l))
                                        {
                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                            else { PropertyForMainCls.SetValue(resultObject, DataReader<OracleDataReader>.GetDBValue(resultDataReader, columnName), null); }

                                            break;
                                        }
                                    }
                                    break;

                                case "PropertyAttribute":
                                    PropertyAttribute propAttr = (PropertyAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!propAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }

                                    if (propAttr.Column != null)
                                    {
                                        if (easyAttr.Table != null)
                                        {
                                            columnName = easyAttr.Table + "_" + propAttr.Column;
                                        }
                                        else
                                        {
                                            columnName = type.Name + "_" + propAttr.Column;
                                        }
                                    }
                                    else
                                    {
                                        if (easyAttr.Table != null)
                                        {
                                            columnName = easyAttr.Table + "_" + classProperty.Name;
                                        }
                                        else
                                        {
                                            columnName = type.Name + "_" + classProperty.Name;
                                        }
                                    }

                                    for (int l = 0; l < intFieldCount; l++)
                                    {
                                        if (columnName == resultDataReader.GetName(l))
                                        {
                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                            else { PropertyForMainCls.SetValue(resultObject, DataReader<OracleDataReader>.GetDBValue(resultDataReader, columnName), null); }

                                            break;
                                        }
                                    }
                                    break;

                                case "BelongsToAttribute":
                                    BelongsToAttribute belongAttr = (BelongsToAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!belongAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }
                                    else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                                    {
                                        break;
                                    }

                                    Type foreignClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                                    PropertyInfo[] foreignClassProperties = foreignClassPropType.GetProperties();
                                    EasyDataAttribute easySubAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignClassPropType, typeof(EasyDataAttribute));

                                    foreach (PropertyInfo foreignClassProperty in foreignClassProperties)
                                    {
                                        Attribute[] foreignCustomAttr = Attribute.GetCustomAttributes(foreignClassProperty);
                                        for (int j = 0; j < foreignCustomAttr.Length; j++)
                                        {
                                            Type foreignAttrType = foreignCustomAttr[j].GetType();
                                            switch (foreignAttrType.Name)
                                            {
                                                case "PrimaryKeyAttribute":

                                                    PrimaryKeyAttribute priSubKeyAttr = (PrimaryKeyAttribute)foreignCustomAttr[i];
                                                    if (priSubKeyAttr.Column != null)
                                                    {
                                                        if (easySubAttr.Table != null)
                                                        {
                                                            columnName = easySubAttr.Table + "_" + priSubKeyAttr.Column;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignClassPropType.Name + "_" + priSubKeyAttr.Column;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (easySubAttr.Table != null)
                                                        {
                                                            columnName = easySubAttr.Table + "_" + foreignClassProperty.Name;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignClassPropType.Name + "_" + foreignClassProperty.Name;
                                                        }
                                                    }

                                                    for (int l = 0; l < intFieldCount; l++)
                                                    {
                                                        if (columnName == resultDataReader.GetName(l))
                                                        {
                                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                                            object SubClass = PropertyForMainCls.GetValue(resultObject, null);
                                                            System.Reflection.PropertyInfo PropertyForSubCls = foreignClassPropType.GetProperty(foreignClassProperty.Name);
                                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                            else { PropertyForSubCls.SetValue(SubClass, DataReader<OracleDataReader>.GetDBValue(resultDataReader, columnName), null); }
                                                            break;
                                                        }
                                                    }
                                                    break;

                                                case "PropertyAttribute":
                                                    PropertyAttribute propSubAttr = (PropertyAttribute)foreignCustomAttr[i];
                                                    if (propSubAttr.Column != null)
                                                    {
                                                        if (easySubAttr.Table != null)
                                                        {
                                                            columnName = easySubAttr.Table + "_" + propSubAttr.Column;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignClassPropType.Name + "_" + propSubAttr.Column;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (easySubAttr.Table != null)
                                                        {
                                                            columnName = easySubAttr.Table + "_" + foreignClassProperty.Name;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignClassPropType.Name + "_" + foreignClassProperty.Name;
                                                        }
                                                    }

                                                    for (int l = 0; l < intFieldCount; l++)
                                                    {
                                                        if (columnName == resultDataReader.GetName(l))
                                                        {
                                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                                            object SubClass = PropertyForMainCls.GetValue(resultObject, null);
                                                            System.Reflection.PropertyInfo PropertyForSubCls = foreignClassPropType.GetProperty(foreignClassProperty.Name);
                                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                            else { PropertyForSubCls.SetValue(SubClass, DataReader<OracleDataReader>.GetDBValue(resultDataReader, columnName), null); }
                                                            break;
                                                        }
                                                    }
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                    break;

                                case "OneToOneAttribute":
                                    OneToOneAttribute oneToOneAttr = (OneToOneAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!oneToOneAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }
                                    else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                                    {
                                        break;
                                    }

                                    Type foreignOneToOneClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                                    PropertyInfo[] foreignOneToOneClassProperties = foreignOneToOneClassPropType.GetProperties();
                                    EasyDataAttribute easyOneToOneAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignOneToOneClassPropType, typeof(EasyDataAttribute));

                                    foreach (PropertyInfo foreignOneToOneClassProperty in foreignOneToOneClassProperties)
                                    {
                                        Attribute[] foreignOneToOneCustomAttr = Attribute.GetCustomAttributes(foreignOneToOneClassProperty);
                                        for (int j = 0; j < foreignOneToOneCustomAttr.Length; j++)
                                        {
                                            Type foreignOneToOneAttrType = foreignOneToOneCustomAttr[j].GetType();
                                            switch (foreignOneToOneAttrType.Name)
                                            {
                                                case "PrimaryKeyAttribute":

                                                    PrimaryKeyAttribute priOneToOneKeyAttr = (PrimaryKeyAttribute)foreignOneToOneCustomAttr[i];
                                                    if (priOneToOneKeyAttr.Column != null)
                                                    {
                                                        if (easyOneToOneAttr.Table != null)
                                                        {
                                                            columnName = easyOneToOneAttr.Table + "_" + priOneToOneKeyAttr.Column;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignOneToOneClassPropType.Name + "_" + priOneToOneKeyAttr.Column;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (easyOneToOneAttr.Table != null)
                                                        {
                                                            columnName = easyOneToOneAttr.Table + "_" + foreignOneToOneClassProperty.Name;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignOneToOneClassPropType.Name + "_" + foreignOneToOneClassProperty.Name;
                                                        }
                                                    }

                                                    for (int l = 0; l < intFieldCount; l++)
                                                    {
                                                        if (columnName == resultDataReader.GetName(l))
                                                        {
                                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                                            object SubClass = PropertyForMainCls.GetValue(resultObject, null);
                                                            System.Reflection.PropertyInfo PropertyForSubCls = foreignOneToOneClassPropType.GetProperty(foreignOneToOneClassProperty.Name);
                                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                            else { PropertyForSubCls.SetValue(SubClass, DataReader<OracleDataReader>.GetDBValue(resultDataReader, columnName), null); }
                                                            break;
                                                        }
                                                    }
                                                    break;

                                                case "PropertyAttribute":
                                                    PropertyAttribute propOneToOneAttr = (PropertyAttribute)foreignOneToOneCustomAttr[i];
                                                    if (propOneToOneAttr.Column != null)
                                                    {
                                                        if (easyOneToOneAttr.Table != null)
                                                        {
                                                            columnName = easyOneToOneAttr.Table + "_" + propOneToOneAttr.Column;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignOneToOneClassPropType.Name + "_" + propOneToOneAttr.Column;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (easyOneToOneAttr.Table != null)
                                                        {
                                                            columnName = easyOneToOneAttr.Table + "_" + foreignOneToOneClassProperty.Name;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignOneToOneClassPropType.Name + "_" + foreignOneToOneClassProperty.Name;
                                                        }
                                                    }

                                                    for (int l = 0; l < intFieldCount; l++)
                                                    {
                                                        if (columnName == resultDataReader.GetName(l))
                                                        {
                                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                                            object SubClass = PropertyForMainCls.GetValue(resultObject, null);
                                                            System.Reflection.PropertyInfo PropertyForSubCls = foreignOneToOneClassPropType.GetProperty(foreignOneToOneClassProperty.Name);
                                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                            else { PropertyForSubCls.SetValue(SubClass, DataReader<OracleDataReader>.GetDBValue(resultDataReader, columnName), null); }
                                                            break;
                                                        }
                                                    }
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                    break;

                                case "HasAndBelongsToManyAttribute":
                                    HasAndBelongsToManyAttribute HasAndBelongsToManyAttr = (HasAndBelongsToManyAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!HasAndBelongsToManyAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }
                                    else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                                    {
                                        break;
                                    }

                                    IList listManyToMany = new List<object>();
                                    string tableManyToMany;
                                    if (easyAttr.Table != null)
                                    {
                                        tableManyToMany = easyAttr.Table;
                                    }
                                    else
                                    {
                                        tableManyToMany = type.Name;
                                    }

                                    this.GetPrimaryKeyColumn(type);
                                    pKeyColumn = propertyColumnByType;
                                    if (HasAndBelongsToManyAttr.ColumnKey != null)
                                    {
                                        aKeyColumn1 = HasAndBelongsToManyAttr.ColumnKey;
                                    }
                                    else
                                    {
                                        aKeyColumn1 = pKeyColumn;
                                    }

                                    string columnManyToMany = tableManyToMany + "_" + pKeyColumn;
                                    object pKeyValManyToMany = DataReader<OracleDataReader>.GetDBValue(resultDataReader, columnManyToMany);

                                    this.GetPrimaryKeyColumn(HasAndBelongsToManyAttr.MapType);
                                    fKeyColumn = propertyColumnByType;
                                    if (HasAndBelongsToManyAttr.ColumnRef != null)
                                    {
                                        aKeyColumn2 = HasAndBelongsToManyAttr.ColumnRef;
                                    }
                                    else
                                    {
                                        aKeyColumn2 = fKeyColumn;
                                    }

                                    EasyDataAttribute easyAttrManyToMany = (EasyDataAttribute)Attribute.GetCustomAttribute(HasAndBelongsToManyAttr.MapType, typeof(EasyDataAttribute));
                                    Type foreignHasAndBelongsClassPropType = HasAndBelongsToManyAttr.MapType;
                                    PropertyInfo[] foreignHasAndBelongsClassProperties = foreignHasAndBelongsClassPropType.GetProperties();
                                    StringBuilder sbQuery = new StringBuilder();
                                    StringBuilder sbSelect = new StringBuilder();
                                    StringBuilder sbJoin = new StringBuilder();
                                    string seperator = string.Empty;
                                    sbQuery.Append("SELECT ");

                                    foreach (PropertyInfo foreignHasAndBelongsClassProperty in foreignHasAndBelongsClassProperties)
                                    {
                                        Attribute[] foreignHasAndBelongsCustomAttr = Attribute.GetCustomAttributes(foreignHasAndBelongsClassProperty);
                                        for (int j = 0; j < foreignHasAndBelongsCustomAttr.Length; j++)
                                        {
                                            Type foreignHasAndBelongsAttrType = foreignHasAndBelongsCustomAttr[j].GetType();
                                            switch (foreignHasAndBelongsAttrType.Name)
                                            {
                                                case "PrimaryKeyAttribute":
                                                    PrimaryKeyAttribute primaryKeyAttr = (PrimaryKeyAttribute)foreignHasAndBelongsCustomAttr[j];
                                                    if (primaryKeyAttr.Column != null)
                                                    {
                                                        columnName = primaryKeyAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasAndBelongsClassProperty.Name;
                                                    }

                                                    if (sbSelect.Length != 0)
                                                    {
                                                        seperator = ", ";
                                                    }
                                                    sbSelect.Append(seperator);
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append(".");
                                                    sbSelect.Append(columnName);
                                                    sbSelect.Append(" AS ");
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append("_");
                                                    sbSelect.Append(columnName);
                                                    break;
                                                case "PropertyAttribute":
                                                    PropertyAttribute fPropAttr = (PropertyAttribute)foreignHasAndBelongsCustomAttr[j];
                                                    if (fPropAttr.Column != null)
                                                    {
                                                        columnName = fPropAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasAndBelongsClassProperty.Name;
                                                    }

                                                    if (sbSelect.Length != 0)
                                                    {
                                                        seperator = ", ";
                                                    }
                                                    sbSelect.Append(seperator);
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append(".");
                                                    sbSelect.Append(columnName);
                                                    sbSelect.Append(" AS ");
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append("_");
                                                    sbSelect.Append(columnName);
                                                    break;
                                                case "BelongsToAttribute":
                                                    BelongsToAttribute fBelongsToAttr = (BelongsToAttribute)foreignHasAndBelongsCustomAttr[j];
                                                    if (fBelongsToAttr.Column != null)
                                                    {
                                                        columnName = fBelongsToAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasAndBelongsClassProperty.Name;
                                                    }

                                                    if (sbSelect.Length != 0)
                                                    {
                                                        seperator = ", ";
                                                    }
                                                    sbSelect.Append(seperator);
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append(".");
                                                    sbSelect.Append(columnName);
                                                    sbSelect.Append(" AS ");
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append("_");
                                                    sbSelect.Append(columnName);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }

                                    sbQuery.Append(sbSelect.ToString());
                                    sbQuery.Append(" FROM ");
                                    if (easyAttr.Schema != null)
                                    {
                                        sbQuery.Append(easyAttr.Schema);
                                        sbQuery.Append(".");
                                    }
                                    string primaryTable = string.Empty;
                                    if (easyAttr.Table != null)
                                    {
                                        sbQuery.Append(easyAttr.Table);
                                        primaryTable = easyAttr.Table;
                                    }
                                    else
                                    {
                                        sbQuery.Append(type.Name);
                                        primaryTable = type.Name;
                                    }

                                    sbJoin.Append(" INNER JOIN ");
                                    if (HasAndBelongsToManyAttr.Schema != null)
                                    {
                                        sbJoin.Append(HasAndBelongsToManyAttr.Schema);
                                        sbJoin.Append(".");
                                    }
                                    sbJoin.Append(HasAndBelongsToManyAttr.Table);
                                    sbJoin.Append(" ON ");
                                    sbJoin.Append(primaryTable);
                                    sbJoin.Append(".");
                                    sbJoin.Append(pKeyColumn);
                                    sbJoin.Append(" = ");
                                    sbJoin.Append(HasAndBelongsToManyAttr.Table);
                                    sbJoin.Append(".");
                                    sbJoin.Append(aKeyColumn1);

                                    sbJoin.Append(" INNER JOIN ");
                                    if (easyAttrManyToMany.Schema != null)
                                    {
                                        sbJoin.Append(easyAttrManyToMany.Schema);
                                        sbJoin.Append(".");
                                    }
                                    string secondaryTable = string.Empty;
                                    if (easyAttrManyToMany.Table != null)
                                    {
                                        sbJoin.Append(easyAttrManyToMany.Table);
                                        secondaryTable = easyAttrManyToMany.Table;
                                    }
                                    else
                                    {
                                        sbQuery.Append(HasAndBelongsToManyAttr.MapType.Name);
                                        secondaryTable = HasAndBelongsToManyAttr.MapType.Name;
                                    }
                                    sbJoin.Append(" ON ");
                                    sbJoin.Append(HasAndBelongsToManyAttr.Table);
                                    sbJoin.Append(".");
                                    sbJoin.Append(aKeyColumn2);
                                    sbJoin.Append(" = ");
                                    sbJoin.Append(secondaryTable);
                                    sbJoin.Append(".");
                                    sbJoin.Append(fKeyColumn);

                                    sbQuery.Append(sbJoin.ToString());
                                    sbQuery.Append(" WHERE ");
                                    sbQuery.Append(primaryTable);
                                    sbQuery.Append(".");
                                    sbQuery.Append(pKeyColumn);
                                    sbQuery.Append(" = ");
                                    sbQuery.Append(pKeyValManyToMany);
                                    if (HasAndBelongsToManyAttr.Where != null)
                                    {
                                        sbQuery.Append(" AND ");
                                        sbQuery.Append(HasAndBelongsToManyAttr.Where);
                                    }
                                    if (HasAndBelongsToManyAttr.OrderBy != null)
                                    {
                                        sbQuery.Append(" ORDER BY ");
                                        sbQuery.Append(HasAndBelongsToManyAttr.OrderBy);
                                    }
                                    using (EasySession newSession = new EasySession())
                                    {
                                        using (var subCommand = new OracleCommand(sbQuery.ToString(), newSession.OConnection, newSession.OTransaction))
                                        {
                                            try
                                            {
                                                using (OracleDataReader subResultDataReader = subCommand.ExecuteReader())
                                                {
                                                    while (true)
                                                    {
                                                        object subResultObject = Activator.CreateInstance(HasAndBelongsToManyAttr.MapType);
                                                        if (subResultDataReader.Read())
                                                        {
                                                            int fieldCount = subResultDataReader.FieldCount;
                                                            if (fieldCount != 0)
                                                            {
                                                                Type foreignSubHasAndBelongsClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, HasAndBelongsToManyAttr.MapType.FullName);
                                                                PropertyInfo[] foreignSubHasAndBelongsClassProperties = foreignSubHasAndBelongsClassPropType.GetProperties();
                                                                EasyDataAttribute easyAttrSubHasAndBelongs = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignSubHasAndBelongsClassPropType, typeof(EasyDataAttribute));

                                                                foreach (PropertyInfo foreignSubHasAndBelongsClassProperty in foreignSubHasAndBelongsClassProperties)
                                                                {
                                                                    Attribute[] foreignSubHasAndBelongsCustomAttr = Attribute.GetCustomAttributes(foreignSubHasAndBelongsClassProperty);
                                                                    for (int j = 0; j < foreignSubHasAndBelongsCustomAttr.Length; j++)
                                                                    {
                                                                        Type foreignSubHasAndBelongsAttrType = foreignSubHasAndBelongsCustomAttr[j].GetType();
                                                                        switch (foreignSubHasAndBelongsAttrType.Name)
                                                                        {
                                                                            case "PrimaryKeyAttribute":

                                                                                PrimaryKeyAttribute priSubKeyAttr = (PrimaryKeyAttribute)foreignSubHasAndBelongsCustomAttr[i];
                                                                                if (priSubKeyAttr.Column != null)
                                                                                {
                                                                                    if (easyAttrSubHasAndBelongs.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasAndBelongs.Table + "_" + priSubKeyAttr.Column;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasAndBelongsClassPropType.Name + "_" + priSubKeyAttr.Column;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (easyAttrSubHasAndBelongs.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasAndBelongs.Table + "_" + foreignSubHasAndBelongsClassProperty.Name;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasAndBelongsClassPropType.Name + "_" + foreignSubHasAndBelongsClassProperty.Name;
                                                                                    }
                                                                                }

                                                                                for (int l = 0; l < intFieldCount; l++)
                                                                                {
                                                                                    if (columnName == subResultDataReader.GetName(l))
                                                                                    {
                                                                                        PropertyInfo PropertyForMainCls = HasAndBelongsToManyAttr.MapType.GetProperty(foreignSubHasAndBelongsClassProperty.Name);
                                                                                        if (subResultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                                                        else { PropertyForMainCls.SetValue(subResultObject, DataReader<OracleDataReader>.GetDBValue(subResultDataReader, columnName), null); }
                                                                                        break;
                                                                                    }
                                                                                }
                                                                                break;

                                                                            case "PropertyAttribute":
                                                                                PropertyAttribute propSubAttr = (PropertyAttribute)foreignSubHasAndBelongsCustomAttr[i];
                                                                                if (propSubAttr.Column != null)
                                                                                {
                                                                                    if (easyAttrSubHasAndBelongs.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasAndBelongs.Table + "_" + propSubAttr.Column;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasAndBelongsClassPropType.Name + "_" + propSubAttr.Column;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (easyAttrSubHasAndBelongs.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasAndBelongs.Table + "_" + foreignSubHasAndBelongsClassProperty.Name;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasAndBelongsClassPropType.Name + "_" + foreignSubHasAndBelongsClassProperty.Name;
                                                                                    }
                                                                                }

                                                                                for (int l = 0; l < intFieldCount; l++)
                                                                                {
                                                                                    if (columnName == subResultDataReader.GetName(l))
                                                                                    {
                                                                                        PropertyInfo PropertyForMainCls = HasAndBelongsToManyAttr.MapType.GetProperty(foreignSubHasAndBelongsClassProperty.Name);
                                                                                        if (subResultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                                                        else { PropertyForMainCls.SetValue(subResultObject, DataReader<OracleDataReader>.GetDBValue(subResultDataReader, columnName), null); }
                                                                                        break;
                                                                                    }
                                                                                }
                                                                                break;

                                                                            default:
                                                                                break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            subResultObject = null;
                                                        }

                                                        if (subResultObject == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            listManyToMany.Add(subResultObject);
                                                        }
                                                    }
                                                    subResultDataReader.Close();
                                                }
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }

                                    PropertyInfo HasAndBelongsProperty = type.GetProperty(classProperty.Name);
                                    HasAndBelongsProperty.SetValue(resultObject, listManyToMany, null);

                                    break;
                                case "HasManyAttribute":
                                    HasManyAttribute HasManyAttr = (HasManyAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!HasManyAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }
                                    else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                                    {
                                        break;
                                    }

                                    IList list = new List<object>();
                                    this.GetPrimaryKeyColumn(type);
                                    pKeyColumn = propertyColumnByType;
                                    string column = easyAttr.Table + "_" + pKeyColumn;
                                    object pKeyVal = DataReader<OracleDataReader>.GetDBValue(resultDataReader, column);

                                    if (HasManyAttr.ColumnKey != null)
                                    {
                                        fKeyColumn = HasManyAttr.ColumnKey;
                                    }
                                    else
                                    {
                                        fKeyColumn = classProperty.Name;
                                    }

                                    EasyDataAttribute easyAttrHasMany = (EasyDataAttribute)Attribute.GetCustomAttribute(HasManyAttr.MapType, typeof(EasyDataAttribute));
                                    Type foreignHasManyClassPropType = HasManyAttr.MapType;
                                    PropertyInfo[] foreignHasManyClassProperties = foreignHasManyClassPropType.GetProperties();
                                    StringBuilder sbHasManyQuery = new StringBuilder();
                                    StringBuilder sbHasManySelect = new StringBuilder();
                                    string seperatorHasMany = string.Empty;
                                    sbHasManyQuery.Append("SELECT ");

                                    foreach (PropertyInfo foreignHasManyClassProperty in foreignHasManyClassProperties)
                                    {
                                        Attribute[] foreignHasManyCustomAttr = Attribute.GetCustomAttributes(foreignHasManyClassProperty);
                                        for (int j = 0; j < foreignHasManyCustomAttr.Length; j++)
                                        {
                                            Type foreignHasManyAttrType = foreignHasManyCustomAttr[j].GetType();
                                            switch (foreignHasManyAttrType.Name)
                                            {
                                                case "PrimaryKeyAttribute":
                                                    PrimaryKeyAttribute primaryKeyAttr = (PrimaryKeyAttribute)foreignHasManyCustomAttr[j];
                                                    if (primaryKeyAttr.Column != null)
                                                    {
                                                        columnName = primaryKeyAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasManyClassProperty.Name;
                                                    }

                                                    if (sbHasManySelect.Length != 0)
                                                    {
                                                        seperatorHasMany = ", ";
                                                    }
                                                    sbHasManySelect.Append(seperatorHasMany);
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append(".");
                                                    sbHasManySelect.Append(columnName);
                                                    sbHasManySelect.Append(" AS ");
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append("_");
                                                    sbHasManySelect.Append(columnName);
                                                    break;
                                                case "PropertyAttribute":
                                                    PropertyAttribute fPropAttr = (PropertyAttribute)foreignHasManyCustomAttr[j];
                                                    if (fPropAttr.Column != null)
                                                    {
                                                        columnName = fPropAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasManyClassProperty.Name;
                                                    }

                                                    if (sbHasManySelect.Length != 0)
                                                    {
                                                        seperatorHasMany = ", ";
                                                    }
                                                    sbHasManySelect.Append(seperatorHasMany);
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append(".");
                                                    sbHasManySelect.Append(columnName);
                                                    sbHasManySelect.Append(" AS ");
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append("_");
                                                    sbHasManySelect.Append(columnName);
                                                    break;
                                                case "BelongsToAttribute":
                                                    BelongsToAttribute fBelongsToAttr = (BelongsToAttribute)foreignHasManyCustomAttr[j];
                                                    if (fBelongsToAttr.Column != null)
                                                    {
                                                        columnName = fBelongsToAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasManyClassProperty.Name;
                                                    }

                                                    if (sbHasManySelect.Length != 0)
                                                    {
                                                        seperatorHasMany = ", ";
                                                    }
                                                    sbHasManySelect.Append(seperatorHasMany);
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append(".");
                                                    sbHasManySelect.Append(columnName);
                                                    sbHasManySelect.Append(" AS ");
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append("_");
                                                    sbHasManySelect.Append(columnName);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }

                                    sbHasManyQuery.Append(sbHasManySelect.ToString());
                                    sbHasManyQuery.Append(" FROM ");
                                    if (easyAttrHasMany.Schema != null)
                                    {
                                        sbHasManyQuery.Append(easyAttrHasMany.Schema);
                                        sbHasManyQuery.Append(".");
                                    }
                                    sbHasManyQuery.Append(easyAttrHasMany.Table);
                                    sbHasManyQuery.Append(" WHERE ");
                                    sbHasManyQuery.Append(easyAttrHasMany.Table);
                                    sbHasManyQuery.Append(".");
                                    sbHasManyQuery.Append(fKeyColumn);
                                    sbHasManyQuery.Append(" = ");
                                    sbHasManyQuery.Append(pKeyVal);
                                    if (HasManyAttr.Where != null)
                                    {
                                        sbHasManyQuery.Append(" AND ");
                                        sbHasManyQuery.Append(HasManyAttr.Where);
                                    }
                                    if (HasManyAttr.OrderBy != null)
                                    {
                                        sbHasManyQuery.Append(" ORDER BY ");
                                        sbHasManyQuery.Append(HasManyAttr.OrderBy);
                                    }
                                    using (EasySession newSession = new EasySession())
                                    {
                                        using (var subCommand = new OracleCommand(sbHasManyQuery.ToString(), newSession.OConnection, newSession.OTransaction))
                                        {
                                            try
                                            {
                                                using (OracleDataReader subResultDataReader = subCommand.ExecuteReader())
                                                {
                                                    while (true)
                                                    {
                                                        object subResultObject = Activator.CreateInstance(HasManyAttr.MapType);
                                                        if (subResultDataReader.Read())
                                                        {
                                                            int fieldCount = subResultDataReader.FieldCount;
                                                            if (fieldCount != 0)
                                                            {
                                                                Type foreignSubHasManyClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, HasManyAttr.MapType.FullName);
                                                                PropertyInfo[] foreignSubHasManyClassProperties = foreignSubHasManyClassPropType.GetProperties();
                                                                EasyDataAttribute easyAttrSubHasMany = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignSubHasManyClassPropType, typeof(EasyDataAttribute));

                                                                foreach (PropertyInfo foreignSubHasManyClassProperty in foreignSubHasManyClassProperties)
                                                                {
                                                                    Attribute[] foreignSubHasManyCustomAttr = Attribute.GetCustomAttributes(foreignSubHasManyClassProperty);
                                                                    for (int j = 0; j < foreignSubHasManyCustomAttr.Length; j++)
                                                                    {
                                                                        Type foreignSubHasManyAttrType = foreignSubHasManyCustomAttr[j].GetType();
                                                                        switch (foreignSubHasManyAttrType.Name)
                                                                        {
                                                                            case "PrimaryKeyAttribute":

                                                                                PrimaryKeyAttribute priSubKeyAttr = (PrimaryKeyAttribute)foreignSubHasManyCustomAttr[i];
                                                                                if (priSubKeyAttr.Column != null)
                                                                                {
                                                                                    if (easyAttrSubHasMany.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasMany.Table + "_" + priSubKeyAttr.Column;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasManyClassPropType.Name + "_" + priSubKeyAttr.Column;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (easyAttrSubHasMany.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasMany.Table + "_" + foreignSubHasManyClassProperty.Name;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasManyClassPropType.Name + "_" + foreignSubHasManyClassProperty.Name;
                                                                                    }
                                                                                }

                                                                                for (int l = 0; l < intFieldCount; l++)
                                                                                {
                                                                                    if (columnName == subResultDataReader.GetName(l))
                                                                                    {
                                                                                        PropertyInfo PropertyForMainCls = HasManyAttr.MapType.GetProperty(foreignSubHasManyClassProperty.Name);
                                                                                        if (subResultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                                                        else { PropertyForMainCls.SetValue(subResultObject, DataReader<OracleDataReader>.GetDBValue(subResultDataReader, columnName), null); }
                                                                                        break;
                                                                                    }
                                                                                }
                                                                                break;

                                                                            case "PropertyAttribute":
                                                                                PropertyAttribute propSubAttr = (PropertyAttribute)foreignSubHasManyCustomAttr[i];
                                                                                if (propSubAttr.Column != null)
                                                                                {
                                                                                    if (easyAttrSubHasMany.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasMany.Table + "_" + propSubAttr.Column;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasManyClassPropType.Name + "_" + propSubAttr.Column;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (easyAttrSubHasMany.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasMany.Table + "_" + foreignSubHasManyClassProperty.Name;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasManyClassPropType.Name + "_" + foreignSubHasManyClassProperty.Name;
                                                                                    }
                                                                                }

                                                                                for (int l = 0; l < intFieldCount; l++)
                                                                                {
                                                                                    if (columnName == subResultDataReader.GetName(l))
                                                                                    {
                                                                                        PropertyInfo PropertyForMainCls = HasManyAttr.MapType.GetProperty(foreignSubHasManyClassProperty.Name);
                                                                                        if (subResultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                                                        else { PropertyForMainCls.SetValue(subResultObject, DataReader<OracleDataReader>.GetDBValue(subResultDataReader, columnName), null); }
                                                                                        break;
                                                                                    }
                                                                                }
                                                                                break;

                                                                            default:
                                                                                break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            subResultObject = null;
                                                        }

                                                        if (subResultObject == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            list.Add(subResultObject);
                                                        }
                                                    }
                                                    subResultDataReader.Close();
                                                }
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }

                                    PropertyInfo HasManyProperty = type.GetProperty(classProperty.Name);
                                    HasManyProperty.SetValue(resultObject, list, null);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                resultObject = null;
            }

            T returnObject = (T)Convert.ChangeType(resultObject, typeof(T));
            return returnObject;
        }

        /// <summary>
        /// Creates the result instance.
        /// </summary>
        /// <param name="resultDataReader">The result data reader.</param>
        /// <param name="type">The type.</param>
        /// <returns>T type instance</returns>
        protected T MySqlCreateInstance(MySqlDataReader resultDataReader, Type type)
        {
            object resultObject = Activator.CreateInstance(typeof(T));

            if (resultDataReader.Read())
            {
                EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(type, typeof(EasyDataAttribute));
                int intFieldCount = resultDataReader.FieldCount;

                if (intFieldCount != 0)
                {
                    PropertyInfo[] classProperties = type.GetProperties();
                    foreach (PropertyInfo classProperty in classProperties)
                    {
                        Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                        for (int i = 0; i < customAttr.Length; i++)
                        {
                            string columnName = string.Empty;
                            Type attrType = customAttr[i].GetType();
                            switch (attrType.Name)
                            {
                                case "PrimaryKeyAttribute":
                                    PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!priKeyAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }

                                    if (priKeyAttr.Column != null)
                                    {
                                        if (easyAttr.Table != null)
                                        {
                                            columnName = easyAttr.Table + "_" + priKeyAttr.Column;
                                        }
                                        else
                                        {
                                            columnName = type.Name + "_" + priKeyAttr.Column;
                                        }
                                    }
                                    else
                                    {
                                        if (easyAttr.Table != null)
                                        {
                                            columnName = easyAttr.Table + "_" + classProperty.Name;
                                        }
                                        else
                                        {
                                            columnName = type.Name + "_" + classProperty.Name;
                                        }
                                    }

                                    for (int l = 0; l < intFieldCount; l++)
                                    {
                                        if (columnName == resultDataReader.GetName(l))
                                        {
                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                            else { PropertyForMainCls.SetValue(resultObject, DataReader<MySqlDataReader>.GetDBValue(resultDataReader, columnName), null); }

                                            break;
                                        }
                                    }
                                    break;

                                case "PropertyAttribute":
                                    PropertyAttribute propAttr = (PropertyAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!propAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }

                                    if (propAttr.Column != null)
                                    {
                                        if (easyAttr.Table != null)
                                        {
                                            columnName = easyAttr.Table + "_" + propAttr.Column;
                                        }
                                        else
                                        {
                                            columnName = type.Name + "_" + propAttr.Column;
                                        }
                                    }
                                    else
                                    {
                                        if (easyAttr.Table != null)
                                        {
                                            columnName = easyAttr.Table + "_" + classProperty.Name;
                                        }
                                        else
                                        {
                                            columnName = type.Name + "_" + classProperty.Name;
                                        }
                                    }

                                    for (int l = 0; l < intFieldCount; l++)
                                    {
                                        if (columnName == resultDataReader.GetName(l))
                                        {
                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                            else { PropertyForMainCls.SetValue(resultObject, DataReader<MySqlDataReader>.GetDBValue(resultDataReader, columnName), null); }

                                            break;
                                        }
                                    }
                                    break;

                                case "BelongsToAttribute":
                                    BelongsToAttribute belongAttr = (BelongsToAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!belongAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }
                                    else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                                    {
                                        break;
                                    }

                                    Type foreignClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                                    PropertyInfo[] foreignClassProperties = foreignClassPropType.GetProperties();
                                    EasyDataAttribute easySubAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignClassPropType, typeof(EasyDataAttribute));

                                    foreach (PropertyInfo foreignClassProperty in foreignClassProperties)
                                    {
                                        Attribute[] foreignCustomAttr = Attribute.GetCustomAttributes(foreignClassProperty);
                                        for (int j = 0; j < foreignCustomAttr.Length; j++)
                                        {
                                            Type foreignAttrType = foreignCustomAttr[j].GetType();
                                            switch (foreignAttrType.Name)
                                            {
                                                case "PrimaryKeyAttribute":

                                                    PrimaryKeyAttribute priSubKeyAttr = (PrimaryKeyAttribute)foreignCustomAttr[i];
                                                    if (priSubKeyAttr.Column != null)
                                                    {
                                                        if (easySubAttr.Table != null)
                                                        {
                                                            columnName = easySubAttr.Table + "_" + priSubKeyAttr.Column;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignClassPropType.Name + "_" + priSubKeyAttr.Column;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (easySubAttr.Table != null)
                                                        {
                                                            columnName = easySubAttr.Table + "_" + foreignClassProperty.Name;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignClassPropType.Name + "_" + foreignClassProperty.Name;
                                                        }
                                                    }

                                                    for (int l = 0; l < intFieldCount; l++)
                                                    {
                                                        if (columnName == resultDataReader.GetName(l))
                                                        {
                                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                                            object SubClass = PropertyForMainCls.GetValue(resultObject, null);
                                                            System.Reflection.PropertyInfo PropertyForSubCls = foreignClassPropType.GetProperty(foreignClassProperty.Name);
                                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                            else { PropertyForSubCls.SetValue(SubClass, DataReader<MySqlDataReader>.GetDBValue(resultDataReader, columnName), null); }
                                                            break;
                                                        }
                                                    }
                                                    break;

                                                case "PropertyAttribute":
                                                    PropertyAttribute propSubAttr = (PropertyAttribute)foreignCustomAttr[i];
                                                    if (propSubAttr.Column != null)
                                                    {
                                                        if (easySubAttr.Table != null)
                                                        {
                                                            columnName = easySubAttr.Table + "_" + propSubAttr.Column;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignClassPropType.Name + "_" + propSubAttr.Column;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (easySubAttr.Table != null)
                                                        {
                                                            columnName = easySubAttr.Table + "_" + foreignClassProperty.Name;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignClassPropType.Name + "_" + foreignClassProperty.Name;
                                                        }
                                                    }

                                                    for (int l = 0; l < intFieldCount; l++)
                                                    {
                                                        if (columnName == resultDataReader.GetName(l))
                                                        {
                                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                                            object SubClass = PropertyForMainCls.GetValue(resultObject, null);
                                                            System.Reflection.PropertyInfo PropertyForSubCls = foreignClassPropType.GetProperty(foreignClassProperty.Name);
                                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                            else { PropertyForSubCls.SetValue(SubClass, DataReader<MySqlDataReader>.GetDBValue(resultDataReader, columnName), null); }
                                                            break;
                                                        }
                                                    }
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                    break;

                                case "OneToOneAttribute":
                                    OneToOneAttribute oneToOneAttr = (OneToOneAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!oneToOneAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }
                                    else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                                    {
                                        break;
                                    }

                                    Type foreignOneToOneClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                                    PropertyInfo[] foreignOneToOneClassProperties = foreignOneToOneClassPropType.GetProperties();
                                    EasyDataAttribute easyOneToOneAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignOneToOneClassPropType, typeof(EasyDataAttribute));

                                    foreach (PropertyInfo foreignOneToOneClassProperty in foreignOneToOneClassProperties)
                                    {
                                        Attribute[] foreignOneToOneCustomAttr = Attribute.GetCustomAttributes(foreignOneToOneClassProperty);
                                        for (int j = 0; j < foreignOneToOneCustomAttr.Length; j++)
                                        {
                                            Type foreignOneToOneAttrType = foreignOneToOneCustomAttr[j].GetType();
                                            switch (foreignOneToOneAttrType.Name)
                                            {
                                                case "PrimaryKeyAttribute":

                                                    PrimaryKeyAttribute priOneToOneKeyAttr = (PrimaryKeyAttribute)foreignOneToOneCustomAttr[i];
                                                    if (priOneToOneKeyAttr.Column != null)
                                                    {
                                                        if (easyOneToOneAttr.Table != null)
                                                        {
                                                            columnName = easyOneToOneAttr.Table + "_" + priOneToOneKeyAttr.Column;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignOneToOneClassPropType.Name + "_" + priOneToOneKeyAttr.Column;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (easyOneToOneAttr.Table != null)
                                                        {
                                                            columnName = easyOneToOneAttr.Table + "_" + foreignOneToOneClassProperty.Name;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignOneToOneClassPropType.Name + "_" + foreignOneToOneClassProperty.Name;
                                                        }
                                                    }

                                                    for (int l = 0; l < intFieldCount; l++)
                                                    {
                                                        if (columnName == resultDataReader.GetName(l))
                                                        {
                                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                                            object SubClass = PropertyForMainCls.GetValue(resultObject, null);
                                                            System.Reflection.PropertyInfo PropertyForSubCls = foreignOneToOneClassPropType.GetProperty(foreignOneToOneClassProperty.Name);
                                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                            else { PropertyForSubCls.SetValue(SubClass, DataReader<MySqlDataReader>.GetDBValue(resultDataReader, columnName), null); }
                                                            break;
                                                        }
                                                    }
                                                    break;

                                                case "PropertyAttribute":
                                                    PropertyAttribute propOneToOneAttr = (PropertyAttribute)foreignOneToOneCustomAttr[i];
                                                    if (propOneToOneAttr.Column != null)
                                                    {
                                                        if (easyOneToOneAttr.Table != null)
                                                        {
                                                            columnName = easyOneToOneAttr.Table + "_" + propOneToOneAttr.Column;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignOneToOneClassPropType.Name + "_" + propOneToOneAttr.Column;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (easyOneToOneAttr.Table != null)
                                                        {
                                                            columnName = easyOneToOneAttr.Table + "_" + foreignOneToOneClassProperty.Name;
                                                        }
                                                        else
                                                        {
                                                            columnName = foreignOneToOneClassPropType.Name + "_" + foreignOneToOneClassProperty.Name;
                                                        }
                                                    }

                                                    for (int l = 0; l < intFieldCount; l++)
                                                    {
                                                        if (columnName == resultDataReader.GetName(l))
                                                        {
                                                            System.Reflection.PropertyInfo PropertyForMainCls = type.GetProperty(classProperty.Name);
                                                            object SubClass = PropertyForMainCls.GetValue(resultObject, null);
                                                            System.Reflection.PropertyInfo PropertyForSubCls = foreignOneToOneClassPropType.GetProperty(foreignOneToOneClassProperty.Name);
                                                            if (resultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                            else { PropertyForSubCls.SetValue(SubClass, DataReader<MySqlDataReader>.GetDBValue(resultDataReader, columnName), null); }
                                                            break;
                                                        }
                                                    }
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                    break;

                                case "HasAndBelongsToManyAttribute":
                                    HasAndBelongsToManyAttribute HasAndBelongsToManyAttr = (HasAndBelongsToManyAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!HasAndBelongsToManyAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }
                                    else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                                    {
                                        break;
                                    }

                                    IList listManyToMany = new List<object>();
                                    string tableManyToMany;
                                    if (easyAttr.Table != null)
                                    {
                                        tableManyToMany = easyAttr.Table;
                                    }
                                    else
                                    {
                                        tableManyToMany = type.Name;
                                    }

                                    this.GetPrimaryKeyColumn(type);
                                    pKeyColumn = propertyColumnByType;
                                    if (HasAndBelongsToManyAttr.ColumnKey != null)
                                    {
                                        aKeyColumn1 = HasAndBelongsToManyAttr.ColumnKey;
                                    }
                                    else
                                    {
                                        aKeyColumn1 = pKeyColumn;
                                    }

                                    string columnManyToMany = tableManyToMany + "_" + pKeyColumn;
                                    object pKeyValManyToMany = DataReader<MySqlDataReader>.GetDBValue(resultDataReader, columnManyToMany);

                                    this.GetPrimaryKeyColumn(HasAndBelongsToManyAttr.MapType);
                                    fKeyColumn = propertyColumnByType;
                                    if (HasAndBelongsToManyAttr.ColumnRef != null)
                                    {
                                        aKeyColumn2 = HasAndBelongsToManyAttr.ColumnRef;
                                    }
                                    else
                                    {
                                        aKeyColumn2 = fKeyColumn;
                                    }

                                    EasyDataAttribute easyAttrManyToMany = (EasyDataAttribute)Attribute.GetCustomAttribute(HasAndBelongsToManyAttr.MapType, typeof(EasyDataAttribute));
                                    Type foreignHasAndBelongsClassPropType = HasAndBelongsToManyAttr.MapType;
                                    PropertyInfo[] foreignHasAndBelongsClassProperties = foreignHasAndBelongsClassPropType.GetProperties();
                                    StringBuilder sbQuery = new StringBuilder();
                                    StringBuilder sbSelect = new StringBuilder();
                                    StringBuilder sbJoin = new StringBuilder();
                                    string seperator = string.Empty;
                                    sbQuery.Append("SELECT ");

                                    foreach (PropertyInfo foreignHasAndBelongsClassProperty in foreignHasAndBelongsClassProperties)
                                    {
                                        Attribute[] foreignHasAndBelongsCustomAttr = Attribute.GetCustomAttributes(foreignHasAndBelongsClassProperty);
                                        for (int j = 0; j < foreignHasAndBelongsCustomAttr.Length; j++)
                                        {
                                            Type foreignHasAndBelongsAttrType = foreignHasAndBelongsCustomAttr[j].GetType();
                                            switch (foreignHasAndBelongsAttrType.Name)
                                            {
                                                case "PrimaryKeyAttribute":
                                                    PrimaryKeyAttribute primaryKeyAttr = (PrimaryKeyAttribute)foreignHasAndBelongsCustomAttr[j];
                                                    if (primaryKeyAttr.Column != null)
                                                    {
                                                        columnName = primaryKeyAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasAndBelongsClassProperty.Name;
                                                    }

                                                    if (sbSelect.Length != 0)
                                                    {
                                                        seperator = ", ";
                                                    }
                                                    sbSelect.Append(seperator);
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append(".");
                                                    sbSelect.Append(columnName);
                                                    sbSelect.Append(" AS ");
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append("_");
                                                    sbSelect.Append(columnName);
                                                    break;
                                                case "PropertyAttribute":
                                                    PropertyAttribute fPropAttr = (PropertyAttribute)foreignHasAndBelongsCustomAttr[j];
                                                    if (fPropAttr.Column != null)
                                                    {
                                                        columnName = fPropAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasAndBelongsClassProperty.Name;
                                                    }

                                                    if (sbSelect.Length != 0)
                                                    {
                                                        seperator = ", ";
                                                    }
                                                    sbSelect.Append(seperator);
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append(".");
                                                    sbSelect.Append(columnName);
                                                    sbSelect.Append(" AS ");
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append("_");
                                                    sbSelect.Append(columnName);
                                                    break;
                                                case "BelongsToAttribute":
                                                    BelongsToAttribute fBelongsToAttr = (BelongsToAttribute)foreignHasAndBelongsCustomAttr[j];
                                                    if (fBelongsToAttr.Column != null)
                                                    {
                                                        columnName = fBelongsToAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasAndBelongsClassProperty.Name;
                                                    }

                                                    if (sbSelect.Length != 0)
                                                    {
                                                        seperator = ", ";
                                                    }
                                                    sbSelect.Append(seperator);
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append(".");
                                                    sbSelect.Append(columnName);
                                                    sbSelect.Append(" AS ");
                                                    sbSelect.Append(easyAttrManyToMany.Table);
                                                    sbSelect.Append("_");
                                                    sbSelect.Append(columnName);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }

                                    sbQuery.Append(sbSelect.ToString());
                                    sbQuery.Append(" FROM ");
                                    if (easyAttr.Schema != null)
                                    {
                                        sbQuery.Append(easyAttr.Schema);
                                        sbQuery.Append(".");
                                    }
                                    string primaryTable = string.Empty;
                                    if (easyAttr.Table != null)
                                    {
                                        sbQuery.Append(easyAttr.Table);
                                        primaryTable = easyAttr.Table;
                                    }
                                    else
                                    {
                                        sbQuery.Append(type.Name);
                                        primaryTable = type.Name;
                                    }

                                    sbJoin.Append(" INNER JOIN ");
                                    if (HasAndBelongsToManyAttr.Schema != null)
                                    {
                                        sbJoin.Append(HasAndBelongsToManyAttr.Schema);
                                        sbJoin.Append(".");
                                    }
                                    sbJoin.Append(HasAndBelongsToManyAttr.Table);
                                    sbJoin.Append(" ON ");
                                    sbJoin.Append(primaryTable);
                                    sbJoin.Append(".");
                                    sbJoin.Append(pKeyColumn);
                                    sbJoin.Append(" = ");
                                    sbJoin.Append(HasAndBelongsToManyAttr.Table);
                                    sbJoin.Append(".");
                                    sbJoin.Append(aKeyColumn1);

                                    sbJoin.Append(" INNER JOIN ");
                                    if (easyAttrManyToMany.Schema != null)
                                    {
                                        sbJoin.Append(easyAttrManyToMany.Schema);
                                        sbJoin.Append(".");
                                    }
                                    string secondaryTable = string.Empty;
                                    if (easyAttrManyToMany.Table != null)
                                    {
                                        sbJoin.Append(easyAttrManyToMany.Table);
                                        secondaryTable = easyAttrManyToMany.Table;
                                    }
                                    else
                                    {
                                        sbQuery.Append(HasAndBelongsToManyAttr.MapType.Name);
                                        secondaryTable = HasAndBelongsToManyAttr.MapType.Name;
                                    }
                                    sbJoin.Append(" ON ");
                                    sbJoin.Append(HasAndBelongsToManyAttr.Table);
                                    sbJoin.Append(".");
                                    sbJoin.Append(aKeyColumn2);
                                    sbJoin.Append(" = ");
                                    sbJoin.Append(secondaryTable);
                                    sbJoin.Append(".");
                                    sbJoin.Append(fKeyColumn);

                                    sbQuery.Append(sbJoin.ToString());
                                    sbQuery.Append(" WHERE ");
                                    sbQuery.Append(primaryTable);
                                    sbQuery.Append(".");
                                    sbQuery.Append(pKeyColumn);
                                    sbQuery.Append(" = ");
                                    sbQuery.Append(pKeyValManyToMany);
                                    if (HasAndBelongsToManyAttr.Where != null)
                                    {
                                        sbQuery.Append(" AND ");
                                        sbQuery.Append(HasAndBelongsToManyAttr.Where);
                                    }
                                    if (HasAndBelongsToManyAttr.OrderBy != null)
                                    {
                                        sbQuery.Append(" ORDER BY ");
                                        sbQuery.Append(HasAndBelongsToManyAttr.OrderBy);
                                    }
                                    using (EasySession newSession = new EasySession())
                                    {
                                        using (var subCommand = new MySqlCommand(sbQuery.ToString(), newSession.MConnection, newSession.MTransaction))
                                        {
                                            try
                                            {
                                                using (MySqlDataReader subResultDataReader = subCommand.ExecuteReader())
                                                {
                                                    while (true)
                                                    {
                                                        object subResultObject = Activator.CreateInstance(HasAndBelongsToManyAttr.MapType);
                                                        if (subResultDataReader.Read())
                                                        {
                                                            int fieldCount = subResultDataReader.FieldCount;
                                                            if (fieldCount != 0)
                                                            {
                                                                Type foreignSubHasAndBelongsClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, HasAndBelongsToManyAttr.MapType.FullName);
                                                                PropertyInfo[] foreignSubHasAndBelongsClassProperties = foreignSubHasAndBelongsClassPropType.GetProperties();
                                                                EasyDataAttribute easyAttrSubHasAndBelongs = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignSubHasAndBelongsClassPropType, typeof(EasyDataAttribute));

                                                                foreach (PropertyInfo foreignSubHasAndBelongsClassProperty in foreignSubHasAndBelongsClassProperties)
                                                                {
                                                                    Attribute[] foreignSubHasAndBelongsCustomAttr = Attribute.GetCustomAttributes(foreignSubHasAndBelongsClassProperty);
                                                                    for (int j = 0; j < foreignSubHasAndBelongsCustomAttr.Length; j++)
                                                                    {
                                                                        Type foreignSubHasAndBelongsAttrType = foreignSubHasAndBelongsCustomAttr[j].GetType();
                                                                        switch (foreignSubHasAndBelongsAttrType.Name)
                                                                        {
                                                                            case "PrimaryKeyAttribute":

                                                                                PrimaryKeyAttribute priSubKeyAttr = (PrimaryKeyAttribute)foreignSubHasAndBelongsCustomAttr[i];
                                                                                if (priSubKeyAttr.Column != null)
                                                                                {
                                                                                    if (easyAttrSubHasAndBelongs.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasAndBelongs.Table + "_" + priSubKeyAttr.Column;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasAndBelongsClassPropType.Name + "_" + priSubKeyAttr.Column;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (easyAttrSubHasAndBelongs.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasAndBelongs.Table + "_" + foreignSubHasAndBelongsClassProperty.Name;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasAndBelongsClassPropType.Name + "_" + foreignSubHasAndBelongsClassProperty.Name;
                                                                                    }
                                                                                }

                                                                                for (int l = 0; l < intFieldCount; l++)
                                                                                {
                                                                                    if (columnName == subResultDataReader.GetName(l))
                                                                                    {
                                                                                        PropertyInfo PropertyForMainCls = HasAndBelongsToManyAttr.MapType.GetProperty(foreignSubHasAndBelongsClassProperty.Name);
                                                                                        if (subResultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                                                        else { PropertyForMainCls.SetValue(subResultObject, DataReader<MySqlDataReader>.GetDBValue(subResultDataReader, columnName), null); }
                                                                                        break;
                                                                                    }
                                                                                }
                                                                                break;

                                                                            case "PropertyAttribute":
                                                                                PropertyAttribute propSubAttr = (PropertyAttribute)foreignSubHasAndBelongsCustomAttr[i];
                                                                                if (propSubAttr.Column != null)
                                                                                {
                                                                                    if (easyAttrSubHasAndBelongs.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasAndBelongs.Table + "_" + propSubAttr.Column;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasAndBelongsClassPropType.Name + "_" + propSubAttr.Column;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (easyAttrSubHasAndBelongs.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasAndBelongs.Table + "_" + foreignSubHasAndBelongsClassProperty.Name;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasAndBelongsClassPropType.Name + "_" + foreignSubHasAndBelongsClassProperty.Name;
                                                                                    }
                                                                                }

                                                                                for (int l = 0; l < intFieldCount; l++)
                                                                                {
                                                                                    if (columnName == subResultDataReader.GetName(l))
                                                                                    {
                                                                                        PropertyInfo PropertyForMainCls = HasAndBelongsToManyAttr.MapType.GetProperty(foreignSubHasAndBelongsClassProperty.Name);
                                                                                        if (subResultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                                                        else { PropertyForMainCls.SetValue(subResultObject, DataReader<MySqlDataReader>.GetDBValue(subResultDataReader, columnName), null); }
                                                                                        break;
                                                                                    }
                                                                                }
                                                                                break;

                                                                            default:
                                                                                break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            subResultObject = null;
                                                        }

                                                        if (subResultObject == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            listManyToMany.Add(subResultObject);
                                                        }
                                                    }
                                                    subResultDataReader.Close();
                                                }
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }

                                    PropertyInfo HasAndBelongsProperty = type.GetProperty(classProperty.Name);
                                    HasAndBelongsProperty.SetValue(resultObject, listManyToMany, null);

                                    break;
                                case "HasManyAttribute":
                                    HasManyAttribute HasManyAttr = (HasManyAttribute)customAttr[i];
                                    if (easyLoadGlobal.Equals(EasyLoad.Specified))
                                    {
                                        if (!HasManyAttr.PartialLoad)
                                        {
                                            break;
                                        }
                                    }
                                    else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                                    {
                                        break;
                                    }

                                    IList list = new List<object>();
                                    this.GetPrimaryKeyColumn(type);
                                    pKeyColumn = propertyColumnByType;
                                    string column = easyAttr.Table + "_" + pKeyColumn;
                                    object pKeyVal = DataReader<MySqlDataReader>.GetDBValue(resultDataReader, column);

                                    if (HasManyAttr.ColumnKey != null)
                                    {
                                        fKeyColumn = HasManyAttr.ColumnKey;
                                    }
                                    else
                                    {
                                        fKeyColumn = classProperty.Name;
                                    }

                                    EasyDataAttribute easyAttrHasMany = (EasyDataAttribute)Attribute.GetCustomAttribute(HasManyAttr.MapType, typeof(EasyDataAttribute));
                                    Type foreignHasManyClassPropType = HasManyAttr.MapType;
                                    PropertyInfo[] foreignHasManyClassProperties = foreignHasManyClassPropType.GetProperties();
                                    StringBuilder sbHasManyQuery = new StringBuilder();
                                    StringBuilder sbHasManySelect = new StringBuilder();
                                    string seperatorHasMany = string.Empty;
                                    sbHasManyQuery.Append("SELECT ");

                                    foreach (PropertyInfo foreignHasManyClassProperty in foreignHasManyClassProperties)
                                    {
                                        Attribute[] foreignHasManyCustomAttr = Attribute.GetCustomAttributes(foreignHasManyClassProperty);
                                        for (int j = 0; j < foreignHasManyCustomAttr.Length; j++)
                                        {
                                            Type foreignHasManyAttrType = foreignHasManyCustomAttr[j].GetType();
                                            switch (foreignHasManyAttrType.Name)
                                            {
                                                case "PrimaryKeyAttribute":
                                                    PrimaryKeyAttribute primaryKeyAttr = (PrimaryKeyAttribute)foreignHasManyCustomAttr[j];
                                                    if (primaryKeyAttr.Column != null)
                                                    {
                                                        columnName = primaryKeyAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasManyClassProperty.Name;
                                                    }

                                                    if (sbHasManySelect.Length != 0)
                                                    {
                                                        seperatorHasMany = ", ";
                                                    }
                                                    sbHasManySelect.Append(seperatorHasMany);
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append(".");
                                                    sbHasManySelect.Append(columnName);
                                                    sbHasManySelect.Append(" AS ");
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append("_");
                                                    sbHasManySelect.Append(columnName);
                                                    break;
                                                case "PropertyAttribute":
                                                    PropertyAttribute fPropAttr = (PropertyAttribute)foreignHasManyCustomAttr[j];
                                                    if (fPropAttr.Column != null)
                                                    {
                                                        columnName = fPropAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasManyClassProperty.Name;
                                                    }

                                                    if (sbHasManySelect.Length != 0)
                                                    {
                                                        seperatorHasMany = ", ";
                                                    }
                                                    sbHasManySelect.Append(seperatorHasMany);
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append(".");
                                                    sbHasManySelect.Append(columnName);
                                                    sbHasManySelect.Append(" AS ");
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append("_");
                                                    sbHasManySelect.Append(columnName);
                                                    break;
                                                case "BelongsToAttribute":
                                                    BelongsToAttribute fBelongsToAttr = (BelongsToAttribute)foreignHasManyCustomAttr[j];
                                                    if (fBelongsToAttr.Column != null)
                                                    {
                                                        columnName = fBelongsToAttr.Column;
                                                    }
                                                    else
                                                    {
                                                        columnName = foreignHasManyClassProperty.Name;
                                                    }

                                                    if (sbHasManySelect.Length != 0)
                                                    {
                                                        seperatorHasMany = ", ";
                                                    }
                                                    sbHasManySelect.Append(seperatorHasMany);
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append(".");
                                                    sbHasManySelect.Append(columnName);
                                                    sbHasManySelect.Append(" AS ");
                                                    sbHasManySelect.Append(easyAttrHasMany.Table);
                                                    sbHasManySelect.Append("_");
                                                    sbHasManySelect.Append(columnName);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }

                                    sbHasManyQuery.Append(sbHasManySelect.ToString());
                                    sbHasManyQuery.Append(" FROM ");
                                    if (easyAttrHasMany.Schema != null)
                                    {
                                        sbHasManyQuery.Append(easyAttrHasMany.Schema);
                                        sbHasManyQuery.Append(".");
                                    }
                                    sbHasManyQuery.Append(easyAttrHasMany.Table);
                                    sbHasManyQuery.Append(" WHERE ");
                                    sbHasManyQuery.Append(easyAttrHasMany.Table);
                                    sbHasManyQuery.Append(".");
                                    sbHasManyQuery.Append(fKeyColumn);
                                    sbHasManyQuery.Append(" = ");
                                    sbHasManyQuery.Append(pKeyVal);
                                    if (HasManyAttr.Where != null)
                                    {
                                        sbHasManyQuery.Append(" AND ");
                                        sbHasManyQuery.Append(HasManyAttr.Where);
                                    }
                                    if (HasManyAttr.OrderBy != null)
                                    {
                                        sbHasManyQuery.Append(" ORDER BY ");
                                        sbHasManyQuery.Append(HasManyAttr.OrderBy);
                                    }
                                    using (EasySession newSession = new EasySession())
                                    {
                                        using (var subCommand = new MySqlCommand(sbHasManyQuery.ToString(), newSession.MConnection, newSession.MTransaction))
                                        {
                                            try
                                            {
                                                using (MySqlDataReader subResultDataReader = subCommand.ExecuteReader())
                                                {
                                                    while (true)
                                                    {
                                                        object subResultObject = Activator.CreateInstance(HasManyAttr.MapType);
                                                        if (subResultDataReader.Read())
                                                        {
                                                            int fieldCount = subResultDataReader.FieldCount;
                                                            if (fieldCount != 0)
                                                            {
                                                                Type foreignSubHasManyClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, HasManyAttr.MapType.FullName);
                                                                PropertyInfo[] foreignSubHasManyClassProperties = foreignSubHasManyClassPropType.GetProperties();
                                                                EasyDataAttribute easyAttrSubHasMany = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignSubHasManyClassPropType, typeof(EasyDataAttribute));

                                                                foreach (PropertyInfo foreignSubHasManyClassProperty in foreignSubHasManyClassProperties)
                                                                {
                                                                    Attribute[] foreignSubHasManyCustomAttr = Attribute.GetCustomAttributes(foreignSubHasManyClassProperty);
                                                                    for (int j = 0; j < foreignSubHasManyCustomAttr.Length; j++)
                                                                    {
                                                                        Type foreignSubHasManyAttrType = foreignSubHasManyCustomAttr[j].GetType();
                                                                        switch (foreignSubHasManyAttrType.Name)
                                                                        {
                                                                            case "PrimaryKeyAttribute":

                                                                                PrimaryKeyAttribute priSubKeyAttr = (PrimaryKeyAttribute)foreignSubHasManyCustomAttr[i];
                                                                                if (priSubKeyAttr.Column != null)
                                                                                {
                                                                                    if (easyAttrSubHasMany.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasMany.Table + "_" + priSubKeyAttr.Column;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasManyClassPropType.Name + "_" + priSubKeyAttr.Column;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (easyAttrSubHasMany.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasMany.Table + "_" + foreignSubHasManyClassProperty.Name;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasManyClassPropType.Name + "_" + foreignSubHasManyClassProperty.Name;
                                                                                    }
                                                                                }

                                                                                for (int l = 0; l < intFieldCount; l++)
                                                                                {
                                                                                    if (columnName == subResultDataReader.GetName(l))
                                                                                    {
                                                                                        PropertyInfo PropertyForMainCls = HasManyAttr.MapType.GetProperty(foreignSubHasManyClassProperty.Name);
                                                                                        if (subResultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                                                        else { PropertyForMainCls.SetValue(subResultObject, DataReader<MySqlDataReader>.GetDBValue(subResultDataReader, columnName), null); }
                                                                                        break;
                                                                                    }
                                                                                }
                                                                                break;

                                                                            case "PropertyAttribute":
                                                                                PropertyAttribute propSubAttr = (PropertyAttribute)foreignSubHasManyCustomAttr[i];
                                                                                if (propSubAttr.Column != null)
                                                                                {
                                                                                    if (easyAttrSubHasMany.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasMany.Table + "_" + propSubAttr.Column;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasManyClassPropType.Name + "_" + propSubAttr.Column;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (easyAttrSubHasMany.Table != null)
                                                                                    {
                                                                                        columnName = easyAttrSubHasMany.Table + "_" + foreignSubHasManyClassProperty.Name;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        columnName = foreignSubHasManyClassPropType.Name + "_" + foreignSubHasManyClassProperty.Name;
                                                                                    }
                                                                                }

                                                                                for (int l = 0; l < intFieldCount; l++)
                                                                                {
                                                                                    if (columnName == subResultDataReader.GetName(l))
                                                                                    {
                                                                                        PropertyInfo PropertyForMainCls = HasManyAttr.MapType.GetProperty(foreignSubHasManyClassProperty.Name);
                                                                                        if (subResultDataReader.IsDBNull(l)) { /*Do Nothing*/ }
                                                                                        else { PropertyForMainCls.SetValue(subResultObject, DataReader<MySqlDataReader>.GetDBValue(subResultDataReader, columnName), null); }
                                                                                        break;
                                                                                    }
                                                                                }
                                                                                break;

                                                                            default:
                                                                                break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            subResultObject = null;
                                                        }

                                                        if (subResultObject == null)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            list.Add(subResultObject);
                                                        }
                                                    }
                                                    subResultDataReader.Close();
                                                }
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }

                                    PropertyInfo HasManyProperty = type.GetProperty(classProperty.Name);
                                    HasManyProperty.SetValue(resultObject, list, null);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                resultObject = null;
            }

            T returnObject = (T)Convert.ChangeType(resultObject, typeof(T));
            return returnObject;
        }

        /// <summary>
        /// Build the select query.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>Return the SqlCommand to execute</returns>
        protected SqlCommand SelectQueryBuilder(Type type, string where, string property, object value, string query, EasySession easySession)
        {
            SqlCommand command = new SqlCommand(query, easySession.Connection, easySession.Transaction);
            command.CommandType = CommandType.Text;

            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(type, typeof(EasyDataAttribute));
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ");
            StringBuilder sbSelect = new StringBuilder();
            StringBuilder sbFrom = new StringBuilder();
            sbFrom.Append(" FROM ");
            if (easyAttr.Schema != null)
            {
                sbFrom.Append(easyAttr.Schema);
                sbFrom.Append(".");
            }
            if (easyAttr.Table != null)
            {
                sbFrom.Append(easyAttr.Table);
            }
            else
            {
                sbFrom.Append(type.Name);
            }
            StringBuilder sbJoin = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbOrder = new StringBuilder();
            string columnName;
            string seperator = string.Empty;
            PropertyInfo[] classProperties = type.GetProperties();

            foreach (PropertyInfo classProperty in classProperties)
            {
                Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                for (int i = 0; i < customAttr.Length; i++)
                {
                    Type attrType = customAttr[i].GetType();
                    switch (attrType.Name)
                    {
                        case "PrimaryKeyAttribute":
                            PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                            if (easyLoadGlobal.Equals(EasyLoad.Specified))
                            {
                                if (!priKeyAttr.PartialLoad)
                                {
                                    break;
                                }
                            }
                            
                            if (priKeyAttr.Column != null)
                            {
                                columnName = priKeyAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }

                            if (sbSelect.Length != 0)
                            {
                                seperator = ", ";
                            }
                            sbSelect.Append(seperator);
                            sbSelect.Append(easyAttr.Table);
                            sbSelect.Append(".");
                            sbSelect.Append(columnName);
                            sbSelect.Append(" AS ");
                            sbSelect.Append(easyAttr.Table);
                            sbSelect.Append("_");
                            sbSelect.Append(columnName);
                            break;
                        
                        case "BelongsToAttribute":
                            BelongsToAttribute belongAttr = (BelongsToAttribute)customAttr[i];
                            if (easyLoadGlobal.Equals(EasyLoad.Specified))
                            {
                                if (!belongAttr.PartialLoad)
                                {
                                    break;
                                }
                            }
                            else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                            {
                                break;
                            }
                            
                            if (belongAttr.Column != null)
                            {
                                fKeyColumn = belongAttr.Column;
                            }
                            else
                            {
                                fKeyColumn = classProperty.Name;
                            }

                            Type foreignClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                            PropertyInfo foreignPropType = null;

                            EasyDataAttribute easyAttrBelongsTo = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignClassPropType, typeof(EasyDataAttribute));

                            if (belongAttr.PropertyRef != null)
                            {
                                foreignPropType = foreignClassPropType.GetProperty(belongAttr.PropertyRef);
                            }
                            PropertyInfo[] foreignClassProperties = foreignClassPropType.GetProperties();

                            foreach (PropertyInfo foreignClassProperty in foreignClassProperties)
                            {
                                Attribute[] foreignCustomAttr = Attribute.GetCustomAttributes(foreignClassProperty);
                                for (int j = 0; j < foreignCustomAttr.Length; j++)
                                {
                                    Type foreignAttrType = foreignCustomAttr[j].GetType();
                                    switch (foreignAttrType.Name)
                                    {
                                        case "PrimaryKeyAttribute":
                                            PrimaryKeyAttribute fPrimaryAttr = (PrimaryKeyAttribute)foreignCustomAttr[j];

                                            if (fPrimaryAttr.Column != null)
                                            {
                                                columnName = fPrimaryAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignClassProperty.Name;
                                            }

                                            if (foreignPropType == null)
                                            {
                                                pKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "PropertyAttribute":
                                            PropertyAttribute fPropAttr = (PropertyAttribute)foreignCustomAttr[j];
                                            if (fPropAttr.Column != null)
                                            {
                                                columnName = fPropAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignClassProperty.Name;
                                            }

                                            if (foreignPropType != null && foreignPropType == foreignClassProperty)
                                            {
                                                pKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "HasManyAttribute":
                                            HasManyAttribute fHasManyAttr = (HasManyAttribute)foreignCustomAttr[j];
                                            if (fHasManyAttr.ColumnKey != null)
                                            {
                                                columnName = fHasManyAttr.ColumnKey;
                                            }
                                            else
                                            {
                                                columnName = foreignClassProperty.Name;
                                            }

                                            if (foreignPropType != null && foreignPropType == foreignClassProperty)
                                            {
                                                pKeyColumn = columnName;
                                            }
                                            break;
                                        case "BelongsToAttribute":
                                            BelongsToAttribute fBelongsToAttr = (BelongsToAttribute)foreignCustomAttr[j];
                                            if (fBelongsToAttr.Column != null)
                                            {
                                                columnName = fBelongsToAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignClassProperty.Name;
                                            }

                                            if (foreignPropType != null && foreignPropType == foreignClassProperty)
                                            {
                                                pKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "HasAndBelongsToManyAttribute":
                                            HasAndBelongsToManyAttribute fHasBelongsToAttr = (HasAndBelongsToManyAttribute)foreignCustomAttr[j];
                                            if (fHasBelongsToAttr.ColumnKey != null)
                                            {
                                                columnName = fHasBelongsToAttr.ColumnKey;
                                            }
                                            else
                                            {
                                                columnName = foreignClassProperty.Name;
                                            }

                                            if (foreignPropType != null && foreignPropType == foreignClassProperty)
                                            {
                                                pKeyColumn = columnName;
                                            }
                                            break;
                                        case "OneToOneAttribute":
                                            //OneToOneAttribute fOneToOneAttr = (OneToOneAttribute)foreignCustomAttr[j];
                                            //if (fOneToOneAttr.Column != null)
                                            //{
                                            //    columnName = fOneToOneAttr.Column;
                                            //}
                                            //else
                                            //{
                                            //    columnName = foreignClassProperty.Name;
                                            //}

                                            //if (foreignPropType != null && foreignPropType == foreignClassProperty)
                                            //{
                                                //pKeyColumn = columnName;
                                            //}
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }

                            sbJoin.Append(" INNER JOIN ");
                            if (easyAttrBelongsTo.Schema != null)
                            {
                                sbJoin.Append(easyAttrBelongsTo.Schema);
                                sbJoin.Append(".");
                            }
                            sbJoin.Append(easyAttrBelongsTo.Table);
                            sbJoin.Append(" ON ");
                            sbJoin.Append(easyAttr.Table);
                            sbJoin.Append(".");
                            sbJoin.Append(fKeyColumn);
                            sbJoin.Append(" = ");
                            sbJoin.Append(easyAttrBelongsTo.Table);
                            sbJoin.Append(".");
                            sbJoin.Append(pKeyColumn);
                            break;
                        
                        case "OneToOneAttribute":
                            OneToOneAttribute OneToOneAttr = (OneToOneAttribute)customAttr[i];
                            if (easyLoadGlobal.Equals(EasyLoad.Specified))
                            {
                                if (!OneToOneAttr.PartialLoad)
                                {
                                    break;
                                }
                            }
                            else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                            {
                                break;
                            }

                            this.GetPrimaryKeyColumn(type);
                            pKeyColumn = propertyColumnByType;
                            Type foreignOneToOneClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                            PropertyInfo foreignOneToOnePropType = null;

                            EasyDataAttribute easyAttrOneToOne = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignOneToOneClassPropType, typeof(EasyDataAttribute));

                            if (OneToOneAttr.PropertyRef != null)
                            {
                                foreignOneToOnePropType = foreignOneToOneClassPropType.GetProperty(OneToOneAttr.PropertyRef);
                            }
                            PropertyInfo[] foreignOneToOneClassProperties = foreignOneToOneClassPropType.GetProperties();

                            foreach (PropertyInfo foreignOneToOneClassProperty in foreignOneToOneClassProperties)
                            {
                                Attribute[] foreignOneToOneCustomAttr = Attribute.GetCustomAttributes(foreignOneToOneClassProperty);
                                for (int j = 0; j < foreignOneToOneCustomAttr.Length; j++)
                                {
                                    Type foreignOneToOneAttrType = foreignOneToOneCustomAttr[j].GetType();
                                    switch (foreignOneToOneAttrType.Name)
                                    {
                                        case "PrimaryKeyAttribute":
                                            PrimaryKeyAttribute fPrimaryAttr = (PrimaryKeyAttribute)foreignOneToOneCustomAttr[j];

                                            if (fPrimaryAttr.Column != null)
                                            {
                                                columnName = fPrimaryAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignOneToOneClassProperty.Name;
                                            }

                                            if (foreignOneToOnePropType == null)
                                            {
                                                fKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "PropertyAttribute":
                                            PropertyAttribute fPropAttr = (PropertyAttribute)foreignOneToOneCustomAttr[j];
                                            if (fPropAttr.Column != null)
                                            {
                                                columnName = fPropAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignOneToOneClassProperty.Name;
                                            }

                                            if (foreignOneToOnePropType != null && foreignOneToOnePropType == foreignOneToOneClassProperty)
                                            {
                                                fKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "HasManyAttribute":
                                            HasManyAttribute fHasManyAttr = (HasManyAttribute)foreignOneToOneCustomAttr[j];
                                            if (fHasManyAttr.ColumnKey != null)
                                            {
                                                columnName = fHasManyAttr.ColumnKey;
                                            }
                                            else
                                            {
                                                columnName = foreignOneToOneClassProperty.Name;
                                            }

                                            if (foreignOneToOnePropType != null && foreignOneToOnePropType == foreignOneToOneClassProperty)
                                            {
                                                fKeyColumn = columnName;
                                            }
                                            break;
                                        case "BelongsToAttribute":
                                            BelongsToAttribute fBelongsToAttr = (BelongsToAttribute)foreignOneToOneCustomAttr[j];
                                            if (fBelongsToAttr.Column != null)
                                            {
                                                columnName = fBelongsToAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignOneToOneClassProperty.Name;
                                            }

                                            if (foreignOneToOnePropType != null && foreignOneToOnePropType == foreignOneToOneClassProperty)
                                            {
                                                fKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "HasAndBelongsToManyAttribute":
                                            HasAndBelongsToManyAttribute fHasBelongsToAttr = (HasAndBelongsToManyAttribute)foreignOneToOneCustomAttr[j];
                                            if (fHasBelongsToAttr.ColumnKey != null)
                                            {
                                                columnName = fHasBelongsToAttr.ColumnKey;
                                            }
                                            else
                                            {
                                                columnName = foreignOneToOneClassProperty.Name;
                                            }

                                            if (foreignOneToOnePropType != null && foreignOneToOnePropType == foreignOneToOneClassProperty)
                                            {
                                                fKeyColumn = columnName;
                                            }
                                            break;
                                        case "OneToOneAttribute":
                                            //OneToOneAttribute fOneToOneAttr = (OneToOneAttribute)foreignOneToOneCustomAttr[j];
                                            //if (fOneToOneAttr.Column != null)
                                            //{
                                            //    columnName = fOneToOneAttr.Column;
                                            //}
                                            //else
                                            //{
                                            //    columnName = foreignOneToOneClassProperty.Name;
                                            //}

                                            //if (foreignOneToOnePropType != null && foreignOneToOnePropType == foreignOneToOneClassProperty)
                                            //{
                                                //fKeyColumn = columnName;
                                            //}
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }

                            sbJoin.Append(" LEFT JOIN ");
                            if (easyAttrOneToOne.Schema != null)
                            {
                                sbJoin.Append(easyAttrOneToOne.Schema);
                                sbJoin.Append(".");
                            }
                            sbJoin.Append(easyAttrOneToOne.Table);
                            sbJoin.Append(" ON ");
                            sbJoin.Append(easyAttr.Table);
                            sbJoin.Append(".");
                            sbJoin.Append(pKeyColumn);
                            sbJoin.Append(" = ");
                            sbJoin.Append(easyAttrOneToOne.Table);
                            sbJoin.Append(".");
                            sbJoin.Append(fKeyColumn);
                            break;

                        case "PropertyAttribute":
                            PropertyAttribute propAttr = (PropertyAttribute)customAttr[i];
                            if (easyLoadGlobal.Equals(EasyLoad.Specified))
                            {
                                if (!propAttr.PartialLoad)
                                {
                                    break;
                                }
                            }

                            if (propAttr.Column != null)
                            {
                                columnName = propAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }
                            if (sbSelect.Length != 0)
                            {
                                seperator = ", ";
                            }
                            sbSelect.Append(seperator);
                            sbSelect.Append(easyAttr.Table);
                            sbSelect.Append(".");
                            sbSelect.Append(columnName);
                            sbSelect.Append(" AS ");
                            sbSelect.Append(easyAttr.Table);
                            sbSelect.Append("_");
                            sbSelect.Append(columnName);

                            break;
                        default:

                            break;
                    }
                }
            }

            if (sbSelect.Length == 0)
            {
                sbSelect.Append("*");
            }

            if (property == null && value != null)
            {
                GetPrimaryKeyColumn(type);

                command.Parameters.Add(SqlParameters.CreateInputParameter("@" + propertyColumnByType + "_", SqlDBTypes.GetDbType(propertyTypeByType), value));

                sbWhere.Append(" WHERE ");
                sbWhere.Append(easyAttr.Table);
                sbWhere.Append(".");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append(" = ");
                sbWhere.Append("@");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append("_");
            }
            else if (property != null && value != null)
            {
                GetPropertyColumn(type, property);
                sbWhere.Append(" WHERE ");
                sbWhere.Append(easyAttr.Table);
                sbWhere.Append(".");
                sbWhere.Append(propertyColumnByType);
                if (propertyTypeByType == "System.DateTime")
                {
                    command.Parameters.Add(SqlParameters.CreateInputParameter("@From_", SqlDBTypes.GetDbType(propertyTypeByType), value));
                    command.Parameters.Add(SqlParameters.CreateInputParameter("@To_", SqlDBTypes.GetDbType(propertyTypeByType), value));

                    sbWhere.Append(" BETWEEN ");
                    sbWhere.Append("@From_");
                    sbWhere.Append(" AND ");
                    sbWhere.Append("DATEADD(Day, 1, ");
                    sbWhere.Append("@To_)");
                }
                else
                {
                    sbWhere.Append(" = ");
                    sbWhere.Append("@");
                    sbWhere.Append(propertyColumnByType);
                    sbWhere.Append("_");

                    command.Parameters.Add(SqlParameters.CreateInputParameter("@" + propertyColumnByType + "_", SqlDBTypes.GetDbType(propertyTypeByType), value));
                }
            }
            else if (where != null)
            {
                sbWhere.Append(" WHERE ");
                sbWhere.Append(where);
            }

            sbQuery.Append(sbSelect.ToString());
            sbQuery.Append(sbFrom.ToString());
            sbQuery.Append(sbJoin.ToString());
            sbQuery.Append(sbWhere.ToString());
            sbQuery.Append(sbOrder.ToString());

            command.CommandText = sbQuery.ToString();
            return command;
        }

        /// <summary>
        /// Build the select query.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>Return the SqlCommand to execute</returns>
        protected OracleCommand OracleSelectQueryBuilder(Type type, string where, string property, object value, string query, EasySession easySession)
        {
            OracleCommand command = new OracleCommand(query, easySession.OConnection, easySession.OTransaction);
            command.CommandType = CommandType.Text;

            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(type, typeof(EasyDataAttribute));
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ");
            StringBuilder sbSelect = new StringBuilder();
            StringBuilder sbFrom = new StringBuilder();
            sbFrom.Append(" FROM ");
            if (easyAttr.Schema != null)
            {
                sbFrom.Append(easyAttr.Schema);
                sbFrom.Append(".");
            }
            if (easyAttr.Table != null)
            {
                sbFrom.Append(easyAttr.Table);
            }
            else
            {
                sbFrom.Append(type.Name);
            }
            StringBuilder sbJoin = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbOrder = new StringBuilder();
            string columnName;
            string seperator = string.Empty;
            PropertyInfo[] classProperties = type.GetProperties();

            foreach (PropertyInfo classProperty in classProperties)
            {
                Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                for (int i = 0; i < customAttr.Length; i++)
                {
                    Type attrType = customAttr[i].GetType();
                    switch (attrType.Name)
                    {
                        case "PrimaryKeyAttribute":
                            PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                            if (easyLoadGlobal.Equals(EasyLoad.Specified))
                            {
                                if (!priKeyAttr.PartialLoad)
                                {
                                    break;
                                }
                            }

                            if (priKeyAttr.Column != null)
                            {
                                columnName = priKeyAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }

                            if (sbSelect.Length != 0)
                            {
                                seperator = ", ";
                            }
                            sbSelect.Append(seperator);
                            sbSelect.Append(easyAttr.Table);
                            sbSelect.Append(".");
                            sbSelect.Append(columnName);
                            sbSelect.Append(" AS ");
                            sbSelect.Append(easyAttr.Table);
                            sbSelect.Append("_");
                            sbSelect.Append(columnName);
                            break;

                        case "BelongsToAttribute":
                            BelongsToAttribute belongAttr = (BelongsToAttribute)customAttr[i];
                            if (easyLoadGlobal.Equals(EasyLoad.Specified))
                            {
                                if (!belongAttr.PartialLoad)
                                {
                                    break;
                                }
                            }
                            else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                            {
                                break;
                            }

                            if (belongAttr.Column != null)
                            {
                                fKeyColumn = belongAttr.Column;
                            }
                            else
                            {
                                fKeyColumn = classProperty.Name;
                            }

                            Type foreignClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                            PropertyInfo foreignPropType = null;

                            EasyDataAttribute easyAttrBelongsTo = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignClassPropType, typeof(EasyDataAttribute));

                            if (belongAttr.PropertyRef != null)
                            {
                                foreignPropType = foreignClassPropType.GetProperty(belongAttr.PropertyRef);
                            }
                            PropertyInfo[] foreignClassProperties = foreignClassPropType.GetProperties();

                            foreach (PropertyInfo foreignClassProperty in foreignClassProperties)
                            {
                                Attribute[] foreignCustomAttr = Attribute.GetCustomAttributes(foreignClassProperty);
                                for (int j = 0; j < foreignCustomAttr.Length; j++)
                                {
                                    Type foreignAttrType = foreignCustomAttr[j].GetType();
                                    switch (foreignAttrType.Name)
                                    {
                                        case "PrimaryKeyAttribute":
                                            PrimaryKeyAttribute fPrimaryAttr = (PrimaryKeyAttribute)foreignCustomAttr[j];

                                            if (fPrimaryAttr.Column != null)
                                            {
                                                columnName = fPrimaryAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignClassProperty.Name;
                                            }

                                            if (foreignPropType == null)
                                            {
                                                pKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "PropertyAttribute":
                                            PropertyAttribute fPropAttr = (PropertyAttribute)foreignCustomAttr[j];
                                            if (fPropAttr.Column != null)
                                            {
                                                columnName = fPropAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignClassProperty.Name;
                                            }

                                            if (foreignPropType != null && foreignPropType == foreignClassProperty)
                                            {
                                                pKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "HasManyAttribute":
                                            HasManyAttribute fHasManyAttr = (HasManyAttribute)foreignCustomAttr[j];
                                            if (fHasManyAttr.ColumnKey != null)
                                            {
                                                columnName = fHasManyAttr.ColumnKey;
                                            }
                                            else
                                            {
                                                columnName = foreignClassProperty.Name;
                                            }

                                            if (foreignPropType != null && foreignPropType == foreignClassProperty)
                                            {
                                                pKeyColumn = columnName;
                                            }
                                            break;
                                        case "BelongsToAttribute":
                                            BelongsToAttribute fBelongsToAttr = (BelongsToAttribute)foreignCustomAttr[j];
                                            if (fBelongsToAttr.Column != null)
                                            {
                                                columnName = fBelongsToAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignClassProperty.Name;
                                            }

                                            if (foreignPropType != null && foreignPropType == foreignClassProperty)
                                            {
                                                pKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "HasAndBelongsToManyAttribute":
                                            HasAndBelongsToManyAttribute fHasBelongsToAttr = (HasAndBelongsToManyAttribute)foreignCustomAttr[j];
                                            if (fHasBelongsToAttr.ColumnKey != null)
                                            {
                                                columnName = fHasBelongsToAttr.ColumnKey;
                                            }
                                            else
                                            {
                                                columnName = foreignClassProperty.Name;
                                            }

                                            if (foreignPropType != null && foreignPropType == foreignClassProperty)
                                            {
                                                pKeyColumn = columnName;
                                            }
                                            break;
                                        case "OneToOneAttribute":
                                            //OneToOneAttribute fOneToOneAttr = (OneToOneAttribute)foreignCustomAttr[j];
                                            //if (fOneToOneAttr.Column != null)
                                            //{
                                            //    columnName = fOneToOneAttr.Column;
                                            //}
                                            //else
                                            //{
                                            //    columnName = foreignClassProperty.Name;
                                            //}

                                            //if (foreignPropType != null && foreignPropType == foreignClassProperty)
                                            //{
                                            //pKeyColumn = columnName;
                                            //}
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }

                            sbJoin.Append(" INNER JOIN ");
                            if (easyAttrBelongsTo.Schema != null)
                            {
                                sbJoin.Append(easyAttrBelongsTo.Schema);
                                sbJoin.Append(".");
                            }
                            sbJoin.Append(easyAttrBelongsTo.Table);
                            sbJoin.Append(" ON ");
                            sbJoin.Append(easyAttr.Table);
                            sbJoin.Append(".");
                            sbJoin.Append(fKeyColumn);
                            sbJoin.Append(" = ");
                            sbJoin.Append(easyAttrBelongsTo.Table);
                            sbJoin.Append(".");
                            sbJoin.Append(pKeyColumn);
                            break;

                        case "OneToOneAttribute":
                            OneToOneAttribute OneToOneAttr = (OneToOneAttribute)customAttr[i];
                            if (easyLoadGlobal.Equals(EasyLoad.Specified))
                            {
                                if (!OneToOneAttr.PartialLoad)
                                {
                                    break;
                                }
                            }
                            else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                            {
                                break;
                            }

                            this.GetPrimaryKeyColumn(type);
                            pKeyColumn = propertyColumnByType;
                            Type foreignOneToOneClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                            PropertyInfo foreignOneToOnePropType = null;

                            EasyDataAttribute easyAttrOneToOne = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignOneToOneClassPropType, typeof(EasyDataAttribute));

                            if (OneToOneAttr.PropertyRef != null)
                            {
                                foreignOneToOnePropType = foreignOneToOneClassPropType.GetProperty(OneToOneAttr.PropertyRef);
                            }
                            PropertyInfo[] foreignOneToOneClassProperties = foreignOneToOneClassPropType.GetProperties();

                            foreach (PropertyInfo foreignOneToOneClassProperty in foreignOneToOneClassProperties)
                            {
                                Attribute[] foreignOneToOneCustomAttr = Attribute.GetCustomAttributes(foreignOneToOneClassProperty);
                                for (int j = 0; j < foreignOneToOneCustomAttr.Length; j++)
                                {
                                    Type foreignOneToOneAttrType = foreignOneToOneCustomAttr[j].GetType();
                                    switch (foreignOneToOneAttrType.Name)
                                    {
                                        case "PrimaryKeyAttribute":
                                            PrimaryKeyAttribute fPrimaryAttr = (PrimaryKeyAttribute)foreignOneToOneCustomAttr[j];

                                            if (fPrimaryAttr.Column != null)
                                            {
                                                columnName = fPrimaryAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignOneToOneClassProperty.Name;
                                            }

                                            if (foreignOneToOnePropType == null)
                                            {
                                                fKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "PropertyAttribute":
                                            PropertyAttribute fPropAttr = (PropertyAttribute)foreignOneToOneCustomAttr[j];
                                            if (fPropAttr.Column != null)
                                            {
                                                columnName = fPropAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignOneToOneClassProperty.Name;
                                            }

                                            if (foreignOneToOnePropType != null && foreignOneToOnePropType == foreignOneToOneClassProperty)
                                            {
                                                fKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "HasManyAttribute":
                                            HasManyAttribute fHasManyAttr = (HasManyAttribute)foreignOneToOneCustomAttr[j];
                                            if (fHasManyAttr.ColumnKey != null)
                                            {
                                                columnName = fHasManyAttr.ColumnKey;
                                            }
                                            else
                                            {
                                                columnName = foreignOneToOneClassProperty.Name;
                                            }

                                            if (foreignOneToOnePropType != null && foreignOneToOnePropType == foreignOneToOneClassProperty)
                                            {
                                                fKeyColumn = columnName;
                                            }
                                            break;
                                        case "BelongsToAttribute":
                                            BelongsToAttribute fBelongsToAttr = (BelongsToAttribute)foreignOneToOneCustomAttr[j];
                                            if (fBelongsToAttr.Column != null)
                                            {
                                                columnName = fBelongsToAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignOneToOneClassProperty.Name;
                                            }

                                            if (foreignOneToOnePropType != null && foreignOneToOnePropType == foreignOneToOneClassProperty)
                                            {
                                                fKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "HasAndBelongsToManyAttribute":
                                            HasAndBelongsToManyAttribute fHasBelongsToAttr = (HasAndBelongsToManyAttribute)foreignOneToOneCustomAttr[j];
                                            if (fHasBelongsToAttr.ColumnKey != null)
                                            {
                                                columnName = fHasBelongsToAttr.ColumnKey;
                                            }
                                            else
                                            {
                                                columnName = foreignOneToOneClassProperty.Name;
                                            }

                                            if (foreignOneToOnePropType != null && foreignOneToOnePropType == foreignOneToOneClassProperty)
                                            {
                                                fKeyColumn = columnName;
                                            }
                                            break;
                                        case "OneToOneAttribute":
                                            //OneToOneAttribute fOneToOneAttr = (OneToOneAttribute)foreignOneToOneCustomAttr[j];
                                            //if (fOneToOneAttr.Column != null)
                                            //{
                                            //    columnName = fOneToOneAttr.Column;
                                            //}
                                            //else
                                            //{
                                            //    columnName = foreignOneToOneClassProperty.Name;
                                            //}

                                            //if (foreignOneToOnePropType != null && foreignOneToOnePropType == foreignOneToOneClassProperty)
                                            //{
                                            //fKeyColumn = columnName;
                                            //}
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }

                            sbJoin.Append(" LEFT JOIN ");
                            if (easyAttrOneToOne.Schema != null)
                            {
                                sbJoin.Append(easyAttrOneToOne.Schema);
                                sbJoin.Append(".");
                            }
                            sbJoin.Append(easyAttrOneToOne.Table);
                            sbJoin.Append(" ON ");
                            sbJoin.Append(easyAttr.Table);
                            sbJoin.Append(".");
                            sbJoin.Append(pKeyColumn);
                            sbJoin.Append(" = ");
                            sbJoin.Append(easyAttrOneToOne.Table);
                            sbJoin.Append(".");
                            sbJoin.Append(fKeyColumn);
                            break;

                        case "PropertyAttribute":
                            PropertyAttribute propAttr = (PropertyAttribute)customAttr[i];
                            if (easyLoadGlobal.Equals(EasyLoad.Specified))
                            {
                                if (!propAttr.PartialLoad)
                                {
                                    break;
                                }
                            }

                            if (propAttr.Column != null)
                            {
                                columnName = propAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }
                            if (sbSelect.Length != 0)
                            {
                                seperator = ", ";
                            }
                            sbSelect.Append(seperator);
                            sbSelect.Append(easyAttr.Table);
                            sbSelect.Append(".");
                            sbSelect.Append(columnName);
                            sbSelect.Append(" AS ");
                            sbSelect.Append(easyAttr.Table);
                            sbSelect.Append("_");
                            sbSelect.Append(columnName);

                            break;
                        default:

                            break;
                    }
                }
            }

            if (sbSelect.Length == 0)
            {
                sbSelect.Append("*");
            }

            if (property == null && value != null)
            {
                GetPrimaryKeyColumn(type);

                command.Parameters.Add(OracleParameters.CreateInputParameter(":" + propertyColumnByType + "_", OracleDBTypes.GetDbType(propertyTypeByType), value));

                sbWhere.Append(" WHERE ");
                sbWhere.Append(easyAttr.Table);
                sbWhere.Append(".");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append(" = ");
                sbWhere.Append(":");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append("_");
            }
            else if (property != null && value != null)
            {
                GetPropertyColumn(type, property);
                sbWhere.Append(" WHERE ");
                sbWhere.Append(easyAttr.Table);
                sbWhere.Append(".");
                sbWhere.Append(propertyColumnByType);
                if (propertyTypeByType == "System.DateTime")
                {
                    command.Parameters.Add(OracleParameters.CreateInputParameter(":From_", OracleDBTypes.GetDbType(propertyTypeByType), value));
                    command.Parameters.Add(OracleParameters.CreateInputParameter(":To_", OracleDBTypes.GetDbType(propertyTypeByType), value));

                    sbWhere.Append(" BETWEEN ");
                    sbWhere.Append(":From_");
                    sbWhere.Append(" AND ");
                    sbWhere.Append("DATEADD(Day, 1, ");
                    sbWhere.Append(":To_)");
                }
                else
                {
                    sbWhere.Append(" = ");
                    sbWhere.Append(":");
                    sbWhere.Append(propertyColumnByType);
                    sbWhere.Append("_");

                    command.Parameters.Add(OracleParameters.CreateInputParameter(":" + propertyColumnByType + "_", OracleDBTypes.GetDbType(propertyTypeByType), value));
                }
            }
            else if (where != null)
            {
                sbWhere.Append(" WHERE ");
                sbWhere.Append(where);
            }

            sbQuery.Append(sbSelect.ToString());
            sbQuery.Append(sbFrom.ToString());
            sbQuery.Append(sbJoin.ToString());
            sbQuery.Append(sbWhere.ToString());
            sbQuery.Append(sbOrder.ToString());

            command.CommandText = sbQuery.ToString();
            return command;
        }

        /// <summary>
        /// Build the select query.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>Return the SqlCommand to execute</returns>
        protected MySqlCommand MySqlSelectQueryBuilder(Type type, string where, string property, object value, string query, EasySession easySession)
        {
            MySqlCommand command = new MySqlCommand(query, easySession.MConnection, easySession.MTransaction);
            command.CommandType = CommandType.Text;

            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(type, typeof(EasyDataAttribute));
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT ");
            StringBuilder sbSelect = new StringBuilder();
            StringBuilder sbFrom = new StringBuilder();
            sbFrom.Append(" FROM ");
            if (easyAttr.Schema != null)
            {
                sbFrom.Append(easyAttr.Schema);
                sbFrom.Append(".");
            }
            if (easyAttr.Table != null)
            {
                sbFrom.Append(easyAttr.Table);
            }
            else
            {
                sbFrom.Append(type.Name);
            }
            StringBuilder sbJoin = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbOrder = new StringBuilder();
            string columnName;
            string seperator = string.Empty;
            PropertyInfo[] classProperties = type.GetProperties();

            foreach (PropertyInfo classProperty in classProperties)
            {
                Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                for (int i = 0; i < customAttr.Length; i++)
                {
                    Type attrType = customAttr[i].GetType();
                    switch (attrType.Name)
                    {
                        case "PrimaryKeyAttribute":
                            PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                            if (easyLoadGlobal.Equals(EasyLoad.Specified))
                            {
                                if (!priKeyAttr.PartialLoad)
                                {
                                    break;
                                }
                            }

                            if (priKeyAttr.Column != null)
                            {
                                columnName = priKeyAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }

                            if (sbSelect.Length != 0)
                            {
                                seperator = ", ";
                            }
                            sbSelect.Append(seperator);
                            sbSelect.Append(easyAttr.Table);
                            sbSelect.Append(".");
                            sbSelect.Append(columnName);
                            sbSelect.Append(" AS ");
                            sbSelect.Append(easyAttr.Table);
                            sbSelect.Append("_");
                            sbSelect.Append(columnName);
                            break;

                        case "BelongsToAttribute":
                            BelongsToAttribute belongAttr = (BelongsToAttribute)customAttr[i];
                            if (easyLoadGlobal.Equals(EasyLoad.Specified))
                            {
                                if (!belongAttr.PartialLoad)
                                {
                                    break;
                                }
                            }
                            else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                            {
                                break;
                            }

                            if (belongAttr.Column != null)
                            {
                                fKeyColumn = belongAttr.Column;
                            }
                            else
                            {
                                fKeyColumn = classProperty.Name;
                            }

                            Type foreignClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                            PropertyInfo foreignPropType = null;

                            EasyDataAttribute easyAttrBelongsTo = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignClassPropType, typeof(EasyDataAttribute));

                            if (belongAttr.PropertyRef != null)
                            {
                                foreignPropType = foreignClassPropType.GetProperty(belongAttr.PropertyRef);
                            }
                            PropertyInfo[] foreignClassProperties = foreignClassPropType.GetProperties();

                            foreach (PropertyInfo foreignClassProperty in foreignClassProperties)
                            {
                                Attribute[] foreignCustomAttr = Attribute.GetCustomAttributes(foreignClassProperty);
                                for (int j = 0; j < foreignCustomAttr.Length; j++)
                                {
                                    Type foreignAttrType = foreignCustomAttr[j].GetType();
                                    switch (foreignAttrType.Name)
                                    {
                                        case "PrimaryKeyAttribute":
                                            PrimaryKeyAttribute fPrimaryAttr = (PrimaryKeyAttribute)foreignCustomAttr[j];

                                            if (fPrimaryAttr.Column != null)
                                            {
                                                columnName = fPrimaryAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignClassProperty.Name;
                                            }

                                            if (foreignPropType == null)
                                            {
                                                pKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "PropertyAttribute":
                                            PropertyAttribute fPropAttr = (PropertyAttribute)foreignCustomAttr[j];
                                            if (fPropAttr.Column != null)
                                            {
                                                columnName = fPropAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignClassProperty.Name;
                                            }

                                            if (foreignPropType != null && foreignPropType == foreignClassProperty)
                                            {
                                                pKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "HasManyAttribute":
                                            HasManyAttribute fHasManyAttr = (HasManyAttribute)foreignCustomAttr[j];
                                            if (fHasManyAttr.ColumnKey != null)
                                            {
                                                columnName = fHasManyAttr.ColumnKey;
                                            }
                                            else
                                            {
                                                columnName = foreignClassProperty.Name;
                                            }

                                            if (foreignPropType != null && foreignPropType == foreignClassProperty)
                                            {
                                                pKeyColumn = columnName;
                                            }
                                            break;
                                        case "BelongsToAttribute":
                                            BelongsToAttribute fBelongsToAttr = (BelongsToAttribute)foreignCustomAttr[j];
                                            if (fBelongsToAttr.Column != null)
                                            {
                                                columnName = fBelongsToAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignClassProperty.Name;
                                            }

                                            if (foreignPropType != null && foreignPropType == foreignClassProperty)
                                            {
                                                pKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrBelongsTo.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "HasAndBelongsToManyAttribute":
                                            HasAndBelongsToManyAttribute fHasBelongsToAttr = (HasAndBelongsToManyAttribute)foreignCustomAttr[j];
                                            if (fHasBelongsToAttr.ColumnKey != null)
                                            {
                                                columnName = fHasBelongsToAttr.ColumnKey;
                                            }
                                            else
                                            {
                                                columnName = foreignClassProperty.Name;
                                            }

                                            if (foreignPropType != null && foreignPropType == foreignClassProperty)
                                            {
                                                pKeyColumn = columnName;
                                            }
                                            break;
                                        case "OneToOneAttribute":
                                            //OneToOneAttribute fOneToOneAttr = (OneToOneAttribute)foreignCustomAttr[j];
                                            //if (fOneToOneAttr.Column != null)
                                            //{
                                            //    columnName = fOneToOneAttr.Column;
                                            //}
                                            //else
                                            //{
                                            //    columnName = foreignClassProperty.Name;
                                            //}

                                            //if (foreignPropType != null && foreignPropType == foreignClassProperty)
                                            //{
                                            //pKeyColumn = columnName;
                                            //}
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }

                            sbJoin.Append(" INNER JOIN ");
                            if (easyAttrBelongsTo.Schema != null)
                            {
                                sbJoin.Append(easyAttrBelongsTo.Schema);
                                sbJoin.Append(".");
                            }
                            sbJoin.Append(easyAttrBelongsTo.Table);
                            sbJoin.Append(" ON ");
                            sbJoin.Append(easyAttr.Table);
                            sbJoin.Append(".");
                            sbJoin.Append(fKeyColumn);
                            sbJoin.Append(" = ");
                            sbJoin.Append(easyAttrBelongsTo.Table);
                            sbJoin.Append(".");
                            sbJoin.Append(pKeyColumn);
                            break;

                        case "OneToOneAttribute":
                            OneToOneAttribute OneToOneAttr = (OneToOneAttribute)customAttr[i];
                            if (easyLoadGlobal.Equals(EasyLoad.Specified))
                            {
                                if (!OneToOneAttr.PartialLoad)
                                {
                                    break;
                                }
                            }
                            else if (easyLoadGlobal.Equals(EasyLoad.Simple))
                            {
                                break;
                            }

                            this.GetPrimaryKeyColumn(type);
                            pKeyColumn = propertyColumnByType;
                            Type foreignOneToOneClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                            PropertyInfo foreignOneToOnePropType = null;

                            EasyDataAttribute easyAttrOneToOne = (EasyDataAttribute)Attribute.GetCustomAttribute(foreignOneToOneClassPropType, typeof(EasyDataAttribute));

                            if (OneToOneAttr.PropertyRef != null)
                            {
                                foreignOneToOnePropType = foreignOneToOneClassPropType.GetProperty(OneToOneAttr.PropertyRef);
                            }
                            PropertyInfo[] foreignOneToOneClassProperties = foreignOneToOneClassPropType.GetProperties();

                            foreach (PropertyInfo foreignOneToOneClassProperty in foreignOneToOneClassProperties)
                            {
                                Attribute[] foreignOneToOneCustomAttr = Attribute.GetCustomAttributes(foreignOneToOneClassProperty);
                                for (int j = 0; j < foreignOneToOneCustomAttr.Length; j++)
                                {
                                    Type foreignOneToOneAttrType = foreignOneToOneCustomAttr[j].GetType();
                                    switch (foreignOneToOneAttrType.Name)
                                    {
                                        case "PrimaryKeyAttribute":
                                            PrimaryKeyAttribute fPrimaryAttr = (PrimaryKeyAttribute)foreignOneToOneCustomAttr[j];

                                            if (fPrimaryAttr.Column != null)
                                            {
                                                columnName = fPrimaryAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignOneToOneClassProperty.Name;
                                            }

                                            if (foreignOneToOnePropType == null)
                                            {
                                                fKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "PropertyAttribute":
                                            PropertyAttribute fPropAttr = (PropertyAttribute)foreignOneToOneCustomAttr[j];
                                            if (fPropAttr.Column != null)
                                            {
                                                columnName = fPropAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignOneToOneClassProperty.Name;
                                            }

                                            if (foreignOneToOnePropType != null && foreignOneToOnePropType == foreignOneToOneClassProperty)
                                            {
                                                fKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "HasManyAttribute":
                                            HasManyAttribute fHasManyAttr = (HasManyAttribute)foreignOneToOneCustomAttr[j];
                                            if (fHasManyAttr.ColumnKey != null)
                                            {
                                                columnName = fHasManyAttr.ColumnKey;
                                            }
                                            else
                                            {
                                                columnName = foreignOneToOneClassProperty.Name;
                                            }

                                            if (foreignOneToOnePropType != null && foreignOneToOnePropType == foreignOneToOneClassProperty)
                                            {
                                                fKeyColumn = columnName;
                                            }
                                            break;
                                        case "BelongsToAttribute":
                                            BelongsToAttribute fBelongsToAttr = (BelongsToAttribute)foreignOneToOneCustomAttr[j];
                                            if (fBelongsToAttr.Column != null)
                                            {
                                                columnName = fBelongsToAttr.Column;
                                            }
                                            else
                                            {
                                                columnName = foreignOneToOneClassProperty.Name;
                                            }

                                            if (foreignOneToOnePropType != null && foreignOneToOnePropType == foreignOneToOneClassProperty)
                                            {
                                                fKeyColumn = columnName;
                                            }

                                            if (sbSelect.Length != 0)
                                            {
                                                seperator = ", ";
                                            }
                                            sbSelect.Append(seperator);
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append(".");
                                            sbSelect.Append(columnName);
                                            sbSelect.Append(" AS ");
                                            sbSelect.Append(easyAttrOneToOne.Table);
                                            sbSelect.Append("_");
                                            sbSelect.Append(columnName);
                                            break;
                                        case "HasAndBelongsToManyAttribute":
                                            HasAndBelongsToManyAttribute fHasBelongsToAttr = (HasAndBelongsToManyAttribute)foreignOneToOneCustomAttr[j];
                                            if (fHasBelongsToAttr.ColumnKey != null)
                                            {
                                                columnName = fHasBelongsToAttr.ColumnKey;
                                            }
                                            else
                                            {
                                                columnName = foreignOneToOneClassProperty.Name;
                                            }

                                            if (foreignOneToOnePropType != null && foreignOneToOnePropType == foreignOneToOneClassProperty)
                                            {
                                                fKeyColumn = columnName;
                                            }
                                            break;
                                        case "OneToOneAttribute":
                                            //OneToOneAttribute fOneToOneAttr = (OneToOneAttribute)foreignOneToOneCustomAttr[j];
                                            //if (fOneToOneAttr.Column != null)
                                            //{
                                            //    columnName = fOneToOneAttr.Column;
                                            //}
                                            //else
                                            //{
                                            //    columnName = foreignOneToOneClassProperty.Name;
                                            //}

                                            //if (foreignOneToOnePropType != null && foreignOneToOnePropType == foreignOneToOneClassProperty)
                                            //{
                                            //fKeyColumn = columnName;
                                            //}
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }

                            sbJoin.Append(" LEFT JOIN ");
                            if (easyAttrOneToOne.Schema != null)
                            {
                                sbJoin.Append(easyAttrOneToOne.Schema);
                                sbJoin.Append(".");
                            }
                            sbJoin.Append(easyAttrOneToOne.Table);
                            sbJoin.Append(" ON ");
                            sbJoin.Append(easyAttr.Table);
                            sbJoin.Append(".");
                            sbJoin.Append(pKeyColumn);
                            sbJoin.Append(" = ");
                            sbJoin.Append(easyAttrOneToOne.Table);
                            sbJoin.Append(".");
                            sbJoin.Append(fKeyColumn);
                            break;

                        case "PropertyAttribute":
                            PropertyAttribute propAttr = (PropertyAttribute)customAttr[i];
                            if (easyLoadGlobal.Equals(EasyLoad.Specified))
                            {
                                if (!propAttr.PartialLoad)
                                {
                                    break;
                                }
                            }

                            if (propAttr.Column != null)
                            {
                                columnName = propAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }
                            if (sbSelect.Length != 0)
                            {
                                seperator = ", ";
                            }
                            sbSelect.Append(seperator);
                            sbSelect.Append(easyAttr.Table);
                            sbSelect.Append(".");
                            sbSelect.Append(columnName);
                            sbSelect.Append(" AS ");
                            sbSelect.Append(easyAttr.Table);
                            sbSelect.Append("_");
                            sbSelect.Append(columnName);

                            break;
                        default:

                            break;
                    }
                }
            }

            if (sbSelect.Length == 0)
            {
                sbSelect.Append("*");
            }

            if (property == null && value != null)
            {
                GetPrimaryKeyColumn(type);

                command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + propertyColumnByType + "_", MySqlDBTypes.GetDbType(propertyTypeByType), value));

                sbWhere.Append(" WHERE ");
                sbWhere.Append(easyAttr.Table);
                sbWhere.Append(".");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append(" = ");
                sbWhere.Append(":");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append("_");
            }
            else if (property != null && value != null)
            {
                GetPropertyColumn(type, property);
                sbWhere.Append(" WHERE ");
                sbWhere.Append(easyAttr.Table);
                sbWhere.Append(".");
                sbWhere.Append(propertyColumnByType);
                if (propertyTypeByType == "System.DateTime")
                {
                    command.Parameters.Add(MySqlParameters.CreateInputParameter(":From_", MySqlDBTypes.GetDbType(propertyTypeByType), value));
                    command.Parameters.Add(MySqlParameters.CreateInputParameter(":To_", MySqlDBTypes.GetDbType(propertyTypeByType), value));

                    sbWhere.Append(" BETWEEN ");
                    sbWhere.Append(":From_");
                    sbWhere.Append(" AND ");
                    sbWhere.Append("DATEADD(Day, 1, ");
                    sbWhere.Append(":To_)");
                }
                else
                {
                    sbWhere.Append(" = ");
                    sbWhere.Append(":");
                    sbWhere.Append(propertyColumnByType);
                    sbWhere.Append("_");

                    command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + propertyColumnByType + "_", MySqlDBTypes.GetDbType(propertyTypeByType), value));
                }
            }
            else if (where != null)
            {
                sbWhere.Append(" WHERE ");
                sbWhere.Append(where);
            }

            sbQuery.Append(sbSelect.ToString());
            sbQuery.Append(sbFrom.ToString());
            sbQuery.Append(sbJoin.ToString());
            sbQuery.Append(sbWhere.ToString());
            sbQuery.Append(sbOrder.ToString());

            command.CommandText = sbQuery.ToString();
            return command;
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="property">The property.</param>
        protected void SetPropertyValue(T instance, string property)
        {
            string propertyColumn = string.Empty;
            Type type = instance.GetType();
            PropertyInfo[] classProperties = type.GetProperties();

            foreach (PropertyInfo classProperty in classProperties)
            {
                if (classProperty.Name == property)
                {
                    Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                    for (int i = 0; i < customAttr.Length; i++)
                    {
                        Type attrType = customAttr[i].GetType();
                        switch (attrType.Name)
                        {
                            case "PrimaryKeyAttribute":
                                PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                                if (priKeyAttr.Column != null)
                                {
                                    propertyColumn = priKeyAttr.Column;
                                }
                                else
                                {
                                    propertyColumn = classProperty.Name;
                                }
                                break;
                            case "BelongsToAttribute":
                                BelongsToAttribute belongAttr = (BelongsToAttribute)customAttr[i];
                                if (belongAttr.Column != null)
                                {
                                    propertyColumn = belongAttr.Column;
                                }
                                else
                                {
                                    propertyColumn = classProperty.Name;
                                }
                                break;
                            case "PropertyAttribute":
                                PropertyAttribute propAttr = (PropertyAttribute)customAttr[i];
                                if (propAttr.Column != null)
                                {
                                    propertyColumn = propAttr.Column;
                                }
                                else
                                {
                                    propertyColumn = classProperty.Name;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    propertyColumnByInstance = propertyColumn;
                    propertyTypeByInstance = classProperty.PropertyType.ToString();
                    propertyValueByInstance = classProperty.GetValue(instance, null);
                    break;
                }
            }
        }

        /// <summary>
        /// Gets the property column.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        protected void GetPropertyColumn(Type type, string property)
        {
            string propertyColumn = string.Empty;
            string propertyType = string.Empty;
            PropertyInfo[] classProperties = type.GetProperties();

            foreach (PropertyInfo classProperty in classProperties)
            {
                if (classProperty.Name == property)
                {
                    Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                    for (int i = 0; i < customAttr.Length; i++)
                    {
                        Type attrType = customAttr[i].GetType();
                        switch (attrType.Name)
                        {
                            case "PrimaryKeyAttribute":
                                PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                                if (priKeyAttr.Column != null)
                                {
                                    propertyColumn = priKeyAttr.Column;
                                }
                                else
                                {
                                    propertyColumn = classProperty.Name;
                                }
                                propertyType = classProperty.PropertyType.ToString();
                                break;
                            case "BelongsToAttribute":
                                BelongsToAttribute belongAttr = (BelongsToAttribute)customAttr[i];
                                if (belongAttr.Column != null)
                                {
                                    propertyColumn = belongAttr.Column;
                                }
                                else
                                {
                                    propertyColumn = classProperty.Name;
                                }

                                //get foreignkey data type
                                Type foreignClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                                
                                PropertyInfo foreignPropType = null;
                                if (belongAttr.PropertyRef != null)
                                {
                                    foreignPropType = foreignClassPropType.GetProperty(belongAttr.PropertyRef);
                                    propertyType = foreignPropType.PropertyType.ToString();
                                }
                                else
                                {
                                    GetPrimaryKeyColumn(foreignClassPropType);
                                    propertyType = propertyTypeByType;
                                }
                                break;
                            case "PropertyAttribute":
                                PropertyAttribute propAttr = (PropertyAttribute)customAttr[i];
                                if (propAttr.Column != null)
                                {
                                    propertyColumn = propAttr.Column;
                                }
                                else
                                {
                                    propertyColumn = classProperty.Name;
                                }
                                propertyType = classProperty.PropertyType.ToString();
                                break;
                            default:
                                break;
                        }
                    }

                    propertyTypeByType = propertyType;
                    propertyColumnByType = propertyColumn;
                    break;
                }
            }
        }

        /// <summary>
        /// Sets the primary key value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        protected void SetPrimaryKeyValue(T instance)
        {
            string propertyColumn = string.Empty;
            Type type = instance.GetType();
            PropertyInfo[] classProperties = type.GetProperties();

            foreach (PropertyInfo classProperty in classProperties)
            {
                Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                for (int i = 0; i < customAttr.Length; i++)
                {
                    Type attrType = customAttr[i].GetType();
                    switch (attrType.Name)
                    {
                        case "PrimaryKeyAttribute":
                            PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                            if (priKeyAttr.Column != null)
                            {
                                propertyColumn = priKeyAttr.Column;
                            }
                            else
                            {
                                propertyColumn = classProperty.Name;
                            }
                            break;
                        default:
                            break;
                    }
                }
                propertyColumnByInstance = propertyColumn;
                propertyTypeByInstance = classProperty.PropertyType.ToString();
                propertyValueByInstance = classProperty.GetValue(instance, null);
                break;
            }
        }

        /// <summary>
        /// Gets the primary key column.
        /// </summary>
        /// <param name="type">The type.</param>
        protected void GetPrimaryKeyColumn(Type type)
        {
            string propertyColumn = string.Empty;
            PropertyInfo[] classProperties = type.GetProperties();

            foreach (PropertyInfo classProperty in classProperties)
            {
                Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                for (int i = 0; i < customAttr.Length; i++)
                {
                    Type attrType = customAttr[i].GetType();
                    switch (attrType.Name)
                    {
                        case "PrimaryKeyAttribute":
                            PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                            if (priKeyAttr.Column != null)
                            {
                                propertyColumn = priKeyAttr.Column;
                            }
                            else
                            {
                                propertyColumn = classProperty.Name;
                            }
                            break;
                        default:
                            break;
                    }
                }

                propertyTypeByType = classProperty.PropertyType.ToString();
                propertyColumnByType = propertyColumn;
                break;
            }
        }
    
    }
}
