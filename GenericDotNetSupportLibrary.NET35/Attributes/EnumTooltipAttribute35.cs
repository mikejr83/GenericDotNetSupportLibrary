using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GenericDotNetSupportLibrary.Attributes
{
  [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
  public class EnumTooltipAttribute : System.Attribute
  {
    string _Tooltip = null;

    public string Tooltip
    {
      get
      {
        if (string.IsNullOrEmpty(this._Tooltip))
        {
          Type type = this.ResourcesType;

          PropertyInfo info = null;

          if (type != null)
          {
            info = type.GetProperty(this.ResourcesPropertyName, BindingFlags.Static | BindingFlags.NonPublic);

            if (info != null)
            {
              object value = info.GetValue(null, null);

              if (value != null)
                this._Tooltip = (string)value;
            }
          }
        }

        return this._Tooltip;
      }
      set { this._Tooltip = value; }
    }
    public string ResourcesPropertyName { get; set; }
    public Type ResourcesType { get; set; }

    public EnumTooltipAttribute()
    {
    }
  }
}
