using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyData.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class EasyDataBase<T> where T : class
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public EasyDataBase()
        {
        }

        /// <summary>
        /// Data insert method
        /// </summary>
        /// <param name="easySession">Session for the current connection and transaction</param>
        public void Save(EasySession easySession)
        {
            Save(this, easySession);
        }

        /// <summary>
        /// Delete all data in a table
        /// </summary>
        /// <param name="easySession">Session for the current connection and transaction</param>
        public bool DeleteAll(EasySession easySession)
        {
            return DeleteAll(this.GetType(), easySession);
        }

        /// <summary>
        /// Delete all data in a table
        /// </summary>
        /// <param name="easySession">Session for the current connection and transaction</param>
        /// <param name="where">Whare clause of the query to filter the result</param>
        public bool DeleteAllByCustomWhere(EasySession easySession, string where)
        {
            return DeleteAll(this.GetType(), where, easySession);
        }

        /// <summary>
        /// Delete a record by the primary key of the record
        /// </summary>
        /// <param name="easySession">Session for the current connection and transaction</param>
        public bool Delete(EasySession easySession)
        {
            return DeleteByPrimaryKey(this, easySession);
        }

        /// <summary>
        /// Deletes the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Delete(EasySession easySession, string property, object value)
        {
            return DeleteByProperty(this.GetType(), property, value, easySession);
        }

        /// <summary>
        /// Deletes the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public bool Delete(EasySession easySession, string property)
        {
            return DeleteByProperty(this, property, easySession);
        }

        /// <summary>
        /// Creates the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        public void Create(EasySession easySession)
        {
            Create(this, easySession);
        }

        /// <summary>
        /// Update a record by the primery key of the record
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        public bool Update(EasySession easySession)
        {
            return Update(this, easySession, EasyUpdate.False);
        }

        /// <summary>
        /// Updates the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="easyUpdate">The easy update.</param>
        /// <returns></returns>
        public bool Update(EasySession easySession, EasyUpdate easyUpdate)
        {
            return Update(this, easySession, easyUpdate);
        }

        /// <summary>
        /// Updates the by custom where.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="where">The where.</param>
        /// <returns></returns>
        public bool UpdateByCustomWhere(EasySession easySession, string where)
        {
            return Update(this, where, easySession, EasyUpdate.False);
        }

        /// <summary>
        /// Updates the by custom where.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="where">The where.</param>
        /// <param name="easyUpdate">The easy update.</param>
        /// <returns></returns>
        public bool UpdateByCustomWhere(EasySession easySession, string where, EasyUpdate easyUpdate)
        {
            return Update(this, where, easySession, easyUpdate);
        }

        /// <summary>
        /// Updates the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Update(EasySession easySession, string property, object value)
        {
            return UpdateByProperty(this, property, value, easySession, EasyUpdate.False);
        }

        /// <summary>
        /// Updates the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="easyUpdate">The easy update.</param>
        /// <returns></returns>
        public bool Update(EasySession easySession, string property, object value, EasyUpdate easyUpdate)
        {
            return UpdateByProperty(this, property, value, easySession, easyUpdate);
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        public List<T> FindAll(EasySession easySession)
        {
            return FindAll(this.GetType(), easySession, EasyLoad.None);
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="easyLoad">The easy load.</param>
        /// <returns></returns>
        public List<T> FindAll(EasySession easySession, EasyLoad easyLoad)
        {
            return FindAll(this.GetType(), easySession, easyLoad);
        }

        /// <summary>
        /// Finds all by custom where.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="where">The where.</param>
        /// <returns></returns>
        public List<T> FindAllByCustomWhere(EasySession easySession, string where)
        {
            return FindAll(this.GetType(), where, easySession, EasyLoad.None);
        }

        /// <summary>
        /// Finds all by custom where.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="easyLoad">The easy load.</param>
        /// <param name="where">The where.</param>
        /// <returns></returns>
        public List<T> FindAllByCustomWhere(EasySession easySession, EasyLoad easyLoad, string where)
        {
            return FindAll(this.GetType(), where, easySession, easyLoad);
        }

        /// <summary>
        /// Finds the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public T Find(EasySession easySession, object id)
        {
            return FindByPrimaryKey(this.GetType(), id, easySession, EasyLoad.None);
        }

        /// <summary>
        /// Finds the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="id">The id.</param>
        /// <param name="easyLoad">The easy load.</param>
        /// <returns></returns>
        public T Find(EasySession easySession, object id, EasyLoad easyLoad)
        {
            return FindByPrimaryKey(this.GetType(), id, easySession, easyLoad);
        }

        /// <summary>
        /// Finds the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        public T Find(EasySession easySession)
        {
            return FindByPrimaryKey(this, easySession, EasyLoad.None);
        }

        /// <summary>
        /// Finds the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="easyLoad">The easy load.</param>
        /// <returns></returns>
        public T Find(EasySession easySession, EasyLoad easyLoad)
        {
            return FindByPrimaryKey(this, easySession, easyLoad);
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public List<T> FindAll(EasySession easySession, string property, object value)
        {
            return FindAllByProperty(this.GetType(), property, value, easySession, EasyLoad.None);
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="easyLoad">The easy load.</param>
        /// <returns></returns>
        public List<T> FindAll(EasySession easySession, string property, object value, EasyLoad easyLoad)
        {
            return FindAllByProperty(this.GetType(), property, value, easySession, easyLoad);
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public List<T> FindAll(EasySession easySession, string property)
        {
            return FindAllByProperty(this, property, easySession, EasyLoad.None);
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="property">The property.</param>
        /// <param name="easyLoad">The easy load.</param>
        /// <returns></returns>
        public List<T> FindAll(EasySession easySession, string property, EasyLoad easyLoad)
        {
            return FindAllByProperty(this, property, easySession, easyLoad);
        }

        /// <summary>
        /// Counts the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        public int Count(EasySession easySession)
        {
            return Count(this.GetType(), easySession);
        }

        /// <summary>
        /// Counts the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="where">The where.</param>
        /// <returns></returns>
        public int CountByCustomWhere(EasySession easySession, string where)
        {
            return Count(this.GetType(), where, easySession);
        }

        /// <summary>
        /// Counts the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int Count(EasySession easySession, string property, object value)
        {
            return CountByProperty(this.GetType(), property, value, easySession);
        }

        /// <summary>
        /// Counts the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public int Count(EasySession easySession, string property)
        {
            return CountByProperty(this, property, easySession);
        }

        /// <summary>
        /// Existses the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <returns></returns>
        public bool Exists(EasySession easySession)
        {
            return Exists(this.GetType(), easySession);
        }

        /// <summary>
        /// Existses the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public bool Exists(EasySession easySession, object id)
        {
            return ExistsByProperty(this.GetType(), null, id, easySession);
        }

        /// <summary>
        /// Existses the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="where">The where.</param>
        /// <returns></returns>
        public bool ExistsByCustomWhere(EasySession easySession, string where)
        {
            return Exists(this.GetType(), where, easySession);
        }

        /// <summary>
        /// Existses the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Exists(EasySession easySession, string property, object value)
        {
            return ExistsByProperty(this.GetType(), property, value, easySession);
        }

        /// <summary>
        /// Existses the specified easy session.
        /// </summary>
        /// <param name="easySession">The easy session.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public bool Exists(EasySession easySession, string property)
        {
            return ExistsByProperty(this, property, easySession);
        }






        protected internal static bool DeleteAll(Type type, EasySession easySession)
        {
            EasyDelete<T> delInstance = new EasyDelete<T>();
            return delInstance.DeleteAll(type, null, easySession);
        }

        protected internal static bool DeleteAll(Type type, string where, EasySession easySession)
        {
            EasyDelete<T> delInstance = new EasyDelete<T>();
            return delInstance.DeleteAll(type, where, easySession);
        }

        protected internal static bool DeleteByPrimaryKey(object instance, EasySession easySession)
        {
            T tInstance = (T)instance;
            EasyDelete<T> delInstance = new EasyDelete<T>();
            return delInstance.Delete(tInstance, easySession);
        }

        protected internal static bool DeleteByProperty(Type type, string property, object value, EasySession easySession)
        {
            EasyDelete<T> delInstance = new EasyDelete<T>();
            return delInstance.DeleteByProperty(null, type, property, value, easySession);
        }

        protected internal static bool DeleteByProperty(object instance, string property, EasySession easySession)
        {
            T tInstance = (T)instance;
            EasyDelete<T> delInstance = new EasyDelete<T>();
            return delInstance.DeleteByProperty(tInstance, null, property, null, easySession);
        }

        protected internal static bool Update(object instance, EasySession easySession, EasyUpdate easyUpdate)
        {
            T tInstance = (T)instance;
            EasyUpdate<T> updateInstance = new EasyUpdate<T>();
            return updateInstance.Update(tInstance, easySession, easyUpdate);
        }

        protected internal static bool Update(object instance, string where, EasySession easySession, EasyUpdate easyUpdate)
        {
            T tInstance = (T)instance;
            EasyUpdate<T> updateInstance = new EasyUpdate<T>();
            return updateInstance.Update(tInstance, where, easySession, easyUpdate);
        }

        protected internal static bool UpdateByProperty(object instance, string property, object value, EasySession easySession, EasyUpdate easyUpdate)
        {
            T tInstance = (T)instance;
            EasyUpdate<T> updateInstance = new EasyUpdate<T>();
            return updateInstance.Update(tInstance, property, value, easySession, easyUpdate);
        }

        protected internal static void Save(object instance, EasySession easySession)
        {
            T tInstance = (T)instance;
            EasySave<T> saveInstance = new EasySave<T>();
            saveInstance.Save(tInstance, easySession);
        }

        protected internal static void Create(object instance, EasySession easySession)
        {
            T tInstance = (T)instance;
            EasySave<T> saveInstance = new EasySave<T>();
            saveInstance.Save(tInstance, easySession);
        }

        protected internal static List<T> FindAll(Type type, EasySession easySession, EasyLoad easyLoad)
        {
            EasySelect<T> selectInstance = new EasySelect<T>();
            return selectInstance.FindAll(type, null, easySession, easyLoad);
        }

        protected internal static List<T> FindAll(Type type, string where, EasySession easySession, EasyLoad easyLoad)
        {
            EasySelect<T> selectInstance = new EasySelect<T>();
            return selectInstance.FindAll(type, where, easySession, easyLoad);
        }

        protected internal static T FindByPrimaryKey(Type type, object id, EasySession easySession, EasyLoad easyLoad)
        {
            EasySelect<T> selectInstance = new EasySelect<T>();
            return selectInstance.Find(type, id, easySession, easyLoad);
        }

        protected internal static T FindByPrimaryKey(object instance, EasySession easySession, EasyLoad easyLoad)
        {
            T tInstance = (T)instance;
            EasySelect<T> selectInstance = new EasySelect<T>();
            return selectInstance.Find(tInstance, easySession, easyLoad);
        }

        protected internal static List<T> FindAllByProperty(Type type, string property, object value, EasySession easySession, EasyLoad easyLoad)
        {
            EasySelect<T> selectInstance = new EasySelect<T>();
            return selectInstance.FindByProperty(type, property, value, easySession, easyLoad);
        }

        protected internal static List<T> FindAllByProperty(object instance, string property, EasySession easySession, EasyLoad easyLoad)
        {
            T tInstance = (T)instance;
            EasySelect<T> selectInstance = new EasySelect<T>();
            return selectInstance.FindByProperty(tInstance, property, easySession, easyLoad);
        }

        protected internal static int Count(Type type, EasySession easySession)
        {
            EasySelect<T> selectInstance = new EasySelect<T>();
            return selectInstance.Count(type, easySession);
        }

        protected internal static int CountByProperty(Type type, string property, object value, EasySession easySession)
        {
            EasySelect<T> selectInstance = new EasySelect<T>();
            return selectInstance.CountByProperty(type, property, value, easySession);
        }

        protected internal static int CountByProperty(object instance, string property, EasySession easySession)
        {
            T tInstance = (T)instance;
            EasySelect<T> selectInstance = new EasySelect<T>();
            return selectInstance.CountByProperty(tInstance, property, easySession);
        }

        protected internal static int Count(Type type, string where, EasySession easySession)
        {
            EasySelect<T> selectInstance = new EasySelect<T>();
            return selectInstance.Count(type, where, easySession);
        }

        protected internal static bool Exists(Type type, EasySession easySession)
        {
            EasySelect<T> selectInstance = new EasySelect<T>();
            return selectInstance.Exists(type, easySession);
        }

        protected internal static bool ExistsByProperty(Type type, string property, object value, EasySession easySession)
        {
            EasySelect<T> selectInstance = new EasySelect<T>();
            return selectInstance.ExistsByProperty(type, property, value, easySession);
        }

        protected internal static bool ExistsByProperty(object instance, string property, EasySession easySession)
        {
            T tInstance = (T)instance;
            EasySelect<T> selectInstance = new EasySelect<T>();
            return selectInstance.ExistsByProperty(tInstance, property, easySession);
        }

        protected internal static bool Exists(Type type, string where, EasySession easySession)
        {
            EasySelect<T> selectInstance = new EasySelect<T>();
            return selectInstance.Exists(type, where, easySession);
        }
    }
}
