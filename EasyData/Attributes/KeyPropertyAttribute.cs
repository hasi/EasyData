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
    public class KeyPropertyAttribute : PropertyAttribute
    {
        private String unsavedValue;

        /// <summary>
        /// Gets or sets the unsaved value.
        /// </summary>
        /// <value>The unsaved value.</value>
        public String UnsavedValue
        {
            get { return unsavedValue; }
            set { unsavedValue = value; }
        }
    }
}
