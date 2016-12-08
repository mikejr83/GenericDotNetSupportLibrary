using System;

namespace GenericDotNetSupportLibrary.Attributes
{
    /// <summary>
    /// Attribute for providing a display name to an enumeration field
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class EnumDisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDisplayNameAttribute"/> class.
        /// </summary>
        /// <param name="displayName">the name which should be displayed</param>
        public EnumDisplayNameAttribute(string displayName)
            : base(displayName)
        { }

        /// <summary>
        /// Overrides the default to string for the attribute to return the display name that the attribute defines.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
