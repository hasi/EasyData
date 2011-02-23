using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyData.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property), Serializable]
    public class PropertyAttribute : Attribute
    {
        private bool _load = false;
        private string _column;
        private string _type;
        private string _uniqueKey;
        private string _index;
        private string _check;
        private int _length;
        private bool _notNull;
        private bool _unique;
        private bool _update = true;
        private bool _insert = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAttribute"/> class.
        /// </summary>
        public PropertyAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAttribute"/> class.
        /// </summary>
        /// <param name="column">The column.</param>
        public PropertyAttribute(string column)
            : this()
        {
            this._column = column;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAttribute"/> class.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="type">The type.</param>
        public PropertyAttribute(string column, string type)
            : this(column)
        {
            this._type = type;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this property allow null.
        /// </summary>
        /// <value><c>true</c> if allow null; otherwise, <c>false</c>.</value>
        public bool NotNull
        {
            get { return _notNull; }
            set { _notNull = value; }
        }

        /// <summary>
        /// Gets or sets the length of the property (for strings - nvarchar(50) )
        /// </summary>
        /// <value>The length.</value>
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// Gets or sets the column name
        /// </summary>
        /// <value>The column.</value>
        public string Column
        {
            get { return _column; }
            set { _column = value; }
        }

        /// <summary>
        /// Set to <c>false</c> to ignore this property when updating entities of this EasyData class.
        /// </summary>
        public bool Update
        {
            get { return _update; }
            set { _update = value; }
        }

        /// <summary>
        /// Set to <c>false</c> to ignore this property when inserting entities of this EasyData class.
        /// </summary>
        public bool Insert
        {
            get { return _insert; }
            set { _insert = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PropertyAttribute"/> is unique.
        /// </summary>
        /// <value><c>true</c> if unique; otherwise, <c>false</c>.</value>
        public bool Unique
        {
            get { return _unique; }
            set { _unique = value; }
        }

        /// <summary>
        /// A unique-key attribute can be used to group columns 
        /// in a single unit key constraint. 
        /// </summary>
        /// <value>unique key name</value>
        /// <remarks>
        /// Currently, the 
        /// specified value of the unique-key attribute is not 
        /// used to name the constraint, only to group the columns 
        /// in the mapping file.
        /// </remarks>
        public string UniqueKey
        {
            get { return _uniqueKey; }
            set { _uniqueKey = value; }
        }

        /// <summary>
        /// Gets or sets the type of the column.
        /// </summary>
        /// <value>The type of the column.</value>
        public string ColumnType
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// specifies the name of a (multi-column) index
        /// </summary>
        /// <value>index name</value>
        public string Index
        {
            get { return _index; }
            set { _index = value; }
        }

        /// <summary>
        /// From NHibernate documentation:
        /// create an SQL check constraint on either column or table
        /// </summary>
        /// <value>Sql Expression</value>
        public string Check
        {
            get { return _check; }
            set { _check = value; }
        }

        /// <summary>
        /// Gets or sets the loading type.
        /// </summary>
        /// <value>The loading type.</value>
        public bool PartialLoad
        {
            get { return _load; }
            set { _load = value; }
        }
    }
}
