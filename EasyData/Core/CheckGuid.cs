using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyData.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckGuid
    {
        /// <summary>
        /// Checks the default GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        internal static bool CheckDefaultGuid(object guid)
        {
            bool isDefault = false;

            Guid inputGuid = (Guid)guid;
            if (inputGuid.Equals(Guid.Empty))
            {
                isDefault = true;
            }

            return isDefault;
        }
    }
}
