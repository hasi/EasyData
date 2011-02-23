using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace EasyData.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EasyDataBase
    {
        /// <summary>
        /// Constructor method
        /// </summary>
        public EasyDataBase()
        {
        }

        //public IList FindAll(EasySession easySession)
        //{
        //    return FindAll(this.GetType(), easySession, EasyLoad.None);
        //}

        //protected internal static IList FindAll(Type type, EasySession easySession, EasyLoad easyLoad)
        //{
        //    EasySelect<> selectInstance = new EasySelect<>();
        //    return selectInstance.FindAll(type, null, easySession, easyLoad);
        //}
    }
}
