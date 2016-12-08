using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDotNetSupportLibrary.Attributes
{
    /// <summary>
    /// Property level attribute which declares that the property should not be listened to when the property changed event for the class fires.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SkipPropertyChangedAttribute : System.Attribute
    {
        /// <summary>
        /// Default constructor for the attribute.
        /// </summary>
        public SkipPropertyChangedAttribute() { }
    }
}
