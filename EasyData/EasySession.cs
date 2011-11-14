using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Configuration;
using System.Data.OracleClient;
using Devart.Data.MySql;
using EasyData.Core;
using System.Reflection;
using System.IO;

namespace EasyData
{
    /// <summary>
    /// 
    /// </summary>
    public class EasySession : IDisposable
    {
        string connecString = ConfigurationManager.AppSettings[Constants.CONNECTION_STRING];
        string dbType = ConfigurationManager.AppSettings[Constants.DB_TYPE];

        private SqlConnection _connection;
        private SqlTransaction _transaction;

        private OracleConnection _oConnection;
        private OracleTransaction _oTransaction;

        private MySqlConnection _mConnection;
        private MySqlTransaction _mTransaction;

        private bool _setRollback;
        private bool _setCommit;
        private FlushAction _flushAction;

        /// <summary>
        /// 
        /// </summary>
        public EasySession() : this(FlushAction.Auto)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flushAction"></param>
        public EasySession(FlushAction flushAction)
        {
            if (dbType == null || dbType == string.Empty || dbType == "SQL")
            {
                this.Connection = new SqlConnection(connecString);
                this.Connection.Open();
                this.Transaction = Connection.BeginTransaction();
            }
            else if (dbType == "ORACLE")
            {
                this.OConnection = new OracleConnection(connecString);
                this.OConnection.Open();
                this.OTransaction = OConnection.BeginTransaction();
            }
            else if (dbType == "MYSQL")
            {
                this.MConnection = new MySqlConnection(connecString);
                this.MConnection.Open();
                this.MTransaction = MConnection.BeginTransaction();
            }

            this.FlushAction = flushAction;
        }

        /// <summary>
        /// 
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public OracleConnection OConnection
        {
            get { return _oConnection; }
            set { _oConnection = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public OracleTransaction OTransaction
        {
            get { return _oTransaction; }
            set { _oTransaction = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public MySqlConnection MConnection
        {
            get { return _mConnection; }
            set { _mConnection = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public MySqlTransaction MTransaction
        {
            get { return _mTransaction; }
            set { _mTransaction = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SetCommit
        {
            get { return _setCommit; }
            set { _setCommit = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SetRollback
        {
            get { return _setRollback; }
            set { _setRollback = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public FlushAction FlushAction
        {
            get { return _flushAction; }
            set { _flushAction = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (SetRollback || !SetCommit)
                {
                    if (dbType == null || dbType == string.Empty || dbType == "SQL")
                    {
                        Transaction.Rollback();
                    }
                    else if (dbType == "ORACLE")
                    {
                        OTransaction.Rollback();
                    }
                    else if (dbType == "MYSQL")
                    {
                        MTransaction.Rollback();
                    }
                }
                else if (SetCommit && !SetRollback)
                {
                    if (dbType == null || dbType == string.Empty || dbType == "SQL")
                    {
                        Transaction.Commit();
                    }
                    else if (dbType == "ORACLE")
                    {
                        OTransaction.Commit();
                    }
                    else if (dbType == "MYSQL")
                    {
                        MTransaction.Commit();
                    }
                }

                if (dbType == null || dbType == string.Empty || dbType == "SQL")
                {
                    Connection.Close();
                }
                else if (dbType == "ORACLE")
                {
                    OConnection.Close();
                }
                else if (dbType == "MYSQL")
                {
                    MConnection.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        ~EasySession()
        {
            Dispose(false);
        }
    }
}
