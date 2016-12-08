using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Collections;

namespace GenericDotNetSupportLibrary.Helpers
{
  /// <summary>
  /// Utility class for helper classes that helps 
  /// </summary>
  public class TypeHelper
  {
    #region Private Fields

    private static ThreadSafeDictionary<Type, ThreadSafeDictionary<BindingFlags, PropertyInfo[]>> _PropertiesByType = new ThreadSafeDictionary<Type, ThreadSafeDictionary<BindingFlags, PropertyInfo[]>>();

    #endregion


    /// <summary>
    /// Generic version of GetFullName.
    /// Gets fully qualified classname and assembly output.  As needed by configuration files and data orm providers.
    /// This ignores NHibernate Proxy Objects and gets the real type.
    /// </summary>
    /// <typeparam name="T">The type to get the fullname of.</typeparam>
    /// <returns>This method will output the type name in [Namespaces.Class, Assembly Name] format</returns>
    public static string GetFullName<T>()
    {
      return GetFullName(typeof(T));
    }


    /// <summary>
    /// Overload of GetFullName(Type) allowing the caller to just pass an object instead of calling .GetType() manually
    /// on the object.
    /// </summary>
    /// <param name="objectToGetTypeFrom">Object to get full type name of</param>
    /// <returns>This method will output the type name in [Namespaces.Class, Assembly Name] format</returns>
    public static string GetFullName(object objectToGetTypeFrom)
    {
      if (objectToGetTypeFrom == null)
        return "";
      return GetFullName(objectToGetTypeFrom.GetType());
    }

    /// <summary>
    ///  Same as GetFullName, but only returns the fully qualified class name (no assemply info).
    /// </summary>
    /// <param name="objectToGetTypeFrom"></param>
    /// <returns></returns>
    public static string GetBaseName(object objectToGetTypeFrom)
    {
      if (objectToGetTypeFrom == null)
        return "";

      return GetBaseName(objectToGetTypeFrom.GetType());
    }

    /// <summary>
    /// Same as GetBaseName(Type), but takes a string containing a fully qualified class name
    /// </summary>
    /// <param name="fullyQualifiedName">A string containing a fully qualified class name</param>
    /// <returns>A fully qualified class name minus the assembly information</returns>
    public static string GetBaseName(string fullyQualifiedName)
    {
      if ((string.IsNullOrEmpty(fullyQualifiedName)) || (fullyQualifiedName.IndexOf(',') < 0))
        return "";

      return fullyQualifiedName.Split(',')[0].Trim();
    }


    /// <summary>
    /// A quicker and safer way to convert a string into an integer.  If value is not a valid int,
    /// it will return 0.
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static int ParseInt(object test)
    {
      if (test == null)
        return 0;

      int ret = 0;

      try
      {
        if (int.TryParse(test.ToString(), out ret))
          return ret;
        else
          return 0;
      }
      catch { return 0; }
    }

    /// <summary>
    /// Returns a nullable integer
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static int? ParseNullableInt(object test)
    {
      int ret = 0;

      if (test == null)
        return null;

      try
      {
        if (int.TryParse(test.ToString(), out ret))
          return ret;
        else
          return null;
      }
      catch { return null; }
    }
    /// <summary>
    /// A quicker and safer way to convert a string into a boolean.  If value is not a valid
    /// boolean, then it will return false.
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static bool ParseBool(object test)
    {
      bool ret = false;

      if (test == null)
        return false;

      try
      {
        string stringTest = test.ToString();
        if (stringTest == "1" || stringTest.ToLower() == "true")
          return true;
        else if (bool.TryParse(stringTest, out ret))
          return ret;
        else
          return false;
      }
      catch { return false; }
    }

    /// <summary>
    /// A quicker and safer way to convert a string into a long.  If value is not a valid long,
    /// it will return 0.
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static long ParseLong(object test)
    {
      long ret = 0;

      if (test == null)
        return 0;

      try
      {
        if (long.TryParse(test.ToString(), out ret))
          return ret;
        else
          return 0;
      }
      catch { return 0; }
    }

    /// <summary>
    /// Tries to parse the string as a Guid. If the string is not a guid, empty guid is returned back
    /// </summary>
    /// <param name="guidString"></param>
    /// <returns></returns>
    public static Guid ParseGuid(string guidString)
    {
      Guid guid = Guid.Empty;
      try
      {
        guid = new Guid(guidString);
      }
      catch
      {
        //do nothing
      }

      return guid;
    }

    /// <summary>
    /// A quicker and safer way to convert a string into a double.  If value is not a valid double,
    /// it will return 0.
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static double ParseDouble(object test)
    {
      double ret = 0;
      if (test == null)
        return ret;

      try
      {
        if (double.TryParse(test.ToString(), out ret))
          return ret;
        else
          return 0;
      }
      catch { return 0; }
    }
    /// <summary>
    /// Parse into Float type
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static float ParseFloat(object test)
    {
      float ret = 0;

      if (test == null)
        return 0;

      try
      {
        if (float.TryParse(test.ToString(), out ret))
          return ret;
        else
          return 0;
      }
      catch { return 0; }
    }

    /// <summary>
    /// Parse into Nullable Float type
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static float? ParseNullableFloat(object test)
    {
      float ret = 0;

      if (test == null)
        return null;

      try
      {
        if (float.TryParse(test.ToString(), out ret))
          return ret;
        else
          return null;
      }
      catch { return null; }
    }

    /// <summary>
    /// Parse into Nullable Double type
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static double? ParseNullableDouble(object test)
    {
      double ret = 0;

      if (test == null)
        return null;

      try
      {
        if (double.TryParse(test.ToString(), out ret))
          return ret;
        else
          return null;
      }
      catch { return null; }
    }

    /// <summary>
    /// Parse into Short type
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static short ParseShort(object test)
    {
      short ret = 0;

      if (test == null)
        return ret;

      try
      {
        if (short.TryParse(test.ToString(), out ret))
          return ret;
        else
          return 0;
      }
      catch { return 0; }
    }
    /// <summary>
    /// Parse into Decimal type
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static Decimal ParseDecimal(object test)
    {
      Decimal ret = 0;

      if (test == null)
        return ret;

      try
      {
        if (Decimal.TryParse(test.ToString(), out ret))
          return ret;
        else
          return 0;
      }
      catch { return 0; }
    }

    /// <summary>
    /// Parse into Nullable Decimal type
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static Decimal? ParseNullableDecimal(object test)
    {
      Decimal ret = 0;

      if (test == null)
        return null;

      try
      {
        if (Decimal.TryParse(test.ToString(), out ret))
          return ret;
        else
          return null;
      }
      catch { return null; }
    }


    /// <summary>
    /// Converts an object to a DateTime.
    /// </summary>
    /// <param name="test"></param>
    /// <returns>Returns Constants.NullDate if it cannot be parsed.</returns>
    public static DateTime? ParseDate(object test)
    {
      if (test == null)
        return null;

      DateTime ret;
      try
      {
        DateTime.TryParse(test.ToString(), out ret);
        return ret;
      }
      catch
      {
      }

      return null;
    }

    /// <summary>
    /// Returns the Type of the Base Modal class
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>  
    //public static Type GetBaseModelType(Type type)
    //{
    //  //return type.Name.Contains("INHibernateProxy") ? type.BaseType : type;
    //  //this supports the new version of Castle Dynamice Proxy 2.1
    //  return type.Assembly.FullName.ToLower().StartsWith("dynamic") ? type.BaseType : type;      //for new version of nhibernate 2.0 or castle 2.0
    //}

    /// <summary>
    /// This method returns a Type reference for the FullyQualifiedName passed in as a parameter.  If the type cannot be located or loaded 
    /// this method will return null.
    /// </summary>
    /// <param name="FullyQualifiedName"></param>
    /// <returns></returns>
    public static Type CreateInterfaceType(string FullyQualifiedName)
    {
      return Type.GetType(FullyQualifiedName, false);
    }



    /// <summary>
    /// Gets the properties for a type w/ the specified Binding Flags - caches the result.
    /// </summary>
    /// <param name="type">The type to retrieve properties for.</param>
    /// <param name="bindingFlags">The binding flags to use when retrieving properties.</param>
    /// <returns>The an array of the resulting property info objects.</returns>
    public static PropertyInfo[] GetProperties(Type type, BindingFlags bindingFlags)
    {
      if (!_PropertiesByType.ContainsKey(type))
        _PropertiesByType[type] = new ThreadSafeDictionary<BindingFlags, PropertyInfo[]>();
      if (!_PropertiesByType[type].ContainsKey(bindingFlags))
        _PropertiesByType[type][bindingFlags] = type.GetProperties(bindingFlags);
      return _PropertiesByType[type][bindingFlags];
    }


    public static Dictionary<string, PropertyInfo> GetPropertiesAsDictionary(Type type, BindingFlags bindingFlags)
    {
      Dictionary<string, PropertyInfo> asDictionary = new Dictionary<string, PropertyInfo>();

      PropertyInfo[] asArray = GetProperties(type, bindingFlags);
      foreach (PropertyInfo pi in asArray)
      {
        // should always be a new name, but just for sanity
        if (!asDictionary.ContainsKey(pi.Name))
          asDictionary.Add(pi.Name, pi);
      }

      return asDictionary;
    }


    public static PropertyInfo GetPropertyInfo(Type type, string propertyPath)
    {
      PropertyInfo result = null;
      string property = "";
      string remainingProperties = "";
      if (propertyPath.IndexOf('.') != -1)
      {
        property = propertyPath.Substring(0, propertyPath.IndexOf('.'));
        remainingProperties = propertyPath.Substring(propertyPath.IndexOf('.') + 1);
      }
      else
        property = propertyPath;

      foreach (PropertyInfo prop in type.GetProperties())
      {
        if (prop.Name == property)
        {
          // We found the name.
          if (remainingProperties == "")
            // Evaluate the property (make sure you don't get a proxy type)
            result = prop;
          else
            result = (PropertyInfo)GetPropertyInfo(prop.PropertyType, remainingProperties);

          break;
        }
      }
      return result;
    }

    /// <summary>
    /// Nullable wrapper for Convert.ChangeType
    /// </summary>
    public static object ChangeType(object obj, Type type)
    {
      try
      {
        if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
          return Convert.ChangeType(obj, Nullable.GetUnderlyingType(type));
        else
          return Convert.ChangeType(obj, type);
      }
      catch (InvalidCastException)
      {
        return null;
      }
    }
  }
}
