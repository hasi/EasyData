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
using Devart.Data.MySql;
using EasyData.DB.MySql;
using EasyData.DB.Oracle;
using System.Data.OracleClient;
using EasyData.Core;

namespace EasyData
{
    /// <summary>
    /// Provide the delete functionality for the EasyData
    /// </summary>
    /// <typeparam name="T">Type of the entity class</typeparam>
    public class EasyDelete<T>
    {
        string assemblyPath = Assembly.GetExecutingAssembly().CodeBase.Remove(Assembly.GetExecutingAssembly().CodeBase.IndexOf("bin")) + ConfigurationManager.AppSettings[Constants.ASSEMBLY_PATH];
        string dbType = ConfigurationManager.AppSettings[Constants.DB_TYPE];
        
        static object propertyValueByInstance;

        static string propertyTypeByType;
        static string propertyTypeByInstance;

        static string propertyColumnByInstance;
        static string propertyColumnByType;

        /// <summary>
        /// Deletes all.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="where">The where.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>If success return <c>true</c>, else <c>false</c></returns>
        public bool DeleteAll(Type type, string where, EasySession easySession)
        {
            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SQLDelete(type, null, null, where, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleDelete(type, null, null, where, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlDelete(type, null, null, where, easySession);
            }

            return false;
        }

        /// <summary>
        /// Deletes the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>If success return <c>true</c>, else <c>false</c></returns>
        public bool Delete(T instance, EasySession easySession)
        {
            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SQLDelete(instance, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleDelete(instance, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlDelete(instance, easySession);
            }

            return false;
        }

        public bool DeleteByProperty(T instance, Type type, string property, object value, EasySession easySession)
        {
            if (value == null)
            {
                SetPropertyValue(instance, property);
                type = instance.GetType();
            }
            else
            {
                propertyValueByInstance = value;
            }

            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                return this.SQLDelete(type, property, propertyValueByInstance, null, easySession);
            }
            else if (dbType == "ORACLE")
            {
                return this.OracleDelete(type, property, propertyValueByInstance, null, easySession);
            }
            else if (dbType == "MYSQL")
            {
                return this.MySqlDelete(type, property, propertyValueByInstance, null, easySession);
            }

            return false;
        }

        /// <summary>
        /// SQL delete all.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="where">The where.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>
        /// If success return <c>true</c>, else <c>false</c>
        /// </returns>
        protected bool SQLDelete(Type type, string property, object value, string where, EasySession easySession)
        {
            string query = string.Empty;
            bool isSuccess = false;

            try
            {
                using (var command = this.SqlQueryBuilderForDelete(type, property, value, where, query, easySession))
                {
                    command.ExecuteNonQuery();
                    easySession.SetCommit = true;
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                easySession.SetRollback = true;
                isSuccess = false;
                throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
            }
            
            return isSuccess;
        }

        /// <summary>
        /// Oracle delete all.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="where">The where.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>If success return <c>true</c>, else <c>false</c></returns>
        protected bool OracleDelete(Type type, string property, object value, string where, EasySession easySession)
        {
            string query = string.Empty;
            bool isSuccess = false;

            try
            {
                using (var command = this.OracleQueryBuilderForDelete(type, property, value, where, query, easySession))
                {
                    command.ExecuteNonQuery();
                    easySession.SetCommit = true;
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                easySession.SetRollback = true;
                isSuccess = false;
                throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
            }
            
            return isSuccess;
        }

        /// <summary>
        /// MySql delete all.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="where">The where.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>If success return <c>true</c>, else <c>false</c></returns>
        protected bool MySqlDelete(Type type, string property, object value, string where, EasySession easySession)
        {
            string query = string.Empty;
            bool isSuccess = false;

            try
            {
                using (var command = this.MySqlQueryBuilderForDelete(type, property, value, where, query, easySession))
                {
                    command.ExecuteNonQuery();
                    easySession.SetCommit = true;
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                easySession.SetRollback = true;
                isSuccess = false;
                throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
            }
            
            return isSuccess;
        }

        /// <summary>
        /// SQL delete.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>If success return <c>true</c>, else <c>false</c></returns>
        protected bool SQLDelete(T instance, EasySession easySession)
        {
            string query = string.Empty;
            bool isSuccess = false;

            try
            {
                using (var command = this.SqlQueryBuilderForDelete(instance, query, easySession))
                {
                    command.ExecuteNonQuery();
                    easySession.SetCommit = true;
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                easySession.SetRollback = true;
                isSuccess = false;
                throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
            }
            
            return isSuccess;
        }

        /// <summary>
        /// Oracle delete.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>If success return <c>true</c>, else <c>false</c></returns>
        protected bool OracleDelete(T instance, EasySession easySession)
        {
            string query = string.Empty;
            bool isSuccess = false;
            
            try
            {
                using (var command = this.OracleQueryBuilderForDelete(instance, query, easySession))
                {
                    command.ExecuteNonQuery();
                    easySession.SetCommit = true;
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                easySession.SetRollback = true;
                isSuccess = false;
                throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
            }
            
            return isSuccess;
        }

        /// <summary>
        /// MySql delete.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>If success return <c>true</c>, else <c>false</c></returns>
        protected bool MySqlDelete(T instance, EasySession easySession)
        {
            string query = string.Empty;
            bool isSuccess = false;

            try
            {
                using (var command = this.MySqlQueryBuilderForDelete(instance, query, easySession))
                {
                    command.ExecuteNonQuery();
                    easySession.SetCommit = true;
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                easySession.SetRollback = true;
                isSuccess = false;
                throw new ApplicationException("Internal Error!, Please contact administrator with a screen shot of error screen...", ex);
            }
            
            return isSuccess;
        }

        /// <summary>
        /// SQL delete query builder .
        /// <remarks>TO DO: Cascade Delete</remarks>
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="where">The where.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>Returns the SqlCommand with the querystring and parameters</returns>
        protected SqlCommand SqlQueryBuilderForDelete(Type type, string property, object value, string where, string query, EasySession easySession)
        {
            string table;
            SqlCommand command = new SqlCommand(query, easySession.Connection, easySession.Transaction);
            command.CommandType = CommandType.Text;

            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(type, typeof(EasyDataAttribute));
            StringBuilder sbQuery = new StringBuilder();
            if (where != null || property != null)
            {
                sbQuery.Append("DELETE FROM ");
            }
            else
            {
                sbQuery.Append("DELETE * FROM ");
            }

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
                table = type.Name;
                sbQuery.Append(type.Name);
            }

            if (where != null)
            {
                sbQuery.Append(" WHERE ");
                sbQuery.Append(where);
            }
            else if (property != null)
            {
                GetPropertyColumn(type, property);

                command.Parameters.Add(SqlParameters.CreateInputParameter("@" + propertyColumnByType + "_", SqlDBTypes.GetDbType(propertyTypeByType), value));

                sbQuery.Append(" WHERE ");
                sbQuery.Append(table);
                sbQuery.Append(".");
                sbQuery.Append(propertyColumnByType);
                sbQuery.Append(" = ");
                sbQuery.Append("@");
                sbQuery.Append(propertyColumnByType);
                sbQuery.Append("_");
            }

            command.CommandText = sbQuery.ToString();
            return command;
        }

        /// <summary>
        /// Oracle delete query builder .
        /// <remarks>TO DO: Cascade Delete</remarks>
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="where">The where.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>Returns the OracleCommand with the querystring and parameters</returns>
        protected OracleCommand OracleQueryBuilderForDelete(Type type, string property, object value, string where, string query, EasySession easySession)
        {
            string table;
            OracleCommand command = new OracleCommand(query, easySession.OConnection, easySession.OTransaction);
            command.CommandType = CommandType.Text;

            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(type, typeof(EasyDataAttribute));
            StringBuilder sbQuery = new StringBuilder();
            if (where != null)
            {
                sbQuery.Append("DELETE FROM ");
            }
            else
            {
                sbQuery.Append("DELETE * FROM ");
            }

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
                table = type.Name;
                sbQuery.Append(type.Name);
            }

            if (where != null)
            {
                sbQuery.Append(" WHERE ");
                sbQuery.Append(where);
            }
            else if (property != null)
            {
                GetPropertyColumn(type, property);

                command.Parameters.Add(OracleParameters.CreateInputParameter(":" + propertyColumnByType + "_", OracleDBTypes.GetDbType(propertyTypeByType), value));

                sbQuery.Append(" WHERE ");
                sbQuery.Append(table);
                sbQuery.Append(".");
                sbQuery.Append(propertyColumnByType);
                sbQuery.Append(" = ");
                sbQuery.Append(":");
                sbQuery.Append(propertyColumnByType);
                sbQuery.Append("_");
            }

            command.CommandText = sbQuery.ToString();
            return command;
        }

        /// <summary>
        /// MySQL delete query builder .
        /// <remarks>TO DO: Cascade Delete</remarks>
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="where">The where.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>Returns the MySqlCommand with the querystring and parameters</returns>
        protected MySqlCommand MySqlQueryBuilderForDelete(Type type, string property, object value, string where, string query, EasySession easySession)
        {
            string table;
            MySqlCommand command = new MySqlCommand(query, easySession.MConnection, easySession.MTransaction);
            command.CommandType = CommandType.Text;

            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(type, typeof(EasyDataAttribute));
            StringBuilder sbQuery = new StringBuilder();
            if (where != null)
            {
                sbQuery.Append("DELETE FROM ");
            }
            else
            {
                sbQuery.Append("DELETE * FROM ");
            }

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
                table = type.Name;
                sbQuery.Append(type.Name);
            }

            if (where != null)
            {
                sbQuery.Append(" WHERE ");
                sbQuery.Append(where);
            }
            else if (property != null)
            {
                GetPropertyColumn(type, property);

                command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + propertyColumnByType + "_", MySqlDBTypes.GetDbType(propertyTypeByType), value));

                sbQuery.Append(" WHERE ");
                sbQuery.Append(table);
                sbQuery.Append(".");
                sbQuery.Append(propertyColumnByType);
                sbQuery.Append(" = ");
                sbQuery.Append(":");
                sbQuery.Append(propertyColumnByType);
                sbQuery.Append("_");
            }

            command.CommandText = sbQuery.ToString();
            return command;
        }


        /// <summary>
        /// SQL delete query builder.
        /// <remarks>TO DO: Cascade Delete</remarks>
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>Returns the SqlCommand with the querystring and parameters</returns>
        protected SqlCommand SqlQueryBuilderForDelete(T instance, string query, EasySession easySession)
        {
            string table;
            SqlCommand command = new SqlCommand(query, easySession.Connection, easySession.Transaction);
            command.CommandType = CommandType.Text;

            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(EasyDataAttribute));
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("DELETE FROM ");
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
                table = instance.GetType().Name;
                sbQuery.Append(instance.GetType().Name);
            }
            sbQuery.Append(" WHERE ");

            string columnName = string.Empty;
            Type instanceType = instance.GetType();
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

                            PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                            if (priKeyAttr.Column != null)
                            {
                                columnName = priKeyAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }

                            command.Parameters.Add(SqlParameters.CreateInputParameter("@" + columnName + "_", SqlDBTypes.GetDbType(classProperty.PropertyType.ToString()), classProperty.GetValue(instance, null)));

                            sbQuery.Append(table);
                            sbQuery.Append(".");
                            sbQuery.Append(columnName);
                            sbQuery.Append(" = ");
                            sbQuery.Append("@");
                            sbQuery.Append(columnName);
                            sbQuery.Append("_");
                            break;
                        default:
                            break;
                    }
                }
            }

            command.CommandText = sbQuery.ToString();
            return command;
        }

        /// <summary>
        /// SQL delete query builder.
        /// <remarks>TO DO: Cascade Delete</remarks>
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>Returns the OracleCommand with the querystring and parameters</returns>
        protected OracleCommand OracleQueryBuilderForDelete(T instance, string query, EasySession easySession)
        {
            string table;
            OracleCommand command = new OracleCommand(query, easySession.OConnection, easySession.OTransaction);
            command.CommandType = CommandType.Text;

            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(EasyDataAttribute));
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("DELETE FROM ");
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
                table = instance.GetType().Name;
                sbQuery.Append(instance.GetType().Name);
            }
            sbQuery.Append(" WHERE ");

            string columnName = string.Empty;
            Type instanceType = instance.GetType();
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

                            PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                            if (priKeyAttr.Column != null)
                            {
                                columnName = priKeyAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }

                            command.Parameters.Add(OracleParameters.CreateInputParameter(":" + columnName + "_", OracleDBTypes.GetDbType(classProperty.PropertyType.ToString()), classProperty.GetValue(instance, null)));

                            sbQuery.Append(table);
                            sbQuery.Append(".");
                            sbQuery.Append(columnName);
                            sbQuery.Append(" = ");
                            sbQuery.Append(":");
                            sbQuery.Append(columnName);
                            sbQuery.Append("_");
                            break;
                        default:
                            break;
                    }
                }
            }

            command.CommandText = sbQuery.ToString();
            return command;
        }

        /// <summary>
        /// MySQL delete query builder.
        /// <remarks>TO DO: Cascade Delete</remarks>
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="query">The query.</param>
        /// <param name="easySession">The easy session.</param>
        /// <returns>Returns the MySqlCommand with the querystring and parameters</returns>
        protected MySqlCommand MySqlQueryBuilderForDelete(T instance, string query, EasySession easySession)
        {
            string table;
            MySqlCommand command = new MySqlCommand(query, easySession.MConnection, easySession.MTransaction);
            command.CommandType = CommandType.Text;

            EasyDataAttribute easyAttr = (EasyDataAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(EasyDataAttribute));
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("DELETE FROM ");
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
                table = instance.GetType().Name;
                sbQuery.Append(instance.GetType().Name);
            }
            sbQuery.Append(" WHERE ");

            string columnName = string.Empty;
            Type instanceType = instance.GetType();
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

                            PrimaryKeyAttribute priKeyAttr = (PrimaryKeyAttribute)customAttr[i];
                            if (priKeyAttr.Column != null)
                            {
                                columnName = priKeyAttr.Column;
                            }
                            else
                            {
                                columnName = classProperty.Name;
                            }

                            command.Parameters.Add(MySqlParameters.CreateInputParameter(":" + columnName + "_", MySqlDBTypes.GetDbType(classProperty.PropertyType.ToString()), classProperty.GetValue(instance, null)));

                            sbQuery.Append(table);
                            sbQuery.Append(".");
                            sbQuery.Append(columnName);
                            sbQuery.Append(" = ");
                            sbQuery.Append(":");
                            sbQuery.Append(columnName);
                            sbQuery.Append("_");
                            break;
                        default:
                            break;
                    }
                }
            }

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

                    propertyTypeByType = classProperty.PropertyType.ToString();
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
