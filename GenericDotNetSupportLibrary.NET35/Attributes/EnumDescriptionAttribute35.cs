using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GenericDotNetSupportLibrary.Attributes
{
  [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
  public class EnumDescriptionAttribute : System.Attribute
  {
    string _Description = null;

    public string Description
    {
      get
      {
        if (string.IsNullOrEmpty(this._Description))
        {
          Type type = this.ResourcesType;
          PropertyInfo info = type.GetProperty(this.ResourcesPropertyName, BindingFlags.Static | BindingFlags.NonPublic);

          object value = info.GetValue(null, null);

          if (value != null)
            this._Description = (string)value;
        }

        return this._Description;
      }
      set { this._Description = value; }
    }
    public string ResourcesPropertyName { get; set; }
    public Type ResourcesType { get; set; }

    public EnumDescriptionAttribute()
    {
    }
  }
}
