using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;

namespace EasyData.Query
{
    public class ObjectQuery
    {
        public IQuery EasyQuery(EasySession easySession)
        {
            return new QueryImpl(easySession);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="easySession"></param>
        /// <returns></returns>
        public IQuery DeleteQuery(string query, EasySession easySession)
        {
            return new QueryImpl(query, easySession, CommandType.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spname"></param>
        /// <param name="easySession"></param>
        /// <returns></returns>
        public IQuery DeleteSP(string spname, EasySession easySession)
        {
            return new QueryImpl(spname, easySession, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="easySession"></param>
        /// <returns></returns>
        public IQuery UpdateQuery(string query, EasySession easySession)
        {
            return new QueryImpl(query, easySession, CommandType.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spname"></param>
        /// <param name="easySession"></param>
        /// <returns></returns>
        public IQuery UpdateSP(string spname, EasySession easySession)
        {
            return new QueryImpl(spname, easySession, CommandType.StoredProcedure);
        }
    }
}
