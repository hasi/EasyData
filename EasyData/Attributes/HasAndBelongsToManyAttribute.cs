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
    public class HasAndBelongsToManyAttribute : RelationAttribute
    {
        private string _columnRef;
        private string[] _compositeKeyColumnRefs;
        private string _columnKey;
        private string[] _compositeKeyColumnKeys;
        private FetchEnum _fetchMethod = FetchEnum.Unspecified;
        private Type _customCollectionType;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasAndBelongsToManyAttribute"/> class.
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        public HasAndBelongsToManyAttribute(Type mapType)
        {
            this.mapType = mapType;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="HasAndBelongsToManyAttribute"/> class.
        /// </summary>
        public HasAndBelongsToManyAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HasAndBelongsToManyAttribute"/> class.
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <param name="type">The type.</param>
        public HasAndBelongsToManyAttribute(Type mapType, RelationType type)
            : this(mapType)
        {
            base.relType = type;
        }

        /// <summary>
        /// Gets or sets the column that represent the other side on the assoication table
        /// </summary>
        /// <value>The column ref.</value>
        public string ColumnRef
        {
            get { return _columnRef; }
            set { _columnRef = value; }
        }

        /// <summary>
        /// Gets or sets the composite key columns that represent the other side on the assoication table
        /// </summary>
        /// <value>The composite key column refs.</value>
        public string[] CompositeKeyColumnRefs
        {
            get { return _compositeKeyColumnRefs; }
            set { _compositeKeyColumnRefs = value; }
        }

        /// <summary>
        /// Gets or sets the key column name
        /// </summary>
        /// <value>The column key.</value>
        public string ColumnKey
        {
            get { return _columnKey; }
            set { _columnKey = value; }
        }

        /// <summary>
        /// Gets or sets the composite key columns names.
        /// </summary>
        /// <value>The composite key column keys.</value>
        public string[] CompositeKeyColumnKeys
        {
            get { return _compositeKeyColumnKeys; }
            set { _compositeKeyColumnKeys = value; }
        }

        /// <summary>
        /// Chooses between outer-join fetching
        /// or sequential select fetching.
        /// </summary>
        public FetchEnum Fetch
        {
            get { return _fetchMethod; }
            set { _fetchMethod = value; }
        }

        /// <summary>
        /// Provides a custom collection type.
        /// </summary>
        public Type CollectionType
        {
            get { return _customCollectionType; }
            set { _customCollectionType = value; }
        }
    }
}
