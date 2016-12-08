using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDotNetSupportLibrary.Helpers;

namespace GenericDotNetSupportLibrary.Extensions
{
  /// <summary>
  /// Extensions class for all string related utilities.
  /// </summary>
  public static class StringExtensions
  {
    /// <summary>
    /// Returns the base 64 encoded version of the string.
    /// </summary>
    /// <param name="inputString"></param>
    /// <returns></returns>
    public static string ConvertToBase64String(this string inputString)
    {
      return StringHelper.ConvertToBase64String(inputString);
    }
  }
}
