using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyData.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false), Serializable]
    public class EasyDataAttribute : Attribute
    {
        private string _table;
        private string _schema;
        private string _discriminatorType;
        private string _discriminatorValue;
        private string _discriminatorColumn;
        private string _where;
		private bool _dynamicUpdate;
		private bool _dynamicInsert;
	
		
		/// <summary>
		/// Uses the class name as table name
		/// </summary>
		public EasyDataAttribute() 
        {
        }

		/// <summary>
		/// Associates the specified table with the target type
		/// </summary>
		/// <param name="table"></param>
        public EasyDataAttribute(string table)
		{
			this._table = table;
		}

		/// <summary>
		/// Associates the specified table and schema with the target type
		/// </summary>
        public EasyDataAttribute(string table, string schema)
		{
			this._table = table;
			this._schema = schema;
		}

		/// <summary>
		/// Gets or sets the table name associated with the type
		/// </summary>
        public string Table
		{
			get { return _table; }
			set { _table = value; }
		}

		/// <summary>
		/// Gets or sets the schema name associated with the type
		/// </summary>
        public string Schema
		{
			get { return _schema; }
			set { _schema = value; }
		}

		/// <summary>
		/// Gets or sets the Discriminator column for
		/// a table inheritance modeling
		/// </summary>
        public string DiscriminatorColumn
		{
			get { return _discriminatorColumn; }
			set { _discriminatorColumn = value; }
		}

		/// <summary>
		/// Gets or sets the column type (like string or integer)
		/// for the discriminator column
		/// </summary>
        public string DiscriminatorType
		{
			get { return _discriminatorType; }
			set { _discriminatorType = value; }
		}

		/// <summary>
		/// Gets or sets the value that represents the
		/// target class on the discriminator column
		/// </summary>
        public string DiscriminatorValue
		{
			get { return _discriminatorValue; }
			set { _discriminatorValue = value; }
		}

		/// <summary>
		/// SQL condition to retrieve objects
		/// </summary>
        public string Where
		{
			get { return _where; }
			set { _where = value; }
		}

		/// <summary>
		/// Specifies that UPDATE SQL should be 
		/// generated at runtime and contain only 
		/// those columns whose values have changed.
		/// </summary>
		public bool DynamicUpdate
		{
			get { return _dynamicUpdate; }
			set { _dynamicUpdate = value; }
		}

		/// <summary>
		/// Specifies that INSERT SQL should be 
		/// generated at runtime and contain only 
		/// the columns whose values are not null.
		/// </summary>
		public bool DynamicInsert
		{
			get { return _dynamicInsert; }
			set { _dynamicInsert = value; }
		}
    }
}
