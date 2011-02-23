using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyData.Attributes
{
    /// <summary>
    /// Custom attribute for the primary key relationship
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false), Serializable]
    public class PrimaryKeyAttribute : Attribute
    {
        private PrimaryKeyType _generator = PrimaryKeyType.Native;
        private bool _load = true;
        private string _column;
        private string _unsavedValue;
        private string _sequenceName;
        private string _type;
        private string _params;
        private int _length;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryKeyAttribute"/> class.
        /// </summary>
        public PrimaryKeyAttribute()
            : this(PrimaryKeyType.Native)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryKeyAttribute"/> class.
        /// </summary>
        /// <param name="generator">The generator.</param>
        public PrimaryKeyAttribute(PrimaryKeyType generator)
        {
            this._generator = generator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryKeyAttribute"/> class.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="column">The PK column.</param>
        public PrimaryKeyAttribute(PrimaryKeyType generator, String column)
            : this(generator)
        {
            this._column = column;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryKeyAttribute"/> class.
        /// </summary>
        /// <param name="column">The PK column.</param>
        public PrimaryKeyAttribute(string column)
        {
            this._column = column;
        }

        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        public PrimaryKeyType Generator
        {
            get { return _generator; }
            set { _generator = value; }
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
        /// Gets or sets the unsaved value.
        /// </summary>
        /// <value>The unsaved value.</value>
        public string UnsavedValue
        {
            get { return _unsavedValue; }
            set { _unsavedValue = value; }
        }

        /// <summary>
        /// Gets or sets the name of the sequence.
        /// </summary>
        /// <value>The name of the sequence.</value>
        public string SequenceName
        {
            get { return _sequenceName; }
            set { _sequenceName = value; }
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
        /// Gets or sets the length of values in the column
        /// </summary>
        /// <value>The length.</value>
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// Comma separated value of parameters to the generator
        /// </summary>
        public string Params
        {
            get { return _params; }
            set { _params = value; }
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
