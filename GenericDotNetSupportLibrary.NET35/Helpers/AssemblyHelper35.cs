using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GenericDotNetSupportLibrary.Helpers
{
  public static class AssemblyHelper
  {
    /// <summary>
    /// Returns the GUID identifier for the given assembly.  Looks for the Guid attribute on the assembly.
    /// </summary>
    /// <param name="assembly">.NET managed assembly</param>
    /// <returns>Guid identifier of the assembly</returns>
    public static Guid FindAssemblyIdentifier(Assembly assembly)
    {
      object[] objects = assembly.GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);

      return objects.Length > 0 ? new Guid(((System.Runtime.InteropServices.GuidAttribute)objects[0]).Value) : new Guid();
    }
  }
}
