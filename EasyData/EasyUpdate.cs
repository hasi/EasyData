using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using EasyData.Attributes;
using System.Reflection;
using EasyData.DB.Sql;
using System.Collections;
using EasyData.Core;
using EasyData.Query;
using System.Data.OracleClient;
using EasyData.DB.Oracle;
using Devart.Data.MySql;
using EasyData.DB.MySql;

namespace EasyData
{
    /// <summary>
    /// Provide the update functionality for the EasyData
    /// </summary>
    /// <typeparam name="T">Type of the entity class</typeparam>
    public class EasyUpdate<T>
    {
        string projPath = Assembly.GetExecutingAssembly().CodeBase.Remove(Assembly.GetExecutingAssembly().CodeBase.IndexOf("bin"));
        string assemblyPath = ConfigurationManager.AppSettings[Constants.ASSEMBLY_PATH];
        string dbType = ConfigurationManager.AppSettings[Constants.DB_TYPE];

        static PrimaryKeyType keyType;
        static object key;
        static string keyColumn;
        static bool isContainManyToMany;
        static int collectionCount;
        static HasAndBelongsToManyAttribute hasAndBelongsToManyAttr;

        static object propertyValueByInstance;

        static string propertyTypeByType;
        static string propertyTypeByInstance;

        static string propertyColumnByInstance;
        static string propertyColumnByType;

        static EasyUpdate easyUpdateGlobal;

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>If success return <c>true</c>, else <c>false</c></returns>
        public bool Update(T instance, EasySession easySession, EasyUpdate easyUpdate)
        {
            easyUpdateGlobal = easyUpdate;

            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SQLUpdate(instance, null, null, null, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleUpdate(instance, null, null, null, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlUpdate(instance, null, null, null, easySession);
            }

            return false;
        }

        public bool Update(T instance, string where, EasySession easySession, EasyUpdate easyUpdate)
        {
            easyUpdateGlobal = easyUpdate;

            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SQLUpdate(instance, null, null, where, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleUpdate(instance, null, null, where, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlUpdate(instance, null, null, where, easySession);
            }

            return false;
        }

        public bool Update(T instance, string property, object value, EasySession easySession, EasyUpdate easyUpdate)
        {
            easyUpdateGlobal = easyUpdate;

            if (value == null)
            {
                SetPropertyValue(instance, property);
            }
            else
            {
                propertyValueByInstance = value;
            }
            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SQLUpdate(instance, property, propertyValueByInstance, null, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleUpdate(instance, property, propertyValueByInstance, null, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlUpdate(instance, property, propertyValueByInstance, null, easySession);
            }

            return false;
        }

        /// <summary>
        /// SQL update.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>If success return <c>true</c>, else <c>false</c></returns>
        protected bool SQLUpdate(T instance, string property, object value, string where, EasySession easySession)
        {
            bool isSuccess;
            string query = string.Empty;

            try
            {
                using (var command = this.SQLQueryBuilderForUpdate(instance, property, value, where, query, easySession))
                {
                    command.ExecuteNonQuery();
                    isSuccess = true;
                    easySession.SetCommit = true;

                    if (isContainManyToMany)
                    {
                        DeleteAllAssociationRecordsSql(instance, query, easySession);

                        for (int listCount = 0; listCount < collectionCount; listCount++)
                        {
                            using (var commandToUpdateIds = this.SqlQueryBuilderForUpdateAssociation(instance, query, easySession, listCount))
                            {
                                if (!commandToUpdateIds.CommandText.Equals(string.Empty))
                                {
                                    commandToUpdateIds.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                easySession.SetRollback = true;
                throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
            }

            return isSuccess;
        }

        /// <summary>
        /// Oracle update.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>If success return <c>true</c>, else <c>false</c></returns>
        protected bool OracleUpdate(T instance, string property, object value, string where, EasySession easySession)
        {
            bool isSuccess;
            string query = string.Empty;

            try
            {
                using (var command = this.OracleQueryBuilderForUpdate(instance, property, value, where, query, easySession))
                {
                    command.ExecuteNonQuery();
                    isSuccess = true;
                    easySession.SetCommit = true;

                    if (isContainManyToMany)
                    {
                        DeleteAllAssociationRecordsOracle(instance, query, easySession);

                        for (int listCount = 0; listCount < collectionCount; listCount++)
                        {
                            using (var commandToUpdateIds = this.OracleQueryBuilderForUpdateAssociation(instance, query, easySession, listCount))
                            {
                                if (!commandToUpdateIds.CommandText.Equals(string.Empty))
                                {
                                    commandToUpdateIds.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                easySession.SetRollback = true;
                throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
            }

            return isSuccess;
        }

        /// <summary>
        /// MySql update.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>If success return <c>true</c>, else <c>false</c></returns>
        protected bool MySqlUpdate(T instance, string property, object value, string where, EasySession easySession)
        {
            bool isSuccess;
            string query = string.Empty;

            try
            {
                using (var command = this.MySqlQueryBuilderForUpdate(instance, property, value, where, query, easySession))
                {
                    command.ExecuteNonQuery();
                    isSuccess = true;
                    easySession.SetCommit = true;

                    if (isContainManyToMany)
                    {
                        DeleteAllAssociationRecordsMySql(instance, query, easySession);

                        for (int listCount = 0; listCount < collectionCount; listCount++)
                        {
                            using (var commandToUpdateIds = this.MySqlQueryBuilderForUpdateAssociation(instance, query, easySession, listCount))
                            {
                                if (!commandToUpdateIds.CommandText.Equals(string.Empty))
                                {
                                    commandToUpdateIds.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                easySession.SetRollback = true;
                throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
            }

            return isSuccess;
        }

        /// <summary>
        /// Deletes all association records SQL.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        protected void DeleteAllAssociationRecordsSql(T instance, string query, EasySession easySession)
        {
            try
            {
                using (var command = new SqlCommand(query, easySession.Connection, easySession.Transaction))
                {
                    command.CommandType = CommandType.Text;

                    StringBuilder sbDelete = new StringBuilder();
                    sbDelete.Append("DELETE FROM ");
                    StringBuilder sbTable = new StringBuilder();
                    if (hasAndBelongsToManyAttr.Schema != null)
                    {
                        sbTable.Append(hasAndBelongsToManyAttr.Schema);
                        sbTable.Append(".");
                    }

                    sbTable.Append(hasAndBelongsToManyAttr.Table);

                    StringBuilder sbDeleteQuery = new StringBuilder();
                    sbDeleteQuery.Append(sbDelete.ToString());
                    sbDeleteQuery.Append(sbTable.ToString());
                    sbDeleteQuery.Append(" WHERE ");
                    sbDeleteQuery.Append(keyColumn);
                    sbDeleteQuery.Append(" = ");
                    sbDeleteQuery.Append(key);

                    command.CommandText = sbDeleteQuery.ToString();

                    command.ExecuteNonQuery();
                    easySession.SetCommit = true;
                }
            }
            catch (Exception ex)
            {
                easySession.SetRollback = true;
                throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
            }
        }

        /// <summary>
        /// Deletes all association records Oracle.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        protected void DeleteAllAssociationRecordsOracle(T instance, string query, EasySession easySession)
        {
            try
            {
                using (var command = new OracleCommand(query, easySession.OConnection, easySession.OTransaction))
                {
                    command.CommandType = CommandType.Text;

                    StringBuilder sbDelete = new StringBuilder();
                    sbDelete.Append("DELETE FROM ");
                    StringBuilder sbTable = new StringBuilder();
                    if (hasAndBelongsToManyAttr.Schema != null)
                    {
                        sbTable.Append(hasAndBelongsToManyAttr.Schema);
                        sbTable.Append(".");
                    }

                    sbTable.Append(hasAndBelongsToManyAttr.Table);

                    StringBuilder sbDeleteQuery = new StringBuilder();
                    sbDeleteQuery.Append(sbDelete.ToString());
                    sbDeleteQuery.Append(sbTable.ToString());
                    sbDeleteQuery.Append(" WHERE ");
                    sbDeleteQuery.Append(keyColumn);
                    sbDeleteQuery.Append(" = ");
                    sbDeleteQuery.Append(key);

                    command.CommandText = sbDeleteQuery.ToString();

                    command.ExecuteNonQuery();
                    easySession.SetCommit = true;
                }
            }
            catch (Exception ex)
            {
                easySession.SetRollback = true;
                throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
            }
        }

        /// <summary>
        /// Deletes all association records MySql.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        protected void DeleteAllAssociationRecordsMySql(T instance, string query, EasySession easySession)
        {
            try
            {
                using (var command = new MySqlCommand(query, easySession.MConnection, easySession.MTransaction))
                {
                    command.CommandType = CommandType.Text;

                    StringBuilder sbDelete = new StringBuilder();
                    sbDelete.Append("DELETE FROM ");
                    StringBuilder sbTable = new StringBuilder();
                    if (hasAndBelongsToManyAttr.Schema != null)
                    {
                        sbTable.Append(hasAndBelongsToManyAttr.Schema);
                        sbTable.Append(".");
                    }

                    sbTable.Append(hasAndBelongsToManyAttr.Table);

                    StringBuilder sbDeleteQuery = new StringBuilder();
                    sbDeleteQuery.Append(sbDelete.ToString());
                    sbDeleteQuery.Append(sbTable.ToString());
                    sbDeleteQuery.Append(" WHERE ");
                    sbDeleteQuery.Append(keyColumn);
                    sbDeleteQuery.Append(" = ");
                    sbDeleteQuery.Append(key);

                    command.CommandText = sbDeleteQuery.ToString();

                    command.ExecuteNonQuery();
                    easySession.SetCommit = true;
                }
            }
            catch (Exception ex)
            {
                easySession.SetRollback = true;
                throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
            }
        }

        /// <summary>
        /// Sql query builder for update association.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <param name="listCount">The related record count.</param>
        /// <returns>Returns the SqlCommand with the querystring and parameters</returns>
        protected SqlCommand SqlQueryBuilderForUpdateAssociation(T instance, string query, EasySession easySession, int listCount)
        {
            SqlCommand command = new SqlCommand(query, easySession.Connection, easySession.Transaction);
            command.CommandType = CommandType.Text;

            StringBuilder sbSelect = new StringBuilder();
            sbSelect.Append("SELECT COUNT(*) FROM ");

            StringBuilder sbInsert = new StringBuilder();
            sbInsert.Append("INSERT INTO ");
            StringBuilder sbTable = new StringBuilder();
            if (hasAndBelongsToManyAttr.Schema != null)
            {
                sbTable.Append(hasAndBelongsToManyAttr.Schema);
                sbTable.Append(".");
            }

            sbTable.Append(hasAndBelongsToManyAttr.Table);

            StringBuilder sbInsertQuery = new StringBuilder();
            sbInsertQuery.Append(sbInsert.ToString());
            sbInsertQuery.Append(sbTable.ToString());
            sbInsertQuery.Append(" (");

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" WHERE ");

            StringBuilder sbSelectQuery = new StringBuilder();
            sbSelectQuery.Append(sbSelect.ToString());
            sbSelectQuery.Append(sbTable.ToString());
            sbSelectQuery.Append(sbWhere.ToString());

            StringBuilder sbWhereParams = new StringBuilder();
            string whereSeperator = string.Empty;

            Type instanceType = instance.GetType();
            PropertyInfo[] classProperties = instanceType.GetProperties();

            StringBuilder sbColumns = new StringBuilder();
            StringBuilder sbParams = new StringBuilder();
            string seperator = string.Empty;
            string columnName = string.Empty;
            bool isValid = false;
            foreach (PropertyInfo classProperty in classProperties)
            {
                Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                for (int i = 0; i < customAttr.Length; i++)
                {
                    if (i > 0)
                        if (columnName != string.Empty)
                            break;

                    Type attrType = customAttr[i].GetType();
                    switch (attrType.Name)
                    {
                        case "PrimaryKeyAttribute":
                            columnName = hasAndBelongsToManyAttr.ColumnKey;
                            command.Parameters.Add(SqlParameters.CreateInputParameter("@" + columnName + "_", SqlDBTypes.GetDbType(classProperty.PropertyType.ToString()), classProperty.GetValue(instance, null)));

                            if (sbWhereParams.Length != 0)
                            {
                                whereSeperator = " AND ";
                            }
                            sbWhereParams.Append(whereSeperator);
                            sbWhereParams.Append(columnName);
                            sbWhereParams.Append(" = ");
                            sbWhereParams.Append(classProperty.GetValue(instance, null).ToString());

                            break;

                        case "HasAndBelongsToManyAttribute":
                            columnName = hasAndBelongsToManyAttr.ColumnRef;
                            Type foreignClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, hasAndBelongsToManyAttr.MapType.FullName);
                            PropertyInfo foreignPropType = null;

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
                                            foreignPropType = foreignClassPropType.GetProperty(foreignClassProperty.Name);
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if (foreignPropType != null)
                                {
                                    break;
                                }
                            }

                            object foreignInstance = classProperty.GetValue(instance, null);
                            IList listInstance = (IList)foreignInstance;
                            if (listInstance.Count > 0)
                            {
                                isValid = true;
                                command.Parameters.Add(SqlParameters.CreateInputParameter("@" + columnName + "_", SqlDBTypes.GetDbType(foreignPropType.PropertyType.ToString()), foreignPropType.GetValue(listInstance[listCount], null)));

                                if (sbWhereParams.Length != 0)
                                {
                                    whereSeperator = " AND ";
                                }
                                sbWhereParams.Append(whereSeperator);
                                sbWhereParams.Append(columnName);
                                sbWhereParams.Append(" = ");
                                sbWhereParams.Append(foreignPropType.GetValue(listInstance[listCount], null).ToString());
                            }
                            else
                            {
                                isValid = false;
                            }

                            break;

                        default:
                            columnName = string.Empty;
                            break;
                    }
                }


                if (!columnName.Equals(string.Empty))
                {
                    if (sbColumns.Length != 0)
                    {
                        seperator = ", ";
                    }

                    ///Adding column names
                    sbColumns.Append(seperator);
                    sbColumns.Append("[");
                    sbColumns.Append(columnName);
                    sbColumns.Append("]");

                    ///Adding parameter names
                    sbParams.Append(seperator);
                    sbParams.Append("@");
                    sbParams.Append(columnName);
                    sbParams.Append("_");
                }

            }

            ///Finalizing insert query
            sbInsertQuery.Append(sbColumns.ToString());
            sbInsertQuery.Append(") VALUES(");
            sbInsertQuery.Append(sbParams.ToString());
            sbInsertQuery.Append(")");

            ///Finalizing select query
            sbSelectQuery.Append(sbWhereParams.ToString());

            if (isValid)
            {
                if (CheckDoubleEntrySql(sbSelectQuery.ToString(), easySession))
                {
                    command.CommandText = sbInsertQuery.ToString();
                }
            }
            else
            {
                command.CommandText = string.Empty;
            }

            return command;
        }

        /// <summary>
        /// Oracle query builder for update association.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <param name="listCount">The related record count.</param>
        /// <returns>Returns the OracleCommand with the querystring and parameters</returns>
        protected OracleCommand OracleQueryBuilderForUpdateAssociation(T instance, string query, EasySession easySession, int listCount)
        {
            OracleCommand command = new OracleCommand(query, easySession.OConnection, easySession.OTransaction);
            command.CommandType = CommandType.Text;

            StringBuilder sbSelect = new StringBuilder();
            sbSelect.Append("SELECT COUNT(*) FROM ");

            StringBuilder sbInsert = new StringBuilder();
            sbInsert.Append("INSERT INTO ");
            StringBuilder sbTable = new StringBuilder();
            if (hasAndBelongsToManyAttr.Schema != null)
            {
                sbTable.Append(hasAndBelongsToManyAttr.Schema);
                sbTable.Append(".");
            }

            sbTable.Append(hasAndBelongsToManyAttr.Table);

            StringBuilder sbInsertQuery = new StringBuilder();
            sbInsertQuery.Append(sbInsert.ToString());
            sbInsertQuery.Append(sbTable.ToString());
            sbInsertQuery.Append(" (");

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" WHERE ");

            StringBuilder sbSelectQuery = new StringBuilder();
            sbSelectQuery.Append(sbSelect.ToString());
            sbSelectQuery.Append(sbTable.ToString());
            sbSelectQuery.Append(sbWhere.ToString());

            StringBuilder sbWhereParams = new StringBuilder();
            string whereSeperator = string.Empty;

            Type instanceType = instance.GetType();
            PropertyInfo[] classProperties = instanceType.GetProperties();

            StringBuilder sbColumns = new StringBuilder();
            StringBuilder sbParams = new StringBuilder();
            string seperator = string.Empty;
            string columnName = string.Empty;
            bool isValid = false;
            foreach (PropertyInfo classProperty in classProperties)
            {
                Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                for (int i = 0; i < customAttr.Length; i++)
                {
                    if (i > 0)
                        if (columnName != string.Empty)
                            break;

                    Type attrType = customAttr[i].GetType();
                    switch (attrType.Name)
                    {
                        case "PrimaryKeyAttribute":
                            columnName = hasAndBelongsToManyAttr.ColumnKey;
                            command.Parameters.Add(OracleParameters.CreateInputParameter(":" + columnName + "_", OracleDBTypes.GetDbType(classProperty.PropertyType.ToString()), classProperty.GetValue(instance, null)));

                            if (sbWhereParams.Length != 0)
                            {
                                whereSeperator = " AND ";
                            }
                            sbWhereParams.Append(whereSeperator);
                            sbWhereParams.Append(columnName);
                            sbWhereParams.Append(" = ");
                            sbWhereParams.Append(classProperty.GetValue(instance, null).ToString());

                            break;

                        case "HasAndBelongsToManyAttribute":
                            columnName = hasAndBelongsToManyAttr.ColumnRef;
                            Type foreignClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, hasAndBelongsToManyAttr.MapType.FullName);
                            PropertyInfo foreignPropType = null;

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
                                            foreignPropType = foreignClassPropType.GetProperty(foreignClassProperty.Name);
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if (foreignPropType != null)
                                {
                                    break;
                                }
                            }

                            object foreignInstance = classProperty.GetValue(instance, null);
                            IList listInstance = (IList)foreignInstance;
                            if (listInstance.Count > 0)
                            {
                                isValid = true;
                                command.Parameters.Add(OracleParameters.CreateInputParameter(":" + columnName + "_", OracleDBTypes.GetDbType(foreignPropType.PropertyType.ToString()), foreignPropType.GetValue(listInstance[listCount], null)));

                                if (sbWhereParams.Length != 0)
                                {
                                    whereSeperator = " AND ";
                                }
                                sbWhereParams.Append(whereSeperator);
                                sbWhereParams.Append(columnName);
                                sbWhereParams.Append(" = ");
                                sbWhereParams.Append(foreignPropType.GetValue(listInstance[listCount], null).ToString());
                            }
                            else
                            {
                                isValid = false;
                            }

                            break;

                        default:
                            columnName = string.Empty;
                            break;
                    }
                }


                if (!columnName.Equals(string.Empty))
                {
                    if (sbColumns.Length != 0)
                    {
                        seperator = ", ";
                    }

                    ///Adding column names
                    sbColumns.Append(seperator);
                    //sbColumns.Append("[");
                    sbColumns.Append(columnName);
                    //sbColumns.Append("]");

                    ///Adding parameter names
                    sbParams.Append(seperator);
                    sbParams.Append(":");
                    sbParams.Append(columnName);
                    sbParams.Append("_");
                }

            }

            ///Finalizing insert query
            sbInsertQuery.Append(sbColumns.ToString());
            sbInsertQuery.Append(") VALUES(");
            sbInsertQuery.Append(sbParams.ToString());
            sbInsertQuery.Append(")");

            ///Finalizing select query
            sbSelectQuery.Append(sbWhereParams.ToString());

            if (isValid)
            {
                if (CheckDoubleEntryOracle(sbSelectQuery.ToString(), easySession))
                {
                    command.CommandText = sbInsertQuery.ToString();
                }
            }
            else
            {
                command.CommandText = string.Empty;
            }

            return command;
        }

        /// <summary>
        /// MySql query builder for update association.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <param name="listCount">The related record count.</param>
        /// <returns>Returns the MySqlCommand with the querystring and parameters</returns>
        protected MySqlCommand MySqlQueryBuilderForUpdateAssociation(T instance, string query, EasySession easySession, int listCount)
        {
            MySqlCommand command = new MySqlCommand(query, easySession.MConnection, easySession.MTransaction);
            command.CommandType = CommandType.Text;

            StringBuilder sbSelect = new StringBuilder();
            sbSelect.Append("SELECT COUNT(*) FROM ");

            StringBuilder sbInsert = new StringBuilder();
            sbInsert.Append("INSERT INTO ");
            StringBuilder sbTable = new StringBuilder();
            if (hasAndBelongsToManyAttr.Schema != null)
            {
                sbTable.Append(hasAndBelongsToManyAttr.Schema);
                sbTable.Append(".");
            }

            sbTable.Append(hasAndBelongsToManyAttr.Table);

            StringBuilder sbInsertQuery = new StringBuilder();
            sbInsertQuery.Append(sbInsert.ToString());
            sbInsertQuery.Append(sbTable.ToString());
            sbInsertQuery.Append(" (");

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" WHERE ");

            StringBuilder sbSelectQuery = new StringBuilder();
            sbSelectQuery.Append(sbSelect.ToString());
            sbSelectQuery.Append(sbTable.ToString());
            sbSelectQuery.Append(sbWhere.ToString());

            StringBuilder sbWhereParams = new StringBuilder();
            string whereSeperator = string.Empty;

            Type instanceType = instance.GetType();
            PropertyInfo[] classProperties = instanceType.GetProperties();

            StringBuilder sbColumns = new StringBuilder();
            StringBuilder sbParams = new StringBuilder();
            string seperator = string.Empty;
            string columnName = string.Empty;
            bool isValid = false;
            foreach (PropertyInfo classProperty in classProperties)
            {
                Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                for (int i = 0; i < customAttr.Length; i++)
                {
                    if (i > 0)
                        if (columnName != string.Empty)
                            break;

                    Type attrType = customAttr[i].GetType();
                    switch (attrType.Name)
                    {
                        case "PrimaryKeyAttribute":
                            columnName = hasAndBelongsToManyAttr.ColumnKey;
                            command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + columnName + "_", MySqlDBTypes.GetDbType(classProperty.PropertyType.ToString()), classProperty.GetValue(instance, null)));

                            if (sbWhereParams.Length != 0)
                            {
                                whereSeperator = " AND ";
                            }
                            sbWhereParams.Append(whereSeperator);
                            sbWhereParams.Append(columnName);
                            sbWhereParams.Append(" = ");
                            sbWhereParams.Append(classProperty.GetValue(instance, null).ToString());

                            break;

                        case "HasAndBelongsToManyAttribute":
                            columnName = hasAndBelongsToManyAttr.ColumnRef;
                            Type foreignClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, hasAndBelongsToManyAttr.MapType.FullName);
                            PropertyInfo foreignPropType = null;

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
                                            foreignPropType = foreignClassPropType.GetProperty(foreignClassProperty.Name);
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if (foreignPropType != null)
                                {
                                    break;
                                }
                            }

                            object foreignInstance = classProperty.GetValue(instance, null);
                            IList listInstance = (IList)foreignInstance;
                            if (listInstance.Count > 0)
                            {
                                isValid = true;
                                command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + columnName + "_", MySqlDBTypes.GetDbType(foreignPropType.PropertyType.ToString()), foreignPropType.GetValue(listInstance[listCount], null)));

                                if (sbWhereParams.Length != 0)
                                {
                                    whereSeperator = " AND ";
                                }
                                sbWhereParams.Append(whereSeperator);
                                sbWhereParams.Append(columnName);
                                sbWhereParams.Append(" = ");
                                sbWhereParams.Append(foreignPropType.GetValue(listInstance[listCount], null).ToString());
                            }
                            else
                            {
                                isValid = false;
                            }

                            break;

                        default:
                            columnName = string.Empty;
                            break;
                    }
                }


                if (!columnName.Equals(string.Empty))
                {
                    if (sbColumns.Length != 0)
                    {
                        seperator = ", ";
                    }

                    ///Adding column names
                    sbColumns.Append(seperator);
                    sbColumns.Append("`");
                    sbColumns.Append(columnName);
                    sbColumns.Append("`");

                    ///Adding parameter names
                    sbParams.Append(seperator);
                    sbParams.Append(":");
                    sbParams.Append(columnName);
                    sbParams.Append("_");
                }

            }

            ///Finalizing insert query
            sbInsertQuery.Append(sbColumns.ToString());
            sbInsertQuery.Append(") VALUES(");
            sbInsertQuery.Append(sbParams.ToString());
            sbInsertQuery.Append(")");

            ///Finalizing select query
            sbSelectQuery.Append(sbWhereParams.ToString());

            if (isValid)
            {
                if (CheckDoubleEntryMySql(sbSelectQuery.ToString(), easySession))
                {
                    command.CommandText = sbInsertQuery.ToString();
                }
            }
            else
            {
                command.CommandText = string.Empty;
            }

            return command;
        }

        /// <summary>
        /// Checks the double entry. (SQL)
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>If exists return <c>false</c>, else <c>true</c></returns>
        protected bool CheckDoubleEntrySql(string query, EasySession easySession)
        {
            object count;
            bool isValid;
            using (var command = new SqlCommand(query, easySession.Connection, easySession.Transaction))
            {
                command.CommandType = CommandType.Text;
                count = command.ExecuteScalar();
            }

            int countConverted = Convert.ToInt32(count);
            if (countConverted > 0)
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }

        /// <summary>
        /// Checks the double entry. (Oracle)
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>If exists return <c>false</c>, else <c>true</c></returns>
        protected bool CheckDoubleEntryOracle(string query, EasySession easySession)
        {
            object count;
            bool isValid;
            using (var command = new OracleCommand(query, easySession.OConnection, easySession.OTransaction))
            {
                command.CommandType = CommandType.Text;
                count = command.ExecuteScalar();
            }

            int countConverted = Convert.ToInt32(count);
            if (countConverted > 0)
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }

        /// <summary>
        /// Checks the double entry. (MySql)
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>If exists return <c>false</c>, else <c>true</c></returns>
        protected bool CheckDoubleEntryMySql(string query, EasySession easySession)
        {
            object count;
            bool isValid;
            using (var command = new MySqlCommand(query, easySession.MConnection, easySession.MTransaction))
            {
                command.CommandType = CommandType.Text;
                count = command.ExecuteScalar();
            }

            int countConverted = Convert.ToInt32(count);
            if (countConverted > 0)
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }

        /// <summary>
        /// SQL query builder for update.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>
        /// Returns the SqlCommand with the querystring and parameters
        /// </returns>
        protected SqlCommand SQLQueryBuilderForUpdate(T instance, string property, object value, string where, string query, EasySession easySession)
        {
            string table;
            SqlCommand command = new SqlCommand(query, easySession.Connection, easySession.Transaction);
            command.CommandType = CommandType.Text;

            Type instanceType = instance.GetType();
            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(EasyDataAttribute));

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("UPDATE ");
            if (easyAttr.Schema != null)
            {
                sbQuery.Append(easyAttr.Schema);
                sbQuery.Append(".");
            }
            if (easyAttr.Table != null)
            {
                table = easyAttr.Table;
                sbQuery.Append(easyAttr.Table);
            }
            else
            {
                table = instanceType.Name;
                sbQuery.Append(instanceType.Name);
            }
            sbQuery.Append(" SET ");

            StringBuilder sbColumnsParams = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" WHERE ");
            string seperator = string.Empty;
            string columnName = string.Empty;

            PropertyInfo[] classProperties = instanceType.GetProperties();

            foreach (PropertyInfo classProperty in classProperties)
            {
                Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                for (int i = 0; i < customAttr.Length; i++)
                {
                    if (i > 0)
                        if (columnName != string.Empty)
                            break;

                    Type attrType = customAttr[i].GetType();
                    switch (attrType.Name)
                    {
                        case "PrimaryKeyAttribute":

                            PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                            keyType = priKeyAttr.Generator;

                            if (priKeyAttr.Column != null)
                            {
                                columnName = priKeyAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }

                            keyColumn = columnName;
                            key = classProperty.GetValue(instance, null);
                            if (property == null && where == null)
                            {
                                command.Parameters.Add(SqlParameters.CreateInputParameter("@" + keyColumn + "_", SqlDBTypes.GetDbType(classProperty.PropertyType.ToString()), key));

                                sbWhere.Append(keyColumn);
                                sbWhere.Append(" = ");
                                sbWhere.Append("@");
                                sbWhere.Append(keyColumn);
                                sbWhere.Append("_");
                            }
                            columnName = string.Empty;
                            break;

                        case "HasAndBelongsToManyAttribute":
                            if (easyUpdateGlobal.Equals(EasyUpdate.True))
                            {
                                break;
                            }

                            hasAndBelongsToManyAttr = (HasAndBelongsToManyAttribute)customAttr[i];
                            Type foreignClassPropTypeMToM = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, hasAndBelongsToManyAttr.MapType.FullName);
                            PropertyInfo foreignPropTypeMToM = null;

                            PropertyInfo[] foreignClassPropertiesMToM = foreignClassPropTypeMToM.GetProperties();

                            foreach (PropertyInfo foreignClassPropertyMToM in foreignClassPropertiesMToM)
                            {
                                Attribute[] foreignCustomAttrMToM = Attribute.GetCustomAttributes(foreignClassPropertyMToM);
                                for (int j = 0; j < foreignCustomAttrMToM.Length; j++)
                                {
                                    Type foreignAttrTypeMToM = foreignCustomAttrMToM[j].GetType();
                                    switch (foreignAttrTypeMToM.Name)
                                    {
                                        case "PrimaryKeyAttribute":
                                            foreignPropTypeMToM = foreignClassPropTypeMToM.GetProperty(foreignClassPropertyMToM.Name);
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if (foreignPropTypeMToM != null)
                                {
                                    break;
                                }
                            }

                            object foreignInstanceMToM = classProperty.GetValue(instance, null);
                            IList listInstance = (IList)foreignInstanceMToM;
                            //TO DO: Need to refine for the count
                            //if (listInstance.Count > 0)
                            //{
                            isContainManyToMany = true;
                            collectionCount = listInstance.Count;
                            //}

                            columnName = string.Empty;
                            break;

                        case "PropertyAttribute":

                            PropertyAttribute propAttr = (PropertyAttribute)customAttr[i];

                            if (propAttr.Column != null)
                            {
                                columnName = propAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }

                            string colType = string.Empty;
                            if (propAttr.ColumnType != null)
                            {
                                colType = propAttr.ColumnType;
                            }
                            else
                            {
                                colType = classProperty.PropertyType.ToString();
                            }

                            if (Convert.ToString(classProperty.GetValue(instance, null)) != "" && Convert.ToString(classProperty.GetValue(instance, null)) != null)
                            {
                                if (classProperty.PropertyType.Name.Equals("DateTime") && CheckSQLDate.CheckDefaultDate(classProperty.GetValue(instance, null)))
                                {
                                    command.Parameters.Add(SqlParameters.CreateInputParameter("@" + columnName + "_", SqlDBTypes.GetDbType(colType), System.Data.SqlTypes.SqlDateTime.MinValue));
                                }
                                else if (classProperty.PropertyType.Name.Equals("Guid") && CheckGuid.CheckDefaultGuid(classProperty.GetValue(instance, null)))
                                {
                                    columnName = string.Empty;
                                }
                                else
                                {
                                    command.Parameters.Add(SqlParameters.CreateInputParameter("@" + columnName + "_", SqlDBTypes.GetDbType(colType), classProperty.GetValue(instance, null)));
                                }
                            }
                            else
                            {
                                columnName = string.Empty;
                            }
                            break;

                        case "BelongsToAttribute":

                            BelongsToAttribute belongAttr = (BelongsToAttribute)customAttr[i];

                            if (belongAttr.Column != null)
                            {
                                columnName = belongAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }

                            Type foreignClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                            PropertyInfo foreignPropType = null;

                            if (belongAttr.PropertyRef != null)
                            {
                                foreignPropType = foreignClassPropType.GetProperty(belongAttr.PropertyRef);
                            }
                            else
                            {
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
                                                foreignPropType = foreignClassPropType.GetProperty(foreignClassProperty.Name);
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    if (foreignPropType != null)
                                    {
                                        break;
                                    }
                                }
                            }

                            object foreignInstance = classProperty.GetValue(instance, null);

                            if (Convert.ToString(foreignPropType.GetValue(foreignInstance, null)) != "" && Convert.ToString(foreignPropType.GetValue(foreignInstance, null)) != null)
                            {
                                command.Parameters.Add(SqlParameters.CreateInputParameter("@" + columnName + "_", SqlDBTypes.GetDbType(foreignPropType.PropertyType.ToString()), foreignPropType.GetValue(foreignInstance, null)));
                            }
                            else
                            {
                                columnName = string.Empty;
                            }

                            break;

                        default:

                            columnName = string.Empty;

                            break;
                    }
                }

                if (!columnName.Equals(string.Empty))
                {
                    if (sbColumnsParams.Length != 0)
                    {
                        seperator = ", ";
                    }

                    ///Adding column names
                    sbColumnsParams.Append(seperator);
                    sbColumnsParams.Append("[");
                    sbColumnsParams.Append(columnName);
                    sbColumnsParams.Append("]");

                    ///Adding parameter names
                    sbColumnsParams.Append(" = ");
                    sbColumnsParams.Append("@");
                    sbColumnsParams.Append(columnName);
                    sbColumnsParams.Append("_");
                }
            }

            ///Finalizing the query
            sbQuery.Append(sbColumnsParams.ToString());

            if (where != null)
            {
                sbWhere.Append(where);
            }
            else if (property != null)
            {
                GetPropertyColumn(instanceType, property);

                command.Parameters.Add(SqlParameters.CreateInputParameter("@" + propertyColumnByType + "Where_", SqlDBTypes.GetDbType(propertyTypeByType), value));

                sbWhere.Append(table);
                sbWhere.Append(".");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append(" = ");
                sbWhere.Append("@");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append("Where_");
            }

            sbQuery.Append(sbWhere.ToString());

            command.CommandText = sbQuery.ToString();
            return command;
        }

        /// <summary>
        /// Oracle query builder for update.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>Returns the OracleCommand with the querystring and parameters</returns>
        protected OracleCommand OracleQueryBuilderForUpdate(T instance, string property, object value, string where, string query, EasySession easySession)
        {
            string table;
            OracleCommand command = new OracleCommand(query, easySession.OConnection, easySession.OTransaction);
            command.CommandType = CommandType.Text;

            Type instanceType = instance.GetType();
            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(EasyDataAttribute));

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("UPDATE ");
            if (easyAttr.Schema != null)
            {
                sbQuery.Append(easyAttr.Schema);
                sbQuery.Append(".");
            }
            if (easyAttr.Table != null)
            {
                table = easyAttr.Table;
                sbQuery.Append(easyAttr.Table);
            }
            else
            {
                table = instanceType.Name;
                sbQuery.Append(instanceType.Name);
            }
            sbQuery.Append(" SET ");

            StringBuilder sbColumnsParams = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" WHERE ");
            string seperator = string.Empty;
            string columnName = string.Empty;

            PropertyInfo[] classProperties = instanceType.GetProperties();

            foreach (PropertyInfo classProperty in classProperties)
            {
                Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                for (int i = 0; i < customAttr.Length; i++)
                {
                    if (i > 0)
                        if (columnName != string.Empty)
                            break;

                    Type attrType = customAttr[i].GetType();
                    switch (attrType.Name)
                    {
                        case "PrimaryKeyAttribute":

                            PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                            keyType = priKeyAttr.Generator;

                            if (priKeyAttr.Column != null)
                            {
                                columnName = priKeyAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }

                            keyColumn = columnName;
                            key = classProperty.GetValue(instance, null);
                            if (property == null && where == null)
                            {
                                command.Parameters.Add(OracleParameters.CreateInputParameter(":" + keyColumn + "_", OracleDBTypes.GetDbType(classProperty.PropertyType.ToString()), key));

                                sbWhere.Append(keyColumn);
                                sbWhere.Append(" = ");
                                sbWhere.Append(":");
                                sbWhere.Append(keyColumn);
                                sbWhere.Append("_");
                            }

                            columnName = string.Empty;
                            break;

                        case "HasAndBelongsToManyAttribute":
                            if (easyUpdateGlobal.Equals(EasyUpdate.True))
                            {
                                break;
                            }

                            hasAndBelongsToManyAttr = (HasAndBelongsToManyAttribute)customAttr[i];
                            Type foreignClassPropTypeMToM = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, hasAndBelongsToManyAttr.MapType.FullName);
                            PropertyInfo foreignPropTypeMToM = null;

                            PropertyInfo[] foreignClassPropertiesMToM = foreignClassPropTypeMToM.GetProperties();

                            foreach (PropertyInfo foreignClassPropertyMToM in foreignClassPropertiesMToM)
                            {
                                Attribute[] foreignCustomAttrMToM = Attribute.GetCustomAttributes(foreignClassPropertyMToM);
                                for (int j = 0; j < foreignCustomAttrMToM.Length; j++)
                                {
                                    Type foreignAttrTypeMToM = foreignCustomAttrMToM[j].GetType();
                                    switch (foreignAttrTypeMToM.Name)
                                    {
                                        case "PrimaryKeyAttribute":
                                            foreignPropTypeMToM = foreignClassPropTypeMToM.GetProperty(foreignClassPropertyMToM.Name);
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if (foreignPropTypeMToM != null)
                                {
                                    break;
                                }
                            }

                            object foreignInstanceMToM = classProperty.GetValue(instance, null);
                            IList listInstance = (IList)foreignInstanceMToM;
                            //if (listInstance.Count > 0)
                            //{
                                isContainManyToMany = true;
                                collectionCount = listInstance.Count;
                            //}

                            columnName = string.Empty;
                            break;

                        case "PropertyAttribute":

                            PropertyAttribute propAttr = (PropertyAttribute)customAttr[i];

                            if (propAttr.Column != null)
                            {
                                columnName = propAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }

                            string colType = string.Empty;
                            if (propAttr.ColumnType != null)
                            {
                                colType = propAttr.ColumnType;
                            }
                            else
                            {
                                colType = classProperty.PropertyType.ToString();
                            }

                            if (Convert.ToString(classProperty.GetValue(instance, null)) != "" && Convert.ToString(classProperty.GetValue(instance, null)) != null)
                            {
                                if (classProperty.PropertyType.Name.Equals("Guid") && CheckGuid.CheckDefaultGuid(classProperty.GetValue(instance, null)))
                                {
                                    columnName = string.Empty;
                                }
                                else
                                {
                                    command.Parameters.Add(OracleParameters.CreateInputParameter(":" + columnName + "_", OracleDBTypes.GetDbType(colType), classProperty.GetValue(instance, null)));
                                }
                            }
                            else
                            {
                                columnName = string.Empty;
                            }
                            break;

                        case "BelongsToAttribute":

                            BelongsToAttribute belongAttr = (BelongsToAttribute)customAttr[i];

                            if (belongAttr.Column != null)
                            {
                                columnName = belongAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }

                            Type foreignClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                            PropertyInfo foreignPropType = null;

                            if (belongAttr.PropertyRef != null)
                            {
                                foreignPropType = foreignClassPropType.GetProperty(belongAttr.PropertyRef);
                            }
                            else
                            {
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
                                                foreignPropType = foreignClassPropType.GetProperty(foreignClassProperty.Name);
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    if (foreignPropType != null)
                                    {
                                        break;
                                    }
                                }
                            }

                            object foreignInstance = classProperty.GetValue(instance, null);

                            if (Convert.ToString(foreignPropType.GetValue(foreignInstance, null)) != "" && Convert.ToString(foreignPropType.GetValue(foreignInstance, null)) != null)
                            {
                                command.Parameters.Add(OracleParameters.CreateInputParameter(":" + columnName + "_", OracleDBTypes.GetDbType(foreignPropType.PropertyType.ToString()), foreignPropType.GetValue(foreignInstance, null)));
                            }
                            else
                            {
                                columnName = string.Empty;
                            }

                            break;

                        default:

                            columnName = string.Empty;

                            break;
                    }
                }

                if (!columnName.Equals(string.Empty))
                {
                    if (sbColumnsParams.Length != 0)
                    {
                        seperator = ", ";
                    }

                    ///Adding column names
                    sbColumnsParams.Append(seperator);
                    //sbColumnsParams.Append("`");
                    sbColumnsParams.Append(columnName);
                    //sbColumnsParams.Append("`");

                    ///Adding parameter names
                    sbColumnsParams.Append(" = ");
                    sbColumnsParams.Append(":");
                    sbColumnsParams.Append(columnName);
                    sbColumnsParams.Append("_");
                }
            }

            ///Finalizing the query
            sbQuery.Append(sbColumnsParams.ToString());

            if (where != null)
            {
                sbWhere.Append(where);
            }
            else if (property != null)
            {
                GetPropertyColumn(instanceType, property);

                command.Parameters.Add(OracleParameters.CreateInputParameter(":" + propertyColumnByType + "Where_", OracleDBTypes.GetDbType(propertyTypeByType), value));

                sbWhere.Append(table);
                sbWhere.Append(".");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append(" = ");
                sbWhere.Append(":");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append("Where_");
            }

            sbQuery.Append(sbWhere.ToString());

            command.CommandText = sbQuery.ToString();
            return command;
        }

        /// <summary>
        ///  MySql query builder for update.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>Returns the  MySqlCommand with the querystring and parameters</returns>
        protected MySqlCommand MySqlQueryBuilderForUpdate(T instance, string property, object value, string where, string query, EasySession easySession)
        {
            string table;
            MySqlCommand command = new MySqlCommand(query, easySession.MConnection, easySession.MTransaction);
            command.CommandType = CommandType.Text;

            Type instanceType = instance.GetType();
            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(EasyDataAttribute));

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("UPDATE ");
            if (easyAttr.Schema != null)
            {
                sbQuery.Append(easyAttr.Schema);
                sbQuery.Append(".");
            }
            if (easyAttr.Table != null)
            {
                table = easyAttr.Table;
                sbQuery.Append(easyAttr.Table);
            }
            else
            {
                table = instanceType.Name;
                sbQuery.Append(instanceType.Name);
            }
            sbQuery.Append(" SET ");

            StringBuilder sbColumnsParams = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" WHERE ");
            string seperator = string.Empty;
            string columnName = string.Empty;

            PropertyInfo[] classProperties = instanceType.GetProperties();

            foreach (PropertyInfo classProperty in classProperties)
            {
                Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                for (int i = 0; i < customAttr.Length; i++)
                {
                    if (i > 0)
                        if (columnName != string.Empty)
                            break;

                    Type attrType = customAttr[i].GetType();
                    switch (attrType.Name)
                    {
                        case "PrimaryKeyAttribute":

                            PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                            keyType = priKeyAttr.Generator;

                            if (priKeyAttr.Column != null)
                            {
                                columnName = priKeyAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }

                            keyColumn = columnName;
                            key = classProperty.GetValue(instance, null);
                            if (property == null && where == null)
                            {
                                command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + keyColumn + "_", MySqlDBTypes.GetDbType(classProperty.PropertyType.ToString()), key));

                                sbWhere.Append(keyColumn);
                                sbWhere.Append(" = ");
                                sbWhere.Append(":");
                                sbWhere.Append(keyColumn);
                                sbWhere.Append("_");
                            }

                            columnName = string.Empty;
                            break;

                        case "HasAndBelongsToManyAttribute":
                            if (easyUpdateGlobal.Equals(EasyUpdate.True))
                            {
                                break;
                            }

                            hasAndBelongsToManyAttr = (HasAndBelongsToManyAttribute)customAttr[i];
                            Type foreignClassPropTypeMToM = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, hasAndBelongsToManyAttr.MapType.FullName);
                            PropertyInfo foreignPropTypeMToM = null;

                            PropertyInfo[] foreignClassPropertiesMToM = foreignClassPropTypeMToM.GetProperties();

                            foreach (PropertyInfo foreignClassPropertyMToM in foreignClassPropertiesMToM)
                            {
                                Attribute[] foreignCustomAttrMToM = Attribute.GetCustomAttributes(foreignClassPropertyMToM);
                                for (int j = 0; j < foreignCustomAttrMToM.Length; j++)
                                {
                                    Type foreignAttrTypeMToM = foreignCustomAttrMToM[j].GetType();
                                    switch (foreignAttrTypeMToM.Name)
                                    {
                                        case "PrimaryKeyAttribute":
                                            foreignPropTypeMToM = foreignClassPropTypeMToM.GetProperty(foreignClassPropertyMToM.Name);
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if (foreignPropTypeMToM != null)
                                {
                                    break;
                                }
                            }

                            object foreignInstanceMToM = classProperty.GetValue(instance, null);
                            IList listInstance = (IList)foreignInstanceMToM;
                            //if (listInstance.Count > 0)
                            //{
                                isContainManyToMany = true;
                                collectionCount = listInstance.Count;
                            //}

                            columnName = string.Empty;
                            break;

                        case "PropertyAttribute":

                            PropertyAttribute propAttr = (PropertyAttribute)customAttr[i];

                            if (propAttr.Column != null)
                            {
                                columnName = propAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }

                            string colType = string.Empty;
                            if (propAttr.ColumnType != null)
                            {
                                colType = propAttr.ColumnType;
                            }
                            else
                            {
                                colType = classProperty.PropertyType.ToString();
                            }

                            if (Convert.ToString(classProperty.GetValue(instance, null)) != "" && Convert.ToString(classProperty.GetValue(instance, null)) != null)
                            {
                                if (classProperty.PropertyType.Name.Equals("Guid") && CheckGuid.CheckDefaultGuid(classProperty.GetValue(instance, null)))
                                {
                                    columnName = string.Empty;
                                }
                                else
                                {
                                    command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + columnName + "_", MySqlDBTypes.GetDbType(colType), classProperty.GetValue(instance, null)));
                                }
                            }
                            else
                            {
                                columnName = string.Empty;
                            }
                            break;

                        case "BelongsToAttribute":

                            BelongsToAttribute belongAttr = (BelongsToAttribute)customAttr[i];

                            if (belongAttr.Column != null)
                            {
                                columnName = belongAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }

                            Type foreignClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classProperty.PropertyType.FullName);
                            PropertyInfo foreignPropType = null;

                            if (belongAttr.PropertyRef != null)
                            {
                                foreignPropType = foreignClassPropType.GetProperty(belongAttr.PropertyRef);
                            }
                            else
                            {
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
                                                foreignPropType = foreignClassPropType.GetProperty(foreignClassProperty.Name);
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    if (foreignPropType != null)
                                    {
                                        break;
                                    }
                                }
                            }

                            object foreignInstance = classProperty.GetValue(instance, null);

                            if (Convert.ToString(foreignPropType.GetValue(foreignInstance, null)) != "" && Convert.ToString(foreignPropType.GetValue(foreignInstance, null)) != null)
                            {
                                command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + columnName + "_", MySqlDBTypes.GetDbType(foreignPropType.PropertyType.ToString()), foreignPropType.GetValue(foreignInstance, null)));
                            }
                            else
                            {
                                columnName = string.Empty;
                            }

                            break;

                        default:

                            columnName = string.Empty;

                            break;
                    }
                }

                if (!columnName.Equals(string.Empty))
                {
                    if (sbColumnsParams.Length != 0)
                    {
                        seperator = ", ";
                    }

                    ///Adding column names
                    sbColumnsParams.Append(seperator);
                    sbColumnsParams.Append("`");
                    sbColumnsParams.Append(columnName);
                    sbColumnsParams.Append("`");

                    ///Adding parameter names
                    sbColumnsParams.Append(" = ");
                    sbColumnsParams.Append(":");
                    sbColumnsParams.Append(columnName);
                    sbColumnsParams.Append("_");
                }
            }

            ///Finalizing the query
            sbQuery.Append(sbColumnsParams.ToString());

            if (where != null)
            {
                sbWhere.Append(where);
            }
            else if (property != null)
            {
                GetPropertyColumn(instanceType, property);

                command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + propertyColumnByType + "Where_", MySqlDBTypes.GetDbType(propertyTypeByType), value));

                sbWhere.Append(table);
                sbWhere.Append(".");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append(" = ");
                sbWhere.Append(":");
                sbWhere.Append(propertyColumnByType);
                sbWhere.Append("Where_");
            }

            sbQuery.Append(sbWhere.ToString());

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

                    propertyTypeByType = classProperty.PropertyType.ToString();
                    propertyColumnByType = propertyColumn;
                    break;
                }
            }
        }
    }
}
