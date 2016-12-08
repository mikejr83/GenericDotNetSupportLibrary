using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace GenericDotNetSupportLibrary.Helpers
{
  /// <summary>
  /// Helper methods for Runtime classes.
  /// <remarks>
  /// These are general application level methods.
  /// </remarks>
  /// </summary>
  public static class RuntimeHelper
  {
    /// <summary>
    /// Gets the version of the assembly based on a type.  This method will determine in the application is network deployed and get the version from that assembly.
    /// If the application is not network deployed and a type is specified then the assembly from the type will be used for version information.  If no type
    /// is supplied then the current executing assembly's version is returned.
    /// </summary>
    /// <returns>Version string Major.Minor.Build.Revision</returns>
    public static string GetComponentVersion()
    {
      return RuntimeHelper.GetComponentVersion(null);
    }

    /// <summary>
    /// Gets the version of the assembly based on a type.  This method will determine in the application is network deployed and get the version from that assembly.
    /// If the application is not network deployed and a type is specified then the assembly from the type will be used for version information.  If no type
    /// is supplied then the current executing assembly's version is returned.
    /// </summary>
    /// <param name="type">Type used to determine the assembly to check.<remarks>Defaults to null which would be the current executing assembly.</remarks></param>
    /// <returns>Version string Major.Minor.Build.Revision</returns>
    public static string GetComponentVersion(Type type)
    {
      string version = "{0}.{1}.{2}.{3}";

      Version
          v =
              type == null ? System.Reflection.Assembly.GetCallingAssembly().GetName().Version : System.Reflection.Assembly.GetAssembly(type).GetName().Version;


      return string.Format(CultureInfo.CurrentUICulture, version, v.Major, v.Minor, v.Build, v.Revision);
    }

    /// <summary>
    /// Gets the timestamp from the assembly.  Used to show when the assembly was built.
    /// </summary>
    /// <returns>Date and time when the assembly was built.</returns>
    public static DateTime RetrieveLinkerTimestamp()
    {
      return RuntimeHelper.RetrieveLinkerTimestamp(null);
    }

    /// <summary>
    /// Gets the timestamp from the assembly.  Used to show when the assmebly was built.
    /// </summary>
    /// <param name="type">Type used to determine the assembly to check.<remarks>Defaults to null which would be the current executing assembly.</remarks></param>
    /// <returns>Date and time when the assembly was built.</returns>
    public static DateTime RetrieveLinkerTimestamp(Type type)
    {
      string filePath =
          type == null ? System.Reflection.Assembly.GetCallingAssembly().Location : System.Reflection.Assembly.GetAssembly(type).Location;

      const int c_PeHeaderOffset = 60;
      const int c_LinkerTimestampOffset = 8;
      byte[] b = new byte[2048];
      System.IO.Stream s = null;

      try
      {
        s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        s.Read(b, 0, 2048);
      }
      finally
      {
        if (s != null)
        {
          s.Close();
        }
      }

      int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
      int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
      DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
      dt = dt.AddSeconds(secondsSince1970);
      dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
      return dt;
    }
  }
}
