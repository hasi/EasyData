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
    public class CompositeKeyAttribute : Attribute
    {
        private string _unsavedValue;

        /// <summary>
        /// Gets or sets the unsaved value.
        /// </summary>
        /// <value>The unsaved value.</value>
        public string UnsavedValue
        {
            get { return _unsavedValue; }
            set { _unsavedValue = value; }
        }
    }
}
