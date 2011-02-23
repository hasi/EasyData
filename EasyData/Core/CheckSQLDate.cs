using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyData.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckSQLDate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        internal static bool CheckDefaultDate(object dateTime)
        {
            bool isDefault = false;

            DateTime inputDateTime = (DateTime)dateTime;
            if(inputDateTime.Equals(DateTime.MinValue))
            {
                isDefault = true;
            }

            return isDefault;
        }
    }
}
