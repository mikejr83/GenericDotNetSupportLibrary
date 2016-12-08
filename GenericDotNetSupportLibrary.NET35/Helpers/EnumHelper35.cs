using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
//using System.Windows.Controls;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Data;
//using System.Windows;
using GenericDotNetSupportLibrary.Attributes;

namespace GenericDotNetSupportLibrary.Helpers
{
  /// <summary>
  /// Provides support methods for enumerations.
  /// </summary>
  public static class EnumHelper
  {
    /// <summary>
    /// Gets a list of EnumDisplayers <seealso cref="EnumDisplayer"/> for an enum.<remarks>This is useful for displaying in a dropdown or for databinding.</remarks>
    /// </summary>
    /// <param name="enumType">Enum type for which the display names and their values will be retrieved.</param>
    /// <returns>List of EnumDisplayer objects.</returns>
    public static IEnumerable<EnumDisplayer> GetDisplayValues(Type enumType)
    {
      List<EnumDisplayer> display = new List<EnumDisplayer>();
      foreach (FieldInfo info in enumType.GetFields())
      {
        if (!info.IsStatic) continue;

        EnumDisplayNameAttribute nameAttr =
            info.GetCustomAttributes(typeof(EnumDisplayNameAttribute), false).FirstOrDefault() as EnumDisplayNameAttribute;

        EnumDescriptionAttribute descAttr =
            info.GetCustomAttributes(typeof(EnumDescriptionAttribute), false).FirstOrDefault() as EnumDescriptionAttribute;

        EnumTooltipAttribute tooltipAttr =
            info.GetCustomAttributes(typeof(EnumTooltipAttribute), false).FirstOrDefault() as EnumTooltipAttribute;

        string displayName = info.Name, description = null, tooltip = null;
        if (nameAttr != null)
          displayName = nameAttr.DisplayName;
        else
        {
          displayName = StringHelper.ToTitleCase(StringHelper.AddSpacesToSentence(displayName));
        }

        if (descAttr != null)
          description = descAttr.Description;

        if (tooltipAttr != null)
        {
          tooltip = tooltipAttr.Tooltip;
        }

        EnumDisplayer ed = null;

        try
        {
          ed = new EnumDisplayer(displayName, description, tooltip, Enum.Parse(enumType, info.Name));
        }
        catch
        {
        }

        display.Add(ed);
      }

      return display;
    }

    /// <summary>
    /// Makes an enum value title case with spaces between the words.
    /// </summary>
    /// <param name="value">Enum value</param>
    /// <returns>String with spaces between the words and title cased.</returns>
    static public string MakeDisplayText(Enum value)
    {
      return StringHelper.ToTitleCase(StringHelper.AddSpacesToSentence(value.ToString()));
    }

    /// <summary>
    /// Gets the value from a string representation of the value by looking at the EnumValue attribute on the fields in the enum.
    /// </summary>
    /// <typeparam name="T">Type of enumeration that contains the value.</typeparam>
    /// <param name="value">String representation of the value.</param>
    /// <returns>Enum value which matched the EnumValue attribute.</returns>
    static public T GetEnumFromValue<T>(string value)
    {
      T returnValue = default(T);

      foreach (FieldInfo info in typeof(T).GetFields())
      {
        if (!info.IsStatic) continue;

        EnumValueAttribute attr =
            info.GetCustomAttributes(typeof(EnumValueAttribute), false).FirstOrDefault() as EnumValueAttribute;

        //string displayName = info.Name;
        if (attr != null && attr.ValueName.Equals(value))
          returnValue = (T)Enum.Parse(typeof(T), info.Name);
      }

      return returnValue;
    }
  }

  /// <summary>
  /// A displayer object for an enum value.  This class contains the display name value of the enum value and the value of the actual enum.
  /// </summary>
  public class EnumDisplayer : IEquatable<EnumDisplayer>
  {
    /// <summary>
    /// Display name of the enum value.
    /// </summary>
    public string DisplayName { get; private set; }
    /// <summary>
    /// The description information pulled from the EnumDescriptionAttribute on the field if one is available.
    /// </summary>
    public string Description { get; private set; }
    /// <summary>
    /// The tooltip text pulled from the EnumTooltipAttribute on the field if one is available.
    /// </summary>
    public string Tooltip { get; private set; }
    /// <summary>
    /// The value of the enum.
    /// </summary>
    public object Value { get; private set; }

    public EnumDisplayer(string displayName, string description, string tooltip, object value)
      : this(displayName, description, value)
    {
      this.Tooltip = tooltip;
    }

    public EnumDisplayer(string displayName, string description, object value)
      : this(displayName, value)
    {
      this.Description = description;
    }

    /// <summary>
    /// Creates a new displayer by taking the display name and the value of the enum.
    /// </summary>
    /// <param name="displayName">The visual display name of the enum value.</param>
    /// <param name="value">The value of the enumeration.</param>
    public EnumDisplayer(string displayName, object value)
    {
      this.DisplayName = displayName;
      this.Value = value;
    }

    /// <summary>
    /// Overrides the default ToString value to be the display name of the enum's value.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return this.DisplayName;
    }

    /// <summary>
    /// Allows the displayer to be implicitly converted to a string representing the display name of the enum's value.
    /// </summary>
    /// <param name="displayer">The displayer</param>
    /// <returns>The string representation of the displayer object.</returns>
    public static implicit operator string(EnumDisplayer displayer)
    {
      return displayer.ToString();
    }

    #region IEquatable<EnumDisplayer> Members

    /// <summary>
    /// Used for comparing another enum displayer to the current dispalyer object.
    /// </summary>
    /// <param name="other">The enum to compare to.</param>
    /// <returns>True/False if the values of the enums are the same.</returns>
    public bool Equals(EnumDisplayer other)
    {
      return this.Value.Equals(other.Value);
    }

    #endregion

    public override bool Equals(object obj)
    {
      EnumDisplayer displayer = obj as EnumDisplayer;
      return displayer != null && displayer.Value.Equals(this.Value);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}
