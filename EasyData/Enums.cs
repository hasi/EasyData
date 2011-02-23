using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyData
{
    /// <summary>
    /// Nature of the data load
    /// </summary>
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

    /// <summary>
    /// Nature of the data load
    /// </summary>
    [Serializable]
    public enum EasyUpdate
    {
        /// <summary>
        /// Update the data only for the current object.
        /// </summary>
        True,
        /// <summary>
        /// Default value, update associate object data according
        /// to the relationship to the current object.
        /// </summary>
        False
    }

    /// <summary>
    /// Behavior of the session
    /// </summary>
    [Serializable]
    public enum FlushAction
    {
        /// <summary>
        /// Original behavior. Changes are persisted at the 
        /// end or before some queries.
        /// </summary>
        Auto,
        /// <summary>
        /// Flush need to be controlled manually. Best choice
        /// for readonly operations
        /// </summary>
        Never
    }
}
