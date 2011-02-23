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
    public abstract class RelationAttribute : Attribute
    {
        internal Type mapType;
        internal String table;
        internal String schema;
        internal String orderBy;
        internal String where;
        internal String sort;
        internal String index;
        internal String indexType;
        internal String element;
        internal bool inverse;
        internal ManyRelationCascadeEnum cascade = ManyRelationCascadeEnum.None;
        internal RelationType relType = RelationType.Guess;
        internal NotFoundBehaviour notFoundBehaviour = NotFoundBehaviour.Default;
        private bool _load = false;

        /// <summary>
        /// Gets or sets the type of the relation.
        /// </summary>
        /// <value>The type of the relation.</value>
        public RelationType RelationType
        {
            get { return relType; }
            set { relType = value; }
        }

        /// <summary>
        /// Gets or sets the type of the map.
        /// </summary>
        /// <value>The type of the map.</value>
        public Type MapType
        {
            get { return mapType; }
            set { mapType = value; }
        }

        /// <summary>
        /// Gets or sets the table for this relation
        /// </summary>
        /// <value>The table.</value>
        public string Table
        {
            get { return table; }
            set { table = value; }
        }

        /// <summary>
        /// Gets or sets the schema for this relation (dbo., etc)
        /// </summary>
        /// <value>The schema name.</value>
        public string Schema
        {
            get { return schema; }
            set { schema = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RelationAttribute"/> is inverse.
        /// </summary>
        /// <value><c>true</c> if inverse; otherwise, <c>false</c>.</value>
        public bool Inverse
        {
            get { return inverse; }
            set { inverse = value; }
        }

        /// <summary>
        /// Gets or sets the cascade options for this <see cref="RelationAttribute"/>
        /// </summary>
        /// <value>The cascade.</value>
        public ManyRelationCascadeEnum Cascade
        {
            get { return cascade; }
            set { cascade = value; }
        }

        /// <summary>
        /// Gets or sets the order by clause for this relation. This is a SQL order, not HQL.
        /// </summary>
        public string OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

        /// <summary>
        /// Gets or sets the where clause for this relation
        /// </summary>
        public string Where
        {
            get { return where; }
            set { where = value; }
        }

        /// <summary>
        /// Only used with sets. The value can be <c>unsorted</c>, <c>natural</c> and the name of a class implementing <c>System.Collections.IComparer</c>
        /// </summary>
        public string Sort
        {
            get { return sort; }
            set { sort = value; }
        }

        /// <summary>
        /// Only used with maps or lists
        /// </summary>
        public string Index
        {
            get { return index; }
            set { index = value; }
        }

        /// <summary>
        /// Only used with maps
        /// </summary>
        public string IndexType
        {
            get { return indexType; }
            set { indexType = value; }
        }

        /// <summary>
        /// Use for simple types.
        /// </summary>
        public string Element
        {
            get { return element; }
            set { element = value; }
        }

        /// <summary>
        /// Gets or sets the way broken relations are handled.
        /// </summary>
        /// <value>The behaviour.</value>
        public NotFoundBehaviour NotFoundBehaviour
        {
            get { return notFoundBehaviour; }
            set { notFoundBehaviour = value; }
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
