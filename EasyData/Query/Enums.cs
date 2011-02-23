using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyData.Query
{
    [Serializable]
    public enum EasyLoad
    {
        /// <summary>
        /// Default, Loads all data for the current object and for the related objects
        /// </summary>
        None,
        /// <summary>
        /// Loads data for the current object, without considering related objects.
        /// </summary>
        Simple,
        /// <summary>
        /// Loads data for the specified properties of the current or related objects.
        /// </summary>
        Specified
    }
}
