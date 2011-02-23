using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyData.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false), Serializable]
    public class OneToOneAttribute : Attribute
    {
        private bool _load = false;
        private CascadeEnum _cascade = CascadeEnum.None;
        private FetchEnum _fetch = FetchEnum.Unspecified;
        private string _propertyRef;
        private Type _mapType;
        private bool _constrained;
        private string _foreignKey;

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
        /// Specifies which operations should be 
        /// cascaded from the parent object to the associated object.
        /// </summary>
        public CascadeEnum Cascade
        {
            get { return _cascade; }
            set { _cascade = value; }
        }

        /// <summary>
        /// Chooses between outer-join fetching 
        /// or sequential select fetching.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="FetchEnum.Select"/>
        /// </remarks>
        public FetchEnum Fetch
        {
            get { return _fetch; }
            set { _fetch = value; }
        }

        /// <summary>
        /// The name of a property of the 
        /// associated class that is joined to the primary key 
        /// of this class. If not specified, the primary key of 
        /// the associated class is used.
        /// </summary>
        public string PropertyRef
        {
            get { return _propertyRef; }
            set { _propertyRef = value; }
        }

        /// <summary>
        /// Specifies that a foreign key 
        /// constraint on the primary key of the mapped table 
        /// references the table of the associated class. 
        /// This option affects the order in which Save() and 
        /// Delete() are cascaded (and is also used by the 
        /// schema export tool).
        /// </summary>
        public bool Constrained
        {
            get { return _constrained; }
            set { _constrained = value; }
        }

        /// <summary>
        /// Gets or sets the name of the foreign key constraint generated for 
        /// an association. EasyData will only use the ForeignKey name one 
        /// the inherited class and Constrained = true.
        /// </summary>
        public string ForeignKey
        {
            get { return _foreignKey; }
            set { _foreignKey = value; }
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
