using System;

namespace GenericDotNetSupportLibrary.Attributes
{
    /// <summary>
    /// Attribute for providing a display name to an enumeration field
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class EnumValueAttribute : System.Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string ValueName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDisplayNameAttribute"/> class.
        /// </summary>
        /// <param name="displayName">the name which should be displayed</param>
        public EnumValueAttribute(string valueName)
        {
            this.ValueName = valueName;
        }

        /// <summary>
        /// Overrides the default to string for the attribute to return the display name that the attribute defines.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ValueName;
        }
    }
}
