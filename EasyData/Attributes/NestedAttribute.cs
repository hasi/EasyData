using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyData.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false), Serializable]
    public class NestedAttribute : Attribute
    {
        private bool _update = true;
        private bool _insert = true;
        private Type _mapType;
        private string _columnPrefix;

        /// <summary>
        /// Informs ActiveRecord that the marked property contains nested elements, contained
        /// in a separate, reusable class.
        /// </summary>
        public NestedAttribute() 
        { 
        }

        /// <summary>
        /// Informs ActiveRecord that the marked property contains nested elements, contained
        /// in a separate, reusable class.
        /// </summary>
        /// <param name="columnPrefix">A prefix to insert before each column in the nested component</param>
        public NestedAttribute(string columnPrefix)
        {
            this._columnPrefix = columnPrefix;
        }

        /// <summary>
        /// Allows one to reference a different type
        /// than the property type
        /// </summary>
        public Type MapType
        {
            get { return _mapType; }
            set { _mapType = value; }
        }

        /// <summary>
        /// Set to <c>false</c> to ignore this nested component when updating entities of this ActiveRecord class.
        /// </summary>
        public bool Update
        {
            get { return _update; }
            set { _update = value; }
        }

        /// <summary>
        /// Set to <c>false</c> to ignore this nested component when inserting entities of this ActiveRecord class.
        /// </summary>
        public bool Insert
        {
            get { return _insert; }
            set { _insert = value; }
        }

        /// <summary>
        /// A prefix to insert before each column in the nested component.
        /// </summary>
        public string ColumnPrefix
        {
            get { return this._columnPrefix; }
            set { this._columnPrefix = value; }
        }
    }
}
