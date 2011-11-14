using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Data.SqlClient;
using EasyData.Attributes;
using System.Reflection;
using System.Data;
using EasyData.DB.Sql;
using EasyData.Core;
using System.Data.OracleClient;
using EasyData.DB.Oracle;
using Devart.Data.MySql;
using EasyData.DB.MySql;
using System.Collections;

namespace EasyData
{
    /// <summary>
    /// Provide the create functionality for the EasyData
    /// </summary>
    /// <typeparam name="T">Type of the entity class</typeparam>
    public class EasySave<T> 
    {
        string projPath = Assembly.GetExecutingAssembly().CodeBase.Remove(Assembly.GetExecutingAssembly().CodeBase.IndexOf("bin"));
        string assemblyPath = ConfigurationManager.AppSettings[Constants.ASSEMBLY_PATH];
        string dbType = ConfigurationManager.AppSettings[Constants.DB_TYPE];

        static PrimaryKeyType keyType;
        static object key;
        static bool isContainManyToMany;
        static int collectionCount;
        static HasAndBelongsToManyAttribute hasAndBelongsToManyAttr;

        /// <summary>
        /// Saves the specified instance.
        /// </summary>
        /// <param name="instance">Type T instance to extract the added values</param>
        /// <param name="easySession">Session variable which contains connection and transaction data</param>
        /// <returns>Return T type instance updated with the primery key</returns>
        public T Save(T instance, EasySession easySession)
        {
            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                instance = this.SQLSave(instance, easySession);
            }
            else if (dbType == "ORACLE")
            {
                instance = this.OracleSave(instance, easySession);
            }
            else if (dbType == "MYSQL")
            {
                instance = this.MySqlSave(instance, easySession);
            }

            return instance;
        }

        /// <summary>
        /// Save function when the DBType is SQL or not defined
        /// </summary>
        /// <param name="instance">Type T instance to extract the added values</param>
        /// <param name="easySession">Session variable which contains connection and transaction data</param>
        /// <returns>Return T type instance updated with the primery key</returns>
        protected T SQLSave(T instance, EasySession easySession)
        {
            string query = string.Empty;

            try
            {
                using (var command = this.InsertQueryBuilder(instance, query, easySession.Connection, easySession.Transaction))
                {
                    command.ExecuteNonQuery();
                    object primeryKey = null;
                    if (keyType.Equals(PrimaryKeyType.Identity) || keyType.Equals(PrimaryKeyType.Native))
                    {
                        using (var commandToGetId = this.GetCurrentIdQueryBuilder(instance, query, easySession.Connection, easySession.Transaction))
                        {
                            primeryKey = commandToGetId.ExecuteScalar();
                        }
                        instance = this.GetInstanceWithId(instance, primeryKey);
                    }
                    else if (keyType.Equals(PrimaryKeyType.Guid) || keyType.Equals(PrimaryKeyType.Foreign))
                    {
                        primeryKey = key;
                        instance = this.GetInstanceWithId(instance, primeryKey);
                    }

                    if (isContainManyToMany)
                    {
                        for (int listCount = 0; listCount < collectionCount; listCount++)
                        {
                            using (var commandToInsertIds = this.InsertAssociationQueryBuilder(instance, hasAndBelongsToManyAttr, query, easySession.Connection, easySession.Transaction, listCount))
                            {
                                if (!commandToInsertIds.CommandText.Equals(string.Empty))
                                {
                                    commandToInsertIds.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    easySession.SetCommit = true;
                }
            }
            catch (Exception ex)
            {
                easySession.SetRollback = true;
                throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
            }

            return instance;
        }

        /// <summary>
        /// Save function when the DBType is ORACLE
        /// </summary>
        /// <param name="instance">Type T instance to extract the added values</param>
        /// <param name="easySession">Session variable which contains connection and transaction data</param>
        /// <returns>Return T type instance updated with the primery key</returns>
        protected T OracleSave(T instance, EasySession easySession)
        {
            string query = string.Empty;

            try
            {
                using (var command = this.OracleInsertQueryBuilder(instance, query, easySession.OConnection, easySession.OTransaction))
                {
                    command.ExecuteNonQuery();
                    object primeryKey = null;
                    if (keyType.Equals(PrimaryKeyType.Sequence) || keyType.Equals(PrimaryKeyType.Native))
                    {
                        primeryKey = command.Parameters["curr_id"].Value;
                        instance = this.GetInstanceWithId(instance, primeryKey);
                    }
                    else if (keyType.Equals(PrimaryKeyType.Guid) || keyType.Equals(PrimaryKeyType.Foreign))
                    {
                        primeryKey = key;
                        instance = this.GetInstanceWithId(instance, primeryKey);
                    }

                    if (isContainManyToMany)
                    {
                        for (int listCount = 0; listCount < collectionCount; listCount++)
                        {
                            using (var commandToInsertIds = this.OracleInsertAssociationQueryBuilder(instance, hasAndBelongsToManyAttr, query, easySession.OConnection, easySession.OTransaction, listCount))
                            {
                                if (!commandToInsertIds.CommandText.Equals(string.Empty))
                                {
                                    commandToInsertIds.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    easySession.SetCommit = true;
                }
            }
            catch (Exception ex)
            {
                easySession.SetRollback = true;
                throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
            }

            return instance;
        }

        /// <summary>
        /// Save function when the DBType is MYSQL
        /// </summary>
        /// <param name="instance">Type T instance to extract the added values</param>
        /// <param name="easySession">Session variable which contains connection and transaction data</param>
        /// <returns>Return T type instance updated with the primery key</returns>
        protected T MySqlSave(T instance, EasySession easySession)
        {
            string query = string.Empty;

            try
            {
                using (var command = this.MySqlInsertQueryBuilder(instance, query, easySession.MConnection, easySession.MTransaction))
                {
                    command.ExecuteNonQuery();

                    object primeryKey = null;
                    if (keyType.Equals(PrimaryKeyType.Identity) || keyType.Equals(PrimaryKeyType.Native))
                    {
                        primeryKey = command.InsertId;
                        instance = this.GetInstanceWithId(instance, primeryKey);
                    }
                    else if (keyType.Equals(PrimaryKeyType.Guid) || keyType.Equals(PrimaryKeyType.Foreign))
                    {
                        primeryKey = key;
                        instance = this.GetInstanceWithId(instance, primeryKey);
                    }

                    if (isContainManyToMany)
                    {
                        for (int listCount = 0; listCount < collectionCount; listCount++)
                        {
                            using (var commandToInsertIds = this.MySqlInsertAssociationQueryBuilder(instance, hasAndBelongsToManyAttr, query, easySession.MConnection, easySession.MTransaction, listCount))
                            {
                                if (!commandToInsertIds.CommandText.Equals(string.Empty))
                                {
                                    commandToInsertIds.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    easySession.SetCommit = true;
                }
            }
            catch (Exception ex)
            {
                easySession.SetRollback = true;
                throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
            }

            return instance;
        }

        /// <summary>
        /// Update the instance with the primery key value
        /// </summary>
        /// <param name="instance">Type T instance to add the key value</param>
        /// <param name="id">Key value</param>
        /// <returns>Return T type instance with Id</returns>
        protected T GetInstanceWithId(T instance, object id)
        {
            bool isSetVal = false;
            Type instanceType = instance.GetType();
            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(EasyDataAttribute));

            PropertyInfo[] classProperties = instanceType.GetProperties();

            foreach (PropertyInfo classProperty in classProperties)
            {
                Attribute[] customAttr = Attribute.GetCustomAttributes(classProperty);

                for (int i = 0; i < customAttr.Length; i++)
                {
                    Type attrType = customAttr[i].GetType();
                    switch (attrType.Name)
                    {
                        case "PrimaryKeyAttribute":
                            Type propType = classProperty.PropertyType;
                            var idToAdd = Convert.ChangeType(id, propType);
                            classProperty.SetValue(instance, idToAdd, null);
                            isSetVal = true;
                            break;
                        default:
                            break;
                    }
                }

                if (isSetVal)
                {
                    break;
                }
            }

            return instance;
        }

        /// <summary>
        /// To build the insert query and command to insert keys to the association table in ManyToMany situation
        /// </summary>
        /// <param name="instance">Type T instance to capture the values to insert</param>
        /// <param name="hasAndBelongsToManyAttr">ManyToMany attribute details</param>
        /// <param name="query">String variables which contains the query</param>
        /// <param name="connection">SqlConnection variable to connect the DB</param>
        /// <param name="transaction">SqlTransation variable to handle the transaction</param>
        /// <param name="listCount">Interger value which contains the count of the collection</param>
        /// <returns>Return the SqlCommand to execute</returns>
        protected SqlCommand InsertAssociationQueryBuilder(T instance, HasAndBelongsToManyAttribute hasAndBelongsToManyAttr, string query, SqlConnection connection, SqlTransaction transaction, int listCount)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);
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

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append(sbInsert.ToString());
            sbQuery.Append(sbTable.ToString());
            sbQuery.Append(" (");

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

            ///Finalizing the query
            sbQuery.Append(sbColumns.ToString());
            sbQuery.Append(") VALUES(");
            sbQuery.Append(sbParams.ToString());
            sbQuery.Append(")");

            sbSelectQuery.Append(sbWhereParams.ToString());

            if (isValid)
            {
                if (CheckDoubleEntry(sbSelectQuery.ToString(), connection, transaction))
                {
                    command.CommandText = sbQuery.ToString();
                }
            }
            else
            {
                command.CommandText = string.Empty;
            }

            return command;
        }

        /// <summary>
        /// To build the insert query and command to insert keys to the association table in ManyToMany situation
        /// </summary>
        /// <param name="instance">Type T instance to capture the values to insert</param>
        /// <param name="hasAndBelongsToManyAttr">ManyToMany attribute details</param>
        /// <param name="query">String variables which contains the query</param>
        /// <param name="connection">SqlConnection variable to connect the DB</param>
        /// <param name="transaction">SqlTransation variable to handle the transaction</param>
        /// <param name="listCount">Interger value which contains the count of the collection</param>
        /// <returns>Return the SqlCommand to execute</returns>
        protected OracleCommand OracleInsertAssociationQueryBuilder(T instance, HasAndBelongsToManyAttribute hasAndBelongsToManyAttr, string query, OracleConnection connection, OracleTransaction transaction, int listCount)
        {
            OracleCommand command = new OracleCommand(query, connection, transaction);
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

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append(sbInsert.ToString());
            sbQuery.Append(sbTable.ToString());
            sbQuery.Append(" (");

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
                    sbColumns.Append(columnName);

                    ///Adding parameter names
                    sbParams.Append(seperator);
                    sbParams.Append(":");
                    sbParams.Append(columnName);
                    sbParams.Append("_");
                }

            }

            ///Finalizing the query
            sbQuery.Append(sbColumns.ToString());
            sbQuery.Append(") VALUES(");
            sbQuery.Append(sbParams.ToString());
            sbQuery.Append(")");

            sbSelectQuery.Append(sbWhereParams.ToString());

            if (isValid)
            {
                if (OracleCheckDoubleEntry(sbSelectQuery.ToString(), connection, transaction))
                {
                    command.CommandText = sbQuery.ToString();
                }
            }
            else
            {
                command.CommandText = string.Empty;
            }

            return command;
        }

        /// <summary>
        /// To build the insert query and command to insert keys to the association table in ManyToMany situation
        /// </summary>
        /// <param name="instance">Type T instance to capture the values to insert</param>
        /// <param name="hasAndBelongsToManyAttr">ManyToMany attribute details</param>
        /// <param name="query">String variables which contains the query</param>
        /// <param name="connection">SqlConnection variable to connect the DB</param>
        /// <param name="transaction">SqlTransation variable to handle the transaction</param>
        /// <param name="listCount">Interger value which contains the count of the collection</param>
        /// <returns>Return the SqlCommand to execute</returns>
        protected MySqlCommand MySqlInsertAssociationQueryBuilder(T instance, HasAndBelongsToManyAttribute hasAndBelongsToManyAttr, string query, MySqlConnection connection, MySqlTransaction transaction, int listCount)
        {
            MySqlCommand command = new MySqlCommand(query, connection, transaction);
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

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append(sbInsert.ToString());
            sbQuery.Append(sbTable.ToString());
            sbQuery.Append(" (");

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

            ///Finalizing the query
            sbQuery.Append(sbColumns.ToString());
            sbQuery.Append(") VALUES(");
            sbQuery.Append(sbParams.ToString());
            sbQuery.Append(")");

            sbSelectQuery.Append(sbWhereParams.ToString());

            if (isValid)
            {
                if (MySqlCheckDoubleEntry(sbSelectQuery.ToString(), connection, transaction))
                {
                    command.CommandText = sbQuery.ToString();
                }
            }
            else
            {
                command.CommandText = string.Empty;
            }

            return command;
        }

        /// <summary>
        /// To check existing entries of the key table before add new
        /// </summary>
        /// <param name="query">Select query to check existing records</param>
        /// <param name="connection">SqlConnection variable to connect the DB</param>
        /// <param name="transaction">SqlTransation variable to handle the transaction</param>
        /// <returns>Return a bool value by giving permission for the insert</returns>
        protected bool CheckDoubleEntry(string query, SqlConnection connection, SqlTransaction transaction)
        {
            bool isValid;
            object count;
            using (var command = new SqlCommand(query, connection, transaction))
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
        /// To check existing entries of the key table before add new
        /// </summary>
        /// <param name="query">Select query to check existing records</param>
        /// <param name="connection">SqlConnection variable to connect the DB</param>
        /// <param name="transaction">SqlTransation variable to handle the transaction</param>
        /// <returns>Return a bool value by giving permission for the insert</returns>
        protected bool OracleCheckDoubleEntry(string query, OracleConnection connection, OracleTransaction transaction)
        {
            bool isValid;
            object count;
            using (var command = new OracleCommand(query, connection, transaction))
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
        /// To check existing entries of the key table before add new
        /// </summary>
        /// <param name="query">Select query to check existing records</param>
        /// <param name="connection">SqlConnection variable to connect the DB</param>
        /// <param name="transaction">SqlTransation variable to handle the transaction</param>
        /// <returns>Return a bool value by giving permission for the insert</returns>
        protected bool MySqlCheckDoubleEntry(string query, MySqlConnection connection, MySqlTransaction transaction)
        {
            bool isValid;
            object count;
            using (var command = new MySqlCommand(query, connection, transaction))
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
        /// Build the query to capture the newly created primery key value and arrange parameters
        /// </summary>
        /// <param name="instance">Type T instance to capture the table name</param>
        /// <param name="query">String variables which contains the query</param>
        /// <param name="connection">SqlConnection variable to connect the DB</param>
        /// <param name="transaction">SqlTransation variable to handle the transaction</param>
        /// <returns>Return the SqlCommand to execute</returns>
        protected SqlCommand GetCurrentIdQueryBuilder(T instance, string query, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);
            command.CommandType = CommandType.Text;

            Type instanceType = instance.GetType();
            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(EasyDataAttribute));

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT IDENT_CURRENT('");
            if (easyAttr.Schema != null)
            {
                sbQuery.Append(easyAttr.Schema);
                sbQuery.Append(".");
            }
            else
            {
                sbQuery.Append("dbo.");
            }
            if (easyAttr.Table != null)
            {
                sbQuery.Append(easyAttr.Table);
            }
            else
            {
                sbQuery.Append(instanceType.Name);
            }
            sbQuery.Append("') AS curr_id");

            command.CommandText = sbQuery.ToString();
            return command;
        }
         
        /// <summary>
        /// Build the query to insert data and arrange parameters
        /// </summary>
        /// <param name="instance">Type T instance to capture the values to insert</param>
        /// <param name="query">String variables which contains the query</param>
        /// <param name="connection">SqlConnection variable to connect the DB</param>
        /// <param name="transaction">SqlTransation variable to handle the transaction</param>
        /// <returns>Return the SqlCommand to execute</returns>
        protected SqlCommand InsertQueryBuilder(T instance, string query, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);
            command.CommandType = CommandType.Text;

            Type instanceType = instance.GetType();
            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(EasyDataAttribute));

            StringBuilder sbTable = new StringBuilder();
            sbTable.Append("INSERT INTO ");
            if (easyAttr.Schema != null)
            {
                sbTable.Append(easyAttr.Schema);
                sbTable.Append(".");
            }
            if (easyAttr.Table != null)
            {
                sbTable.Append(easyAttr.Table);
            }
            else
            {
                sbTable.Append(instanceType.Name);
            }
            sbTable.Append(" (");

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append(sbTable.ToString());

            StringBuilder sbColumns = new StringBuilder();
            StringBuilder sbParams = new StringBuilder();
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

                            if (priKeyAttr.Generator.Equals(PrimaryKeyType.Identity) || priKeyAttr.Generator.Equals(PrimaryKeyType.Native))
                            {
                                columnName = string.Empty;
                            }

                            if (!columnName.Equals(string.Empty))
                            {
                                if (priKeyAttr.Generator.Equals(PrimaryKeyType.Guid))
                                {
                                    key = Guid.NewGuid();
                                    command.Parameters.Add(SqlParameters.CreateInputParameter("@" + columnName + "_", SqlDBTypes.GetDbType(classProperty.PropertyType.ToString()), key));
                                }
                                else if (priKeyAttr.Generator.Equals(PrimaryKeyType.Foreign))
                                {
                                    PropertyInfo[] classPropertiesInner = instanceType.GetProperties();

                                    foreach (PropertyInfo classPropertyInner in classPropertiesInner)
                                    {
                                        Attribute[] customAttrInner = Attribute.GetCustomAttributes(classPropertyInner);

                                        for (int x = 0; x < customAttrInner.Length; x++)
                                        {
                                            Type attrTypeInner = customAttrInner[x].GetType();

                                            switch (attrTypeInner.Name)
                                            {
                                                case "OneToOneAttribute":

                                                    OneToOneAttribute onToOneAttr = (OneToOneAttribute)customAttrInner[x];

                                                    Type onToOneClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classPropertyInner.PropertyType.FullName);
                                                    PropertyInfo oneToOnePropType = null;

                                                    if (onToOneAttr.PropertyRef != null)
                                                    {
                                                        oneToOnePropType = onToOneClassPropType.GetProperty(onToOneAttr.PropertyRef);
                                                    }
                                                    else
                                                    {
                                                        PropertyInfo[] onToOneClassProperties = onToOneClassPropType.GetProperties();

                                                        foreach (PropertyInfo onToOneClassProperty in onToOneClassProperties)
                                                        {
                                                            Attribute[] oneToOneCustomAttr = Attribute.GetCustomAttributes(onToOneClassProperty);
                                                            for (int y = 0; y < oneToOneCustomAttr.Length; y++)
                                                            {
                                                                Type oneToOneAttrType = oneToOneCustomAttr[y].GetType();
                                                                switch (oneToOneAttrType.Name)
                                                                {
                                                                    case "PrimaryKeyAttribute":
                                                                        oneToOnePropType = onToOneClassPropType.GetProperty(onToOneClassProperty.Name);
                                                                        break;
                                                                    default:
                                                                        break;
                                                                }
                                                            }

                                                            if (oneToOnePropType != null)
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    object onToOneInstance = classPropertyInner.GetValue(instance, null);
                                                    
                                                    if (Convert.ToString(oneToOnePropType.GetValue(onToOneInstance, null)) != "" && Convert.ToString(oneToOnePropType.GetValue(onToOneInstance, null)) != null)
                                                    {
                                                        key = oneToOnePropType.GetValue(onToOneInstance, null);
                                                        command.Parameters.Add(SqlParameters.CreateInputParameter("@" + columnName + "_", SqlDBTypes.GetDbType(oneToOnePropType.PropertyType.ToString()), key));
                                                    }
                                                    else
                                                    {
                                                        command.Parameters.Add(new SqlParameter("@" + columnName + "_", DBNull.Value));
                                                    }

                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    key = classProperty.GetValue(instance, null);
                                    command.Parameters.Add(SqlParameters.CreateInputParameter("@" + columnName + "_", SqlDBTypes.GetDbType(classProperty.PropertyType.ToString()), classProperty.GetValue(instance, null)));
                                }
                            }

                            break;

                        case "HasAndBelongsToManyAttribute":

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
                            if (listInstance.Count > 0)
                            {
                                isContainManyToMany = true;
                                collectionCount = listInstance.Count;
                            }
                            
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
                                else
                                {
                                    command.Parameters.Add(SqlParameters.CreateInputParameter("@" + columnName + "_", SqlDBTypes.GetDbType(colType), classProperty.GetValue(instance, null)));
                                }
                            }
                            else
                            {
                                if (propAttr.NotNull)
                                {
                                    command.Parameters.Add(new SqlParameter("@" + columnName + "_", DBNull.Value));
                                }
                                else
                                {
                                    columnName = string.Empty;
                                    ///say it is not allowed to pass null
                                }
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
                                if (belongAttr.NotNull)
                                {
                                    command.Parameters.Add(new SqlParameter("@" + columnName + "_", DBNull.Value));
                                }
                                else
                                {
                                    columnName = string.Empty;
                                    ///say it is not allowed to pass null
                                }
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

            ///Finalizing the query
            sbQuery.Append(sbColumns.ToString());
            sbQuery.Append(") VALUES(");
            sbQuery.Append(sbParams.ToString());
            sbQuery.Append(")");

            command.CommandText = sbQuery.ToString();
            return command;
        }

        /// <summary>
        /// Build the query to insert data and arrange parameters
        /// </summary>
        /// <param name="instance">Type T instance to capture the table name</param>
        /// <param name="query">String variables which contains the query</param>
        /// <param name="connection">SqlConnection variable to connect the DB</param>
        /// <param name="transaction">SqlTransation variable to handle the transaction</param>
        /// <returns>Return the SqlCommand to execute</returns>
        protected OracleCommand OracleInsertQueryBuilder(T instance, string query, OracleConnection connection, OracleTransaction transaction)
        {
            OracleCommand command = new OracleCommand(query, connection, transaction);
            command.CommandType = CommandType.Text;

            Type instanceType = instance.GetType();
            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(EasyDataAttribute));

            StringBuilder sbTable = new StringBuilder();
            sbTable.Append("INSERT INTO ");
            if (easyAttr.Schema != null)
            {
                sbTable.Append(easyAttr.Schema);
                sbTable.Append(".");
            }
            if (easyAttr.Table != null)
            {
                sbTable.Append(easyAttr.Table);
            }
            else
            {
                sbTable.Append(instanceType.Name);
            }
            sbTable.Append(" (");

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append(sbTable.ToString());

            StringBuilder sbColumns = new StringBuilder();
            StringBuilder sbParams = new StringBuilder();
            StringBuilder sbReturn = new StringBuilder();
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

                            if (priKeyAttr.Generator.Equals(PrimaryKeyType.Sequence) || priKeyAttr.Generator.Equals(PrimaryKeyType.Native))
                            {
                                if (sbColumns.Length != 0)
                                {
                                    seperator = ", ";
                                }

                                sbColumns.Append(seperator);
                                sbColumns.Append(columnName);

                                sbParams.Append(seperator);
                                if (priKeyAttr.SequenceName != null)
                                {
                                    sbParams.Append(priKeyAttr.SequenceName);
                                    sbParams.Append(".NEXTVAL");
                                }
                                else
                                {
                                    if (easyAttr.Table != null)
                                    {
                                        sbParams.Append(easyAttr.Table);
                                    }
                                    else
                                    {
                                        sbParams.Append(instanceType.Name);
                                    }
                                    sbParams.Append("_SEQ.NEXTVAL");
                                }

                                sbReturn.Append(" RETURNING ");
                                sbReturn.Append(columnName);
                                sbReturn.Append(" INTO :curr_id");

                                columnName = string.Empty;
                            }

                            if (!columnName.Equals(string.Empty))
                            {
                                if (priKeyAttr.Generator.Equals(PrimaryKeyType.Guid))
                                {
                                    key = Guid.NewGuid();
                                    command.Parameters.Add(OracleParameters.CreateInputParameter(":" + columnName + "_", OracleDBTypes.GetDbType(classProperty.PropertyType.ToString()), key));
                                }
                                else if (priKeyAttr.Generator.Equals(PrimaryKeyType.Foreign))
                                {
                                    PropertyInfo[] classPropertiesInner = instanceType.GetProperties();

                                    foreach (PropertyInfo classPropertyInner in classPropertiesInner)
                                    {
                                        Attribute[] customAttrInner = Attribute.GetCustomAttributes(classPropertyInner);

                                        for (int x = 0; x < customAttrInner.Length; x++)
                                        {
                                            Type attrTypeInner = customAttrInner[x].GetType();

                                            switch (attrTypeInner.Name)
                                            {
                                                case "OneToOneAttribute":

                                                    OneToOneAttribute onToOneAttr = (OneToOneAttribute)customAttrInner[x];

                                                    Type onToOneClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classPropertyInner.PropertyType.FullName);
                                                    PropertyInfo oneToOnePropType = null;

                                                    if (onToOneAttr.PropertyRef != null)
                                                    {
                                                        oneToOnePropType = onToOneClassPropType.GetProperty(onToOneAttr.PropertyRef);
                                                    }
                                                    else
                                                    {
                                                        PropertyInfo[] onToOneClassProperties = onToOneClassPropType.GetProperties();

                                                        foreach (PropertyInfo onToOneClassProperty in onToOneClassProperties)
                                                        {
                                                            Attribute[] oneToOneCustomAttr = Attribute.GetCustomAttributes(onToOneClassProperty);
                                                            for (int y = 0; y < oneToOneCustomAttr.Length; y++)
                                                            {
                                                                Type oneToOneAttrType = oneToOneCustomAttr[y].GetType();
                                                                switch (oneToOneAttrType.Name)
                                                                {
                                                                    case "PrimaryKeyAttribute":
                                                                        oneToOnePropType = onToOneClassPropType.GetProperty(onToOneClassProperty.Name);
                                                                        break;
                                                                    default:
                                                                        break;
                                                                }
                                                            }

                                                            if (oneToOnePropType != null)
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    object onToOneInstance = classPropertyInner.GetValue(instance, null);

                                                    if (Convert.ToString(oneToOnePropType.GetValue(onToOneInstance, null)) != "" && Convert.ToString(oneToOnePropType.GetValue(onToOneInstance, null)) != null)
                                                    {
                                                        key = oneToOnePropType.GetValue(onToOneInstance, null);
                                                        command.Parameters.Add(OracleParameters.CreateInputParameter(":" + columnName + "_", OracleDBTypes.GetDbType(oneToOnePropType.PropertyType.ToString()), key));
                                                    }
                                                    else
                                                    {
                                                        command.Parameters.Add(new OracleParameter(":" + columnName + "_", DBNull.Value));
                                                    }

                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    key = classProperty.GetValue(instance, null);
                                    command.Parameters.Add(OracleParameters.CreateInputParameter(":" + columnName + "_", OracleDBTypes.GetDbType(classProperty.PropertyType.ToString()), classProperty.GetValue(instance, null)));
                                }
                            }
                            else
                            {
                                command.Parameters.Add(OracleParameters.CreateOutputParameter("curr_id", OracleDBTypes.GetDbType(classProperty.PropertyType.ToString())));
                            }

                            break;

                        case "HasAndBelongsToManyAttribute":

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
                            if (listInstance.Count > 0)
                            {
                                isContainManyToMany = true;
                                collectionCount = listInstance.Count;
                            }

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
                                command.Parameters.Add(OracleParameters.CreateInputParameter(":" + columnName + "_", OracleDBTypes.GetDbType(colType), classProperty.GetValue(instance, null)));
                            }
                            else
                            {
                                if (propAttr.NotNull)
                                {
                                    command.Parameters.Add(new OracleParameter(":" + columnName + "_", DBNull.Value));
                                }
                                else
                                {
                                    columnName = string.Empty;
                                    ///say it is not allowed to pass null
                                }
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
                                if (belongAttr.NotNull)
                                {
                                    command.Parameters.Add(new OracleParameter(":" + columnName + "_", DBNull.Value));
                                }
                                else
                                {
                                    columnName = string.Empty;
                                    ///say it is not allowed to pass null
                                }
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
                    ///sbColumns.Append("[");
                    sbColumns.Append(columnName);
                    ///sbColumns.Append("]");

                    ///Adding parameter names
                    sbParams.Append(seperator);
                    sbParams.Append(":");
                    sbParams.Append(columnName);
                    sbParams.Append("_");
                }
            }

            ///Finalizing the query
            sbQuery.Append(sbColumns.ToString());
            sbQuery.Append(") VALUES(");
            sbQuery.Append(sbParams.ToString());
            sbQuery.Append(")");
            sbQuery.Append(sbReturn.ToString());

            command.CommandText = sbQuery.ToString();
            return command;
        }

        /// <summary>
        /// Build the query to insert data and arrange parameters
        /// </summary>
        /// <param name="instance">Type T instance to capture the table name</param>
        /// <param name="query">String variables which contains the query</param>
        /// <param name="connection">SqlConnection variable to connect the DB</param>
        /// <param name="transaction">SqlTransation variable to handle the transaction</param>
        /// <returns>Return the SqlCommand to execute</returns>
        protected MySqlCommand MySqlInsertQueryBuilder(T instance, string query, MySqlConnection connection, MySqlTransaction transaction)
        {
            MySqlCommand command = new MySqlCommand(query, connection, transaction);
            command.CommandType = CommandType.Text;

            Type instanceType = instance.GetType();
            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(EasyDataAttribute));

            StringBuilder sbTable = new StringBuilder();
            sbTable.Append("INSERT INTO ");
            if (easyAttr.Schema != null)
            {
                sbTable.Append(easyAttr.Schema);
                sbTable.Append(".");
            }
            if (easyAttr.Table != null)
            {
                sbTable.Append(easyAttr.Table);
            }
            else
            {
                sbTable.Append(instanceType.Name);
            }
            sbTable.Append(" (");

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append(sbTable.ToString());

            StringBuilder sbColumns = new StringBuilder();
            StringBuilder sbParams = new StringBuilder();
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

                            if (priKeyAttr.Generator.Equals(PrimaryKeyType.Identity) || priKeyAttr.Generator.Equals(PrimaryKeyType.Native))
                            {
                                columnName = string.Empty;
                            }

                            if (!columnName.Equals(string.Empty))
                            {
                                if (priKeyAttr.Generator.Equals(PrimaryKeyType.Guid))
                                {
                                    key = Guid.NewGuid();
                                    command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + columnName + "_", MySqlDBTypes.GetDbType(classProperty.PropertyType.ToString()), key));
                                }
                                else if (priKeyAttr.Generator.Equals(PrimaryKeyType.Foreign))
                                {
                                    PropertyInfo[] classPropertiesInner = instanceType.GetProperties();

                                    foreach (PropertyInfo classPropertyInner in classPropertiesInner)
                                    {
                                        Attribute[] customAttrInner = Attribute.GetCustomAttributes(classPropertyInner);

                                        for (int x = 0; x < customAttrInner.Length; x++)
                                        {
                                            Type attrTypeInner = customAttrInner[x].GetType();

                                            switch (attrTypeInner.Name)
                                            {
                                                case "OneToOneAttribute":

                                                    OneToOneAttribute onToOneAttr = (OneToOneAttribute)customAttrInner[x];

                                                    Type onToOneClassPropType = LoadAssembly.GetPropTypeFromAssembly(projPath, assemblyPath, classPropertyInner.PropertyType.FullName);
                                                    PropertyInfo oneToOnePropType = null;

                                                    if (onToOneAttr.PropertyRef != null)
                                                    {
                                                        oneToOnePropType = onToOneClassPropType.GetProperty(onToOneAttr.PropertyRef);
                                                    }
                                                    else
                                                    {
                                                        PropertyInfo[] onToOneClassProperties = onToOneClassPropType.GetProperties();

                                                        foreach (PropertyInfo onToOneClassProperty in onToOneClassProperties)
                                                        {
                                                            Attribute[] oneToOneCustomAttr = Attribute.GetCustomAttributes(onToOneClassProperty);
                                                            for (int y = 0; y < oneToOneCustomAttr.Length; y++)
                                                            {
                                                                Type oneToOneAttrType = oneToOneCustomAttr[y].GetType();
                                                                switch (oneToOneAttrType.Name)
                                                                {
                                                                    case "PrimaryKeyAttribute":
                                                                        oneToOnePropType = onToOneClassPropType.GetProperty(onToOneClassProperty.Name);
                                                                        break;
                                                                    default:
                                                                        break;
                                                                }
                                                            }

                                                            if (oneToOnePropType != null)
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    object onToOneInstance = classPropertyInner.GetValue(instance, null);

                                                    if (Convert.ToString(oneToOnePropType.GetValue(onToOneInstance, null)) != "" && Convert.ToString(oneToOnePropType.GetValue(onToOneInstance, null)) != null)
                                                    {
                                                        key = oneToOnePropType.GetValue(onToOneInstance, null);
                                                        command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + columnName + "_", MySqlDBTypes.GetDbType(oneToOnePropType.PropertyType.ToString()), key));
                                                    }
                                                    else
                                                    {
                                                        command.Parameters.Add(new MySqlParameter(":" + columnName + "_", DBNull.Value));
                                                    }

                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    key = classProperty.GetValue(instance, null);
                                    command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + columnName + "_", MySqlDBTypes.GetDbType(classProperty.PropertyType.ToString()), classProperty.GetValue(instance, null)));
                                }
                            }

                            break;

                        case "HasAndBelongsToManyAttribute":

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
                            if (listInstance.Count > 0)
                            {
                                isContainManyToMany = true;
                                collectionCount = listInstance.Count;
                            }

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
                                command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + columnName + "_", MySqlDBTypes.GetDbType(colType), classProperty.GetValue(instance, null)));
                            }
                            else
                            {
                                if (propAttr.NotNull)
                                {
                                    command.Parameters.Add(new MySqlParameter(":" + columnName + "_", DBNull.Value));
                                }
                                else
                                {
                                    columnName = string.Empty;
                                    ///say it is not allowed to pass null
                                }
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
                                if (belongAttr.NotNull)
                                {
                                    command.Parameters.Add(new MySqlParameter(":" + columnName + "_", DBNull.Value));
                                }
                                else
                                {
                                    columnName = string.Empty;
                                    ///say it is not allowed to pass null
                                }
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

            ///Finalizing the query
            sbQuery.Append(sbColumns.ToString());
            sbQuery.Append(") VALUES(");
            sbQuery.Append(sbParams.ToString());
            sbQuery.Append(")");

            command.CommandText = sbQuery.ToString();
            return command;
        }
    }
}
