﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyData.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false), Serializable]
    public class BelongsToAttribute : Attribute
    {
        private bool _load = false;
        private Type type;
        private string column;
        private string[] compositeKeyColumns;
        private string uniqueKey;
        private string foreignKey;
        private bool update = true;
        private bool insert = true;
        private bool notnull;
        private bool unique;
        private string propertyRef;
        private FetchEnum fetchMethod = FetchEnum.Unspecified;
        private CascadeEnum cascade = CascadeEnum.None;
        private NotFoundBehaviour notFoundBehaviour = NotFoundBehaviour.Default;

        /// <summary>
        /// Initializes a new instance of the <see cref="BelongsToAttribute"/> class.
        /// </summary>
        public BelongsToAttribute()
        {
        }

        /// <summary>
        /// Indicates the name of the column to be used on the association.
        /// Usually the name of the foreign key field on the underlying database.
        /// </summary>
        public BelongsToAttribute(string column)
        {
            this.column = column;
        }

        /// <summary>
        /// Defines the target type of the association. It's usually inferred from the property type.
        /// </summary>
        public Type Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Defines the column used by association (usually a foreign key)
        /// </summary>
        public string Column
        {
            get { return column; }
            set { column = value; }
        }

        /// <summary>
        /// Defines the Composite Key columns used by association (aka Natural Keys).
        /// </summary>
        public string[] CompositeKeyColumns
        {
            get { return compositeKeyColumns; }
            set { compositeKeyColumns = value; }
        }

        /// <summary>
        /// Defines the cascading behavior of this association.
        /// </summary>
        public CascadeEnum Cascade
        {
            get { return cascade; }
            set { cascade = value; }
        }

        /// <summary>
        /// Chooses between outer-join fetching
        /// or sequential select fetching.
        /// </summary>
        public FetchEnum Fetch
        {
            get { return fetchMethod; }
            set { fetchMethod = value; }
        }

        /// <summary>
        /// The name of a property of the 
        /// associated class that is joined to the primary key 
        /// of this class. If not specified, the primary key of 
        /// the associated class is used.
        /// </summary>
        public string PropertyRef
        {
            get { return propertyRef; }
            set { propertyRef = value; }
        }

        /// <summary>
        /// Set to <c>false</c> to ignore this association when updating entities of this EasyData class.
        /// </summary>
        public bool Update
        {
            get { return update; }
            set { update = value; }
        }

        /// <summary>
        /// Set to <c>false</c> to ignore this association when inserting entities of this EasyData class.
        /// </summary>
        public bool Insert
        {
            get { return insert; }
            set { insert = value; }
        }

        /// <summary>
        /// Indicates whether this association allows nulls or not.
        /// </summary>
        public bool NotNull
        {
            get { return notnull; }
            set { notnull = value; }
        }

        /// <summary>
        /// Indicates whether this association is unique.
        /// </summary>
        public bool Unique
        {
            get { return unique; }
            set { unique = value; }
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
            get { return uniqueKey; }
            set { uniqueKey = value; }
        }

        /// <summary>
        /// Gets and sets the name of the foreign key constraint 
        /// generated for an association.
        /// </summary>
        public string ForeignKey
        {
            get { return foreignKey; }
            set { foreignKey = value; }
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
