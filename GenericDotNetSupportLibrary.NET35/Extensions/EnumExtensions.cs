using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDotNetSupportLibrary.Helpers;

namespace GenericDotNetSupportLibrary.Extensions
{
    /// <summary>
    /// Uses the EnumHelper and the Enum Attributes to extend functionality for enums.
    /// <remarks>
    /// These methods are generally short cuts for the EnumHelper methods.
    /// </remarks>
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the display name from the enum value.
        /// </summary>
        /// <param name="e">The enum value.</param>
        /// <returns>If the enum has a EnumDisplayName defined it will be returned or the text representation of the value (default .ToString functionality).</returns>
        public static string GetDisplayText(this Enum e)
        {
            string valueText = e.ToString();

            string displayText = EnumHelper.GetDisplayValues(e.GetType()).Where(ed => ed.Value.Equals(e)).Select(ed => ed.DisplayName).FirstOrDefault();

            return !string.IsNullOrEmpty(displayText) ? displayText : valueText;
        }
    }
}
